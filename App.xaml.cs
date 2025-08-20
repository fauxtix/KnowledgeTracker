namespace KnowledgeTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var theme = Preferences.Default.Get("AppTheme", "Light");
            if (theme == "Dark")
            {
                Resources.MergedDictionaries.Clear();
                Resources.MergedDictionaries.Add(new Resources.Themes.DarkTheme());
                UserAppTheme = AppTheme.Dark;
            }
            else
            {
                Resources.MergedDictionaries.Clear();
                Resources.MergedDictionaries.Add(new Resources.Themes.LightTheme());
                UserAppTheme = AppTheme.Light;
            }

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