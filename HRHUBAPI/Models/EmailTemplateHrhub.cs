using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class EmailTemplateHrhub
{
    public int TemplateId { get; set; }

    public string? Title { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }
}
