using KnowledgeTracker.Data.Interfaces;
using KnowledgeTracker.Models;
using SQLite;

namespace KnowledgeTracker.Data.Implementations
{
    public class KnowledgeRepositorySQLite : IKnowledgeRepository
    {
        private readonly SQLiteAsyncConnection _db;

        //public KnowledgeRepositorySQLite(string dbPath)
        //{
        //    _db = new SQLiteAsyncConnection(dbPath);
        //    _db.CreateTableAsync<KnowledgeEntry>().Wait();
        //}
        public KnowledgeRepositorySQLite(SQLiteAsyncConnection db)
        {
            _db = db;
            // Inicialize as tabelas se necessário
            _db.CreateTableAsync<KnowledgeEntry>().Wait();
        }
        public async Task<IEnumerable<KnowledgeEntry>> GetAllAsync()
        {
            var data = await _db.Table<KnowledgeEntry>().ToListAsync();
            return data;
        }

        public async Task<KnowledgeEntry?> GetByIdAsync(int id)
        {
            return await _db.FindAsync<KnowledgeEntry>(id);
        }

        public async Task<int> InsertAsync(KnowledgeEntry entry)
        {
            return await _db.InsertAsync(entry);
        }

        public async Task UpdateAsync(KnowledgeEntry entry)
        {
            await _db.UpdateAsync(entry);
        }

        public async Task DeleteAsync(int id)
        {
            var entry = await GetByIdAsync(id);
            if (entry != null)
                await _db.DeleteAsync(entry);
        }

        public async Task<IEnumerable<KnowledgeEntry>> SearchAsync(string searchTerm)
        {
            return await _db.Table<KnowledgeEntry>()
                .Where(e =>
                    e.Title.Contains(searchTerm) ||
                    e.Description.Contains(searchTerm) ||
                    e.ProjectName.Contains(searchTerm) ||
                    e.Tags.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
