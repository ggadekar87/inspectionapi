    using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BankApp.Data.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string CardNumber { get; set; } = default!;
        public DateTime ExpiryDate { get; set; }

        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; } = default!;

        public int CardTypeId { get; set; }
        public CardType CardType { get; set; } = default!;

    }
}
