
namespace Telemetry.Events
{
    public class EndSessionEvent : Event
    {
        public EndSessionEvent() : base((int)EventType.SessionEnd)
        {
        }


    }
}
