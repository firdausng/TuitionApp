using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Classroom
{
    public class GetSubjectQuery : IRequest<GetSubjectDto>
    {
        public Guid Id { get; set; }
        public class GetClassroomItemQueryHandler : IRequestHandler<GetSubjectQuery, GetSubjectDto>
        {
            private readonly IApplicationDbContext context;

            public GetClassroomItemQueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetSubjectDto> Handle(GetSubjectQuery request, CancellationToken cancellationToken)
            {
                var classroom = await context.Classrooms
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                var dto = new GetSubjectDto
                {
                    Id = classroom.Id,
                    Name = classroom.Name,
                    Capacity = classroom.Capacity
                };

                return dto;
            }
        }
    }
}
