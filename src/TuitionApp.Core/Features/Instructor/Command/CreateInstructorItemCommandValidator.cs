using FluentValidation;

namespace TuitionApp.Core.Features.Instructor
{
    public class CreateInstructorItemCommandValidator : AbstractValidator<CreateInstructorItemCommand>
    {
        public CreateInstructorItemCommandValidator()
        {
            RuleFor(command => command.FirstName).NotNull().NotEmpty();
            RuleFor(command => command.HireDate).NotNull().NotEmpty();
            RuleFor(command => command.LastName).NotNull().NotEmpty();
        }
    }
}
