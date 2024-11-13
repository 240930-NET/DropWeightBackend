using Microsoft.EntityFrameworkCore;
using DropWeight.Domain.Entities;

namespace DropWeight.Infrastructure.Data
{
    public class DropWeightContext : DbContext
    {
        public DropWeightContext(DbContextOptions<DropWeightContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Nutrition> Nutritions { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<GeoSpatial> GeoSpatials { get; set; }
        public DbSet<Goal> Goals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One User to many Nutrition
            modelBuilder.Entity<Nutrition>()
                .HasOne(n => n.User)
                .WithMany(u => u.Nutritions)
                .HasForeignKey(n => n.UserId);

            // One User to may Workout
            modelBuilder.Entity<Workout>()
                .HasOne(w => w.User)
                .WithMany(u => u.Workouts)
                .HasForeignKey(w => w.UserId);

            // One Workout to many GeoSpatial
            modelBuilder.Entity<GeoSpatial>()
                .HasOne(g => g.Workout)
                .WithMany(w => w.GeoSpatials)
                .HasForeignKey(g => g.WorkoutId);

            // One User many Goal
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.User)
                .WithMany(u => u.Goals)
                .HasForeignKey(g => g.UserId);
        }
    }
}
