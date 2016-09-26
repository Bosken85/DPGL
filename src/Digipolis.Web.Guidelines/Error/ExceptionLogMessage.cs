using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Guidelines.Error
{
    public class ExceptionLogMessage
    {
        public Error Error { get; set; }

        public Exception Exception { get; set; }
    }
}
