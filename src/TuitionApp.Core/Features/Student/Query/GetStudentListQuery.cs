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

namespace TuitionApp.Core.Features.Student
{
    public class GetWeeklyScheduleListQuery : IRequest<GetObjectListVm<GetStudentItemDto>>
    {
        public class QueryHandler : IRequestHandler<GetWeeklyScheduleListQuery, GetObjectListVm<GetStudentItemDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetStudentItemDto>> Handle(GetWeeklyScheduleListQuery request, CancellationToken cancellationToken)
            {
                var students = await context.Students
                    .ToListAsync(cancellationToken);

                var list = students
                    .Select(x => new GetStudentItemDto
                    {
                        Id = x.Id,
                        Name = $"{x.FirstName} {x.LastName}",
                    }).ToList();


                var dto = new GetObjectListVm<GetStudentItemDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }
    }
}
