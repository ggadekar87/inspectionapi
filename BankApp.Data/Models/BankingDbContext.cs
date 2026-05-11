using BankApp.Data.truck;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace BankApp.Data.Models
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options)
            : base(options) { }

        public DbSet<AccountType> AccountTypes => Set<AccountType>();
        public DbSet<CardType> CardTypes => Set<CardType>();
        public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
        public DbSet<Card> Cards => Set<Card>();
        public DbSet<TruckAppointment> TruckAppointment { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountType>().HasData(
                new AccountType { Id = 1, Name = "Saving" },
                new AccountType { Id = 2, Name = "Current" }
            );

            modelBuilder.Entity<CardType>().HasData(
                new CardType { Id = 1, Name = "Platinum" },
                new CardType { Id = 2, Name = "Gold" },
                new CardType { Id = 3, Name = "Silver" }
            );


            modelBuilder.Entity<TruckAppointment>()
           .ToTable("TruckAppointment"); // map to actual SQL table

            base.OnModelCreating(modelBuilder);
        }
    }

}
