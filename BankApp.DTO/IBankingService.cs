using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.DTO
{
    public interface IBankingService
    {
        public Task CreateAccountWithCard();
    }
}
