using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Data.Models
{
    public class CardType
    {

        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<Card> Cards { get; set; } = new List<Card>();

    }
}
