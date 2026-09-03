using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Model;
namespace StudentManagement.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentGrade> StudentGrades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentGrade>()
                .HasKey(sg => new { sg.StudentID, sg.SubjectID });

            modelBuilder.Entity<StudentGrade>()
                .Property(sg => sg.Mark)
                .HasPrecision(4, 2);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.ClassID);

            modelBuilder.Entity<StudentGrade>()
                .HasOne(sg => sg.Student)
                .WithMany()
                .HasForeignKey(sg => sg.StudentID);

            modelBuilder.Entity<StudentGrade>()
                .HasOne(sg => sg.Subject)
                .WithMany(sub => sub.StudentGrades)
                .HasForeignKey(sg => sg.SubjectID);

            // ===== Seed data: Classes =====
            modelBuilder.Entity<Class>().HasData(
                new Class { ClassID = 1, ClassCode = "cntt01", ClassName = "công nghệ thông tin 1" },
                new Class { ClassID = 2, ClassCode = "cntt02", ClassName = "công nghệ thông tin 2" },
                new Class { ClassID = 4, ClassCode = "cntt03", ClassName = "công nghệ thông tin 3" },
                new Class { ClassID = 5, ClassCode = "cntt04", ClassName = "công nghệ thông tin 4" },
                new Class { ClassID = 6, ClassCode = "cntt05", ClassName = "công nghệ thông tin 5" },
                new Class { ClassID = 7, ClassCode = "cntt06", ClassName = "công nghệ thông tin 6" }
            );

            // ===== Seed data: Students =====
            modelBuilder.Entity<Student>().HasData(
                new Student { StudentID = 1, StudentCode = "SV01", FullName = "Nguyễn Hữu Quang", Gender = true, BirthDate = new DateTime(2005, 2, 9), Email = "an.nv@uneti.com", ClassID = 1 },
                new Student { StudentID = 2, StudentCode = "SV02", FullName = "Trần Thị Bích", Gender = false, BirthDate = new DateTime(2003, 8, 20), Email = "bich.tt@uneti.com", ClassID = 4 },
                new Student { StudentID = 3, StudentCode = "SV03", FullName = "Lê Hoàng Cường", Gender = true, BirthDate = new DateTime(2002, 12, 10), Email = "cuong.lh@yahoo.com", ClassID = 2 },
                new Student { StudentID = 1004, StudentCode = "SV04", FullName = "Nguyen trong tai", Gender = true, BirthDate = new DateTime(2005, 1, 1), Email = "tai@gmail.com", ClassID = 4 },
                new Student { StudentID = 1005, StudentCode = "SV05", FullName = "Nguyen van quan", Gender = true, BirthDate = new DateTime(2005, 1, 2), Email = "quan@gmail.com", ClassID = 5 },
                new Student { StudentID = 1006, StudentCode = "SV06", FullName = "le dieu lan", Gender = false, BirthDate = new DateTime(2005, 6, 1), Email = "lan@gmail.com", ClassID = 4 }
            );

            // ===== Seed data: Subjects =====
            modelBuilder.Entity<Subject>().HasData(
                new Subject { SubjectID = 3, SubjectCode = "CS101", SubjectName = "Lập trình C#", Credits = 4 },
                new Subject { SubjectID = 4, SubjectCode = "DB201", SubjectName = "Cơ sở dữ liệu SQL Server", Credits = 4 },
                new Subject { SubjectID = 5, SubjectCode = "WEB301", SubjectName = "Phát triển ứng dụng Web", Credits = 3 },
                new Subject { SubjectID = 1002, SubjectCode = "html", SubjectName = "lập trình html", Credits = 2 },
                new Subject { SubjectID = 1003, SubjectCode = "android", SubjectName = "lập trình android", Credits = 2 }
            );

            // ===== Seed data: StudentGrades =====
            modelBuilder.Entity<StudentGrade>().HasData(
                new StudentGrade { StudentID = 1, SubjectID = 3, Mark = 8.50m, ExamDate = new DateTime(2026, 8, 7) },
                new StudentGrade { StudentID = 2, SubjectID = 3, Mark = 7.00m, ExamDate = new DateTime(2026, 8, 7) },
                new StudentGrade { StudentID = 2, SubjectID = 4, Mark = 9.00m, ExamDate = new DateTime(2026, 8, 7) },
                new StudentGrade { StudentID = 2, SubjectID = 5, Mark = 6.50m, ExamDate = new DateTime(2026, 8, 7) },
                new StudentGrade { StudentID = 3, SubjectID = 5, Mark = 5.50m, ExamDate = new DateTime(2026, 8, 7) }
            );
        }
    }
}