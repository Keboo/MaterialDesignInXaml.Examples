using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeakEventDemo
{
    /// <summary>
    ///     An example of a subscription object that can be used to represent
    ///     an event handler subscribed to an event.  It supports filtering
    ///     for appropriate senders, as well as up-casting the sender and the
    ///     event args.  This is because sometimes an event will want to offer
    ///     stronger types than may be available in existing signatures.
    /// </summary>
    /// <typeparam name="TSender"></typeparam>
    /// <typeparam name="TEventArgs"></typeparam>
    /// <typeparam name="TSenderEx"></typeparam>
    /// <typeparam name="TEventArgsEx"></typeparam>
    public class EventSubscription<TSender, TEventArgs, TSenderEx, TEventArgsEx> : IDisposable
        where TSender : class
        where TEventArgs : EventArgs
        where TSenderEx : class, TSender
        where TEventArgsEx : TEventArgs
    {
        public EventSubscription(TSender sender,
                                 Action<TSenderEx, TEventArgsEx> handler,
                                 Action<TSender, Action<TSender, TEventArgs>> subscribe,
                                 Action<TSender, Action<TSender, TEventArgs>> unsubscribe)
        {
            _sender = sender;
            _handler = handler;
            _unsubscribe = unsubscribe;

            subscribe(sender, EventHandlerProxy);
        }

        public void Dispose()
        {
            _unsubscribe(_sender, EventHandlerProxy);
        }

        private void EventHandlerProxy(TSender sender, TEventArgs e)
        {
            if (_sender == null || object.ReferenceEquals(_sender, sender))
            {
                if (sender is TSenderEx && e is TEventArgsEx)
                {
                    _handler((TSenderEx)sender, (TEventArgsEx)e);
                }
            }
        }

        private TSender _sender;
        private Action<TSenderEx, TEventArgsEx> _handler;
        private Action<TSender, Action<TSender, TEventArgs>> _unsubscribe;
    }
}
