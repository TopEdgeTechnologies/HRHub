using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class CandidateEmailNotificationSetting
    {
        [NotMapped]
        public string? EmailTitle { get; set; }
        [NotMapped]
        public string? EmailSubject { get; set; }
        [NotMapped]
        public string? EmailBody { get; set; }
    }
}
