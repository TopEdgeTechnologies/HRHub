using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class SectionQuestion
    {
        [NotMapped]
        public string? QuestionName { get; set; } 
        [NotMapped]
        public string? SectionName { get; set; } 
        [NotMapped]
        public string? SectionDescription{ get; set; }
        [NotMapped]
        public string? ReviewName { get; set; } 
        [NotMapped]
        public string? ReviewDescription{ get; set; }
        [NotMapped]
        public bool? IsAnswerWeightage { get; set; } 
        [NotMapped]
        public bool? AllowSelfScoring { get; set; }
        [NotMapped]
        public int? QuestionMaxLimit { get; set; }
    }
}
