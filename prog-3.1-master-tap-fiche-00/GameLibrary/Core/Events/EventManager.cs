namespace GameLibrary.Core.Events
{
    public class EventManager
    {
        private readonly Dictionary<Type, List<Action<IGameEvent>>> _eventTypeTable = new Dictionary<Type, List<Action<IGameEvent>>>();

        private readonly List<IGameEvent> _eventTable = new List<IGameEvent>();

        public void RegisterToEvent<TYPE>(Action<IGameEvent> action) where TYPE : IGameEvent
        {
            Type event_type = typeof(TYPE);

            if (!_eventTypeTable.ContainsKey(event_type))
            {
                _eventTypeTable.Add(event_type, new List<Action<IGameEvent>>());
            }

            _eventTypeTable[event_type].Add(action);
        }

        public void UnregisterFromEvent<TYPE>(Action<IGameEvent> action) where TYPE : IGameEvent
        {
            Type event_type = typeof(TYPE);

            if (_eventTypeTable.ContainsKey(event_type))
            {
                _eventTypeTable[event_type].Remove(action);
            }
        }

        public void TriggerEvent(IGameEvent game_event)
        {
            Type event_type = game_event.GetType();

            if (_eventTypeTable.ContainsKey(event_type))
            {
                for (int event_index = 0; event_index < _eventTypeTable[event_type].Count; event_index++)
                {
                    Action<IGameEvent> action = _eventTypeTable[event_type][event_index];
                    action(game_event);
                }
            }
        }

        public void TriggerDelayedEvent(IGameEvent game_event)
        {
            _eventTable.Add(game_event);
        }

        public void ProcessDelayEvents()
        {
            while (_eventTable.Count > 0)
            {
                IGameEvent game_event = _eventTable[0];

                _eventTable.RemoveAt(0);

                if (_eventTypeTable.TryGetValue(game_event.GetType(), out List<Action<IGameEvent>> actions))
                {
                    foreach (Action<IGameEvent> action in actions)
                    {
                        action(game_event);
                    }
                }
            }
        }
    }
}