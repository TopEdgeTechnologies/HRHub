using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class UserForms2
{
    public int Formid { get; set; }

    public string? FormTitle { get; set; }

    public string? Controller { get; set; }

    public string? Action { get; set; }

    public string? ImageClass { get; set; }

    public bool? Status { get; set; }

    public bool? IsParent { get; set; }

    public int? ParentId { get; set; }

    public int? DOrder { get; set; }

    public bool? IsMenu { get; set; }

    public int? ReferenceId { get; set; }
}
