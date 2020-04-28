using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Enrollment
{
    public class GetEnrollmentItemQuery : IRequest<GetEnrollmentItemDto>
    {
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetEnrollmentItemQuery, GetEnrollmentItemDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetEnrollmentItemDto> Handle(GetEnrollmentItemQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.Enrollments
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                var dto = new GetEnrollmentItemDto
                {
                    Id = entity.Id,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    Grade = entity.Grade,
                    StudentId = entity.StudentId,
                    SessionId = entity.SessionId,
                };

                return dto;
            }
        }

    }
}
