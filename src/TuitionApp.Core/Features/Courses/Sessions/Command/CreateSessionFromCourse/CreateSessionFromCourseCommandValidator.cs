using FluentValidation;

namespace TuitionApp.Core.Features.Courses.Sessions
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
