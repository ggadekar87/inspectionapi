using BankApp.Data.truck;
using BankApp.DTO;
using BankApp.DTO.Model;
using BankApp.Repos.Contrats;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net.NetworkInformation;
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
            var result = await _truckAppointmentService.GetTruckAppointments(page,pageSize,truckNumber,driverName,status,portOfEntry,purpose);
            return Ok(new
            {
                data = result.Data,
                total = result.Total
            });
            //var res = _truckAppointmentService.GetTruckAppointments().Result;
            //var query = res.AsQueryable();
            //bool filer = false;
            //// Apply dynamic filters
            //if (!string.IsNullOrEmpty(truckNumber))
            //{
            //    filer = true;
            //    query = query.Where(a => a.TruckNumber.Contains(truckNumber, StringComparison.OrdinalIgnoreCase));
            //}

            //if (!string.IsNullOrEmpty(driverName))
            //{
            //    query = query.Where(a => a.DriverName.Contains(driverName, StringComparison.OrdinalIgnoreCase));
            //    filer = true;
            //}

            //if (!string.IsNullOrEmpty(status))
            //    query = query.Where(a => a.Status.Contains(status, StringComparison.OrdinalIgnoreCase));

            //if (!string.IsNullOrEmpty(portOfEntry))
            //    query = query.Where(a => a.PortOfEntry.Contains(portOfEntry, StringComparison.OrdinalIgnoreCase));

            //if (!string.IsNullOrEmpty(purpose))
            //    query = query.Where(a => a.Purpose.Contains(purpose, StringComparison.OrdinalIgnoreCase));

            //// Total BEFORE pagination
            //var total = query.Count();
            //if (filer)
            //{
            //    int totalPages = (int)Math.Ceiling((double)total / pageSize);
            //    if (page > totalPages)
            //    {
            //        page = 1;
            //    }
            //}
           
            //// Apply pagination
            //var data = query
            //    .OrderBy(a => a.id)
            //    .Skip((page - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToList();

            //return Ok(new
            //{
            //    data = data,
            //    total = total
            //});
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateAppointment([FromBody] TruckAppointmentRequest payload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _truckAppointmentService.SaveAppointment(payload);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAppointment([FromBody] TruckAppointmentUpdateRequest payload)
        {
            if (payload == null)
                return BadRequest("Request body cannot be empty.");

            if (payload.Id <= 0)
                return BadRequest("A valid appointment Id is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _truckAppointmentService.UpdateAppointment(payload);

            if (updated == null)
                return NotFound("Appointment not found or update failed.");

            return Ok(updated);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var success = await _truckAppointmentService.DeleteAppointment(id);

            if (!success)
                return NotFound(new { message = "Appointment not found or already deleted." });

            return Ok(new { message = "Appointment deleted successfully." });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _truckAppointmentService.GetAppointmentById(id);

            if (appointment == null)
                return NotFound(new { message = "Appointment not found." });

            return Ok(appointment);
        }

        [HttpPatch("update/status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateAppointmentStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _truckAppointmentService.UpdateAppointmentStatus(request);

            if (!success)
                return NotFound(new { message = "Appointment not found or update failed." });

            return Ok(new { message = "Status updated successfully." });
        }
    }
    
}
