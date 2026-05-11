using BankApp.Data.truck;
using BankApp.DTO;
using BankApp.Repos.Contrats;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Services
{
    public class InspectionService : IInspectionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InspectionService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<(List<TruckAppointment> Data, int Total)> GetInspections(int page = 1, int pageSize = 10, string? truckNumber = null, string? driverName = null, string? status = null, string? portOfEntry = null, string? purpose = null)
        {
            var query = _unitOfWork.TruckAppointmentTypes.GetAll();

            // Dynamic Filters
            if (!string.IsNullOrWhiteSpace(truckNumber))
                query = query.Where(a => a.TruckNumber.Contains(truckNumber));

            if (!string.IsNullOrWhiteSpace(driverName))
                query = query.Where(a => a.DriverName.Contains(driverName));

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(a => a.Status.Contains(status));

            if (!string.IsNullOrWhiteSpace(portOfEntry))
                query = query.Where(a => a.PortOfEntry.Contains(portOfEntry));

            if (!string.IsNullOrWhiteSpace(purpose))
                query = query.Where(a => a.Purpose.Contains(purpose));

            // Total count BEFORE pagination
            var total = query.Count();

            // Pagination
            var data = query
                .OrderBy(a => a.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (data, total);
        }
    }
}
