using System;
using System.Collections.Generic;
using System.Text;

namespace MountainServer.Event
{
    public class HandlerList<T> : List<IBaseEvent<T>.EventRaised>
    {

    }
}
