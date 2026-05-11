using BankApp.Data.truck;
using BankApp.DTO;
using BankApp.DTO.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruckAppointmentController : ControllerBase
    {
        private readonly ITruckAppointmentService _truckAppointmentService;
        public TruckAppointmentController(ITruckAppointmentService truckAppointmentService)
        {
            _truckAppointmentService = truckAppointmentService;
        }

        [HttpGet("fetch")]
        public async Task<IActionResult> GetTruckAppointments(int page = 1,
        int pageSize = 10,
        [FromQuery] string? truckNumber = null,
        [FromQuery] string? driverName = null,
        [FromQuery] string? status = null,
        [FromQuery] string? portOfEntry = null,
        [FromQuery] string? purpose = null)
        {
            var res = _truckAppointmentService.GetTruckAppointments().Result;
            var query = res.AsQueryable();
            bool filer = false;
            // Apply dynamic filters
            if (!string.IsNullOrEmpty(truckNumber))
            {
                filer = true;
                query = query.Where(a => a.TruckNumber.Contains(truckNumber, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(driverName))
            {
                query = query.Where(a => a.DriverName.Contains(driverName, StringComparison.OrdinalIgnoreCase));
                filer = true;
            }

            if (!string.IsNullOrEmpty(status))
                query = query.Where(a => a.Status.Contains(status, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(portOfEntry))
                query = query.Where(a => a.PortOfEntry.Contains(portOfEntry, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(purpose))
                query = query.Where(a => a.Purpose.Contains(purpose, StringComparison.OrdinalIgnoreCase));

            // Total BEFORE pagination
            var total = query.Count();
            if (filer)
            {
                int totalPages = (int)Math.Ceiling((double)total / pageSize);
                if (page > totalPages)
                {
                    page = 1;
                }
            }
           
            // Apply pagination
            var data = query
                .OrderBy(a => a.id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                data = data,
                total = total
            });
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest payload)
        {
            if (payload == null)
                return BadRequest("Invalid request");

            // Apply defaults exactly like your JS object
            var newItem = new TruckAppointment
            {
                TruckNumber = payload.TruckNumber ?? "UNKNOWN",
                DriverName = payload.DriverName ?? "UNKNOWN",
                AppointmentDate = payload.AppointmentDate ?? DateTime.UtcNow,
                Purpose = payload.Purpose ?? "Delivery",
                PortOfEntry = payload.PortOfEntry ?? "Port A",
                Status = "Pending",
                Comments = payload.Comments ?? ""
            };

            // Save to DB (EF Core example)
            //_context.TruckAppointments.Add(newItem);
            //await _context.SaveChangesAsync();
            newItem.DriverName = newItem.DriverName + " Called from be api";
            return Ok(newItem);
        }

    }
    
}
