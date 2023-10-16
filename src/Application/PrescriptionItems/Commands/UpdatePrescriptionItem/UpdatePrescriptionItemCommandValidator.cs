using FluentValidation;

namespace BallastLaneTestAssignment.Application.PrescriptionItems.Commands.UpdatePrescriptionItem;

public class UpdatePrescriptionItemCommandValidator : AbstractValidator<UpdatePrescriptionItemCommand>
{
    public UpdatePrescriptionItemCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(200)
            .NotEmpty();
    }
}
