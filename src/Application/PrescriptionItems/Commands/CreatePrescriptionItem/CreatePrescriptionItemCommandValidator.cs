using FluentValidation;

namespace BallastLaneTestAssignment.Application.PrescriptionItems.Commands.CreatePrescriptionItem;

public class CreatePrescriptionItemCommandValidator : AbstractValidator<CreatePrescriptionItemCommand>
{
    public CreatePrescriptionItemCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(200)
            .NotEmpty();
    }
}
