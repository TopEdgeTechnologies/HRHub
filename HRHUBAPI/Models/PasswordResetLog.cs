using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class PasswordResetLog
{
    public int PasswordResetId { get; set; }

    public int? UserId { get; set; }

    public DateTime? RequestOn { get; set; }

    public string? RequestFromIp { get; set; }

    public DateTime? ExpiryTime { get; set; }

    public bool? ResetStatus { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedFromIp { get; set; }

    public string? Token { get; set; }
}
