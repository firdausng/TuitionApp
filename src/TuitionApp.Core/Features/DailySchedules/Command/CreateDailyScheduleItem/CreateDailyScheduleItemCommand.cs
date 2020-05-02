using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.DailySchedules
{
    public class CreateDailyScheduleItemCommand : IRequest<CreateDailyScheduleItemDto>
    {
        public DateTime DateSchedule { get; set; }
        public int WeekNumber { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public bool Disabled { get; set; }
        public Guid ClassroomId { get; set; }

        public class CommandHandler : IRequestHandler<CreateDailyScheduleItemCommand, CreateDailyScheduleItemDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateDailyScheduleItemDto> Handle(CreateDailyScheduleItemCommand request, CancellationToken cancellationToken)
            {
                var classroom = await context.Classrooms.SingleOrDefaultAsync(l => l.Id.Equals(request.ClassroomId));
                if (classroom == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Student), request.ClassroomId);
                }

                //TODO : Filter based on Tenant
                var calendarYearSettings = await context.CalendarSettings.FirstOrDefaultAsync();

                var entity = new Domain.Entities.DailySchedule()
                {
                    DateSchedule = request.DateSchedule,
                    Classroom = classroom,
                    DayOfWeek = request.DayOfWeek,
                    Disabled = request.Disabled,
                    WeekNumber = request.WeekNumber,
                };
                context.DailySchedules.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateDailyScheduleItemDto
                {
                    Id = entity.Id
                };
            }
        }
    }
}
