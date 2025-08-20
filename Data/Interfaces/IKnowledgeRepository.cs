using KnowledgeTracker.Models;

namespace KnowledgeTracker.Data.Interfaces
{
    public interface IKnowledgeRepository
    {
        Task<IEnumerable<KnowledgeEntry>> GetAllAsync();
        Task<KnowledgeEntry?> GetByIdAsync(int id);
        Task<int> InsertAsync(KnowledgeEntry entry);
        Task UpdateAsync(KnowledgeEntry entry);
        Task DeleteAsync(int id);
        Task<IEnumerable<KnowledgeEntry>> SearchAsync(string searchTerm);
        Task RemoveAttachmentAsync(int attachmentId);
        Task<List<AttachmentInfo>> GetAttachmentsAsync(int entryId);
        Task AddAttachmentAsync(int entryId, AttachmentInfo attachment);
    }
}
