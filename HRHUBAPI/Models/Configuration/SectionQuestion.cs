using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class SectionQuestion
    {
        [NotMapped]
        public string? QuestionName { get; set; }
    }
}
