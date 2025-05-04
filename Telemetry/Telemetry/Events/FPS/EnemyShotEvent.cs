using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Events.FPS
{
    public class EnemyShotEvent : Event
    {
        public EnemyShotEvent(int id, string time) : base((int) EventType.EnemyShot)
        {
            _data.Add("Enemy id: ", id);
            _data.Add("Enemy killed at time: ", time);
        }
    }
}
