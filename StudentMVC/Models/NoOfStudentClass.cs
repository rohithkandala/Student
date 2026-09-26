using System.ComponentModel.DataAnnotations;

namespace StudentMVC.Models
{
    public class NoOfStudentClass
    {
        [Key]
        public int NoOfStudentsInClass { get; set; }

    }
}
