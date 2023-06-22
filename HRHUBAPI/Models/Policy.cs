﻿using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class Policy
{
    public int PolicyId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? PolicyCategoryId { get; set; }

    public bool? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
