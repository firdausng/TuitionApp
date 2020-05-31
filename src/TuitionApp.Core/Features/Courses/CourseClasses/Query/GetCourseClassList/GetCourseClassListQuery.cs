using FluentValidation;
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

namespace TuitionApp.Core.Features.Courses.CourseClasses
{
    public class GetCourseClassListQuery : IRequest<GetObjectListVm<GetCourseClassItemDto>>
    {
        public class QueryHandler : IRequestHandler<GetCourseClassListQuery, GetObjectListVm<GetCourseClassItemDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetCourseClassItemDto>> Handle(GetCourseClassListQuery request, CancellationToken cancellationToken)
            {
                var courseClasses = await context.CourseClasses
                    .AsNoTracking()
                    .Include(cc => cc.Enrollments)
                    .ToListAsync(cancellationToken);

                var list = courseClasses
                    .Select(x => new GetCourseClassItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CourseId = x.Id,
                        Capacity = x.Capacity,
                        CapacityLeft = x.Capacity - x.Enrollments.Count
                    }).ToList();


                var dto = new GetObjectListVm<GetCourseClassItemDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }
    }
}
