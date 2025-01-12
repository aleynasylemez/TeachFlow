using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class CourseRegistrationController : Controller
    {
        private readonly DataContext _context;
        public CourseRegistrationController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courseRegistrations = await _context.CourseRegistrations.Include(x => x.Student).Include(x => x.Course).ToListAsync();
            return View(courseRegistrations);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Students = new SelectList(await _context.Students.ToListAsync(), "StudentId", "NameSurname");
            ViewBag.Courses = new SelectList(await _context.Courses.ToListAsync(), "CourseId", "CourseName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseRegistration model)
        {
            model.RegistrationDate = DateTime.Now;
            _context.CourseRegistrations.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crs = await _context.CourseRegistrations.FirstOrDefaultAsync(m => m.RegistrationId == id); ;

            if (crs == null)
            {
                return NotFound();
            }
            return View(crs);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var crs = await _context.CourseRegistrations.FindAsync(id);
            if (crs == null)
            {
                return NotFound();
            }
            _context.CourseRegistrations.Remove(crs);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // HttpGet: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the CourseRegistration with the related Course and Student
            var registration = await _context.CourseRegistrations
                .Include(o => o.Course)
                .Include(o => o.Student)
                .FirstOrDefaultAsync(o => o.RegistrationId == id);

            if (registration == null)
            {
                return NotFound();
            }

            // Populate dropdowns for Students and Courses
            ViewBag.Students = new SelectList(_context.Students, "StudentId", "NameSurname", registration.StudentId);
            ViewBag.Courses = new SelectList(_context.Courses, "CourseId", "CourseName", registration.CourseId);

            return View(registration);
        }

        // HttpPost: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseRegistration model)
        {
            if (id != model.RegistrationId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(new CourseRegistration() { StudentId = model.StudentId, CourseId = model.CourseId });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.CourseRegistrations.Any(o => o.RegistrationId == model.RegistrationId))
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
            ViewBag.Students = new SelectList(await _context.Students.ToListAsync(), "StudentId", "NameSurname");
            ViewBag.Courses = new SelectList(await _context.Courses.ToListAsync(), "CourseId", "CourseName");
            return View(model); 
        }

    }
}