﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Course
{
    public class GetCourseItemQuery : IRequest<GetCourseItemDto>
    {
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetCourseItemQuery, GetCourseItemDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetCourseItemDto> Handle(GetCourseItemQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.Courses
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                var dto = new GetCourseItemDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Rate = entity.Rate
                };

                return dto;
            }
        }

    }
}
