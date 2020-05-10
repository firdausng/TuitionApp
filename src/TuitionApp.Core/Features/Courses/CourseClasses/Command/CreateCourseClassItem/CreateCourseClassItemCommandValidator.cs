using FluentValidation;

namespace TuitionApp.Core.Features.Courses.CourseClasses
{
    public class CreateCourseClassItemCommandValidator : AbstractValidator<CreateCourseClassItemCommand>
    {
        public CreateCourseClassItemCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(command => command.Capacity)
                .NotNull()
                .NotEmpty();

            RuleFor(command => command.CourseId)
                .NotNull()
                .NotEmpty();

            RuleFor(command => command.LocationId)
                .NotNull()
                .NotEmpty();
        }
    }
}
