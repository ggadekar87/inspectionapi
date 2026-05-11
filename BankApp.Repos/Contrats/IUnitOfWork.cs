using BankApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Repos.Contrats
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<BankAccount> BankAccounts { get; }
        IRepository<Card> Cards { get; }
        IRepository<AccountType> AccountTypes { get; }
        IRepository<CardType> CardTypes { get; }
        Task<int> SaveAsync();
    }

}
