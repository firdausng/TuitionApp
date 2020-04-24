using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuitionApp.Api.Models
{
    public class LocationClassroomCreateRequest
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool IsEnabled { get; set; }
    }
}
