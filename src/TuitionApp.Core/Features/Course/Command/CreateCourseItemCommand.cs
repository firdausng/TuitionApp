using MediatR;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Course
{
    public class CreateCourseItemCommand : IRequest<CreateCourseItemDto>
    {
        public string Name { get; set; }
        public int Rate { get; set; }

        public class CommandHandler : IRequestHandler<CreateCourseItemCommand, CreateCourseItemDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateCourseItemDto> Handle(CreateCourseItemCommand request, CancellationToken cancellationToken)
            {
                var entity = new Domain.Entities.Course()
                {
                    Name = request.Name,
                    Rate = request.Rate
                };
                context.Courses.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateCourseItemDto
                {
                    Id = entity.Id
                };
            }
        }
    }
}
