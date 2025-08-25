using System.Text.Json;

namespace KnowledgeTracker.Data.Helpers
{
    public class ChangeTracker<T> where T : class
    {
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        private string _originalSnapshot;
        public T Instance { get; }

        public ChangeTracker(T instance)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            _originalSnapshot = Serialize(instance);
        }

        private string Serialize(T obj) =>
            JsonSerializer.Serialize(obj, _options);

        public bool IsDirty =>
            _originalSnapshot != Serialize(Instance);

        public void AcceptChanges() =>
            _originalSnapshot = Serialize(Instance);
    }
}
