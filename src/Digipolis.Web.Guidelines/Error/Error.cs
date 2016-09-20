using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Guidelines.Error
{
    public class Error
    {
        /// <summary>
        /// A unique id to identify error messages in the logs
        /// </summary>
        public Guid Identifier { get; private set; }

        /// <summary>
        /// A URI to an absolute or relative html resource to identify the problem.
        /// </summary>
        public Uri Type { get; set; }

        /// <summary>
        /// A short description of the error
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The http Status code 
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// A code to identify what error it is.
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Extra parameters to clarify the error
        /// </summary>
        public Dictionary<string, object> ExtraParameters { get; set; }

        public Error()
        {
            Identifier = Guid.NewGuid();
            ExtraParameters = new Dictionary<string, object>();
        }
    }
}
