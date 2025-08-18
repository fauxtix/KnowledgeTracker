using KnowledgeTracker.Data.Implementations;
using KnowledgeTracker.Data.Interfaces;
using KnowledgeTracker.Services;
using KnowledgeTracker.ViewModels;
using SQLite;
using SQLitePCL;

namespace KnowledgeTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            Batteries_V2.Init();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("CascadiaCode-Regular.ttf", "CodeFont");
                });



            // Caminho para base de dados SQLite local
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "knowledge.db3");

            // Alternar aqui se quiser voltar ao SQL Server
            //builder.Services.AddSingleton<IKnowledgeRepository>(sp => new KnowledgeRepositorySQLite(dbPath));

            var sqliteConn = new SQLiteAsyncConnection(dbPath);
            builder.Services.AddSingleton(sqliteConn);

            // Injeta o repositório usando a conexão
            builder.Services.AddSingleton<IKnowledgeRepository>(sp =>
                new KnowledgeRepositorySQLite(sp.GetRequiredService<SQLiteAsyncConnection>()));

            // Para usar SQL Server, descomente as linhas abaixo e comente a linha acima
            //string connectionString = "Server=DESKTOP-SLIMNUD\\SQLEXPRESS;Database=KnowledgeBase;Trusted_Connection=True;TrustServerCertificate=True;";
            //builder.Services.AddSingleton<IKnowledgeRepository>(sp => new KnowledgeRepository(connectionString));

            builder.Services.AddSingleton<IKnowledgeEntryService, KnowledgeEntryService>();
            builder.Services.AddTransient<KnowledgeEntryViewModel>();
            builder.Services.AddTransient<MainPage>();

            return builder.Build();
        }
    }
}
