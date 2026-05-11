using BankApp.Data.truck;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.DTO
{
    public interface ITruckAppointmentService
    {
        Task<List<TruckAppointment>> GetTruckAppointments();
    }
}
