using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.DailySchedule
{
    public class GetDailyScheduleItemQuery : IRequest<GetDailyScheduleItemDto>
    {
        public Guid Id { get; set; }


        public class QueryHandler : IRequestHandler<GetDailyScheduleItemQuery, GetDailyScheduleItemDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetDailyScheduleItemDto> Handle(GetDailyScheduleItemQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.DailySchedules
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                var dto = new GetDailyScheduleItemDto
                {
                    Id = entity.Id,
                    ClassroomId = entity.ClassroomId,
                    DateSchedule = entity.DateSchedule,
                    DayOfWeek = entity.DayOfWeek,
                    Disabled = entity.Disabled,
                    WeekNumber = entity.WeekNumber,
                };

                return dto;
            }
        }

    }
}
