using Telemetry.Events;

namespace Telemetry.Serialization
{
  
    public interface ISerialization
    {
        /// <summary>
        /// Serializes an event
        /// </summary>
        /// <param name="ev">Event to serialize</param>
        /// <returns>The data of the event</returns>
        string Serialize(Event ev);
        SerializeType GetSerializeType();
        
        /// <returns>The format of the data ".json", ".xml"...</returns>
        string getExtension();
    }
}
