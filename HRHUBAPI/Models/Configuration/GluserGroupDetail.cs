using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class GluserGroupDetail
    {
        [NotMapped]
        public string? FormTitle { get; set; }

		[NotMapped]
		public int? level { get; set; }
		[NotMapped]

		public string? form_path { get; set; }

	}
}
