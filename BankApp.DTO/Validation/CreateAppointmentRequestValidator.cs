using BankApp.DTO.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.DTO.Validation
{
    public class CreateAppointmentRequestValidator : AbstractValidator<TruckAppointmentRequest>
    {
        public CreateAppointmentRequestValidator()
        {
            RuleFor(x => x.TruckNumber)
                .NotEmpty().WithMessage("Truck number is required.")
                .MaximumLength(50);

            RuleFor(x => x.DriverName)
                .NotEmpty().WithMessage("Driver name is required.")
                .MaximumLength(100);

            RuleFor(x => x.AppointmentDate)
                .NotNull().WithMessage("Appointment date is required.");

            RuleFor(x => x.Purpose)
                .NotEmpty().WithMessage("Purpose is required.")
                .MaximumLength(200);

            RuleFor(x => x.PortOfEntry)
                .NotEmpty().WithMessage("Port of entry is required.")
                .MaximumLength(100);

            RuleFor(x => x.Comments)
                .MaximumLength(500);
        }
    }
}
