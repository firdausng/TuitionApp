using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.WeeklySchedule
{
    public class GetWeeklyScheduleItemQuery : IRequest<GetWeeklyScheduleItemDto>
    {
        public Guid Id { get; set; }


        public class QueryHandler : IRequestHandler<GetWeeklyScheduleItemQuery, GetWeeklyScheduleItemDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetWeeklyScheduleItemDto> Handle(GetWeeklyScheduleItemQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.WeeklySchedules
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                var dto = new GetWeeklyScheduleItemDto
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
