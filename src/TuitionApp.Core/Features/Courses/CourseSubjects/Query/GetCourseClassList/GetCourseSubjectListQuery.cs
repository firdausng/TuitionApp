using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Features.Common;

namespace TuitionApp.Core.Features.Courses.CourseSubjects
{
    public class GetCourseSubjectListQuery : IRequest<GetObjectListVm<GetCourseSubjectItemDto>>
    {
        public class QueryHandler : IRequestHandler<GetCourseSubjectListQuery, GetObjectListVm<GetCourseSubjectItemDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetCourseSubjectItemDto>> Handle(GetCourseSubjectListQuery request, CancellationToken cancellationToken)
            {
                var courseSubjectList = await context.CourseSubjects
                    .ToListAsync(cancellationToken);

                var list = courseSubjectList
                    .Select(x => new GetCourseSubjectItemDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        CourseId = x.CourseId,
                        SubjectAssignmentId = x.SubjectAssignmentId,
                    }).ToList();


                var dto = new GetObjectListVm<GetCourseSubjectItemDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }
    }
}
