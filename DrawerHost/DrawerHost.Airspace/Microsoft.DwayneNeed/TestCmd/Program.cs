using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DwayneNeed.Utilities;

namespace TestCmd
{
    class Program
    {
        [CommandLineParameter("pf")]
        public static bool PrivateFoo { get; set; }

        [CommandLineParameter("f", IsRequired = true)]
        public static bool Foo { get; set; }

        [CommandLineParameter("rf", ShortDescription="server name")]
        public static bool ReadonlyFoo { get { return true; } }

        [CommandLineParameter("b", ShortDescription="test case", IsRequired=true)]
        public static int Bar { get; set; }

        public static int IgnoreBar { get; set; }

        static void Main(string[] args)
        {
            try
            {
                CommandLineParser.Parse<Program>(args);
            }
            catch (CommandLineUsageException e)
            {
                CommandLineParser.PrintUsageException("TestCmd", e);
            }
        }
    }
}
