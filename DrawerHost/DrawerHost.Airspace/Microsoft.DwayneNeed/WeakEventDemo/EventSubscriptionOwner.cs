using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeakEventDemo
{
    /// <summary>
    ///     Describes the source of a strong reference to an event handler.
    /// </summary>
    public enum EventSubscriptionOwner
    {
        /// <summary>
        ///     The event subscription will be owned by the subscriber.
        /// </summary>
        /// <remarks>
        ///     The subscriber must hold a reference to the subscription
        ///     returned from Event.Subscribe() to avoid garbage collection.
        /// </remarks>
        Subscriber,

        /// <summary>
        ///     The event subscription will be owned by the target of the
        ///     event handler.
        /// </summary>
        /// <remarks>
        ///     This is appropriate for many handlers, but is not appropriate
        ///     for anonymouse delegates that capture variables, since the
        ///     target itself is a generated class and is often not referenced
        ///     by anyone else.  
        /// </remarks>
        Target,

        /// <summary>
        ///     The event subscription will be owned by the sender being
        ///     subscribed to.
        /// </summary>
        /// <remarks>
        ///     This is the type of ownership created with normal CLR events.
        ///     It can cause memory leaks if the lifetime of the event sender
        ///     is much longer than the natural lifetime of the event handler
        ///     target.  Note that this type of ownership may not be specified
        ///     with global subscriptions.
        /// </remarks>
        Sender,

        /// <summary>
        ///     The event subscriptions will be owned by the event instance.
        /// </summary>
        /// <remarks>
        ///     Normally the event instance is strongly rooted and statically
        ///     accessible, so this kind of reference generally roots the
        ///     event handler, and it will never by garbage collected until
        ///     the subscription is disposed.
        /// </remarks>
        Event
    }
}
