// using Microsoft.EntityFrameworkCore;
// using PTManagementSystem.Domain.Entities;

namespace PTManagementSystem.Infrastructure.Data
{
    public class UserDbContext /*: DbContext*/
    {
//         public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

//         public DbSet<User> Users { get; set; }
//         public DbSet<TrainerProfile> TrainerProfiles { get; set; }

//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             base.OnModelCreating(modelBuilder);

//             modelBuilder.Entity<User>().OwnsOne(u => u.Profile, profile =>
//             {
//                 profile.OwnsOne(p => p.PersonalInfo);
//                 profile.OwnsOne(p => p.ContactInfo);
//                 profile.OwnsOne(p => p.Address);
//             });

//             modelBuilder.Entity<User>().OwnsOne(u => u.PaymentInfo);

//             modelBuilder.Entity<User>()
//                 .HasOne(u => u.TrainerProfile)
//                 .WithOne()
//                 .HasForeignKey<TrainerProfile>("UserId");

//             modelBuilder.Entity<TrainerProfile>().OwnsMany(tp => tp.Specializations, a =>
// {
//     a.Property(s => s.Value).HasColumnName("Value");
//     a.WithOwner().HasForeignKey("TrainerProfileId");
//     a.ToTable("TrainerSpecializations");
// });


//             modelBuilder.Entity<TrainerProfile>().OwnsMany(tp => tp.Certifications, a =>
//             {
//                 a.WithOwner().HasForeignKey("TrainerProfileId");
//             });

//             modelBuilder.Entity<TrainerProfile>().OwnsMany(tp => tp.AvailableSlots, a =>
//             {
//                 a.WithOwner().HasForeignKey("TrainerProfileId");
//             });
//         }
    }

}
