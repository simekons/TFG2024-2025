
namespace Telemetry.Events
{
    /// <summary>
    /// Last event of all, this is just to check that all the tracking process was sucessful
    /// </summary>
    public class StopTrackingEvent : Event
    {  
        public StopTrackingEvent() : base((int)EventType.StopTracking)
        {
        }

    }
}