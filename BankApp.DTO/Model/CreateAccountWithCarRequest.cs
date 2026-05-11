using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace BankApp.DTO.Model
{
    public class CreateAccountWithCarRequest
    {
        public BankAccount BankAccount { get; set; }
        public Card Card { get; set; }
    }

    public class BankAccount
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; } = 5000;
        public int AccountTypeId { get; set; } = 1; // Saving
    }

    public class Card
    {
        public string CardNumber { get; set; }   
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.AddYears(5);
        public int BankAccountId { get; set; }
        public int CardTypeId { get; set; }  
    }

}
