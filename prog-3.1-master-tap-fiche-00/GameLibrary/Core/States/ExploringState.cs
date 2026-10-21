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

                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));

                if (selected_child_location_index != -1)
                {
                    LocationComponent current_location = _gameManager.GetCurrentLocation();
                    Connection selected_connection = current_location.GetChildLocationAtIndex(selected_child_location_index);

                    LocationComponent destination_location = selected_connection.GetDestinationLocation();

                    _eventManager.TriggerEvent(new LogMessageGameEvent($"Location changed from {current_location.GetName()} to {destination_location.GetName()} - time taken: {selected_connection.GetTimeToReachDestination()}."));
                    _gameManager.SetCurrentLocation(destination_location);

                    ShopComponent shop_component = destination_location.GetGameObject().GetComponent<ShopComponent>();
                    InventoryComponent inventory_component = destination_location.GetGameObject().GetComponent<InventoryComponent>();

                    if (shop_component != null)
                    {
                        if (shop_component.GetIsOpen())
                        {
                            _stateMachine.ChangeState(new ShopOpenState(_stateMachine, _gameManager, _eventManager, shop_component, inventory_component));
                        }
                        else
                        {
                            _stateMachine.ChangeState(new ShopClosedState(_stateMachine, _gameManager, _eventManager, shop_component, inventory_component));
                        }
                    }
                    else
                    {
                        _eventManager.TriggerEvent(new LogMessageGameEvent($"No ShopComponent on {destination_location.GetName()}"));
                    }
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
                    _gameManager.QuitGame();
                    _stateMachine.ChangeState(new MainMenuState(_stateMachine, _gameManager, _eventManager));
                }

            }
        }

        public void Exit()
        {
            _eventManager.UnregisterFromEvent<GameActionGameEvent>(OnGameActionGameEvent);
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

        public bool GetIsReplaying()
        {
            return _gameManager.GetIsReplaying();
        }

        public int GetReplaySpeed()
        {
            return _gameManager.GetReplaySpeed();
        }

        public GameObject GetHeroObject()
        {
            return _gameManager.GetHeroObject();
        }
    }
}
