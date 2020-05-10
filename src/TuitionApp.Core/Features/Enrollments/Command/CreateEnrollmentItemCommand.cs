using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Core.Features.Enrollments
{
    public class CreateEnrollmentItemCommand : IRequest<CreateEnrollmentItemDto>
    {
        public DateTime StartDate { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseClassId { get; set; }

        public class CommandHandler : IRequestHandler<CreateEnrollmentItemCommand, CreateEnrollmentItemDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateEnrollmentItemDto> Handle(CreateEnrollmentItemCommand request, CancellationToken cancellationToken)
            {
                var student = await context.Students.SingleOrDefaultAsync(l => l.Id.Equals(request.StudentId));
                if (student == null)
                {
                    throw new EntityNotFoundException(nameof(Student), request.StudentId);
                }

                var courseClassQuery = context.CourseClasses
                    .Include(cc => cc.Enrollments)
                    .Include(cc => cc.ClassSubjects)
                    .ThenInclude(cs => cs.Timeslots);
                var courseClass = await courseClassQuery.SingleOrDefaultAsync(l => l.Id.Equals(request.CourseClassId));
                if (courseClass == null)
                {
                    throw new EntityNotFoundException(nameof(CourseClass), request.CourseClassId);
                }

                if (courseClass.Enrollments.Count >= courseClass.Capacity)
                {
                    throw new InvalidAppOperationException("No more slot, class already in full capacity");
                }

                var entity = new Enrollment()
                {
                    StartDate = request.StartDate,
                    Student = student,
                    CourseClass = courseClass,
                };
                context.Enrollments.Add(entity);

                foreach (var classSubject in courseClass.ClassSubjects)
                {
                    foreach (var timeslot in classSubject.Timeslots)
                    {
                        context.Attendances.Add(new Attendance
                        {
                            Enrollment = entity,
                            Timeslot = timeslot,
                            AttendanceStatus = AttendanceStatus.Created,
                        });
                    }
                }


                await context.SaveChangesAsync(cancellationToken);

                return new CreateEnrollmentItemDto
                {
                    Id = entity.Id
                };
            }
        }
    }
}
