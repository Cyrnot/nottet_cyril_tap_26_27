using GameLibrary.Core.Events;


namespace GameLibrary.Core
{
    public class LogManager
    {

        private readonly ILogWriter _logWriter;

        public LogManager(ILogWriter logWriter, EventManager event_manager)
        {
            _logWriter = logWriter;

            InitialLog(event_manager);
        }

        public void InitialLog(EventManager event_manager)
        {
            event_manager.RegisterToEvent<LogMessageGameEvent>(OnRegisterLog);
        }

        private void OnRegisterLog(IGameEvent game_event)
        {
            LogMessageGameEvent log_message_game_event = game_event as LogMessageGameEvent;

            string time_stamped_message = DateTime.Now+ " : " + log_message_game_event._log + "\n";

            _logWriter.WriteLine(time_stamped_message);
        }


    }
}
