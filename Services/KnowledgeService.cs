using KnowledgeTracker.Data.Interfaces;
using KnowledgeTracker.Models;

namespace KnowledgeTracker.Services
{
    public class KnowledgeEntryService : IKnowledgeEntryService
    {
        private readonly IKnowledgeRepository _repository;

        public KnowledgeEntryService(IKnowledgeRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<KnowledgeEntry>> GetAllAsync() => _repository.GetAllAsync();

        public Task<KnowledgeEntry?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<int> InsertAsync(KnowledgeEntry entry) => _repository.InsertAsync(entry);

        public Task UpdateAsync(KnowledgeEntry entry) => _repository.UpdateAsync(entry);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);

        public async Task<IEnumerable<KnowledgeEntry>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await _repository.GetAllAsync();

            return await _repository.SearchAsync(searchTerm);
        }

        public async Task RemoveAttachmentAsync(int attachmentId)
        {
            await _repository.RemoveAttachmentAsync(attachmentId);
        }

        public async Task<List<AttachmentInfo>> GetAttachmentsAsync(int entryId)
        {
            return await _repository.GetAttachmentsAsync(entryId);
        }

        public async Task AddAttachmentAsync(int entryId, AttachmentInfo attachment)
        {
            await _repository.AddAttachmentAsync(entryId, attachment);
        }
    }
}
