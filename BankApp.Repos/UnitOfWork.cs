using BankApp.Data.Models;
using BankApp.Data.truck;
using BankApp.Repos.Contrats;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankingDbContext _context;

        public IRepository<BankAccount> BankAccounts { get; }
        public IRepository<Card> Cards { get; }
        public IRepository<AccountType> AccountTypes { get; }
        public IRepository<CardType> CardTypes { get; }
        public IRepository<TruckAppointment> TruckAppointmentTypes { get; }


        public UnitOfWork(BankingDbContext context)
        {
            _context = context;
            BankAccounts = new Repository<BankAccount>(context);
            Cards = new Repository<Card>(context);
            AccountTypes = new Repository<AccountType>(context);
            CardTypes = new Repository<CardType>(context);
            TruckAppointmentTypes = new Repository<TruckAppointment>(context);
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }

}
