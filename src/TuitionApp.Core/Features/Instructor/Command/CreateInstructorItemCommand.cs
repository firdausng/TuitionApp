using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Instructor
{
    public class CreateInstructorItemCommand : IRequest<CreateInstructorItemDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime HireDate { get; set; }

        public class CommandHandler : IRequestHandler<CreateInstructorItemCommand, CreateInstructorItemDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateInstructorItemDto> Handle(CreateInstructorItemCommand request, CancellationToken cancellationToken)
            {
                var entity = new Domain.Entities.Instructor()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    HireDate = request.HireDate,
                };
                context.Instructor.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateInstructorItemDto
                {
                    Id = entity.Id
                };
            }
        }
    }
}
