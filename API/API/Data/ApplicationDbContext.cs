using API.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
namespace API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //Fluent API methods.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseEducationProgram>()
                .HasKey(courseEdu => new { courseEdu.CourseId, courseEdu.EducationalProgramId });
            modelBuilder.Entity<UserCourseClassroom>()
                .HasKey(userCourseClass => new { userCourseClass.UserId, userCourseClass.CourseClassroomId });
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<EducationalProgram> EducationalProgram { get; set; }
        public DbSet<CourseEducationProgram> CourseEducationProgram { get; set; }
        public DbSet<CourseClassroom> CourseClassroom { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<UserCourseClassroom> UserCourseClassroom { get; set; }
        public DbSet<Classroom> Classroom { get; set; }
        public DbSet<Faculty>  Faculty { get; set; }
        public DbSet<Score> Score { get; set; }
    }
}
