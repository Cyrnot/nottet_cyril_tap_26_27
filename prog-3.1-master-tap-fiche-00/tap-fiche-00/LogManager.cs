using activity_00_tap_26_27.Events;
using Game.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Game
{
    internal class LogManager
    {
        EventManager _eventManager;
        
        private void InitialLog()
        {
            _eventManager.RegisterToEvent<LogMessageGameEvent>(OnRegisterLog);
        }

        private void OnRegisterLog(IGameEvent game_event)
        {
            LogMessageGameEvent log_message_game_event = game_event as LogMessageGameEvent;

            File.AppendAllText("C:\\Users\\cyril\\Fichiers\\GitHub\\nottet_cyril_tap_26_27\\prog-3.1-master-tap-fiche-00\\Log.txt", DateTime.Now + log_message_game_event._log);
        }


    }
}
