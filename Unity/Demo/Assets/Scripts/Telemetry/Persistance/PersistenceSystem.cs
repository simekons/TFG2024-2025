using System;
using System.Collections.Generic;
using Telemetry.Events;
using Telemetry.Serialization;

namespace Telemetry.Persistance
{
    /// <summary>
    /// Basic persistence system, it has the basics a serialization and the queue where the Events are gonna be sended.
    /// </summary>
    public abstract class PersistanceSystem : IPersistance
    {
        protected ISerialization serialization; // format of the data
        public Queue<Event> sessionEvents; // queue of pending events
        public PersistanceSystem(ISerialization serialization)
        {
            sessionEvents = new Queue<Event>();
            this.serialization = serialization;
        }

        public void PersistEvent(Event ev)
        {
            sessionEvents.Enqueue(ev);
        }
        public ISerialization GetSerialization()
        {
            return this.serialization;          
        }
      
        public abstract void Flush();
        public abstract PersistanceType GetPersistanceType();
        public abstract void End();

    }
}