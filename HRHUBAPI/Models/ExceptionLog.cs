using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class ExceptionLog
{
    public int ExceptionId { get; set; }

    public int? UserId { get; set; }

    public DateTime? ExceptionDateTime { get; set; }

    public string? ErrorMessage { get; set; }

    public string? StackTrace { get; set; }

    public string? InnerException { get; set; }
}
