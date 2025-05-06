using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Events.MEMORY
{
    public class ButtonPressedEvent : Event
    {
        public ButtonPressedEvent(int id, string time) : base((int)EventType.ButtonPressed)
        {
            _data.Add("Button id: ", id);
            _data.Add("Button pressed at time: ", time);
        }
    }
}
