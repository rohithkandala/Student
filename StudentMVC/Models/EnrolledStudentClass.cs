using System.ComponentModel.DataAnnotations;

namespace StudentMVC.Models
{
    public class EnrolledStudentClass
    {
        [Key]
        public int NoOfClassesEnrolledByStudent { get; set; }
    }
}
