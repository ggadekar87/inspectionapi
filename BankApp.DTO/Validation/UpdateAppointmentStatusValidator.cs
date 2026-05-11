using BankApp.DTO.Model;
using FluentValidation;

namespace BankApp.DTO.Validation
{
    public class UpdateAppointmentStatusValidator : AbstractValidator<UpdateAppointmentStatusRequest>
    {
        public UpdateAppointmentStatusValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid appointment Id.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(s => new[] { "Pending", "Approved", "Rejected", "Cancelled" }.Contains(s))
                .WithMessage("Invalid status value.");
        }
    }
}
