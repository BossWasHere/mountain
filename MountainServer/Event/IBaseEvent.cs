using System;
using System.Collections.Generic;
using System.Text;

namespace MountainServer.Event
{
    public interface IBaseEvent<T>
    {
        public delegate void EventRaised(T e);
        public static event EventRaised OnEvent;

        public EventRaised GetHandlers();
    }
}
