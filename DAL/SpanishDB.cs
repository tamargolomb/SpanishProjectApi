using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public class SpanishDB : DbContext
    {
        public SpanishDB(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Levels> Levels { get; set; }
        public DbSet<ReviewsAndRatings> ReviewsAndRatings { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<ReviewsAndRatings>()
        //        .HasOne(r => r.User)
        //        .WithMany()
        //        .HasForeignKey(r => r.UserId)
        //        .OnDelete(DeleteBehavior.NoAction);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReviewsAndRatings>()
                .HasOne(r => r.User)
                .WithMany(u => u.ReviewsAndRatings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }

}
