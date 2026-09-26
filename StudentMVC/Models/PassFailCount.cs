using System.ComponentModel.DataAnnotations;

namespace StudentMVC.Models
{
    public class PassFailCount
    {
        [Key]
        public string ClassName { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
    }
}
