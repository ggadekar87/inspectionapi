using BankApp.DTO;
using BankApp.DTO.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankingService _bankingService;
        public BankController(IBankingService bankingService)
        {
            _bankingService = bankingService;
        }
        [HttpPost]
        public IActionResult CreateAccountWithCard(CreateAccountWithCarRequest request)
        {
            // Simulate payment processing logic
            if (request.BankAccount.Balance <= 0)
                return BadRequest("Invalid balance amount.");
            // Here you would typically call a payment gateway API
            // For this example, we'll just return a success response
            var res = _bankingService.CreateAccountWithCard();
            return Ok(new { Message = "Payment processed successfully", TransactionId = Guid.NewGuid() });
        }
    }
}
