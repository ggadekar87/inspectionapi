using BankApp.Data.truck;
using BankApp.DTO;
using BankApp.Repos.Contrats;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Services
{
    public class TruckAppointmentService : ITruckAppointmentService
    {
        private readonly IUnitOfWork _uow;

        public TruckAppointmentService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<TruckAppointment>> GetTruckAppointments()
        {
            List<string> AppointmentStatus = new List<string>() { "Pending", "Approved", "Rejected", "Cancelled" };
            List<string> purposes = new List<string>() { "Delivery", "Pickup", "Maintenance" };
            List<string> ports = new List<string>() { "Port A", "Port B", "Port C" };
            List<TruckAppointment> truckAppointments = new List<TruckAppointment>();

            for (int i = 0; i < 100; i++)
            {
                int _id = i + 1;
                TruckAppointment truckAppointment = new TruckAppointment()
                {
                    id = _id,
                    TruckNumber = $"TRKBE-{1000 + _id}",
                    DriverName = $"DriverBE {_id % 12 + 1}",
                    AppointmentDate = DateTime.UtcNow.AddDays(i),
                    Purpose = purposes[i % purposes.Count],
                    PortOfEntry = ports[i % ports.Count],
                    Status = AppointmentStatus[i % AppointmentStatus.Count],
                    Comments = $"Comments {_id}",
                };
                truckAppointments.Add(truckAppointment);
            }

            return await Task.FromResult(truckAppointments);
        }
    }
}
