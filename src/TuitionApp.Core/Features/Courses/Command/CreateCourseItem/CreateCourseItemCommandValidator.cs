using FluentValidation;

namespace TuitionApp.Core.Features.Courses
{
    public class CreateCourseItemCommandValidator : AbstractValidator<CreateCourseItemCommand>
    {
        public CreateCourseItemCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(command => command.ClassNameList)
               .NotNull();

            RuleFor(command => command.SubjectIdList)
               .NotNull();
        }
    }
}
