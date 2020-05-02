using FluentValidation;

namespace TuitionApp.Core.Features.Student
{
    public class CreateStudentItemCommandValidator : AbstractValidator<CreateStudentItemCommand>
    {
        public CreateStudentItemCommandValidator()
        {
            RuleFor(command => command.FirstName).NotNull().NotEmpty();
            RuleFor(command => command.LastName).NotNull().NotEmpty();
        }
    }
}
