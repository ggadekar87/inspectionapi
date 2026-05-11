using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.DTO.Model
{
    public class UpdateAppointmentStatusRequest
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }

}
