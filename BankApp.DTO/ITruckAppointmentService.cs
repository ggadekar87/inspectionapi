using BankApp.Data.truck;
using BankApp.DTO.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.DTO
{
    public interface ITruckAppointmentService
    {
        Task<(List<TruckAppointment> Data, int Total)> GetTruckAppointments(int page = 1,int pageSize = 10,string? truckNumber = null,string? driverName = null,string? status = null,string? portOfEntry = null,string? purpose = null);
        Task<bool> SaveAppointment(TruckAppointmentRequest createAppointmentRequest);
        Task<bool> UpdateAppointment(TruckAppointmentUpdateRequest truckAppointmentUpdateRequest);
        Task<bool> DeleteAppointment(int id);
        Task<TruckAppointment?> GetAppointmentById(int id);
        Task<bool> UpdateAppointmentStatus(UpdateAppointmentStatusRequest request);
    }
}
