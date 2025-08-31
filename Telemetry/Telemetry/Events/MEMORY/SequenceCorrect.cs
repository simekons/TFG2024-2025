using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Events.MEMORY
{
    public class SequenceCorrect : Event
    {
        public SequenceCorrect(int count) : base((int)EventType.SequenceCorrect)
        {
            _data.Add("Sequence correct X times: ", count);
        }
    }
}
