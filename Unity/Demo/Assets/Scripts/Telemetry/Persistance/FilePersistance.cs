using System;
using System.IO;
using Telemetry.Events;
using Telemetry.Serialization;

namespace Telemetry.Persistance
{
    internal class FilePersistance : PersistanceSystem
    {
        string _fileName;
        StreamWriter writer;
        public FilePersistance(string fileName,ISerialization serialization): 
            base(serialization)
       {
            _fileName = fileName + GetSerialization().getExtension();
        }

        public override void Flush()
        {
            using (StreamWriter writer = new StreamWriter(_fileName, true))
            {
                while (sessionEvents.Count != 0)
                {
                    Event ev = sessionEvents.Dequeue();
                    string serializationString = GetSerialization().Serialize(ev);
                    writer.WriteLine(serializationString);
                }
            }
        }

        public override PersistanceType GetPersistanceType()
        {
            return PersistanceType.File;
        }

        public override void End()
        {
            sessionEvents.Clear(); //should be empty
        } 

    }
}
