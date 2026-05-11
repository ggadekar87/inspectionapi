using api.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

[EnableCors("AllowSpecificOrigin")]
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    [HttpPost]
    public IActionResult Register(RegisterUserRequest request)
    {
        return Ok("User registered successfully");
    }
}
