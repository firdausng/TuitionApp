using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Course
{
    public class GetTimetableItemFromCourseQuery : IRequest<GetTimetableItemFromCourseDto>
    {
        public Guid CourseId { get; set; }
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetTimetableItemFromCourseQuery, GetTimetableItemFromCourseDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetTimetableItemFromCourseDto> Handle(GetTimetableItemFromCourseQuery request, CancellationToken cancellationToken)
            {
                var course = await context.Courses.SingleOrDefaultAsync(l => l.Id.Equals(request.CourseId));
                if (course == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Course), request.CourseId);
                }

                var entity = await context.Timetables
                    .SingleOrDefaultAsync(x => x.CourseId.Equals(request.CourseId));

                if (entity == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Timetable), request.Id);
                }

                var dto = new GetTimetableItemFromCourseDto
                {
                    Id = entity.Id,
                };

                return dto;
            }
        }

    }
}
