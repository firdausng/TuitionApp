using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Core.Features.WeeklySchedule.Timeslot
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
                var weeklySchedule = await context.WeeklySchedules
                    .Include(w => w.Timeslots)
                    .SingleOrDefaultAsync(l => l.Id.Equals(request.WeeklyScheduleId));
                if (weeklySchedule == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.WeeklySchedule), request.WeeklyScheduleId);
                }

                var bookedTimeslots = weeklySchedule.Timeslots.GetOverlapTimeslot(request.StartTime, request.Duration);

                if (bookedTimeslots.Count > 0)
                {
                    throw new EntityAlreadyExistException(nameof(Domain.Entities.Timeslot), string.Join(",", bookedTimeslots.Select(b => b.Id.ToString()).ToArray()));
                }

                var session = await context.Sessions.SingleOrDefaultAsync(l => l.Id.Equals(request.SessionId));
                if (session == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Session), request.SessionId);
                }

                var entity = new Domain.Entities.Timeslot()
                {
                    Session = session,
                    WeeklySchedule = weeklySchedule,
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

            public bool IstimeslotOverlapping(Domain.Entities.Timeslot currentTimeslot, List<Domain.Entities.Timeslot> timeslots)
            {
                foreach (var timeslot in timeslots)
                {
                    if (timeslot.Id == currentTimeslot.Id) continue;

                    if (timeslot.StartTime > currentTimeslot.StartTime && timeslot.StartTime < currentTimeslot.StartTime.Add(currentTimeslot.Duration))
                    {
                        return true;
                    }
                    else if (currentTimeslot.StartTime > timeslot.StartTime && currentTimeslot.StartTime < timeslot.StartTime.Add(timeslot.Duration))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }

    public class CreateTimeslotItem
    {
        public Guid Id { get; set; }
    }
}
