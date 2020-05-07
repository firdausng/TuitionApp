using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Courses.CourseSubjects
{
    public class GetCourseSubjectItemQuery : IRequest<GetCourseSubjectItemDto>
    {
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetCourseSubjectItemQuery, GetCourseSubjectItemDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetCourseSubjectItemDto> Handle(GetCourseSubjectItemQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.CourseSubjects
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                var dto = new GetCourseSubjectItemDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    CourseId = entity.CourseId,
                    SubjectAssignmentId = entity.SubjectAssignmentId,
                };

                return dto;
            }
        }

    }
}
