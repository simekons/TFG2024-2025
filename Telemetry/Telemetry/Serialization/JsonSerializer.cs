using Telemetry.Events;
using Newtonsoft.Json;

namespace Telemetry.Serialization
{
    internal class JsonSerializer: ISerialization
    {

        public string getExtension()
        {
            return ".json";
        }

        public SerializeType GetSerializeType()
        {
            return SerializeType.JSON;
        }

        public string Serialize(Event ev)
        {
            //return JsonConvert.SerializeObject(ev, Formatting.Indented);
            return JsonConvert.SerializeObject(ev);
        }
        
    }

}
