using System.ComponentModel.DataAnnotations;

namespace efcoreApp.Data
{
    public class Course
    {
        public int CourseId { get; set; }

        public string? CourseName { get; set; }

        public ICollection<CourseRegistration> CourseRegistrations { get; set; } = new List<CourseRegistration>();

        public Teacher Teacher { get; set; } = null!;

        public int TeacherId { get; set; }
    }
}