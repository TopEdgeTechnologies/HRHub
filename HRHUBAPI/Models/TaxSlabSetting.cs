using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class TaxSlabSetting
{
    public int SlabId { get; set; }

    public int? CompanyId { get; set; }

    public string? Title { get; set; }

    public decimal? MinIncome { get; set; }

    public decimal? MaxIncome { get; set; }

    public decimal? TaxRate { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
