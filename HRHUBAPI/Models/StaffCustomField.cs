using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffCustomField
{
    public int StaffCustomFieldId { get; set; }

    public int? CompanyId { get; set; }

    public string? FieldName { get; set; }

    public string? DataType { get; set; }

    public string? PlaceholderHelpText { get; set; }

    public string? DefaultValue { get; set; }

    public bool? IsMandatory { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
