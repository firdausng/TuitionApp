using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.CalendarSettings
{
    public class CreateCalendarSettingItemCommand : IRequest<CreateCalendarSettingDto>
    {
        public DayOfWeek FirstDayOfWeek { get; set; }
        public TimeSpan DefaultClosingTime { get; set; }
        public TimeSpan DefaultOpeningTime { get; set; }
        public bool AllowedTimeslotOverlap { get; set; }

        public class CommandHandler : IRequestHandler<CreateCalendarSettingItemCommand, CreateCalendarSettingDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateCalendarSettingDto> Handle(CreateCalendarSettingItemCommand request, CancellationToken cancellationToken)
            {
                var entity = new Domain.Entities.CalendarSetting()
                {
                    FirstDayOfWeek = request.FirstDayOfWeek,
                    DefaultClosingTime = request.DefaultClosingTime,
                    DefaultOpeningTime = request.DefaultOpeningTime,
                    AllowedTimeslotOverlap = request.AllowedTimeslotOverlap,
                };
                context.CalendarSettings.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateCalendarSettingDto
                {
                    Id = entity.Id
                };
            }
        }
    }

    public class CreateCalendarSettingDto
    {
        public Guid Id { get; set; }
    }
}
