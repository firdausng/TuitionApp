﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.WeeklySchedule.Timeslot
{
    public class GetTimeslotItemQuery : IRequest<GetTimeslotItemDto>
    {
        public Guid Id { get; set; }


        public class QueryHandler : IRequestHandler<GetTimeslotItemQuery, GetTimeslotItemDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetTimeslotItemDto> Handle(GetTimeslotItemQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.Timeslots
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                var dto = new GetTimeslotItemDto
                {
                    Id = entity.Id,
                    Duration = entity.Duration,
                    StartTime = entity.StartTime,
                    SessionId = entity.SessionId,
                    Disabled = entity.Disabled,
                    WeeklyScheduleId = entity.WeeklyScheduleId,
                };

                return dto;
            }
        }

    }
}