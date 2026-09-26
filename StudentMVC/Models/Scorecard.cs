using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentMVC.Models
{
    public class Scorecard
    {
        [Key]
        public string StudentName { get; set; }
        [DefaultValue(0)]
        public int Java { get; set; }
        [DefaultValue(0)]
        public int DBMS { get; set; }
        [DefaultValue(0)]
        public int MSSQL { get; set; }
        [DefaultValue(0)]
        public int CSharp { get; set; }
        [DefaultValue(0)]
        public int Angular { get; set; }
        [DefaultValue(0)]
        public int React { get; set; }
        [DefaultValue(0)]
        public int Python { get; set; }
        public string Grade { get; set; }



    }
}
