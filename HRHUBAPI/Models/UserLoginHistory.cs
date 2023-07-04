using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class UserLoginHistory
{
    public int UserLoginHistoryId { get; set; }

    public int? UserId { get; set; }

    public DateTime? SessionFrom { get; set; }

    public DateTime? SessionTo { get; set; }

    public int? CreateBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
