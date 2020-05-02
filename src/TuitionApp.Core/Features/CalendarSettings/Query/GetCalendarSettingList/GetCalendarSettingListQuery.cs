using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Features.Common;

namespace TuitionApp.Core.Features.CalendarSettings
{
    public class GetCalendarSettingListQuery : IRequest<GetObjectListVm<GetCalendarSettingItemDto>>
    {
        public class QueryHandler : IRequestHandler<GetCalendarSettingListQuery, GetObjectListVm<GetCalendarSettingItemDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetCalendarSettingItemDto>> Handle(GetCalendarSettingListQuery request, CancellationToken cancellationToken)
            {
                var calendarSettings = await context.CalendarSettings
                    .ToListAsync(cancellationToken);

                var list = calendarSettings
                    .Select(entity => new GetCalendarSettingItemDto
                    {
                        Id = entity.Id,
                        FirstDayOfWeek = entity.FirstDayOfWeek,
                        DefaultOpeningTime = entity.DefaultOpeningTime,
                        DefaultClosingTime = entity.DefaultClosingTime,
                        AllowedTimeslotOverlap = entity.AllowedTimeslotOverlap,
                    }).ToList();


                var dto = new GetObjectListVm<GetCalendarSettingItemDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }
    }
}
