using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Dayslot.Timeslot
{
    public class CreateTimeslotItemCommand : IRequest<CreateTimeslotItem>
    {
        public TimeSpan Duration { get; set; }
        public TimeSpan StartTime { get; set; }
        public bool Disabled { get; set; }
        public Guid WeeklyScheduleId { get; set; }
        public Guid SessionId { get; set; }

        public class CommandHandler : IRequestHandler<CreateTimeslotItemCommand, CreateTimeslotItem>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateTimeslotItem> Handle(CreateTimeslotItemCommand request, CancellationToken cancellationToken)
            {
                var weeklySchedule = await context.WeeklySchedules.SingleOrDefaultAsync(l => l.Id.Equals(request.WeeklyScheduleId));
                if (weeklySchedule == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.WeeklySchedule), request.WeeklyScheduleId);
                }

                //var timeslot = await context.Timeslots
                //    .Where(l => l.Classroom.Equals(classroom))
                //    .Where(l => l.WeekNumber == request.Week)
                //    .Where(l => l.Day == request.Day)
                //    .Where(l => l.Time == request.Time)
                //    .SingleOrDefaultAsync();

                //if (timeslot != null)
                //{
                //    throw new EntityAlreadyExistException(nameof(Domain.Entities.Timeslot), timeslot.Id);
                //}

                var session = await context.Sessions.SingleOrDefaultAsync(l => l.Id.Equals(request.SessionId));
                if (session == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Session), request.SessionId);
                }

                var entity = new Domain.Entities.Timeslot()
                {
                    Session = session,
                    Disabled = request.Disabled,
                    Duration = request.Duration,
                    StartTime = request.StartTime,
                    
                };
                context.Timeslots.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateTimeslotItem
                {
                    Id = entity.Id
                };
            }
        }
    }

    public class CreateTimeslotItem
    {
        public Guid Id { get; set; }
    }
}
