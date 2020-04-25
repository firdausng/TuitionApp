using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Student
{
    public class GetStudentItemQuery : IRequest<GetStudentItemDto>
    {
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetStudentItemQuery, GetStudentItemDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetStudentItemDto> Handle(GetStudentItemQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.Students
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                var dto = new GetStudentItemDto
                {
                    Id = entity.Id,
                    Name = $"{entity.FirstName} {entity.LastName}",
                };

                return dto;
            }
        }

    }
}
