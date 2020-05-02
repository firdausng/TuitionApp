using MediatR;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Students
{
    public class CreateStudentItemCommand : IRequest<CreateStudentDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public class CommandHandler : IRequestHandler<CreateStudentItemCommand, CreateStudentDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateStudentDto> Handle(CreateStudentItemCommand request, CancellationToken cancellationToken)
            {
                var entity = new Domain.Entities.Student()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                };
                context.Students.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateStudentDto
                {
                    Id = entity.Id
                };
            }
        }
    }
}
