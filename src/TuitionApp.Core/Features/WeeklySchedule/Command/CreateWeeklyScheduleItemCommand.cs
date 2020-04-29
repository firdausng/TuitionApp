using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.WeeklySchedule
{
    public class CreateWeeklyScheduleItemCommand : IRequest<CreateWeeklyScheduleItem>
    {
        public DateTime DateSchedule { get; set; }
        public int WeekNumber { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public bool Disabled { get; set; }
        public Guid ClassroomId { get; set; }

        public class CommandHandler : IRequestHandler<CreateWeeklyScheduleItemCommand, CreateWeeklyScheduleItem>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateWeeklyScheduleItem> Handle(CreateWeeklyScheduleItemCommand request, CancellationToken cancellationToken)
            {
                var classroom = await context.Classrooms.SingleOrDefaultAsync(l => l.Id.Equals(request.ClassroomId));
                if (classroom == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Student), request.ClassroomId);
                }

                //TODO : Filter based on Tenant
                var calendarYearSettings = await context.CalendarSettings.FirstOrDefaultAsync();

                var entity = new Domain.Entities.WeeklySchedule()
                {
                    DateSchedule = request.DateSchedule,
                    Classroom = classroom,
                    DayOfWeek = request.DayOfWeek,
                    Disabled = request.Disabled,
                    WeekNumber = request.WeekNumber,
                };
                context.WeeklySchedules.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateWeeklyScheduleItem
                {
                    Id = entity.Id
                };
            }
        }
    }

    public class CreateWeeklyScheduleItem
    {
        public Guid Id { get; set; }
    }
}
