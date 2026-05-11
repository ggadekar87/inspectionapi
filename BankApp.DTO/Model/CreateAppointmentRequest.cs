using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.DTO.Model
{
    public class CreateAppointmentRequest
    {
        public string? TruckNumber { get; set; }
        public string? DriverName { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? Purpose { get; set; }
        public string? PortOfEntry { get; set; }
        public string? Comments { get; set; }
    }
}
