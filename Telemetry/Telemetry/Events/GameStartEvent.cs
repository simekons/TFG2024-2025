
namespace Telemetry.Events
{
    /// <summary>
    /// 
    /// </summary>
    internal class GameStartEvent : Event
    {  
        public GameStartEvent() : base((int)EventType.GameStart)
        {
        }

    }
}