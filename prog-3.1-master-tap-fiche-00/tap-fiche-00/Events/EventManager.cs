using System.Collections.Generic;
using System;
using System.Linq;

namespace activity_00_tap_26_27.Events
{
    public class EventManager
    {
        private readonly Dictionary<Type, List<Action<IGameEvent>>> _eventTypeTable = new Dictionary<Type, List<Action<IGameEvent>>>();

        private List<IGameEvent> _eventTable = new List<IGameEvent>();

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
            for(int game_object_table_index = 0; game_object_table_index > _eventTable.Count; game_object_table_index++)
            {
                IGameEvent game_event = _eventTable.ElementAt(game_object_table_index);
                
                _eventTable.RemoveAt(game_object_table_index);

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