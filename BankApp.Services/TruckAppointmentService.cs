using Azure.Core;
using BankApp.Data.truck;
using BankApp.DTO;
using BankApp.DTO.Model;
using BankApp.Repos;
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
        private readonly IUnitOfWork _unitOfWork;

        public TruckAppointmentService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public async Task<List<TruckAppointment>> GetTruckAppointmentsMock()
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
                    Id = _id,
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

        public async Task<(List<TruckAppointment> Data, int Total)> GetTruckAppointments(int page = 1, int pageSize = 10, string? truckNumber = null, string? driverName = null, string? status = null, string? portOfEntry = null, string? purpose = null)
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
            var total =   query.Count();

            // Pagination
            var data = query
                .OrderByDescending(a => a.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (data, total);
        }

        public async Task<bool> SaveAppointment(TruckAppointmentRequest request)
        {
            if (request == null)
                return false;

            var newItem = new TruckAppointment
            {
                TruckNumber = request.TruckNumber ?? "UNKNOWN",
                DriverName = request.DriverName ?? "UNKNOWN",
                AppointmentDate = request.AppointmentDate ?? DateTime.UtcNow,
                Purpose = request.Purpose ?? "Delivery",
                PortOfEntry = request.PortOfEntry ?? "Port A",
                Status = "Pending",
                Comments = request.Comments ?? "",
                CreatedAt=DateTime.Now,
                UpdatedAt=DateTime.Now
            };

            await _unitOfWork.TruckAppointmentTypes.AddAsync(newItem);

            var result = await _unitOfWork.SaveAsync();

            return result > 0;
        }

        public async Task<bool> UpdateAppointment(TruckAppointmentUpdateRequest request)
        {
            if (request == null || request.Id <= 0)
                return false;

            // Fetch existing record
            var existing = await _unitOfWork.TruckAppointmentTypes.GetByIdAsync(request.Id);

            if (existing == null)
                return false;

            // Update fields only if provided
            existing.TruckNumber = request.TruckNumber ?? existing.TruckNumber;
            existing.DriverName = request.DriverName ?? existing.DriverName;
            existing.AppointmentDate = request.AppointmentDate ?? existing.AppointmentDate;
            existing.Purpose = request.Purpose ?? existing.Purpose;
            existing.PortOfEntry = request.PortOfEntry ?? existing.PortOfEntry;
            existing.Status = request.Status ?? existing.Status;
            existing.Comments = request.Comments ?? existing.Comments;

            // Update timestamp
            existing.UpdatedAt = DateTime.UtcNow;

            // Mark entity as updated
            _unitOfWork.TruckAppointmentTypes.Update(existing);

            // Save changes
            var result = await _unitOfWork.SaveAsync();

            return result > 0;
        }

        public async Task<bool> DeleteAppointment(int id)
        {
            if (id <= 0)
                return false;

            // Fetch existing record
            var existing = await _unitOfWork.TruckAppointmentTypes.GetByIdAsync(id);

            if (existing == null)
                return false;

            // Remove entity
            _unitOfWork.TruckAppointmentTypes.Delete(existing);

            // Save changes
            var result = await _unitOfWork.SaveAsync();

            return result > 0;
        }

        public async Task<TruckAppointment?> GetAppointmentById(int id)
        {
            if (id <= 0)
                return null;

            var appointment = await _unitOfWork.TruckAppointmentTypes.GetByIdAsync(id);

            return appointment;
        }

        public async Task<bool> UpdateAppointmentStatus(UpdateAppointmentStatusRequest request)
        {
            if (request == null || request.Id <= 0)
                return false;

            // Fetch existing appointment
            var existing = await _unitOfWork.TruckAppointmentTypes.GetByIdAsync(request.Id);

            if (existing == null)
                return false;

            // Update only the status
            existing.Status = request.Status;

            // Optional: update timestamp
            existing.UpdatedAt = DateTime.UtcNow;

            // Mark entity as updated
            _unitOfWork.TruckAppointmentTypes.Update(existing);

            // Save changes
            var result = await _unitOfWork.SaveAsync();

            return result > 0;
        }

    }
}
