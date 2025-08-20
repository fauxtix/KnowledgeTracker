using SQLite;

namespace KnowledgeTracker.Models
{
    public class AttachmentInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int KnowledgeEntryId { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
    }
}