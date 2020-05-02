using FluentValidation;

namespace TuitionApp.Core.Features.Course
{
    public class CreateSessionFromCourseCommandValidator : AbstractValidator<CreateSessionFromCourseCommand>
    {
        public CreateSessionFromCourseCommandValidator()
        {
            RuleFor(command => command.CourseId)
                .NotNull()
                .NotEmpty();

            RuleFor(command => command.Timeslots)
               .NotNull();
        }
    }
}
