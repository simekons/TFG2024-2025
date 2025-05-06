using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Events.Audiometry
{
    public class LeftEqEvent: Event
    {
        public LeftEqEvent(int[] frecuency) : base((int)EventType.LeftEq)
        {
            _data.Add("Left equalization", frecuency);
        }
    }
}
