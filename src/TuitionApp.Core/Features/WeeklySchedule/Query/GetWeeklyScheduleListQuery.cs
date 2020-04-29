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

namespace TuitionApp.Core.Features.WeeklySchedule
{
    public class GetWeeklyScheduleListQuery : IRequest<GetObjectListVm<GetWeeklyScheduleItemDto>>
    {
        public class QueryHandler : IRequestHandler<GetWeeklyScheduleListQuery, GetObjectListVm<GetWeeklyScheduleItemDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetWeeklyScheduleItemDto>> Handle(GetWeeklyScheduleListQuery request, CancellationToken cancellationToken)
            {
                var WeeklySchedules = await context.WeeklySchedules
                    .ToListAsync(cancellationToken);

                var list = WeeklySchedules
                    .Select(entity => new GetWeeklyScheduleItemDto
                    {
                        Id = entity.Id,
                        ClassroomId = entity.ClassroomId,
                        DateSchedule = entity.DateSchedule,
                        DayOfWeek = entity.DayOfWeek,
                        Disabled = entity.Disabled,
                        WeekNumber = entity.WeekNumber,
                    }).ToList();


                var dto = new GetObjectListVm<GetWeeklyScheduleItemDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }
    }
}
