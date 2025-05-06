
namespace Telemetry.Events
{
    /// <summary>
    /// 
    /// </summary>
    internal class GameEndEvent : Event
    {  
        public GameEndEvent() : base((int)EventType.GameEnd)
        {
        }

    }
}