using FluentValidation;

namespace TuitionApp.Core.Features.Locations.Classrooms
{
    public class CreateClassroomFromLocationCommandValidator : AbstractValidator<CreateClassroomFromLocationCommand>
    {
        public CreateClassroomFromLocationCommandValidator()
        {
            RuleFor(command => command.Capacity).NotNull().NotEmpty();
            RuleFor(command => command.IsEnabled).NotNull().NotEmpty();
            RuleFor(command => command.LocationId).NotNull().NotEmpty();
            RuleFor(command => command.Name).NotNull().NotEmpty();
        }
    }
}
