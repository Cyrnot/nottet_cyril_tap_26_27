using activity_00_tap_26_27.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Events
{
    internal class LogMessageGameEvent : IGameEvent
    {
        public string _log;

        public LogMessageGameEvent(string log)
        {
            _log = log;
        }
    }
}
