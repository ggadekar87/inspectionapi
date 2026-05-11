using BankApp.Data.truck;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.DTO
{
    public interface IInspectionService
    {
        Task<(List<TruckAppointment> Data, int Total)> GetInspections(int page = 1, int pageSize = 10, string? truckNumber = null, string? driverName = null, string? status = null, string? portOfEntry = null, string? purpose = null);
    }
}
