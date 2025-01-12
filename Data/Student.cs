using System.ComponentModel.DataAnnotations;
namespace efcoreApp.Data
{
    public class Student
    {
        //id => primary key
        [Key]
        public int StudentId { get; set; }

        public string? StudentName { get; set; }

        public string? StudentSurname { get; set; }

        public string NameSurname { 
            get
            {
                return this.StudentName + " " + this.StudentSurname;
            }
        }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public ICollection<CourseRegistration> CourseRegistrations { get; set; } = new List<CourseRegistration>();
    }
}
