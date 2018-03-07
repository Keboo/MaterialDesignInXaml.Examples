using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WeakEventDemo
{
    public class CompositionTargetRenderingEvent : Event<Dispatcher, RenderingEventArgs>
    {
        private CompositionTargetRenderingEvent()
        {
        }

        public static CompositionTargetRenderingEvent Singleton
        {
            get
            {
                return _singleton;
            }
        }

        private static CompositionTargetRenderingEvent _singleton = new CompositionTargetRenderingEvent();

        protected override object SubscribeCore(Dispatcher sender, Action<Dispatcher, RenderingEventArgs> handler)
        {
            EventHandler proxy = delegate(object proxySender, EventArgs proxyEventArgs)
            {
                handler((Dispatcher)proxySender, (RenderingEventArgs)proxyEventArgs);
            };

            CompositionTarget.Rendering += proxy;

            return proxy;
        }

        protected override void UnubscribeCore(Dispatcher sender, Action<Dispatcher, RenderingEventArgs> handler, object tag)
        {
            EventHandler proxy = (EventHandler)tag;
            CompositionTarget.Rendering -= proxy;
        }
    }
}
