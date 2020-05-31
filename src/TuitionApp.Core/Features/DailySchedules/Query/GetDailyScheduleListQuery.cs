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

namespace TuitionApp.Core.Features.DailySchedules
{
    public class GetDailyScheduleListQuery : IRequest<GetObjectListVm<GetDailyScheduleItemDto>>
    {
        public class QueryHandler : IRequestHandler<GetDailyScheduleListQuery, GetObjectListVm<GetDailyScheduleItemDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetDailyScheduleItemDto>> Handle(GetDailyScheduleListQuery request, CancellationToken cancellationToken)
            {
                var dailySchedules = await context.DailySchedules
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var list = dailySchedules
                    .Select(entity => new GetDailyScheduleItemDto
                    {
                        Id = entity.Id,
                        ClassroomId = entity.ClassroomId,
                        DateSchedule = entity.DateSchedule,
                        DayOfWeek = entity.DayOfWeek,
                        Disabled = entity.Disabled,
                        WeekNumber = entity.WeekNumber,
                    }).ToList();


                var dto = new GetObjectListVm<GetDailyScheduleItemDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }
    }
}
