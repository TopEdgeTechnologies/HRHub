﻿using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StatusInfo
{
    public int StatusId { get; set; }

    public string? Title { get; set; }

    public bool? Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? BackGroundClass { get; set; }
}
