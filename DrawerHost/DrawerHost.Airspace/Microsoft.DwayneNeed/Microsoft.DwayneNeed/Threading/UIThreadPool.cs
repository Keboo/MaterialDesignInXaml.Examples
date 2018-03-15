using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DwayneNeed.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Threading;
using System.Diagnostics;

namespace Microsoft.DwayneNeed.Threading
{
    public static class UIThreadPool
    {
        static UIThreadPool()
        {
            _minThreads = 0;
            _maxThreads = System.Environment.ProcessorCount;
            _numThreads = 0;
        }

        /// <summary>
        ///     Specifies the preferred minimum number of threads in the pool.
        /// </summary>
        /// <remarks>
        ///     Threads will be created to ensure the minimum number of
        ///     threads exist in the pool.
        /// </remarks>
        public static int MinThreads
        {
            get
            {
                return _minThreads;
            }

            set
            {
                lock (_guard)
                {
                    _minThreads = value;

                    while (_numThreads < _minThreads)
                    {
                        AddNewThreadToPool();
                    }
                }
            }
        }

        /// <summary>
        ///     Specifies the preferred maximum number of threads in the pool.
        /// </summary>
        /// <remarks>
        ///     Because UI objects are bound to their owner threads, it is not
        ///     safe to destroy a thread that has live UIThreadPoolThread
        ///     references. However, further calls to AcquireThread will be
        ///     constrained to the maximum, and the threads will be reclaimed
        ///     as they become available.
        /// </remarks>
        public static int MaxThreads
        {
            get
            {
                return _maxThreads;
            }

            set
            {
                lock (_guard)
                {
                    _maxThreads = value;
                    while (_numThreads > _maxThreads)
                    {
                        Dispatcher dispatcher = GetLeastReferencedThread(true);
                        RemoveThreadFromPool(dispatcher);
                    }
                }
            }
        }

        /// <summary>
        ///     Returns a UIThreadPoolThread instance that provides access to
        ///     a worker thread from the UIThreadPool.
        /// </summary>
        /// <remarks>
        ///     The UIThreadPool class returns unique UIThreadPoolThread
        ///     instances for every call to RequestThread.  Multiple
        ///     UIThreadPoolThread instances may actually reference the
        ///     same worker thread.  The caller can ensure that the worker
        ///     thread remains alive by holding the reference to the
        ///     UIThreadPoolThread instance.
        /// </remarks>
        /// <param name="inPool">
        ///     Whether or not the thread should be from the pool, or a
        ///     dedicated thread.
        /// </param>
        public static UIThreadPoolThread AcquireThread()
        {
            Dispatcher dispatcher;

            lock (_guard)
            {
                // Find the least referenced thread in the pool.
                dispatcher = GetLeastReferencedThread(true);

                // If no threads exist, or if the thread already has a
                // reference, and we are allowed to have more threads, get a
                // new thread.
                if((dispatcher == null || _poolThreads[dispatcher].Item2 > 0) && _numThreads < _maxThreads)
                {
                    // Before making a new thread from scratch, look to see
                    // if we can reuse one currently excluded from the pool.
                    dispatcher = GetLeastReferencedThread(false);
                    if(dispatcher == null)
                    {
                        // If we still need a thread, make one.
                        dispatcher = AddNewThreadToPool();
                    }
                }

                if (dispatcher == null)
                {
                    throw new InvalidOperationException("Unable to acquire worker thread.");
                }

                return new UIThreadPoolThread(dispatcher);
            }
        }

        /// <summary>
        ///     Increments the reference count for the worker thread
        ///     associated with the specified Dispatcher.
        /// </summary>
        /// <param name="dispatcher">
        ///     The dispatcher associated with the worker thread.
        /// </param>
        /// <remarks>
        ///     In all cases, the thread is considered to be back in the pool.
        ///     This is called from the UIThreadPoolThread constructor.
        /// </remarks>
        internal static void IncrementThread(Dispatcher dispatcher)
        {
            lock(_guard)
            {
                Debug.Assert(_poolThreads.ContainsKey(dispatcher));

                int refCount = _poolThreads[dispatcher].Item2;
                _poolThreads[dispatcher] = new Tuple<bool, int>(true, refCount+1);
            }
        }

        /// <summary>
        ///     Decrements the reference count for the worker thread
        ///     associated with the specified Dispatcher.
        /// </summary>
        /// <param name="dispatcher">
        ///     The dispatcher associated with the worker thread.
        /// </param>
        /// <remarks>
        ///     This is called from the UIThreadPoolThread Dispose method.
        /// </remarks>
        internal static void DecrementThread(Dispatcher dispatcher)
        {
            lock (_guard)
            {
                Debug.Assert(_poolThreads.ContainsKey(dispatcher));
                Debug.Assert(_poolThreads[dispatcher].Item2 > 0);

                bool inPool = _poolThreads[dispatcher].Item1;
                int refCount = _poolThreads[dispatcher].Item2 - 1;
                _poolThreads[dispatcher] = new Tuple<bool, int>(inPool, refCount);

                if (refCount == 0 && inPool && _numThreads > _minThreads)
                {
                    RemoveThreadFromPool(dispatcher);
                }
                else if(refCount == 0 && !inPool)
                {
                    DestroyWorkerThread(dispatcher);
                }
            }
        }

        /// <summary>
        ///     Creates a new worker thread and adds it to the pool.
        /// </summary>
        /// <remarks>
        ///     The new thread has a ref count of 0.
        /// </remarks>
        /// <returns>
        ///     The dispatcher associated with the new worker thread.
        /// </returns>
        private static Dispatcher AddNewThreadToPool()
        {
            Debug.Assert(_numThreads < _maxThreads, "Creating a thread would exceed the maximum allowed!");

            AutoResetEvent isRunning = new AutoResetEvent(false);
            int threadId = _threadIdCounter++;

            // Spin up our worker thread.  The important aspects of this
            // thread are:
            // 1) It runs a WPF Dispatcher loop, which is a message pump.
            // 2) It is marked as STA.
            // 3) It is marked as a background thread, which will allow the
            //    app to exit even with active UIThreadPool threads.
            Thread thread = new Thread(new ParameterizedThreadStart(WorkerThreadMain));

            thread.Name = "UIThreadPoolThread #" + threadId;
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start(isRunning);

            // Wait for the worker thread to get started.
            isRunning.WaitOne();
            isRunning.Dispose();

            // Add the dispatcher to the list of free worker threads,
            Dispatcher dispatcher = Dispatcher.FromThread(thread);
            _poolThreads.Add(dispatcher, new Tuple<bool, int>(true, 0));
            _numThreads++;

            return dispatcher;
        }

        /// <summary>
        ///     Removes a worker thread from the pool.
        /// </summary>
        /// <param name="dispatcher">
        ///     The dispatcher associated with the worker thread.
        /// </param>
        /// <remarks>
        ///     If the reference count is 0, the thread is destroyed.
        ///     Otherwise, it is just marked as not in the pool.
        /// </remarks>
        private static void RemoveThreadFromPool(Dispatcher dispatcher)
        {
            Debug.Assert(_poolThreads.ContainsKey(dispatcher));
            Debug.Assert(_poolThreads[dispatcher].Item1 == true);

            if (_poolThreads[dispatcher].Item2 == 0)
            {
                DestroyWorkerThread(dispatcher);
            }
            else
            {
                int refCount = _poolThreads[dispatcher].Item2;
                _poolThreads[dispatcher] = new Tuple<bool, int>(false, refCount);
            }

            _numThreads--;
        }

        /// <summary>
        ///     Removes the thread from our dictionary and asynchronously
        ///     signals it to shut down.
        /// </summary>
        /// <param name="dispatcher">
        ///     The dispatcher associated with the worker thread.
        /// </param>
        private static void DestroyWorkerThread(Dispatcher dispatcher)
        {
            Debug.Assert(_poolThreads.ContainsKey(dispatcher));
            Debug.Assert(_poolThreads[dispatcher].Item2 == 0);

            _poolThreads.Remove(dispatcher);

            // Exit our thread politely.  This is asynchronous, so the
            // worker will exit eventually.
            dispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
        }

        /// <summary>
        ///     Returns the first thread with the least refernce count.
        /// </summary>
        /// <param name="inPool">
        ///     Whether to search for threads in or out of the pool.
        /// </param>
        /// <returns>
        ///     The Dispatcher associated with the least referenced thread.
        /// </returns>
        private static Dispatcher GetLeastReferencedThread(bool inPool)
        {
            KeyValuePair<Dispatcher, Tuple<bool, int>>? minPair = null;
            foreach (KeyValuePair<Dispatcher, Tuple<bool, int>> pair in _poolThreads)
            {
                if (pair.Value.Item1 == inPool)
                {
                    if (pair.Value.Item2 == 0)
                    {
                        minPair = pair;

                        // 0 is obviously the minimum.
                        break;
                    }
                    else if (minPair == null || pair.Value.Item2 < minPair.Value.Value.Item2)
                    {
                        minPair = pair;
                    }
                }
            }

            return minPair != null ? minPair.Value.Key : null;
        }

        /// <summary>
        ///     The thread proc used for all worker threads in the pool.
        /// </summary>
        /// <param name="parameter">
        ///     The AutoResetEvent to signal once the thread is started.
        /// </param>
        private static void WorkerThreadMain(object parameter)
        {
            // Force the creation of the Dispatcher for this thread, and then
            // signal that we are running.
            Dispatcher d = Dispatcher.CurrentDispatcher;
            ((AutoResetEvent)parameter).Set();

            // Run a dispatcher for this UIThreadPool thread.
            // This is the central processing loop for WPF.
            Dispatcher.Run();
        }

        private static object _guard = new object();

        private static int _threadIdCounter = 0;

        private static int _minThreads;
        private static int _maxThreads;
        private static int _numThreads;

        private static Dictionary<Dispatcher, Tuple<bool, int>> _poolThreads = new Dictionary<Dispatcher,Tuple<bool,int>>();
    }
}
