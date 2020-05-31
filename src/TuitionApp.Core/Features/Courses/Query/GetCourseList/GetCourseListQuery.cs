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

namespace TuitionApp.Core.Features.Courses
{
    public class GetCourseListQuery : IRequest<GetObjectListVm<GetCourseItemDto>>
    {
        public class QueryHandler : IRequestHandler<GetCourseListQuery, GetObjectListVm<GetCourseItemDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetCourseItemDto>> Handle(GetCourseListQuery request, CancellationToken cancellationToken)
            {
                var courses = await context.Courses
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var list = courses
                    .Select(x => new GetCourseItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Rate = x.Rate,
                    }).ToList();


                var dto = new GetObjectListVm<GetCourseItemDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }
    }
}
