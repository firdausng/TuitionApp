using System;

namespace TuitionApp.Core.Features.Location
{
    public class GetLocationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
