using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class LoanApplication
{
    public int LoanApplicationId { get; set; }

    public int? StaffId { get; set; }

    public int? LoanTypeId { get; set; }

    public DateTime? ApplicationDate { get; set; }

    public string? Reason { get; set; }

    public DateTime? PaymentDate { get; set; }

    public decimal? Amount { get; set; }

    public int? NoOfInstallments { get; set; }

    public decimal? InstallmentPerMonth { get; set; }
}
