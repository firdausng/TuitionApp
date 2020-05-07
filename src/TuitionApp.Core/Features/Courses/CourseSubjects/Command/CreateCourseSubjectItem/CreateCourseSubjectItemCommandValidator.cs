using FluentValidation;

namespace TuitionApp.Core.Features.Courses.CourseSubjects
{
    public class CreateCourseSubjectItemCommandValidator : AbstractValidator<CreateCourseSubjectItemCommand>
    {
        public CreateCourseSubjectItemCommandValidator()
        {
            RuleFor(command => command.Title)
                .NotNull()
                .NotEmpty();
            RuleFor(command => command.SubjectAssignmentId)
                .NotNull()
                .NotEmpty();
            RuleFor(command => command.CourseId)
                .NotNull()
                .NotEmpty();
        }
    }
}
