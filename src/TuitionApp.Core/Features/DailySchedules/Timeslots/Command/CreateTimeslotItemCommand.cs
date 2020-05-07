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

namespace TuitionApp.Core.Features.DailySchedules.Timeslots
{
    public class CreateTimeslotItemCommand : IRequest<CreateTimeslotItemDto>
    {
        public TimeSpan Duration { get; set; }
        public TimeSpan StartTime { get; set; }
        public bool Disabled { get; set; }
        public Guid DailyScheduleId { get; set; }

        public class CommandHandler : IRequestHandler<CreateTimeslotItemCommand, CreateTimeslotItemDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateTimeslotItemDto> Handle(CreateTimeslotItemCommand request, CancellationToken cancellationToken)
            {
                var dailySchedule = await context.DailySchedules
                    .Include(w => w.Timeslots)
                    .SingleOrDefaultAsync(l => l.Id.Equals(request.DailyScheduleId));
                if (dailySchedule == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.DailySchedule), request.DailyScheduleId);
                }

                var bookedTimeslots = dailySchedule.Timeslots.GetOverlapTimeslot(request.StartTime, request.Duration);

                if (bookedTimeslots.Count > 0)
                {
                    throw new EntityAlreadyExistException(nameof(Domain.Entities.Timeslot), string.Join(",", bookedTimeslots.Select(b => b.Id.ToString()).ToArray()));
                }

                var entity = new Domain.Entities.Timeslot()
                {
                    DailySchedule = dailySchedule,
                    Disabled = request.Disabled,
                    Duration = request.Duration,
                    StartTime = request.StartTime,

                };
                context.Timeslots.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateTimeslotItemDto
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
}
