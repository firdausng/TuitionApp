using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Location
{
    public class CreateLocationCommand : IRequest<CreateLocationDto>
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }

        public class CommandHandler : IRequestHandler<CreateLocationCommand, CreateLocationDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateLocationDto> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
            {
                var entity = new Domain.Entities.Location()
                {
                    Name = request.Name,
                    IsEnabled = request.IsEnabled
                };
                context.Locations.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateLocationDto
                {
                    Id = entity.Id
                };
            }
        }
    }

    public class CreateLocationDto
    {
        public Guid Id { get; set; }
    }
}
