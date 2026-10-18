using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.States
{
    public class ShopOpenState : IState
    {
        private StateMachine _stateMachine;
        private GameManager _gameManager;
        private EventManager _eventManager;
        private ShopComponent _shopComponent;

        private int _selectedItemIndex = -1;
        private string _text;

        public ShopOpenState(StateMachine state_machine, GameManager game_manager, EventManager event_manager, ShopComponent shopComponent)
        {
            _stateMachine = state_machine;
            _gameManager = game_manager;
            _eventManager = event_manager;
            _shopComponent = shopComponent;
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
                _selectedItemIndex = (_selectedItemIndex - 1 + _shopComponent.GetItemCount()) % _shopComponent.GetItemCount();
            }
            else if (game_action_game_event._gameActionType == GameActionType.NAVIGATE_DOWN)
            {
                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));
                _selectedItemIndex = (_selectedItemIndex + 1 + _shopComponent.GetItemCount()) % _shopComponent.GetItemCount(); ;
            }
            else if (game_action_game_event._gameActionType == GameActionType.CONFIRM)
            {
                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));

                if (_selectedItemIndex >= 0 && _selectedItemIndex < _shopComponent.GetItemCount())
                {
                    if (_shopComponent.TryRemoveOneAtIndex(_selectedItemIndex))
                    {
                        _text = $" You bought {_shopComponent.GetItemAtIndex(_selectedItemIndex)._name} for {_shopComponent.GetItemAtIndex(_selectedItemIndex)._price}";
                    }
                }
            }
            else if (game_action_game_event._gameActionType == GameActionType.QUIT || game_action_game_event._gameActionType == GameActionType.CANCEL)
            {
                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));

                LocationComponent parent_location = _gameManager.GetCurrentLocation().GetParentLocation();

                _gameManager.SetCurrentLocation(_shopComponent.GetGameObject().GetComponent<LocationComponent>().GetParentLocation());
                _stateMachine.ChangeState(new ExploringState(_stateMachine, _gameManager, _eventManager));

            }
        }

        public void Exit()
        {
            _eventManager.UnregisterFromEvent<GameActionGameEvent>(OnGameActionGameEvent);
        }

        public void FixedUpdate(float fixed_elapsed_time)
        {
            if (!_shopComponent.GetIsOpen())
            {
                _stateMachine.ChangeState(new ShopClosedState(_stateMachine, _gameManager, _eventManager, _shopComponent));
            }
        }

        public void Update(float elapsed_time)
        {

        }
    }
}
