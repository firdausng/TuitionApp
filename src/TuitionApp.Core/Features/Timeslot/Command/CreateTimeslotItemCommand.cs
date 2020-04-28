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

namespace TuitionApp.Core.Features.Timeslot
{
    public class CreateTimeslotItemCommand : IRequest<CreateTimeslotItem>
    {
        public int Day { get; set; }
        public int Week { get; set; }
        public TimeSpan Time { get; set; }
        public Guid RoomId { get; set; }
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
                var classroom = await context.Classrooms.SingleOrDefaultAsync(l => l.Id.Equals(request.RoomId));
                if (classroom == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Classroom), request.RoomId);
                }

                var timeslot = await context.Timeslots
                    .Where(l => l.Classroom.Equals(classroom))
                    .Where(l => l.WeekNumber == request.Week)
                    .Where(l => l.Day == request.Day)
                    .Where(l => l.Time == request.Time)
                    .SingleOrDefaultAsync();

                if (timeslot != null)
                {
                    throw new EntityAlreadyExistException(nameof(Domain.Entities.Timeslot), timeslot.Id);
                }

                var session = await context.Sessions.SingleOrDefaultAsync(l => l.Id.Equals(request.SessionId));
                if (session == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Session), request.SessionId);
                }

                var entity = new Domain.Entities.Timeslot()
                {
                    Classroom = classroom,
                    Session = session,
                    WeekNumber = request.Week,
                    Day = request.Day,
                    Time = request.Time,
                    
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
