using efcoreApp.Data;
using efcoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace efcoreApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly DataContext _context;
        public CourseController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "TeacherId", "NameSurname");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(new Course() { CourseId = model.CourseId, CourseName = model.CourseName, TeacherId = model.TeacherId });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "TeacherId", "NameSurname");
            return View(model);
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var coursesQuery = _context.Courses.Include(k => k.Teacher).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.SearchString = searchString;
                coursesQuery = coursesQuery.Where(p => p.CourseName!.ToLower().Contains(searchString.ToLower()));
            }

            var courses = await coursesQuery.ToListAsync(); // Execute the query with the filter applied.

            return View(courses);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crs = await _context.Courses.Include(k => k.CourseRegistrations).ThenInclude(x => x.Student).Select(k => new CourseViewModel { CourseId = k.CourseId, CourseName = k.CourseName, TeacherId = k.TeacherId, CourseRegistrations = k.CourseRegistrations }).FirstOrDefaultAsync(k => k.CourseId == id);

            if (crs == null)
            {
                return NotFound();
            }
            ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "TeacherId", "NameSurname");
            return View(crs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseViewModel course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(new Course() { CourseId = course.CourseId, CourseName = course.CourseName, TeacherId = course.TeacherId });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!_context.Courses.Any(o => o.CourseId == course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "TeacherId", "NameSurname");
            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crs = await _context.Courses.FindAsync(id);

            if (crs == null)
            {
                return NotFound();
            }
            return View(crs);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var crs = await _context.Courses.FindAsync(id);
            if (crs == null)
            {
                return NotFound();
            }
            _context.Courses.Remove(crs);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}