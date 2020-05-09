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

namespace TuitionApp.Core.Features.Enrollments
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

                if (entity == null)
                {
                    throw new EntityNotFoundException(nameof(Enrollment), request.Id);
                }

                var dto = new GetEnrollmentItemDto
                {
                    Id = entity.Id,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    Grade = entity.Grade,
                    StudentId = entity.StudentId,
                    CourseClassIdId = entity.CourseClassId,
                };

                return dto;
            }
        }

    }
}
