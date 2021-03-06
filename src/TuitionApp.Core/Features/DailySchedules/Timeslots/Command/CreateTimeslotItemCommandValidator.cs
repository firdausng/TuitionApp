﻿using FluentValidation;

namespace TuitionApp.Core.Features.DailySchedules.Timeslots
{
    public class CreateTimeslotItemCommandValidator : AbstractValidator<CreateTimeslotItemCommand>
    {
        public CreateTimeslotItemCommandValidator()
        {
            RuleFor(command => command.Disabled).NotNull().Must(x => x == false || x == true);
            RuleFor(command => command.DailyScheduleId).NotNull().NotEmpty();
            RuleFor(command => command.ClassSubjectId).NotNull().NotEmpty();
            RuleFor(command => command.Duration).NotNull().NotEmpty();
            RuleFor(command => command.StartTime).NotNull().NotEmpty();
        }
    }
}
