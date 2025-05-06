using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Events.MEMORY
{
    public class ButtonAppearedEvent : Event
    {
        public ButtonAppearedEvent(int id, string time) : base((int)EventType.ButtonAppeared)
        {
            _data.Add("Button id: ", id);
            _data.Add("Button appeared at time: ", time);
        }
    }
}
