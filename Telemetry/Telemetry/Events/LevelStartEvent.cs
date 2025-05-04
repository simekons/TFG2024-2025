
namespace Telemetry.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class LevelStartEvent : Event
    {  
        public LevelStartEvent() : base((int)EventType.LevelStart)
        {
        }

    }
}