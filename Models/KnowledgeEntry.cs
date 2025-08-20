using SQLite;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeTracker.Models
{
    public class KnowledgeEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [MinLength(10, ErrorMessage = "O título deve ter pelo menos 10 caracteres.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [MinLength(30, ErrorMessage = "A descrição deve ter pelo menos 30 caracteres.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data da resolução é obrigatória.")]
        [CustomValidation(typeof(KnowledgeEntry), nameof(ValidateDateResolved))]
        public DateTime DateResolved { get; set; }

        [Required(ErrorMessage = "O nome do projeto é obrigatório.")]
        public string ProjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Os passos para resolução são obrigatórios.")]
        public string ResolutionSteps { get; set; } = string.Empty;

        // Outros campos opcionais
        public string Technologies { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;

        public string? YouTubeUrl { get; set; }

        [Ignore]
        public List<AttachmentInfo> Attachments { get; set; } = new();
        public static ValidationResult? ValidateDateResolved(DateTime date, ValidationContext context)
        {
            if (date.Year < 1990)
                return new ValidationResult("A data da resolução não pode ser anterior a 1990.");

            if (date > DateTime.Today)
                return new ValidationResult("A data da resolução não pode ser futura.");

            return ValidationResult.Success;
        }
    }
}
