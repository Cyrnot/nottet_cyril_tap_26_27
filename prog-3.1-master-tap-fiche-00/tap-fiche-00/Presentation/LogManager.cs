using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Game.Core.Events;


namespace Game.Presentation
{
    internal class LogManager
    {
        
        public void InitialLog(EventManager event_manager)
        {
            event_manager.RegisterToEvent<LogMessageGameEvent>(OnRegisterLog);

        }

        public void OnRegisterLog(IGameEvent game_event)
        {
            LogMessageGameEvent log_message_game_event = game_event as LogMessageGameEvent;

            File.AppendAllText("../../../Debug.log", DateTime.Now+ " : " + log_message_game_event._log + "\n");
        }


    }
}
