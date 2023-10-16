using BallastLaneTestAssignment.Application.Common.Interfaces.Database;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BallastLaneTestAssignment.Application.PrescriptionLists.Commands.UpdatePrescriptionList;

public class UpdatePrescriptionListCommandValidator : AbstractValidator<UpdatePrescriptionListCommand>
{
    private readonly IUnitOfWork _context;

    public UpdatePrescriptionListCommandValidator(IUnitOfWork context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
            .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");
    }

    private async Task<bool> BeUniqueTitle(UpdatePrescriptionListCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.PrescriptionLists.GetPrescriptionListCountByTitle(title) == 0;
    }
}
