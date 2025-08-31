using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Events.MEMORY
{
    public class SequenceFailed : Event
    {
        public SequenceFailed(int count) : base((int)EventType.SequenceFailed)
        {
            _data.Add("Sequence failed X times: ", count);
        }
    }
}
