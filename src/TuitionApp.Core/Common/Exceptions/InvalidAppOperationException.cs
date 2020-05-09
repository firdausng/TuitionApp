﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionApp.Core.Common.Exceptions
{
    public class InvalidAppOperationException: InvalidOperationException, IAppException
    {
        public InvalidAppOperationException(string error):base(error)
        {

        }
    }
}
