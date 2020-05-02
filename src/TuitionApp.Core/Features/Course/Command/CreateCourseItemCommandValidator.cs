using FluentValidation;

namespace TuitionApp.Core.Features.Course
{
    public class CreateCourseItemCommandValidator : AbstractValidator<CreateCourseItemCommand>
    {
        public CreateCourseItemCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotNull()
                .NotEmpty();
        }
    }
}
