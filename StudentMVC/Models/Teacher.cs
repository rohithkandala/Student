using System;
using System.Collections.Generic;

namespace StudentMVC.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public string TeacherName { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
