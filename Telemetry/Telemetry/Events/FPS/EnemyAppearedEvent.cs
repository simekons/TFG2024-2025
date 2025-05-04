using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Telemetry.Events.FPS
{
    public class EnemyAppearedEvent : Event
    {
        public EnemyAppearedEvent(int id, string time) : base((int)EventType.EnemyAppeared)
        {
            _data.Add("Enemy id: ", id);
            _data.Add("Enemy appeared at time: ", time);
        }
    }
}
