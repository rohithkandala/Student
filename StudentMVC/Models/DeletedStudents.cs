using System.ComponentModel.DataAnnotations;

namespace StudentMVC.Models
{
    public class DeletedStudents
    {
        [Key]
        public string StudentName { get; set;}
        public int Marks { get; set; }
        public string ClassName { get; set; }
    }
}
