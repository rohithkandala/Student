using System;
using System.Collections.Generic;

namespace StudentMVC.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string ClassName { get; set; }

    public int? TeacherId { get; set; }

    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

    public virtual Teacher Teacher { get; set; }
}
