using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // the relationship between (many) Stadium with (1) Owner
            {
                modelBuilder.Entity<Stadium>()
                    .HasOne(s => s.Owner)
                    .WithMany(u => u.Stadiums)
                    .HasForeignKey(s => s.OwnerId);
            }

            // the relationship between (many) Reservation with (1) Stadium
            {
                modelBuilder.Entity<Stadium>()
                    .HasMany(s => s.Reservations)
                    .WithOne(r => r.Stadium)
                    .HasForeignKey(r => r.StadiumId);
            }

        }

        public DbSet<Client> Clients  { get; set; }
        public DbSet<Stadium> Stadiums  { get; set; }
        public DbSet<Owner> Owners  { get; set; }
        public DbSet<Reservation> Reservations { get; set; }    
    }
}
