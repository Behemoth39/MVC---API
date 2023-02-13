using Microsoft.EntityFrameworkCore;
using westcoasteducation.api.Models;

namespace westcoasteducation.api.Data;

public class WestCoastEducationContext : DbContext
{
    public DbSet<CourseModel> Courses => Set<CourseModel>();
    public DbSet<StudentModel> Students => Set<StudentModel>();
    public DbSet<TeacherModel> Teachers => Set<TeacherModel>();
    public DbSet<QualificationModel> Qualifications => Set<QualificationModel>();
    public WestCoastEducationContext(DbContextOptions options) : base(options)
    {
    }
}
