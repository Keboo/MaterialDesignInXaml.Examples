using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Utilities
{
    /// <summary>
    ///     The CommandLineParameter attribute can be used to indicate how
    ///     properties of a type are mapped to command line parameters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandLineParameterAttribute : Attribute
    {
        public CommandLineParameterAttribute(string name)
        {
            this.Name = name;
        }

        public string Name;
        public string ShortDescription;
        public bool IsRequired;
    }
}
