using System.ComponentModel.DataAnnotations;
using efcoreApp.Data;

namespace efcoreApp.Models
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }

        [Required]
        [StringLength(50)]
        public string? CourseName { get; set; }

        public int TeacherId { get; set; }

        public ICollection<CourseRegistration> CourseRegistrations { get; set; } = new List<CourseRegistration>();

    }
}