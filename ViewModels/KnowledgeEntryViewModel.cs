using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KnowledgeTracker.Data.Interfaces;
using KnowledgeTracker.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeTracker.ViewModels
{
    public partial class KnowledgeEntryViewModel : ObservableObject, IDisposable
    {
        private readonly IKnowledgeEntryService _service;

        public KnowledgeEntryViewModel(IKnowledgeEntryService service)
        {
            _service = service;
            ValidationErrors.CollectionChanged += ValidationErrors_CollectionChanged;
            _ = LoadAllAsync();
            SelectedEntry = new KnowledgeEntry();
            SelectedEntry.DateResolved = DateTime.Today;
            Attachments = new ObservableCollection<AttachmentInfo>();
        }

        public Action<string, string>? ShowMessage { get; set; }
        public string ThemeSwitchIcon => Application.Current?.UserAppTheme == AppTheme.Dark ? "sun.png" : "moon.png";
        public string? FirstValidationError => ValidationErrors.Count > 0 ? ValidationErrors[0] : null;
        public bool HasValidationErrors => ValidationErrors.Count > 0;

        public ObservableCollection<string> IdiomasSuportados { get; } = new() { "Português", "English" };
        [ObservableProperty]
        private string idiomaSelecionado = "Português";

        [ObservableProperty]
        private ObservableCollection<string> validationErrors = new ObservableCollection<string>();

        [ObservableProperty]
        private string? searchTerm;

        [ObservableProperty]
        private ObservableCollection<KnowledgeEntry> entries = new();

        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private string? title;

        [ObservableProperty]
        private string? description;

        [ObservableProperty]
        private DateTime? dateResolved;

        [ObservableProperty]
        private string? projectName;

        [ObservableProperty]
        private string? resolutionSteps;

        [ObservableProperty]
        private string? technologies;

        [ObservableProperty]
        private string? tags;

        [ObservableProperty]
        private string? comments;

        [ObservableProperty]
        private string? youTubeUrl;

        [ObservableProperty]
        private HtmlWebViewSource? youTubeHtmlSource;

        [ObservableProperty]
        private bool isYouTubeVisible;

        [ObservableProperty]
        private ObservableCollection<AttachmentInfo> attachments = new();

        [ObservableProperty]
        private bool isBusy;

        private KnowledgeEntry? selectedEntry;
        public KnowledgeEntry? SelectedEntry
        {
            get => selectedEntry;
            set
            {
                if (SetProperty(ref selectedEntry, value))
                {
                    if (selectedEntry != null)
                    {
                        Id = selectedEntry.Id;
                        Title = selectedEntry.Title;
                        Description = selectedEntry.Description;
                        DateResolved = selectedEntry.DateResolved;
                        ProjectName = selectedEntry.ProjectName;
                        ResolutionSteps = selectedEntry.ResolutionSteps;
                        Technologies = selectedEntry.Technologies;
                        Tags = selectedEntry.Tags;
                        Comments = selectedEntry.Comments;
                        YouTubeUrl = selectedEntry.YouTubeUrl;

                        Attachments = new ObservableCollection<AttachmentInfo>(selectedEntry.Attachments ?? new List<AttachmentInfo>());
                    }
                    else
                    {
                        ClearFields();
                        Attachments = new ObservableCollection<AttachmentInfo>();
                    }
                    IsYouTubeVisible = false;
                    YouTubeHtmlSource = null;
                }
            }
        }

        private void ClearFields()
        {
            Id = 0;
            Title = null;
            Description = null;
            DateResolved = DateTime.Today;
            ProjectName = null;
            ResolutionSteps = null;
            Technologies = null;
            Tags = null;
            Comments = null;
            YouTubeUrl = null;
            YouTubeHtmlSource = null;
            IsYouTubeVisible = false;

            if (SelectedEntry != null)
            {
                SelectedEntry.YouTubeUrl = null;
            }
        }
        private KnowledgeEntry CreateEntryFromFields()
        {
            return new KnowledgeEntry
            {
                Id = Id,
                Title = Title ?? string.Empty,
                Description = Description ?? string.Empty,
                DateResolved = DateResolved ?? DateTime.Today,
                ProjectName = ProjectName ?? string.Empty,
                ResolutionSteps = ResolutionSteps ?? string.Empty,
                Technologies = Technologies ?? string.Empty,
                Tags = Tags ?? string.Empty,
                Comments = Comments ?? string.Empty,
                YouTubeUrl = SelectedEntry?.YouTubeUrl,
                Attachments = Attachments.ToList()
            };
        }

        [RelayCommand]
        public async Task LoadAllAsync()
        {
            IsBusy = true;
            try
            {
                var list = await _service.GetAllAsync();
                Entries = new ObservableCollection<KnowledgeEntry>(list);
            }
            finally
            {
                IsBusy = false;
            }
        }


        [RelayCommand]
        public async Task LoadByIdAsync(int id)
        {
            if (id == 0) return;
            var entry = await _service.GetByIdAsync(id);
            if (entry != null)
            {
                SelectedEntry = entry;
            }
        }

        [RelayCommand]
        public async Task AddAsync()
        {
            if (!ValidateSelectedEntry())
                return;

            var newEntry = CreateEntryFromFields();
            int newId = await _service.InsertAsync(newEntry);
            newEntry.Id = newId;
            Entries.Add(newEntry);

            // Save attachments for the new entry
            foreach (var attachment in Attachments)
            {
                attachment.KnowledgeEntryId = newId;
                await _service.AddAttachmentAsync(newId, attachment);
                // File is already present on disk (created when picked)
            }

            SelectedEntry = new KnowledgeEntry();
            ClearFields();
            Attachments = new ObservableCollection<AttachmentInfo>();
            await ShowNotificationAsync("Sucesso", "Entrada criada com sucesso!", NotificationType.Info);
        }


        [RelayCommand]
        public async Task UpdateAsync()
        {
            if (Id == 0) return;
            if (!ValidateSelectedEntry())
                return;

            var updatedEntry = CreateEntryFromFields();
            await _service.UpdateAsync(updatedEntry);

            // Get persisted and current attachments
            var persistedAttachments = await _service.GetAttachmentsAsync(updatedEntry.Id);
            var currentAttachments = Attachments.ToList();

            // Delete removed attachments (from DB and disk)
            var removed = persistedAttachments.Where(pa => !currentAttachments.Any(ca => ca.FilePath == pa.FilePath)).ToList();
            foreach (var attachment in removed)
            {
                await _service.RemoveAttachmentAsync(attachment.Id);
                if (File.Exists(attachment.FilePath))
                    File.Delete(attachment.FilePath);
            }

            // Add new attachments (to DB)
            var added = currentAttachments.Where(ca => !persistedAttachments.Any(pa => pa.FilePath == ca.FilePath)).ToList();
            foreach (var attachment in added)
            {
                attachment.KnowledgeEntryId = updatedEntry.Id;
                await _service.AddAttachmentAsync(updatedEntry.Id, attachment);
                // File is already present on disk (created when picked)
            }

            await LoadAllAsync();
            SelectedEntry = updatedEntry;
            await ShowNotificationAsync("Sucesso", "Entrada atualizada com sucesso!", NotificationType.Info, ToastDuration.Short);
        }

        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (Id == 0) return;

            // Fix CS8602: Check for null before dereferencing Application.Current or MainPage
            if (Application.Current?.MainPage == null)
            {
                await ShowNotificationAsync("Erro", "Não foi possível exibir a caixa de diálogo.", NotificationType.Error, ToastDuration.Long);
                return;
            }

            // Fix IDE0059: Remove unnecessary assignment of 'confirm' if not used
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmação",
                $"Deseja realmente excluir a entrada '{Title}' e os seus anexos?",
                "Excluir",
                "Cancelar"
            );

            if (!confirm)
                return;

            // Remove all attachments for this entry
            var attachments = await _service.GetAttachmentsAsync(Id);
            foreach (var attachment in attachments)
            {
                await _service.RemoveAttachmentAsync(attachment.Id);
                if (File.Exists(attachment.FilePath))
                    File.Delete(attachment.FilePath);
            }

            await _service.DeleteAsync(Id);
            if (SelectedEntry != null)
                Entries.Remove(SelectedEntry);

            SelectedEntry = new KnowledgeEntry();
            Attachments = new ObservableCollection<AttachmentInfo>();
            await ShowNotificationAsync("Sucesso", "Entrada removida com sucesso!", NotificationType.Info);
        }
        [RelayCommand]
        public void NewEntry()
        {
            SelectedEntry = new KnowledgeEntry();
            SelectedEntry.DateResolved = DateTime.Today;
            SyncFieldsFromSelectedEntry();
            Attachments = new ObservableCollection<AttachmentInfo>();
        }

        [RelayCommand]
        public async Task SearchAsync(string searchTerm)
        {
            var list = await _service.SearchAsync(searchTerm ?? "");
            Entries = new ObservableCollection<KnowledgeEntry>(list);
        }

        public bool ValidateSelectedEntry()
        {
            SyncFieldsFromSelectedEntry();
            var entryToValidate = CreateEntryFromFields();

            var errors = new List<string>();

            var context = new ValidationContext(entryToValidate);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(entryToValidate, context, results, true);
            if (!isValid)
                errors.AddRange(results.Select(r => r.ErrorMessage ?? "Erro desconhecido"));

            // Additional YouTube URL validation
            if (!string.IsNullOrWhiteSpace(entryToValidate.YouTubeUrl) && !IsValidUrl(entryToValidate.YouTubeUrl))
                errors.Add("A URL do YouTube é inválida.");

            ValidationErrors.Clear();
            foreach (var error in errors)
                ValidationErrors.Add(error);

            OnPropertyChanged(nameof(ValidationErrors));
            OnPropertyChanged(nameof(HasValidationErrors));

            return ValidationErrors.Count == 0;
        }
        private void ValidationErrors_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasValidationErrors));
        }

        public void Dispose()
        {
            ValidationErrors.CollectionChanged -= ValidationErrors_CollectionChanged;
        }

        private void SyncFieldsFromSelectedEntry()
        {
            if (SelectedEntry != null)
            {
                Id = SelectedEntry.Id;
                Title = SelectedEntry.Title;
                Description = SelectedEntry.Description;
                DateResolved = SelectedEntry.DateResolved;
                ProjectName = SelectedEntry.ProjectName;
                ResolutionSteps = SelectedEntry.ResolutionSteps;
                Technologies = SelectedEntry.Technologies;
                Tags = SelectedEntry.Tags;
                Comments = SelectedEntry.Comments;
            }
        }

        [RelayCommand]
        public void ClearValidationErrors()
        {
            ValidationErrors.Clear();
        }

        // --- URL Validation Helper ---
        private bool IsValidUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        [RelayCommand]
        public async Task ShowYouTubeVideo()
        {
            var url = SelectedEntry?.YouTubeUrl;
            var videoId = ExtractYouTubeId(url);

            if (string.IsNullOrWhiteSpace(url))
            {
                await ShowNotificationAsync("Aviso", "URL deve ser informada.", NotificationType.Warning);
                YouTubeHtmlSource = null;
                IsYouTubeVisible = false;
                return;
            }

            if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrEmpty(videoId))
            {
                YouTubeHtmlSource = new HtmlWebViewSource
                {
                    Html = $@"
                <html>
                    <body style='margin:0;padding:0;'>
                        <iframe width='100%' height='100%' src='https://www.youtube.com/embed/{videoId}' frameborder='0' allowfullscreen></iframe>
                    </body>
                </html>"
                };
                IsYouTubeVisible = true;
            }
            else
            {
                YouTubeHtmlSource = null;
                IsYouTubeVisible = false;
                if (!string.IsNullOrWhiteSpace(url))
                    await ShowNotificationAsync("Aviso", "URL inválida do YouTube.", NotificationType.Warning);
                return;
            }
        }
        private string? ExtractYouTubeId(string? url)
        {
            if (string.IsNullOrEmpty(url)) return null;
            var regex = new System.Text.RegularExpressions.Regex(@"(?:youtu\.be/|youtube\.com/(?:watch\?v=|embed/|v/|shorts/))([^\s&?/]+)");
            var match = regex.Match(url);
            return match.Success ? match.Groups[1].Value : null;
        }

        [RelayCommand]
        public async Task OpenYouTubeInBrowserAsync()
        {
            var url = SelectedEntry?.YouTubeUrl;
            var videoId = ExtractYouTubeId(url);

            if (string.IsNullOrWhiteSpace(url))
            {
                await ShowNotificationAsync("Aviso", "Url deve ser informada.", NotificationType.Warning);
                return;
            }

            if (string.IsNullOrEmpty(videoId))
            {
                await ShowNotificationAsync("Aviso", "Url deve ser informada.", NotificationType.Warning);
                return;
            }

            try
            {
                await Launcher.OpenAsync(url);
            }
            catch (Exception)
            {
                await ShowNotificationAsync("Erro", "Não foi possível abrir o link", NotificationType.Error, ToastDuration.Long);
            }
        }

        [RelayCommand]
        public async Task AddAttachmentAsync()
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".pdf", ".jpg", ".jpeg", ".png", ".txt" } },
                { DevicePlatform.Android, new[] { "application/pdf", "image/jpeg", "image/png", "text/plain" } },
                { DevicePlatform.iOS, new[] { "com.adobe.pdf", "public.jpeg", "public.png", "plain-text" } },
                { DevicePlatform.MacCatalyst, new[] { "com.adobe.pdf", "public.jpeg", "public.png", "public.plain-text" } }
            });

            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Selecione um arquivo PDF, imagem ou textto",
                FileTypes = customFileType
            });

            if (result == null || string.IsNullOrWhiteSpace(result.FileName))
            {
                await ShowNotificationAsync("Erro", "Nenhum arquivo selecionado.", NotificationType.Error, ToastDuration.Long);
                return;
            }

            var fileName = Path.GetFileName(result.FileName);
            if (string.IsNullOrWhiteSpace(fileName) || fileName == "..." || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                await ShowNotificationAsync("Erro", "Nome de arquivo inválido.", NotificationType.Error, ToastDuration.Long);
                return;
            }

            var localPath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            try
            {
                using var stream = await result.OpenReadAsync();
                using var fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None);
                byte[] buffer = new byte[81920];
                int bytesRead;
                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                await ShowNotificationAsync("Erro", $"Falha ao salvar o arquivo: {ex.Message}", NotificationType.Error, ToastDuration.Long);
                return;
            }

            var attachment = new AttachmentInfo
            {
                FileName = fileName,
                FilePath = localPath,
                DateAdded = DateTime.Now,
                KnowledgeEntryId = 0 // Not set yet!
            };

            if (SelectedEntry.Attachments == null)
                SelectedEntry.Attachments = new List<AttachmentInfo>();

            SelectedEntry.Attachments.Add(attachment);
            Attachments.Add(attachment);
        }
        [RelayCommand]
        public async Task OpenAttachmentAsync(AttachmentInfo attachment)
        {
            IsBusy = true;
            try
            {
                if (attachment != null && File.Exists(attachment.FilePath))
                    await Launcher.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(attachment.FilePath) });
                else
                    await ShowNotificationAsync("Erro", "Problema na abertura do ficheiro....", NotificationType.Error, ToastDuration.Long);

            }
            catch (Exception)
            {
                await ShowNotificationAsync("Erro", "Não foi possível abrir o ficheiro.", NotificationType.Error, ToastDuration.Long);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task RemoveAttachmentAsync(AttachmentInfo attachment)
        {
            if (attachment != null && SelectedEntry != null)
            {
                await _service.RemoveAttachmentAsync(attachment.Id);
                Attachments.Remove(attachment);
                SelectedEntry.Attachments.Remove(attachment);
                if (!string.IsNullOrWhiteSpace(attachment.FilePath) && File.Exists(attachment.FilePath))
                {
                    try
                    {
                        File.Delete(attachment.FilePath);
                    }
                    catch (Exception ex)
                    {
                        await ShowNotificationAsync("Erro", $"Falha ao remover o arquivo: {ex.Message}", NotificationType.Error, ToastDuration.Long);
                    }
                }
            }
        }

        [RelayCommand]
        public void ToggleTheme()
        {
            var app = Application.Current;
            if (app?.Resources?.MergedDictionaries == null)
                return;

            var mergedDictionaries = app.Resources.MergedDictionaries;
            mergedDictionaries.Clear();

            if (app.UserAppTheme == AppTheme.Dark)
            {
                mergedDictionaries.Add(new Resources.Themes.LightTheme());
                app.UserAppTheme = AppTheme.Light;
                Preferences.Default.Set("AppTheme", "Light");
            }
            else
            {
                mergedDictionaries.Add(new Resources.Themes.DarkTheme());
                app.UserAppTheme = AppTheme.Dark;
                Preferences.Default.Set("AppTheme", "Dark");
            }
            OnPropertyChanged(nameof(ThemeSwitchIcon));
        }
        public async Task ShowNotificationAsync(string title, string message, NotificationType type = NotificationType.Info, ToastDuration duration = ToastDuration.Short)
        {
            string prefix = type switch
            {
                NotificationType.Info => "ℹ️ ",
                NotificationType.Warning => "⚠️ ",
                NotificationType.Error => "❌ ",
                _ => ""
            };

            var toastMessage = string.IsNullOrWhiteSpace(title) ? $"{prefix}{message}" : $"{prefix}{title}: {message}";

            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Application.Current?.MainPage?.ShowPopup(
                    new Components.ToastPopup(
                        toastMessage,
                        type,
                        duration == ToastDuration.Long ? 4 : 1
                    )
                );
            }
            else
            {
                var toast = Toast.Make(toastMessage, duration);
                await toast.Show();
            }
        }
        public enum NotificationType
        {
            Info,
            Warning,
            Error
        }
    }
}