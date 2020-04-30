﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Features.Common;

namespace TuitionApp.Core.Features.WeeklySchedule.Timeslot
{
    public class GetTimeslotListQuery : IRequest<GetObjectListVm<GetTimeslotItemDto>>
    {
        public class QueryHandler : IRequestHandler<GetTimeslotListQuery, GetObjectListVm<GetTimeslotItemDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetTimeslotItemDto>> Handle(GetTimeslotListQuery request, CancellationToken cancellationToken)
            {
                var WeeklySchedules = await context.Timeslots
                    .ToListAsync(cancellationToken);

                var list = WeeklySchedules
                    .Select(entity => new GetTimeslotItemDto
                    {
                        Id = entity.Id,
                        Duration = entity.Duration,
                        StartTime = entity.StartTime,
                        SessionId = entity.SessionId,
                        Disabled = entity.Disabled,
                        WeeklyScheduleId = entity.WeeklyScheduleId,
                    }).ToList();


                var dto = new GetObjectListVm<GetTimeslotItemDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }
    }
}