using FluentValidation;

namespace TuitionApp.Core.Features.DailySchedules
{
    public class CreateDailyScheduleItemCommandValidator : AbstractValidator<CreateDailyScheduleItemCommand>
    {
        public CreateDailyScheduleItemCommandValidator()
        {
            RuleFor(command => command.ClassroomId).NotNull().NotEmpty();
            RuleFor(command => command.DateSchedule).NotNull().NotEmpty();
            RuleFor(command => command.DayOfWeek).NotNull().NotEmpty();
            RuleFor(command => command.Disabled).NotNull().NotEmpty();
            RuleFor(command => command.WeekNumber).NotNull().NotEmpty();
        }
    }
}
