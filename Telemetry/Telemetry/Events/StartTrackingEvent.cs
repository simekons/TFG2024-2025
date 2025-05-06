using System;

namespace Telemetry.Events
{
    /// <summary>
    /// First event of all, it sends the id of the game and the user
    /// </summary>

    public class StartTrackingEvent : Event
    {  
        public StartTrackingEvent(Guid user_guid, string game) : base((int)EventType.StartTracking)
        {
            _data.Add("Game", game);
            _data.Add("User", user_guid); //user id 

        }

    }
}