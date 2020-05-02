using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Courses.Sessions
{
    public class GetSessionItemFromCourseQuery : IRequest<GetSessionItemFromCourseDto>
    {
        public Guid CourseId { get; set; }
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetSessionItemFromCourseQuery, GetSessionItemFromCourseDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetSessionItemFromCourseDto> Handle(GetSessionItemFromCourseQuery request, CancellationToken cancellationToken)
            {
                var course = await context.Courses.SingleOrDefaultAsync(l => l.Id.Equals(request.CourseId));
                if (course == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Course), request.CourseId);
                }

                var entity = await context.Sessions
                    .SingleOrDefaultAsync(x => x.CourseId.Equals(request.CourseId));

                if (entity == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Session), request.Id);
                }

                var dto = new GetSessionItemFromCourseDto
                {
                    Id = entity.Id,
                };

                return dto;
            }
        }

    }
}
