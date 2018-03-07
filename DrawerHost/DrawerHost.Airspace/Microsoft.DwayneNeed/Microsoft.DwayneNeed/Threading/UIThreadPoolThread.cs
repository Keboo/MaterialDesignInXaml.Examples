using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using Microsoft.DwayneNeed.Controls;
using System.Windows.Media;

namespace Microsoft.DwayneNeed.Threading
{
    public sealed class UIThreadPoolThread : IDisposable
    {
        internal UIThreadPoolThread(Dispatcher dispatcher)
        {
            UIThreadPool.IncrementThread(dispatcher);
            Dispatcher = dispatcher;
        }

        ~UIThreadPoolThread()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (Dispatcher == null)
            {
                throw new ObjectDisposedException("UIThreadPoolThread");
            }

            Dispatcher dispatcher = Dispatcher;
            Dispatcher = null;

            UIThreadPool.DecrementThread(dispatcher);
        }

        public Dispatcher Dispatcher { get; private set; }
    }
}
