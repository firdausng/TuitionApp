using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Subject
{
    public class CreateSubjectCommand : IRequest<CreateSubjectDto>
    {
        public string Title { get; set; }

        public class CommandHandler : IRequestHandler<CreateSubjectCommand, CreateSubjectDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateSubjectDto> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
            {
                var entity = new Domain.Entities.Subject()
                {
                    Title = request.Title,
                };
                context.Subjects.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateSubjectDto
                {
                    Id = entity.Id
                };
            }
        }
    }

    public class CreateSubjectDto
    {
        public Guid Id { get; set; }
    }
}
