using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Location
{
    public class CreateLocationItemCommand : IRequest<CreateLocationItemDto>
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }

        public class CommandHandler : IRequestHandler<CreateLocationItemCommand, CreateLocationItemDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateLocationItemDto> Handle(CreateLocationItemCommand request, CancellationToken cancellationToken)
            {
                var entity = new Domain.Entities.Location()
                {
                    Name = request.Name,
                    IsEnabled = request.IsEnabled
                };
                context.Locations.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateLocationItemDto
                {
                    Id = entity.Id
                };
            }
        }
    }

    public class CreateLocationItemDto
    {
        public Guid Id { get; set; }
    }
}
