using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentMVC.Models;

namespace StudentMVC.Controllers
{
    public class MarksController : Controller
    {
        private readonly SchoolDbContext _context;

        public MarksController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Marks
        public async Task<IActionResult> Index(int pg = 1)
        {
            var schoolDbContext = _context.Marks.Include(m => m.Class).Include(m => m.Student).Where(a => a.Deleted == false || a.Deleted == null);
            const int pageSize = 8;
            if (pg < 1)
                pg = 1;
            int recsCount = schoolDbContext.Count();
            var pager = new Pager(recsCount,pg,pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = schoolDbContext.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
            //return View(await schoolDbContext.ToListAsync());
        }

        // GET: Marks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Marks == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .Include(m => m.Class)
                .Include(m => m.Student)
                .FirstOrDefaultAsync(m => m.MarksId == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // GET: Marks/Create
        public IActionResult Create()
        {
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassId");
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId");
            return View();
        }

        // POST: Marks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MarksId,Marks,StudentId,ClassId")] Mark mark)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassId", mark.ClassId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", mark.StudentId);
            return View(mark);
        }

        // GET: Marks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Marks == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks.FindAsync(id);
            if (mark == null)
            {
                return NotFound();
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassId", mark.ClassId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", mark.StudentId);
            return View(mark);
        }

        // POST: Marks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MarksId,Marks,StudentId,ClassId")] Mark mark)
        {
            if (id != mark.MarksId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarkExists(mark.MarksId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassId", mark.ClassId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", mark.StudentId);
            return View(mark);
        }

        // GET: Marks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Marks == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .Include(m => m.Class)
                .Include(m => m.Student)
                .FirstOrDefaultAsync(m => m.MarksId == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // POST: Marks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Marks == null)
            {
                return Problem("Entity set 'SchoolDbContext.Marks'  is null.");
            }
            var mark = await _context.Marks.FindAsync(id);
            if (mark != null)
            {
                _context.Marks.Remove(mark);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarkExists(int id)
        {
            return (_context.Marks?.Any(e => e.MarksId == id)).GetValueOrDefault();
        }
    }
}
