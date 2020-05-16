using FluentValidation;

namespace TuitionApp.Core.Features.Courses.CourseClasses
{
    public class GetCourseClassItemQueryValidator: AbstractValidator<GetCourseClassItemQuery>
    {
        public GetCourseClassItemQueryValidator()
        {
            RuleFor(command => command.Id)
                .NotNull()
                .NotEmpty();
        }
    }
}
