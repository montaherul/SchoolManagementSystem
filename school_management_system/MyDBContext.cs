using Microsoft.EntityFrameworkCore;
using school_management_system.Models;
using SchoolManagementSystem.Models;

namespace school_management_system
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options)
    : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<ClassSubject> ClassSubjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<TeacherSalary> TeacherSalaries { get; set; }

        public DbSet<TeacherSalaryPayment> TeacherSalaryPayments { get; set; }

        public DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamRoutine> ExamRoutines { get; set; }

        public DbSet<Mark> Marks { get; set; }

        public DbSet<Result> Results { get; set; }
        public DbSet<FeeType> FeeTypes { get; set; }

        public DbSet<StudentFee> StudentFees { get; set; }

        public DbSet<FeePayment> FeePayments { get; set; }
        public DbSet<SalaryStructure> SalaryStructures { get; set; }

        public DbSet<SalaryPayment> SalaryPayments { get; set; }
        public DbSet<SMSLog> SMSLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Student relationships
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Class)
                .WithMany()
                .HasForeignKey(s => s.ClassID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Section)
                .WithMany()
                .HasForeignKey(s => s.SectionID)
                .OnDelete(DeleteBehavior.Restrict);


            // TeacherSubject relationships
            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Class)
                .WithMany()
                .HasForeignKey(ts => ts.ClassID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Section)
                .WithMany()
                .HasForeignKey(ts => ts.SectionID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Subject)
                .WithMany()
                .HasForeignKey(ts => ts.SubjectID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Teacher)
                .WithMany()
                .HasForeignKey(ts => ts.TeacherID)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
