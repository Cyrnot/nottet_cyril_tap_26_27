namespace GameLibrary.Core.Events
{
    public class LogMessageGameEvent : IGameEvent
    {
        public readonly string _log;

        public LogMessageGameEvent(string log)
        {
            _log = log;
        }
    }
}
