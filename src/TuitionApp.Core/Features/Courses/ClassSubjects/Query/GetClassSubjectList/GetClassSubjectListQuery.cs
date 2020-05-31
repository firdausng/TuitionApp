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

namespace TuitionApp.Core.Features.Courses.ClassSubjects
{
    public class GetClassSubjectListQuery : IRequest<GetObjectListVm<GetClassSubjectItemDto>>
    {
        public class QueryHandler : IRequestHandler<GetClassSubjectListQuery, GetObjectListVm<GetClassSubjectItemDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetClassSubjectItemDto>> Handle(GetClassSubjectListQuery request, CancellationToken cancellationToken)
            {
                var courseSubjectList = await context.ClassSubjects
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var list = courseSubjectList
                    .Select(x => new GetClassSubjectItemDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        CourseId = x.CourseClassId,
                        SubjectAssignmentId = x.SubjectAssignmentId,
                    }).ToList();


                var dto = new GetObjectListVm<GetClassSubjectItemDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }
    }
}
