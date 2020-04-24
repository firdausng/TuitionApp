using System;
using System.Collections.Generic;
using System.Text;

namespace TuitionApp.Core.Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string name, object key)
            : base($"Entity '{name}' ({key}) was not found.")
        {
        }
    }
}
