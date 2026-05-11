using api.model;
using api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase 
    {
        private readonly ProductService _service;
        private readonly MySettings _mySettings;


        public ProductsController(ProductService service, IOptions<MySettings> mySettings)
        {
            _service = service;
            _mySettings = mySettings.Value;

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var apiUrl = _mySettings.ApiUrl;
            var apiKey = _mySettings.ApiKey;

            HttpContext.Session.SetString("Username", "JohnDoe");
            string username = HttpContext.Session.GetString("Username");
            return Ok(await _service.GetProductsAsync());
        }
    }

}
