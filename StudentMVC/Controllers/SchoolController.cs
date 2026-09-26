using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentMVC.Controllers
{
    public class SchoolController : Controller
    {
        private readonly SchoolDbContext context;
        public SchoolController(SchoolDbContext context)
        {
            this.context = context;
        }

        public IActionResult ScoreCard(string studentName)
        {
            IEnumerable<Scorecard> obj = context.Scorecards.FromSqlRaw("EXEC usp_Scorecard {0}", studentName).ToList();
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                return View(obj);
            }

        }
        public IActionResult TeacherClass(string TeacherName)
        {
            ViewBag.IsFirstTime = true;

            var teacherclass = context.TeacherClasses.FromSqlRaw("SELECT * FROM ufn_TeacherClass({0})", TeacherName).ToList();
            return View(teacherclass);
        }

        public IActionResult NoOfStudentClass(string ClassName)
        {
            //className = "Angular";
            var obj = context.NoOfStudentClasses
                 .FromSqlRaw("SELECT dbo.ufn_NoOfStudentClass({0}) AS NoOfStudentsInClass", ClassName)
                 .FirstOrDefault();

            ViewData["ClassName"] = new SelectList(context.NoOfStudentClasses, "ClassName", "ClassName", ClassName);
            return View(obj);
        }

        public IActionResult EnrolledStudentClass(string studentName)
        {
            //studentId = 104;
            ViewData["studentName"] = studentName;
            var obj = context.enrolledStudentClasses.FromSqlRaw("SELECT dbo.ufn_EnrolledStudentClass({0}) AS NoOfClassesEnrolledByStudent", studentName).FirstOrDefault();
            return View(obj);
        }

        public IActionResult PassFailCount()
        {
            var obj = context.passFailCounts.FromSqlRaw("SELECT * FROM dbo.PassFailCountByClass()").ToList();
            return View(obj);
        }

        public IActionResult DeletedStudent(string studentName)
        {

            ViewBag.IsFirstTime = true;
            var obj = context.DeletedStudents.FromSqlRaw("SELECT * FROM DeletedStudent({0});", studentName).ToList();
                return View(obj);
        

        }
    }
}
