using FluentValidation;
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

namespace TuitionApp.Core.Features.Courses.Sessions
{
    public class CreateSessionFromCourseCommand : IRequest<CreateSessionFromCourseDto>
    {
        public Guid CourseId { get; set; }
        public ICollection<Guid> Timeslots { get; set; } = new List<Guid>();

        public class CommandHandler : IRequestHandler<CreateSessionFromCourseCommand, CreateSessionFromCourseDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateSessionFromCourseDto> Handle(CreateSessionFromCourseCommand request, CancellationToken cancellationToken)
            {
                var course = await context.Courses.SingleOrDefaultAsync(l => l.Id.Equals(request.CourseId));
                if (course == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Course), request.CourseId);
                }

                var entity = new Session()
                {
                    Course = course,
                };

                if (request.Timeslots.Count > 0)
                {
                    var timeslotListDbQuery = context.Timeslots
                    .Where(l => request.Timeslots.Contains(l.Id));
                    var timeslotList = timeslotListDbQuery.ToList();

                    if (timeslotList.Count != request.Timeslots.Count)
                    {
                        throw new EntityListCountMismatchException<Timeslot>(timeslotList, request.Timeslots);
                    }

                    foreach (var timeslot in timeslotList)
                    {
                        var booking = new Booking
                        {
                            Session = entity,
                            Timeslot = timeslot
                        };
                        entity.Bookings.Add(booking);
                    }
                }


                context.Sessions.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateSessionFromCourseDto
                {
                    Id = entity.Id
                };
            }
        }
    }
}
