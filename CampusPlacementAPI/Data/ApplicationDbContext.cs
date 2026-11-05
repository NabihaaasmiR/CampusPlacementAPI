using CampusPlacementAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CampusPlacementAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<College> Colleges => Set<College>();
        public DbSet<PlacementOfficer> PlacementOfficers => Set<PlacementOfficer>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<CompanyHR> CompanyHRs => Set<CompanyHR>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<JobOpening> JobOpenings => Set<JobOpening>();
        public DbSet<JobTargetCollege> JobTargetColleges => Set<JobTargetCollege>();
        public DbSet<JobApplication> JobApplications => Set<JobApplication>();
        public DbSet<RecruitmentDrive> RecruitmentDrives => Set<RecruitmentDrive>();
        public DbSet<InterviewRound> InterviewRound => Set<InterviewRound>();
        public DbSet<InterviewSchedule> InterviewSchedules => Set<InterviewSchedule>();
        public DbSet<ApplicationStatusHistory> ApplicationStatusHistories => Set<ApplicationStatusHistory>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // --- 1. Unique Constraints & Indexes ---
            builder.Entity<College>()
                .HasIndex(c => c.CollegeCode)
                .IsUnique();

            builder.Entity<College>()
                .HasIndex(c => c.OfficialEmail)
                .IsUnique();

            builder.Entity<Company>()
                .HasIndex(c => c.CompanyEmail)
                .IsUnique();

            builder.Entity<Student>()
                .HasIndex(s => new { s.CollegeId, s.RegisterNumber })
                .IsUnique();

            builder.Entity<JobApplication>()
                .HasIndex(ja => new { ja.StudentId, ja.JobId })
                .IsUnique();

            // --- 2. Relationships & 1:1 Profiles ---
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.PlacementOfficerProfile)
                .WithOne(p => p.User)
                .HasForeignKey<PlacementOfficer>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.CompanyHRProfile)
                .WithOne(h => h.User)
                .HasForeignKey<CompanyHR>(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.StudentProfile)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- 3. College & Student Relationships ---
            builder.Entity<Student>()
                .HasOne(s => s.College)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Student>()
                .HasOne(s => s.PlacementOfficer)
                .WithMany(po => po.EnrolledStudents)
                .HasForeignKey(s => s.PlacementOfficerId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- 4. Job Target Colleges & Applications ---
            builder.Entity<JobTargetCollege>()
                .HasOne(jt => jt.JobOpening)
                .WithMany(j => j.JobTargetColleges)
                .HasForeignKey(jt => jt.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<JobTargetCollege>()
                .HasOne(jt => jt.College)
                .WithMany(c => c.JobTargetColleges)
                .HasForeignKey(jt => jt.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<JobApplication>()
                .HasOne(ja => ja.Student)
                .WithMany(s => s.JobApplications)
                .HasForeignKey(ja => ja.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<JobApplication>()
                .HasOne(ja => ja.JobOpening)
                .WithMany(j => j.JobApplications)
                .HasForeignKey(ja => ja.JobId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}