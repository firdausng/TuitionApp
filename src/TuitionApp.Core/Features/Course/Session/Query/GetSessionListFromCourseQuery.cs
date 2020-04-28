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
    public class GetSessionListFromCourseQuery : IRequest<GetObjectListVm<GetSessionItemFromCourseDto>>
    {
        public Guid CourseId { get; set; }

        public class QueryHandler : IRequestHandler<GetSessionListFromCourseQuery, GetObjectListVm<GetSessionItemFromCourseDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetSessionItemFromCourseDto>> Handle(GetSessionListFromCourseQuery request, CancellationToken cancellationToken)
            {
                var course = await context.Courses.SingleOrDefaultAsync(l => l.Id.Equals(request.CourseId));
                if (course == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Course), request.CourseId);
                }

                var sessions = await context.Sessions
                    .Where(x => x.CourseId.Equals(request.CourseId))
                    .ToListAsync(cancellationToken);

                var list = sessions
                    .Select(x => new GetSessionItemFromCourseDto
                    {
                        Id = x.Id,
                    }).ToList();

                var dto = new GetObjectListVm<GetSessionItemFromCourseDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }

    }
}
