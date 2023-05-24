using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class LoanApplication
{
    public int? LoanApplicationId { get; set; }

    public int? StaffId { get; set; }

    public DateTime? ApplicationDate { get; set; }

    public decimal? Amount { get; set; }

    public string? DisbursmentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Reason { get; set; }
}
