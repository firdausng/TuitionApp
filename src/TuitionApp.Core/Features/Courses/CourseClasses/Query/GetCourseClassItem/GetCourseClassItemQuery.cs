using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Courses.CourseClasses
{
    public class GetCourseClassItemQuery : IRequest<GetCourseClassItemDto>
    {
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetCourseClassItemQuery, GetCourseClassItemDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetCourseClassItemDto> Handle(GetCourseClassItemQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.CourseClasses
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                var dto = new GetCourseClassItemDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    CourseId = entity.CourseId,
                };

                return dto;
            }
        }

    }
}
