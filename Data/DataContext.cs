using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            
        }
        public DbSet<Course> Courses => Set<Course>();

        public DbSet<Student> Students => Set<Student>();

        public DbSet<CourseRegistration> CourseRegistrations => Set<CourseRegistration>();

        public DbSet<Teacher> Teachers => Set<Teacher>();


        // code-first => entity, dbcontext => database (postgresql)
        // database-first => sql server
    }

}