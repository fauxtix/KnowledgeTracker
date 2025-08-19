using CommunityToolkit.Maui.Views;
using KnowledgeTracker.ViewModels;

namespace KnowledgeTracker.Components
{
    public partial class ToastPopup : Popup
    {
        public ToastPopup(string message, KnowledgeEntryViewModel.NotificationType type = KnowledgeEntryViewModel.NotificationType.Info, int seconds = 3)
        {
            InitializeComponent();
            MessageLabel.Text = message;
            SetFrameColor(type);

            // Use Dispatcher to start timer instead of Device.StartTimer
            Dispatcher.StartTimer(TimeSpan.FromSeconds(seconds), () =>
            {
                Close();
                return false;
            });
        }

        private void SetFrameColor(KnowledgeEntryViewModel.NotificationType type)
        {
            Color bgColor = type switch
            {
                KnowledgeEntryViewModel.NotificationType.Info => Colors.DarkSlateBlue,
                KnowledgeEntryViewModel.NotificationType.Warning => Colors.DarkOrange,
                KnowledgeEntryViewModel.NotificationType.Error => Colors.DarkRed,
                _ => Colors.Black
            };
            ToastFrame.BackgroundColor = bgColor;
        }
    }
}