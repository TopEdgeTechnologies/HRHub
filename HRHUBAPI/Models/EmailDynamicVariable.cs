using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class EmailDynamicVariable
{
    public int VariableId { get; set; }

    public string? VariableName { get; set; }

    public string? Type { get; set; }
}
