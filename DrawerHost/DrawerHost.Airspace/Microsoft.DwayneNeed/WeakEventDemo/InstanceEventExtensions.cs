using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace WeakEventDemo
{
    /// <summary>
    ///     Extensions to instance events.
    /// </summary>
    /// <typeparam name="TSender">
    ///     The type of the sender of the event.
    /// </typeparam>
    /// <typeparam name="TEventArgs">
    ///     The type of the event arguments associated with this event.
    /// </typeparam>
    public static class InstanceEventExtensions
    {
        public static IDisposable SubscribeEx<TSender, TEventArgs, TSenderEx, TEventArgsEx>(
            this Event<TSender, TEventArgs> @this,
            TSenderEx sender,
            Action<TSenderEx, TEventArgsEx> handler,
            EventSubscriptionOwner owner = EventSubscriptionOwner.Sender)
            where TSender : class
            where TEventArgs : EventArgs
            where TSenderEx : class, TSender
            where TEventArgsEx : TEventArgs
        {
            return new Subscription<TSender, TEventArgs, TSenderEx, TEventArgsEx>(@this, sender, handler, owner);
        }

        private class Subscription<TSender, TEventArgs, TSenderEx, TEventArgsEx> : IDisposable
            where TSender : class
            where TEventArgs : EventArgs
            where TSenderEx : class, TSender
            where TEventArgsEx : TEventArgs
        {
            public Subscription(
                Event<TSender, TEventArgs> @event,
                TSenderEx sender,
                Action<TSenderEx, TEventArgsEx> handler,
                EventSubscriptionOwner owner)
            {
                // The subscription object itself only holds a weak reference
                // to the handler.
                _handler = new WeakReference<Action<TSenderEx,TEventArgsEx>>(handler);

                // The handler is also stored in a ConditionalWeakTable to
                // effectively attach the handler to the specified owner.
                switch (owner)
                {
                    case EventSubscriptionOwner.Sender:
                        // Attach the handler to the sender of the event.
                        if (sender == null)
                        {
                            throw new InvalidOperationException("Sender can not be null.");
                        }
                        AttachHandlerToOwner(handler, sender);
                        break;

                    case EventSubscriptionOwner.Target:
                        // Attach the handler to the same object as its target.
                        if (handler.Target == null)
                        {
                            throw new InvalidOperationException("Handler target can not be null.");
                        }
                        AttachHandlerToOwner(handler, handler.Target);
                        break;

                    default:
                        // Do not attach the handler to anyone.
                        break;
                }

                // Subscribe to the underlying event.  This will hold a strong
                // reference to the instance method, and thus to the instance.
                _subscription = @event.Subscribe(sender, ProxyHandler);
            }


            public void Dispose()
            {
                if(_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;

                    _handler = null;
                }
            }

            private void ProxyHandler(TSender sender, TEventArgs e)
            {
                if(sender is TSenderEx && e is TEventArgsEx)
                {
                    Action<TSenderEx, TEventArgsEx> handler = _handler.Target;
                    if (handler != null)
                    {
                        handler((TSenderEx)sender, (TEventArgsEx)e);
                    }
                }
            }

            private static void AttachHandlerToOwner(Action<TSenderEx, TEventArgsEx> handler, object owner)
            {
                if (_owners == null)
                {
                    _owners = new ConditionalWeakTable<object, List<Action<TSenderEx, TEventArgsEx>>>();
                }

                List<Action<TSenderEx, TEventArgsEx>> list = _owners.GetOrCreateValue(owner);
                list.Add(handler);
            }

            private static ConditionalWeakTable<object, List<Action<TSenderEx, TEventArgsEx>>> _owners;

            private IDisposable _subscription;
            private WeakReference<Action<TSenderEx, TEventArgsEx>> _handler;
        }
    }
}
