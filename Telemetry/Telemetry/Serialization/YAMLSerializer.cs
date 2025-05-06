using Telemetry.Events;
using System.Text;
using System.IO;
using YamlDotNet.Serialization;

namespace Telemetry.Serialization
{
    internal class YAMLSerializer : ISerialization
    {

        public string getExtension()
        {
            return ".yaml";
        }

        public SerializeType GetSerializeType()
        {
            return SerializeType.YAML;
        }

        public string Serialize(Event ev)
        {
            var serializer = new Serializer();
            var yaml = new StringBuilder();

            var textWriter = new StringWriter(yaml);

            serializer.Serialize(textWriter, ev, ev.GetType());

            return yaml.ToString();
        }

    }

}
