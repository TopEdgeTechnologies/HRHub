using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class CandidateScreening
    {

        [NotMapped]
        public string? StatusTitle { get; set; }
        [NotMapped]
        public int? day { get; set; }
        [NotMapped]
        public string? Month { get; set; }
        [NotMapped]
        public string? CssColor { get; set; } 
        [NotMapped]
        public string? CandidateName { get; set; }



    }
}
