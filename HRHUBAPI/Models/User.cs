﻿using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? UserGroupId { get; set; }

    public int? StaffId { get; set; }

    public int? StudentId { get; set; }

    public bool IsActive { get; set; }

    public int? CreateBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? InstituteId { get; set; }

    public int? SchoolId { get; set; }

    public int? GroupId { get; set; }
}
