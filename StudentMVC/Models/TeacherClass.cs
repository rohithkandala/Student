using System.ComponentModel.DataAnnotations;

namespace StudentMVC.Models
{
    public class TeacherClass
    {
        [Key]
        public string ClassName { get; set; }
        public string TeacherName { get; set; }
    }
}
