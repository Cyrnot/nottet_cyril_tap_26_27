using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Core.Events
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
