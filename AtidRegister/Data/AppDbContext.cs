using AtidRegister.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        /// <summary>
        /// people who take part of the conference
        /// </summary>
        public DbSet<Person> People { get; set; }
        /// <summary>
        /// contents of conference
        /// </summary>
        public DbSet<Content> Contents { get; set; }
        /// <summary>
        /// many-to-many relationships between contents and people
        /// </summary>
        public DbSet<ContentPerson> ContentPeople { get; set; }
        /// <summary>
        /// many-to-many relationship between user and chosen contents.
        /// </summary>
        public DbSet<ContentUser> ContentUsers { get; set; }
        /// <summary>
        /// Content Types
        /// </summary>
        public DbSet<ContentType> ContentTypes { get; set; }
        /// <summary>
        /// Contents time strips
        /// </summary>
        public DbSet<TimeStrip> TimeStrips { get; set; }
        /// <summary>
        /// Classes
        /// </summary>
        public DbSet<Class> Classes { get; set; }
        /// <summary>
        /// Grades
        /// </summary>
        public DbSet<Grade> Grades { get; set; }
        /// <summary>
        /// FAQs Store
        /// </summary>
        public DbSet<FAQuestion> FAQuestions { get; set; }
        /// <summary>
        /// Finalized scheduled contents
        /// </summary>
        public DbSet<UserSchedule> UserSchedules { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // who needs ctor?
            // - Everyone!
            // HAHA bad joke
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // configure custom entities:
            builder.Entity<Person>(e =>
            {
                e.HasKey(p => p.Id); // pk
                e.Property(p => p.Id).ValueGeneratedOnAdd(); // auto id
                e.Property(p => p.FullName).IsRequired().HasMaxLength(40);
                e.Property(p => p.JobTitle).IsRequired().HasMaxLength(70);
            });
            builder.Entity<Content>(e =>
            {
                e.HasKey(p => p.Id); // pk
                e.Property(p => p.Id).ValueGeneratedOnAdd(); // auto id
                e.Property(p => p.Title).IsRequired().HasMaxLength(50);
                e.Property(p => p.Description).IsRequired().HasMaxLength(300);
                e.Property(p => p.TimeStripId).IsRequired();
                e.HasOne(p => p.TimeStrip).WithMany(ts => ts.Contents)
                .HasForeignKey(p => p.TimeStripId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<ContentPerson>(e =>
            {
                e.HasKey(cp => cp.Id); // pk
                // many-to-many relationships between contents and people.
                e.HasOne(cp => cp.Content).WithMany(c => c.ContentPeople)
                .HasForeignKey(cp => cp.ContentId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                e.HasOne(cp => cp.Person).WithMany(c => c.ContentPeople)
                .HasForeignKey(cp => cp.PersonId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<ContentUser>(e =>
            {
                e.HasKey(cu => cu.Id);
                e.HasOne(cu => cu.Content).WithMany(c => c.ContentUsers)
                .HasForeignKey(cu => cu.ContentId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                e.HasOne(cu => cu.User).WithMany(c => c.ContentUsers)
                .HasForeignKey(cu => cu.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                e.Property(cu => cu.Priority).IsRequired();
            });
            builder.Entity<ContentType>(e =>
            {
                e.HasKey(ct => ct.Id);
                e.HasMany(ct => ct.Contents).WithOne(c => c.Type)
                .HasForeignKey(c => c.TypeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<Class>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.ClassName).IsRequired();
                e.HasData(
                    new Class() { Id = -1, ClassName = "י'" },
                    new Class() { Id = -2, ClassName = "י\"א" },
                    new Class() { Id = -3, ClassName = "י\"ב" }
                    );
            });
            builder.Entity<Grade>(e =>
            {
                e.HasKey(g => g.Id);
                e.Property(g => g.Id).ValueGeneratedOnAdd();
                // Grade number in class.
                e.Property(g => g.ClassNumber).IsRequired();
                // Class->Grade Relatioship
                e.HasOne(g => g.Class).WithMany(c => c.Grades).HasForeignKey(g => g.ClassId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();
                // AppUser->Grade Relationship
                e.HasMany(g => g.Students).WithOne(s => s.Grade).HasForeignKey(s => s.GradeId)
                .OnDelete(DeleteBehavior.Cascade);
                e.HasData(GetClassGradesToSeed(-1, 7));
                e.HasData(GetClassGradesToSeed(-2, 7));
                e.HasData(GetClassGradesToSeed(-3, 7));
            });
            builder.Entity<TimeStrip>(e =>
            {
                e.HasKey(ts => ts.Id);
            });
            builder.Entity<FAQuestion>(e =>
            {
                e.HasKey(f => f.Id);
                e.Property(f => f.Id).ValueGeneratedOnAdd();
                e.Property(f => f.Question).IsRequired();
                e.Property(f => f.Answer).IsRequired();
            });
            builder.Entity<UserSchedule>(e => 
            {
                e.HasKey(uc => uc.Id);
                e.HasOne(uc => uc.User).WithMany(u => u.Schedules)
                    .HasForeignKey(uc => uc.UserId).IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(uc => uc.Content).WithMany(c => c.Schedules)
                    .HasForeignKey(uc => uc.ContentId).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        private IEnumerable<Grade> GetClassGradesToSeed(int classId, int gradeCount)
        {
            for (int i = 1; i <= gradeCount; i++)
            {
                yield return new Grade() { ClassId = classId, ClassNumber = i, Id = -(Math.Abs(classId) * 10 + i) };
            }
        }
    }
}
