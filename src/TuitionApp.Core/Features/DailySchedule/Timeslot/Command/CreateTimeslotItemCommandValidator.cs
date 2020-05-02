using FluentValidation;

namespace TuitionApp.Core.Features.DailySchedule.Timeslot.Command
{
    public class CreateTimeslotItemCommandValidator : AbstractValidator<CreateTimeslotItemCommand>
    {
        public CreateTimeslotItemCommandValidator()
        {
            RuleFor(command => command.Disabled).NotNull().NotEmpty();
            RuleFor(command => command.DailyScheduleId).NotNull().NotEmpty();
            RuleFor(command => command.Duration).NotNull().NotEmpty();
            RuleFor(command => command.StartTime).NotNull().NotEmpty();
        }
    }
}
