using System;
using System.Collections.Generic;

namespace StudentMVC.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string StudentName { get; set; }

    public bool? Deleted { get; set; }

    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
}
