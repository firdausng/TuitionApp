using System.Collections.Generic;

namespace TuitionApp.Core.Features.Common
{
    public class GetObjectListVm<T>
    {
        public IList<T> Data { get; set; }
        public int Count { get; set; }
    }
}
