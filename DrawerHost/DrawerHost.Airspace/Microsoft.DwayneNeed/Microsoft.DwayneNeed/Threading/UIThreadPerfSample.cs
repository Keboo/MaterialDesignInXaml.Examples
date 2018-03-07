using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Threading
{
    /// <summary>
    ///     This class represents a perf sample taken from a UI thread.  The
    ///     time stamp of the same is usually taken from the RenderingEventArgs
    ///     
    /// </summary>
    internal class UIThreadPerfSample
    {
        public UIThreadPerfSample(TimeSpan sampleTime,
                                  int frameCount,
                                  Int64 processCycleTime,
                                  Int64 idleCycleTime)
        {
            SampleTime = sampleTime;
            FrameCount = frameCount;
            ProcessCycleTime = processCycleTime;
            IdleCycleTime = idleCycleTime;
        }

        public TimeSpan SampleTime { get; private set; }
        public int FrameCount { get; private set; }
        public Int64 ProcessCycleTime { get; private set; }
        public Int64 IdleCycleTime { get; private set; }
    }
}
