using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.States
{
    public class ShopClosedState : IState
    {
        private StateMachine _stateMachine;
        private GameManager _gameManager;
        private EventManager _eventManager;
        private ShopComponent _shopComponent;
        private InventoryComponent _InventoryComponent;

        public ShopClosedState(StateMachine state_machine, GameManager game_manager, EventManager event_manager, ShopComponent shop_component, InventoryComponent inventory_component)
        {
            _stateMachine = state_machine;
            _gameManager = game_manager;
            _eventManager = event_manager;
            _shopComponent = shop_component;
            _InventoryComponent = inventory_component;
        }

        public void Enter()
        {
            _eventManager.RegisterToEvent<GameActionGameEvent>(OnGameActionGameEvent);
        }

        private void OnGameActionGameEvent(IGameEvent game_event)
        {
            GameActionGameEvent game_action_game_event = game_event as GameActionGameEvent;

            if ((game_action_game_event._gameActionType == GameActionType.CANCEL) || (game_action_game_event._gameActionType == GameActionType.QUIT))
            {
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
            if (_shopComponent.GetIsOpen())
            {
                _stateMachine.ChangeState(new ShopOpenState(_stateMachine, _gameManager, _eventManager, _shopComponent, _InventoryComponent));
            }
        }

        public void Update(float elapsed_time)
        {
            
        }

        public ShopComponent GetShopComponent()
        {
            return _shopComponent;
        }
    }
}
