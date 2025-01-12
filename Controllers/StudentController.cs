using Microsoft.AspNetCore.Mvc;
using efcoreApp.Data;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class StudentController: Controller
    {
        private readonly DataContext _context;
        public StudentController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Create()
        {
            return View();
        }
 
        [HttpPost]
        public async Task<IActionResult> Create(Student model)
        {
            _context.Students.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index(string searchString){
            var studentQuery = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.SearchString = searchString;
                studentQuery = studentQuery.Where(p => p.StudentName!.ToLower().Contains(searchString.ToLower()));
            }
            var students = await studentQuery.ToListAsync();
            return View(students);
        }

        

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            //var std = await _context.Students.FindAsync(id);
            var std = await _context.Students.Include(x => x.CourseRegistrations).ThenInclude(o => o.Course).FirstOrDefaultAsync(o => o.StudentId == id);

            if(std == null)
            {
                return NotFound();
            }
            return View(std);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Security 
        public async Task<IActionResult> Edit(int id, Student model)
        {
            if(id != model.StudentId)
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
                    if(!_context.Students.Any(o => o.StudentId == model.StudentId))
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


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);

            if(student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id)
        {
            var student = await _context.Students.FindAsync(id);
            if(student == null)
            {
                return NotFound();
            }
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
    }
}