using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class LoanRepayment
{
    public int LoanRepaymentId { get; set; }

    public int? LoanApplicationId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public int? StaffSalaryId { get; set; }
}
