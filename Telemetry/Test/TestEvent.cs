using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public enum newEvts
    {
        //EventTest=Telemetry.Events.EventType.numOfEvents,
        // EventTest2
        EventTest = 3,
        EventTest2
    }
    internal class TestEvent:Telemetry.Events.Event
    {
       public TestEvent() : base((int)newEvts.EventTest2)
        {
            _data.Add("int", 1);
            _data.Add("bool", false);
            _data.Add("string", "str");
            _data.Add("char", 'c');
            _data.Add("EnumSTR", newEvts.EventTest.ToString());
            _data.Add("EnumParse", (int)newEvts.EventTest);
        }
    }
}
