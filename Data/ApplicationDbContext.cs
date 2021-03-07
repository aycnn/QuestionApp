using Microsoft.EntityFrameworkCore;
using QuestionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<ResponseModel> Responses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                  .HasMany(s => s.Responses)
                  .WithOne(r => r.Question)
                  .HasForeignKey(k => k.QuestionId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Responses)
                .WithOne(r => r.User)
                .HasForeignKey(k => k.Username);



            base.OnModelCreating(modelBuilder);
        }
    }
}
