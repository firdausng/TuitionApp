using FluentValidation;

namespace TuitionApp.Core.Features.Courses.ClassSubjects
{
    public class CreateClassSubjectItemCommandValidator : AbstractValidator<CreateClassSubjectItemCommand>
    {
        public CreateClassSubjectItemCommandValidator()
        {
            RuleFor(command => command.Title)
                .NotNull()
                .NotEmpty();
            RuleFor(command => command.SubjectAssignmentId)
                .NotNull()
                .NotEmpty();
            RuleFor(command => command.CourseClassId)
                .NotNull()
                .NotEmpty();
        }
    }
}
