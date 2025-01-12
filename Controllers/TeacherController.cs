using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace efcoreApp.Controllers
{
    public class TeacherController : Controller
    {
        private readonly DataContext _context;
        public TeacherController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString){
            var teacherQuery = _context.Teachers.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.SearchString = searchString;
                teacherQuery = teacherQuery.Where(p => p.TeacherName!.ToLower().Contains(searchString.ToLower()));
            }
            var teachers = await teacherQuery.ToListAsync();
            return View(teachers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Teacher model)
        {
            _context.Teachers.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            //var std = await _context.Students.FindAsync(id);
            var teachers = await _context.Teachers.FirstOrDefaultAsync(o => o.TeacherId == id);

            if(teachers == null)
            {
                return NotFound();
            }
            return View(teachers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Security 
        public async Task<IActionResult> Edit(int id, Teacher model)
        {
            if(id != model.TeacherId)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try 
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!_context.Students.Any(o => o.StudentId == model.TeacherId))
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
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var teacher = await _context.Teachers.FindAsync(id);

            if(teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if(teacher == null)
            {
                return NotFound();
            }
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }

}