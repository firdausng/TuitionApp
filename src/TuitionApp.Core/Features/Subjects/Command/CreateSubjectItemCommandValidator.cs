using FluentValidation;

namespace TuitionApp.Core.Features.Subjects
{
    public class CreateSubjectItemCommandValidator : AbstractValidator<CreateSubjectItemCommand>
    {
        public CreateSubjectItemCommandValidator()
        {
            RuleFor(command => command.Title).NotNull().NotEmpty();
            //RuleFor(command => command.LastName).NotNull().NotEmpty();
        }
    }
}
