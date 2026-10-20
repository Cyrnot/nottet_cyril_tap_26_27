using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
using GameLibrary.Data;
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
        private InventoryComponent _inventoryComponent;

        private int _selectedItemIndex = -1;
        private string _text;

        public ShopOpenState(StateMachine state_machine, GameManager game_manager, EventManager event_manager, ShopComponent shop_component, InventoryComponent inventory_component)
        {
            _stateMachine = state_machine;
            _gameManager = game_manager;
            _eventManager = event_manager;
            _shopComponent = shop_component;
            _inventoryComponent = inventory_component;
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
                int entry_count = _inventoryComponent.GetEntryCount();
                if (entry_count > 0)
                {
                    _selectedItemIndex = (_selectedItemIndex - 1 + entry_count) % entry_count;
                }
            }
            else if (game_action_game_event._gameActionType == GameActionType.NAVIGATE_DOWN)
            {
                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));
                int entry_count = _inventoryComponent.GetEntryCount();
                if (entry_count > 0)
                {
                    _selectedItemIndex = (_selectedItemIndex + 1 + entry_count) % entry_count;
                }
            }
            else if (game_action_game_event._gameActionType == GameActionType.CONFIRM)
            {
                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));

                if (_selectedItemIndex >= 0 && _selectedItemIndex < _inventoryComponent.GetEntryCount())
                {
                    ItemData item_data = _inventoryComponent.GetEntryAtIndex(_selectedItemIndex).GetItem();

                    if (_inventoryComponent.TryRemove(item_data._id, 1))
                    {
                        _text = $" You bought {item_data._name} for {item_data._price}";
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
                _stateMachine.ChangeState(new ShopClosedState(_stateMachine, _gameManager, _eventManager, _shopComponent, _inventoryComponent));
            }
        }

        public void Update(float elapsed_time)
        {

        }

        public ShopComponent GetShopComponent()
        {
            return _shopComponent;
        }

        public string GetText()
        {
            return _text;
        }

        public int GetSelectedItemIndex()
        {
            return _selectedItemIndex;
        }

        public InventoryComponent GetInventoryComponent()
        {
            return _inventoryComponent;
        }
    }
}
