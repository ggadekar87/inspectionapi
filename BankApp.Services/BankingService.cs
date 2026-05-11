using BankApp.Data.Models;
using BankApp.DTO;
using BankApp.Repos.Contrats;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Services
{
    public class BankingService : IBankingService
    {
        private readonly IUnitOfWork _uow;

        public BankingService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task CreateAccountWithCard()
        {
            var account = new BankAccount
            {
                AccountNumber = "ACC1001",
                Balance = 5000,
                AccountTypeId = 1 // Saving
            };

            await _uow.BankAccounts.AddAsync(account);
            await _uow.SaveAsync();

            var card = new Card
            {
                CardNumber = "CARD9999",
                ExpiryDate = DateTime.UtcNow.AddYears(5),
                BankAccountId = account.Id,
                CardTypeId = 2 // Gold
            };

            await _uow.Cards.AddAsync(card);
            await _uow.SaveAsync();
        }
    }

}
