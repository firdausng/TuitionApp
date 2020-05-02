using FluentValidation;

namespace TuitionApp.Core.Features.Enrollments
{
    public class CreateEnrollmentItemCommandValidator : AbstractValidator<CreateEnrollmentItemCommand>
    {
        public CreateEnrollmentItemCommandValidator()
        {
            RuleFor(command => command.SessionId).NotNull().NotEmpty();
            RuleFor(command => command.StartDate).NotNull().NotEmpty();
            RuleFor(command => command.StudentId).NotNull().NotEmpty();
        }
    }
}
