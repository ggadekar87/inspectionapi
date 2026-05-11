using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Data.truck
{
    public class TruckAppointment
    {
        public int id { get; set; }
        public string TruckNumber { get; set; }
        public string DriverName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Purpose { get; set; }
        public string PortOfEntry { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
    }
}
