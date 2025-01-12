using System.ComponentModel.DataAnnotations;

namespace efcoreApp.Data
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }
        
        public string? TeacherName { get; set; }

        public string? TeacherSurname { get; set; }

        public string NameSurname { 
            get
            {
                return this.TeacherName + " " + this.TeacherSurname;
            }
         }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode =false)]
        public DateTime StartDate { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}