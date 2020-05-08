using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Core.Features.CalendarSettings
{
    public class GetCalendarSettingItemQuery : IRequest<GetCalendarSettingItemDto>
    {
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetCalendarSettingItemQuery, GetCalendarSettingItemDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetCalendarSettingItemDto> Handle(GetCalendarSettingItemQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.CalendarSettings
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    throw new EntityNotFoundException(nameof(CalendarSetting), request.Id);
                }

                var dto = new GetCalendarSettingItemDto
                {
                    Id = entity.Id,
                    FirstDayOfWeek = entity.FirstDayOfWeek,
                    DefaultOpeningTime = entity.DefaultOpeningTime,
                    DefaultClosingTime = entity.DefaultClosingTime,
                    AllowedTimeslotOverlap = entity.AllowedTimeslotOverlap,
                };

                return dto;
            }
        }

    }
}
