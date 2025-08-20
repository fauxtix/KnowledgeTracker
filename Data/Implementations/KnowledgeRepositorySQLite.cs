using KnowledgeTracker.Data.Interfaces;
using KnowledgeTracker.Models;
using SQLite;

namespace KnowledgeTracker.Data.Implementations
{
    public class KnowledgeRepositorySQLite : IKnowledgeRepository
    {
        private readonly SQLiteAsyncConnection _db;

        public KnowledgeRepositorySQLite(SQLiteAsyncConnection db)
        {
            _db = db;
            _db.CreateTableAsync<KnowledgeEntry>().Wait();
            _db.CreateTableAsync<AttachmentInfo>().Wait();
        }
        public async Task<IEnumerable<KnowledgeEntry>> GetAllAsync()
        {
            var entries = await _db.Table<KnowledgeEntry>().ToListAsync();
            foreach (var entry in entries)
                entry.Attachments = await GetAttachmentsAsync(entry.Id);
            return entries;
        }

        public async Task<KnowledgeEntry?> GetByIdAsync(int id)
        {
            var entry = await _db.FindAsync<KnowledgeEntry>(id);
            if (entry != null)
                entry.Attachments = await GetAttachmentsAsync(id);
            return entry;
        }

        public async Task<int> InsertAsync(KnowledgeEntry entry)
        {
            await _db.InsertAsync(entry);
            return entry.Id;
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

        public async Task AddAttachmentAsync(int entryId, AttachmentInfo attachment)
        {
            attachment.KnowledgeEntryId = entryId;
            await _db.InsertAsync(attachment);
        }

        public async Task<List<AttachmentInfo>> GetAttachmentsAsync(int entryId)
        {
            return await _db.Table<AttachmentInfo>().Where(a => a.KnowledgeEntryId == entryId).ToListAsync();
        }

        public async Task RemoveAttachmentAsync(int attachmentId)
        {
            await _db.DeleteAsync<AttachmentInfo>(attachmentId);
        }



    }
}
