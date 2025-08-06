using System.Collections.Generic;
using DataAccessLayer.Interceptors;
using EntityLayer.Concrete;
using EntityLayer.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext<AppUser, AppRole, int>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        { }
        public List<(EntityEntry Entry, EntityState OriginalState, Dictionary<string, object> OriginalData)> PendingLogs { get; set; } = new();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PolicyTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ActionConfiguration());
            modelBuilder.Entity<Customer>(c =>
            {
                c.Property(c => c.CreateDate)
                    .HasDefaultValueSql("GETDATE()");

                c.HasMany(c => c.Address)
                   .WithOne(a => a.Customer)
                   .HasForeignKey(a => a.CustomerId)
                   .OnDelete(DeleteBehavior.ClientNoAction);
            });

            modelBuilder.Entity<Agency>(a =>
            {
                a.Property(a => a.CreateDate)
                    .HasDefaultValueSql("GETDATE()");

                a.HasOne(a => a.Address)
                   .WithOne(b => b.Agency)
                   .HasForeignKey<Address>(b => b.AgencyId)
                   .OnDelete(DeleteBehavior.ClientNoAction);
            });


            modelBuilder.Entity<Address>(a =>
            {
                a.HasKey(a => a.Id);

                a.Property(a => a.CreateDate)
                    .HasDefaultValueSql("GETDATE()");

                a.HasOne(a => a.Customer)
                   .WithMany(c => c.Address)
                   .HasForeignKey(a => a.CustomerId)
                   .OnDelete(DeleteBehavior.ClientNoAction);

                a.HasOne(a => a.Agency)
                   .WithOne(c => c.Address)
                   .HasForeignKey<Address>(a => a.AgencyId)
                   .OnDelete(DeleteBehavior.ClientNoAction);
            });

            modelBuilder.Entity<Log>()
               .HasOne(a => a.Actions)
               .WithMany(c => c.Logs)
               .HasForeignKey(a => a.ActionId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Log>()
              .HasOne(a => a.AppUser)
              .WithMany(c => c.Logs)
              .HasForeignKey(a => a.UserId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Policy>()
             .HasOne(a => a.Customer)
             .WithMany(c => c.Policies)
             .HasForeignKey(a => a.CustomerId)
             .OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<Policy>()
             .HasOne(a => a.Agency)
             .WithMany(c => c.Policies)
             .HasForeignKey(a => a.AgencyId)
             .OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<Policy>()
             .HasOne(a => a.PolicyType)
             .WithMany(c => c.Policies)
             .HasForeignKey(a => a.PolicyTypeId)
             .OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<Policy>()
             .HasOne(a => a.AppUser)
             .WithMany(c => c.Policies)
             .HasForeignKey(a => a.UserId)
             .OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<Housing>()
             .HasOne(a => a.Customer)
             .WithMany(c => c.Housings)
             .HasForeignKey(a => a.CustomerId)
             .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Vehicle>()
             .HasOne(a => a.Customer)
             .WithMany(c => c.Vehicles)
             .HasForeignKey(a => a.CustomerId)
             .OnDelete(DeleteBehavior.Cascade);
             

            modelBuilder.Entity<Policy>()
       .Property(c => c.CreateDate)
       .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<PolicyType>()
       .Property(c => c.CreateDate)
       .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Log>()
       .Property(c => c.CreateDate)
       .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Actions>()
      .Property(c => c.CreateDate)
      .HasDefaultValueSql("GETDATE()");
            base.OnModelCreating(modelBuilder);

      //      modelBuilder.Entity<AppUser>()
      //.Property(c => c.CreateDate)
      //.HasDefaultValueSql("GETDATE()");
      //      base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>()
       .Property(c => c.CreateDate)
       .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Housing>()
       .Property(c => c.CreateDate)
       .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<CompanyInformation>()
       .Property(c => c.CreateDate)
       .HasDefaultValueSql("GETDATE()");

        }
        public override DbSet<AppUser> Users { get; set; }
        public override DbSet<AppRole> Roles { get; set; }

        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<PolicyType> PolicyTypes { get; set; }
        public DbSet<Actions> Actions { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Housing> Housings { get; set; }
        public DbSet<CompanyInformation> CompanyInformations { get; set; }
    }
}
