using System;
using System.Collections.Generic;
using System.Text;

namespace TuitionApp.Core.Common.Exceptions
{
    public class EntityAlreadyExistException: Exception, IAppException
    {
        public EntityAlreadyExistException(string name, object key)
            : base($"Entity '{name}' ({key}) was already exist.")
        {
        }
    }
}
