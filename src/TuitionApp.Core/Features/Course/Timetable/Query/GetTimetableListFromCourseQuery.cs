using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Features.Common;

namespace TuitionApp.Core.Features.Course
{
    public class GetTimetableListFromCourseQuery : IRequest<GetObjectListVm<GetTimetableItemFromCourseDto>>
    {
        public Guid CourseId { get; set; }

        public class QueryHandler : IRequestHandler<GetTimetableListFromCourseQuery, GetObjectListVm<GetTimetableItemFromCourseDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetTimetableItemFromCourseDto>> Handle(GetTimetableListFromCourseQuery request, CancellationToken cancellationToken)
            {
                var course = await context.Courses.SingleOrDefaultAsync(l => l.Id.Equals(request.CourseId));
                if (course == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Course), request.CourseId);
                }

                var timetables = await context.Timetables
                    .Where(x => x.CourseId.Equals(request.CourseId))
                    .ToListAsync(cancellationToken);

                var list = timetables
                    .Select(x => new GetTimetableItemFromCourseDto
                    {
                        Id = x.Id,
                    }).ToList();

                var dto = new GetObjectListVm<GetTimetableItemFromCourseDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }

    }
}
