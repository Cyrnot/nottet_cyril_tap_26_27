using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
namespace GameLibrary.Core.States
{
    public class ExploringState : IState
    {
        private StateMachine _stateMachine;
        private GameManager _gameManager;
        private EventManager _eventManager;

        public ExploringState(StateMachine state_machine, GameManager game_manager, EventManager event_manager)
        {
            _stateMachine = state_machine;
            _gameManager = game_manager;
            _eventManager = event_manager;
        }

        public void Enter()
        {
            _eventManager.RegisterToEvent<GameActionGameEvent>(OnGameActionGameEvent);
        }

        private void OnGameActionGameEvent(IGameEvent game_event)
        {
            GameActionGameEvent game_action_game_event = game_event as GameActionGameEvent;

            if (game_action_game_event._gameActionType == GameActionType.NAVIGATE_UP)
            {
                int selected_child_location_index = _gameManager.GetSelectedDestinationIndex();
                int current_location_child_count = _gameManager.GetCurrentLocation().GetChildLocationCount();

                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));
                _gameManager.SetSelectedDestinationIndex((selected_child_location_index - 1 + current_location_child_count) % current_location_child_count);
            }
            else if (game_action_game_event._gameActionType == GameActionType.NAVIGATE_DOWN)
            {
                int selected_child_location_index = _gameManager.GetSelectedDestinationIndex();
                int current_location_child_count = _gameManager.GetCurrentLocation().GetChildLocationCount();

                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));
                _gameManager.SetSelectedDestinationIndex((selected_child_location_index + 1) % current_location_child_count);
            }
            else if (game_action_game_event._gameActionType == GameActionType.CONFIRM)
            {
                int selected_child_location_index = _gameManager.GetSelectedDestinationIndex();
                int current_location_child_count = _gameManager.GetCurrentLocation().GetChildLocationCount();

                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));

                if (selected_child_location_index >= 0 && current_location_child_count > 0)
                {
                    LocationComponent current_location = _gameManager.GetCurrentLocation();

                    Connection selected_way = current_location.GetChildLocationAtIndex(selected_child_location_index);
                    _eventManager.TriggerEvent(new LogMessageGameEvent($"Location changed from {current_location.GetName()} to {selected_way.GetDestinationLocation().GetName()} - time taken: {selected_way.GetTimeToReachDestination()}."));
                    current_location = selected_way.GetDestinationLocation();
                    _gameManager.SetCurrentLocation(current_location);
                    _gameManager.SetSelectedDestinationIndex(-1);
                }
            }
            else if (game_action_game_event._gameActionType == GameActionType.CANCEL)
            {
                int selected_child_location_index = _gameManager.GetSelectedDestinationIndex();

                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));
                _gameManager.SetSelectedDestinationIndex(-1);
            }
            else if (game_action_game_event._gameActionType == GameActionType.QUIT)
            {
                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));

                LocationComponent parent_location = _gameManager.GetCurrentLocation().GetParentLocation();

                if (parent_location != null)
                {
                    _gameManager.SetCurrentLocation(parent_location);
                }
                else
                {
                    _stateMachine.ChangeState(new MainMenuState(_stateMachine, _gameManager, _eventManager));
                }

            }
        }

        public void Exit()
        {
            _eventManager.UnregisterFromEvent<GameActionGameEvent>(OnGameActionGameEvent);
            _gameManager.SetCurrentLocation(null);
        }

        public void FixedUpdate(float fixed_elapsed_time)
        {

        }

        public void Update(float elapsed_time)
        {

        }

        public LocationComponent GetCurrentLocation()
        {
            return _gameManager.GetCurrentLocation();
        }

        public int GetSelectedDestinationIndex()
        {
            return _gameManager.GetSelectedDestinationIndex();
        }
    }
}
