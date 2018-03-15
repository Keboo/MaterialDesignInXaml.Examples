using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Microsoft.DwayneNeed.Utilities
{
    [Serializable]
    public class CommandLineParameterException : Exception
    {
        public CommandLineParameterException()
            : base()
        {
        }

        public CommandLineParameterException(string message)
            : base(message)
        {
        }

        public CommandLineParameterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CommandLineParameterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                //this.fUserName = info.GetString("fUserName");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info != null)
            {
                // info.AddValue("fUserName", this.fUserName);
            }
        }
    }
}
