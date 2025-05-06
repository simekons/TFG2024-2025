using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Events.MEMORY
{
    public class MaximumSequenceEvent : Event
    {
        public MaximumSequenceEvent(int sequence) : base((int)EventType.MaxSequence)
        {
            _data.Add("Sequence", sequence);
        }
    }
}
