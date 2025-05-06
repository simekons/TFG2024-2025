using System;

namespace Telemetry.Events
{
    /// <summary>
    /// Session Start Event in case of having multiple sessions it will be different ids
    /// </summary>
    public class InitSessionEvent : Event
    {
        public InitSessionEvent() : base((int)EventType.SessionStart)
        {
            _data.Add("Session", Guid.NewGuid()); //id of session
        }
        
    }
}