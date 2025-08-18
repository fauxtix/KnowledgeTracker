using KnowledgeTracker.ViewModels;

namespace KnowledgeTracker;
public partial class MainPage : ContentPage
{
    public MainPage(KnowledgeEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        viewModel.ShowMessage = async (title, message) =>
        {
            await DisplayAlert(title, message, "OK");
        };
    }

}