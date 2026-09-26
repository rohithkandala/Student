using System;
using System.Collections.Generic;

namespace StudentMVC.Models;

public partial class Mark
{
    public int MarksId { get; set; }

    public int Marks { get; set; }

    public int? StudentId { get; set; }

    public int? ClassId { get; set; }

    public bool? Deleted { get; set; }

    public virtual Class Class { get; set; }

    public virtual Student Student { get; set; }
}
