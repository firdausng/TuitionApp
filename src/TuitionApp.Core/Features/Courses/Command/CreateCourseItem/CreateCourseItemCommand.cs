using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Core.Features.Courses
{
    public class CreateCourseItemCommand : IRequest<CreateCourseItemDto>
    {
        public string Name { get; set; }
        public int Rate { get; set; }
        public ICollection<string> ClassNameList { get; set; } = new List<string>();
        public ICollection<Guid> SubjectIdList { get; set; } = new List<Guid>();

        public class CommandHandler : IRequestHandler<CreateCourseItemCommand, CreateCourseItemDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateCourseItemDto> Handle(CreateCourseItemCommand request, CancellationToken cancellationToken)
            {
                var entity = new Course()
                {
                    Name = request.Name,
                    Rate = request.Rate
                };

                if (request.ClassNameList.Count > 0)
                {
                    foreach (var className in request.ClassNameList)
                    {
                        entity.CourseClasses.Add(new CourseClass
                        {
                            Course = entity,
                            Name = className
                        });
                    }
                }

                if (request.SubjectIdList.Count > 0)
                {
                    foreach (var subjectId in request.SubjectIdList)
                    {
                        entity.CourseSubjects.Add(new CourseSubject
                        {
                            CourseId = entity.Id,
                            SubjectId = subjectId
                        });
                    }
                }

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
