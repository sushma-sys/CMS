using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Exceptions
{
    public class CmsExceptions : ApplicationException
    {
        public CmsExceptions()
        {
        }

        public CmsExceptions(string message) : base(message)
        {
        }

        public CmsExceptions(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CmsExceptions(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
