using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Whitelog.Barak.Common.Events
{
    public static class PropertyNotify
    {
        public static void RaiseEvent<T>(this object sender, EventHandler<Events.EventArgs<T>> eventHandler, T data)
        {
            var temp = eventHandler;
            if (temp != null)
            {
                temp.Invoke(sender, new Events.EventArgs<T>(data));
            }
        }

        public static bool RaiseEvent<T>(this object sender, EventHandler<Events.HandledEventArgs<T>> eventHandler, T data)
        {
            var temp = eventHandler;
            if (temp != null)
            {
                var eventArgs = new Events.HandledEventArgs<T>(data);
                temp.Invoke(sender, eventArgs);
                return eventArgs.Handled;
            }
            return false;
        }
    }

    public class EventArgs<T> : EventArgs
    {
        public static readonly EventArgs<T> EmptyEventArgs = new EventArgs<T>(default(T));

        public T Data { get; protected set; }
        public EventArgs(T data)
        {
            Data = data;
        }
    }

    public class HandledEventArgs<T> : HandledEventArgs
    {
        public static readonly EventArgs<T> EmptyEventArgs = new EventArgs<T>(default(T));

        public T Data { get; protected set; }
        public HandledEventArgs(T data)
        {
            Data = data;
        }
    }
}
