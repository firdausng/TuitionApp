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
        public Guid ClassSubjectId { get; set; }

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
                    throw new EntityNotFoundException(nameof(DailySchedule), request.DailyScheduleId);
                }

                var bookedTimeslots = dailySchedule.Timeslots.GetOverlapTimeslot(request.StartTime, request.Duration);

                if (bookedTimeslots.Count > 0)
                {
                    throw new EntityAlreadyExistException(nameof(Timeslot), string.Join(",", bookedTimeslots.Select(b => b.Id.ToString()).ToArray()));
                }

                var classSubject = await context.ClassSubjects
                    .Include(w => w.CourseClass)
                    .ThenInclude(cc => cc.Enrollments)
                    .SingleOrDefaultAsync(l => l.Id.Equals(request.ClassSubjectId));
                if (classSubject == null)
                {
                    throw new EntityNotFoundException(nameof(ClassSubject), request.ClassSubjectId);
                }

                var entity = new Timeslot()
                {
                    DailySchedule = dailySchedule,
                    ClassSubject = classSubject,
                    Disabled = request.Disabled,
                    Duration = request.Duration,
                    StartTime = request.StartTime,

                };
                context.Timeslots.Add(entity);

                // check if there are any enrollment created yet
                if (classSubject.CourseClass.Enrollments.Count > 0)
                {
                    foreach (var enrollment in classSubject.CourseClass.Enrollments)
                    {
                        context.Attendances.Add(new Attendance
                        {
                            Enrollment = enrollment,
                            Timeslot = entity,
                            AttendanceStatus = AttendanceStatus.Created,
                        });
                    }
                }
                

                await context.SaveChangesAsync(cancellationToken);

                return new CreateTimeslotItemDto
                {
                    Id = entity.Id
                };
            }

            public bool IstimeslotOverlapping(Timeslot currentTimeslot, List<Timeslot> timeslots)
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
