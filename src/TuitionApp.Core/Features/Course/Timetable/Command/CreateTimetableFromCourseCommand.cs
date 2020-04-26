using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Course
{
    public class CreateTimetableFromCourseCommand : IRequest<CreateTimetableFromCourseDto>
    {
        public Guid CourseId { get; set; }

        public class CommandHandler : IRequestHandler<CreateTimetableFromCourseCommand, CreateTimetableFromCourseDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateTimetableFromCourseDto> Handle(CreateTimetableFromCourseCommand request, CancellationToken cancellationToken)
            {
                var course = await context.Courses.SingleOrDefaultAsync(l => l.Id.Equals(request.CourseId));
                if (course == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Course), request.CourseId);
                }

                var entity = new Domain.Entities.Timetable()
                {
                    Course = course,
                };
                context.Timetables.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateTimetableFromCourseDto
                {
                    Id = entity.Id
                };
            }
        }
    }

    public class CreateTimetableFromCourseDto
    {
        public Guid Id { get; set; }
    }
}
