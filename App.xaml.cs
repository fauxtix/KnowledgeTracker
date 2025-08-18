namespace KnowledgeTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = ServiceHelper.GetService<MainPage>() ?? new MainPage(null);
        }

    }

    public static class ServiceHelper
    {
        public static IServiceProvider? Services =>
            IPlatformApplication.Current?.Services;

        public static T? GetService<T>() where T : class =>
            Services?.GetService(typeof(T)) as T;
    }
}