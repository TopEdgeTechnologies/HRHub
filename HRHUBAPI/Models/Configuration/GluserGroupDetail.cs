using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class GluserGroupDetail
    {
        [NotMapped]
        public string? FormTitle { get; set; }
    }
}
