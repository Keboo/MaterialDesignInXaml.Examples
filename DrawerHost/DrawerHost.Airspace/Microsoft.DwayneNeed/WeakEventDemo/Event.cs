using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WeakEventDemo
{
    /// <summary>
    ///     Describes a notification sent from a sender object when a
    ///     particular event occurs.
    /// </summary>
    /// <typeparam name="TSender">
    ///     The type of the sender of the event.
    /// </typeparam>
    /// <typeparam name="TEventArgs">
    ///     The type of the event arguments associated with this event.
    /// </typeparam>
    public interface IEvent<TSender, TEventArgs>
        where TSender : class
        where TEventArgs : EventArgs
    {
        /// <summary>
        ///     Subscribes to the event.
        /// </summary>
        /// <param name="sender">
        ///     The particular sender to subscribe to.  This may be null to
        ///     specify all senders of this event.
        /// </param>
        /// <param name="handler">
        ///     The handler to receive event invocations through.
        /// </param>
        /// <returns>
        ///     A disposable object identifying this event subscription.
        ///     Disposing this object will cancel the subscription.
        /// </returns>
        IDisposable Subscribe(TSender sender, Action<TSender, TEventArgs> handler);
    }

    public class Event<TSender, TEventArgs> : IEvent<TSender, TEventArgs>
        where TSender : class
        where TEventArgs : EventArgs
    {
        protected Event() : this(null, null)
        {
        }

        public Event(
            Func<TSender, Action<TSender, TEventArgs>, object> subscribe,
            Action<TSender, Action<TSender, TEventArgs>, object> unsubscribe)
        {
            _subscribe = subscribe;
            _unsubscribe = unsubscribe;
        }

        public IDisposable Subscribe(TSender sender, Action<TSender,TEventArgs> handler)
        {
 	        return new Subscription(this, sender, handler);
        }

        protected virtual object SubscribeCore(TSender sender, Action<TSender, TEventArgs> handler)
        {
            if (_subscribe != null)
            {
                return _subscribe(sender, handler);
            }
            else
            {
                return null;
            }
        }

        protected virtual void UnubscribeCore(TSender sender, Action<TSender, TEventArgs> handler, object tag)
        {
            if (_unsubscribe != null)
            {
                _unsubscribe(sender, handler, tag);
            }
        }

        private Func<TSender, Action<TSender, TEventArgs>, object> _subscribe;
        private Action<TSender, Action<TSender, TEventArgs>, object> _unsubscribe;

        private class Subscription : IDisposable
        {
            public Subscription(
                Event<TSender, TEventArgs> @event,
                TSender sender,
                Action<TSender, TEventArgs> handler)
            {
                _event = @event;
                _sender = sender;
                _handler = handler;

                _tag = _event.SubscribeCore(sender, EventHandlerFilter);
            }

            public void  Dispose()
            {
 	            if(_event != null)
                {
                    _event.UnubscribeCore(_sender, EventHandlerFilter, _tag);

                    _event = null;
                    _handler = null;
                    _sender = null;
                }
            }

            private void EventHandlerFilter(TSender sender, TEventArgs e)
            {
                if (_sender == null || object.ReferenceEquals(sender, _sender))
                {
                    _handler(sender, e);
                }
            }

            private Event<TSender, TEventArgs> _event;
            private TSender _sender;
            private Action<TSender,TEventArgs> _handler;
            private object _tag;
        }
    }
}
