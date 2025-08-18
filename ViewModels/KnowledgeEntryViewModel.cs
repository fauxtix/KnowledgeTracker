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

        // Change the declaration of selectedEntry to nullable to fix CS8618
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
                    }
                    else
                    {
                        ClearFields();
                    }
                    // Always hide the WebView when a new record is selected
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
                YouTubeUrl = SelectedEntry?.YouTubeUrl
            };
        }

        [RelayCommand]
        public async Task LoadAllAsync()
        {
            var list = await _service.GetAllAsync();
            Entries = new ObservableCollection<KnowledgeEntry>(list);
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

            SelectedEntry = new KnowledgeEntry();
            ClearFields();
            ShowMessage?.Invoke("Sucesso", "Entrada criada com sucesso!");
        }

        [RelayCommand]
        public async Task UpdateAsync()
        {
            if (Id == 0) return;

            if (!ValidateSelectedEntry())
                return;

            var updatedEntry = CreateEntryFromFields();
            await _service.UpdateAsync(updatedEntry);
            await LoadAllAsync();

            SelectedEntry = updatedEntry;

            ShowMessage?.Invoke("Sucesso", "Entrada atualizada com sucesso!");
        }

        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (Id == 0) return;
            await _service.DeleteAsync(Id);
            if (SelectedEntry != null)
                Entries.Remove(SelectedEntry);

            SelectedEntry = new KnowledgeEntry();

            ShowMessage?.Invoke("Sucesso", "Entrada removida com sucesso!");
        }

        [RelayCommand]
        public void NewEntry()
        {
            SelectedEntry = new KnowledgeEntry();
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

        [RelayCommand]
        public void ShowYouTubeVideo()
        {
            // Usa o campo do modelo
            var url = SelectedEntry?.YouTubeUrl;
            var videoId = ExtractYouTubeId(url);

            if (string.IsNullOrWhiteSpace(url))
            {
                ShowMessage?.Invoke("Aviso", "URL deve ser informada.");
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
                    ShowMessage?.Invoke("Aviso", "URL inválida do YouTube.");
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
                ShowMessage?.Invoke("Aviso", "Url deve ser informada.");
                return;
            }

            if (string.IsNullOrEmpty(videoId))
            {
                ShowMessage?.Invoke("Aviso", "URL inválida do YouTube.");
                return;
            }

            try
            {
                await Launcher.OpenAsync(url);
            }
            catch (Exception)
            {
                ShowMessage?.Invoke("Erro", "Não foi possível abrir o link.");
            }
        }

        //[RelayCommand]
        //public void ToggleTheme()
        //{
        //    var app = Application.Current;
        //    if (app?.Resources?.MergedDictionaries == null)
        //        return;

        //    var mergedDictionaries = app.Resources.MergedDictionaries;

        //    // Remove any currently merged theme dictionary (Light or Dark)
        //    var themeDicts = mergedDictionaries
        //        .Where(d => d.GetType().Name.Contains("Theme"))
        //        .ToList();
        //    foreach (var dict in themeDicts)
        //        mergedDictionaries.Remove(dict);

        //    // Add the new theme dictionary
        //    if (app.UserAppTheme == AppTheme.Dark)
        //    {
        //        mergedDictionaries.Add(new KnowledgeTracker.Resources.Themes.LightTheme());
        //        app.UserAppTheme = AppTheme.Light;
        //    }
        //    else
        //    {
        //        mergedDictionaries.Add(new KnowledgeTracker.Resources.Themes.DarkTheme());
        //        app.UserAppTheme = AppTheme.Dark;
        //    }
        //}

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
            }
            else
            {
                mergedDictionaries.Add(new Resources.Themes.DarkTheme());
                app.UserAppTheme = AppTheme.Dark;
            }
            OnPropertyChanged(nameof(ThemeSwitchIcon));
        }
    }
}