using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BankApp.Data.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = default!;
        public decimal Balance { get; set; }

        public int AccountTypeId { get; set; }
        public AccountType AccountType { get; set; } = default!;

        public ICollection<Card> Cards { get; set; } = new List<Card>();

    }
}
