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

        public ShopClosedState(StateMachine stateMachine, GameManager gameManager, EventManager eventManager, ShopComponent shopComponent)
        {
            _stateMachine = stateMachine;
            _gameManager = gameManager;
            _eventManager = eventManager;
            _shopComponent = shopComponent;
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
                _stateMachine.ChangeState(new ShopOpenState(_stateMachine, _gameManager, _eventManager, _shopComponent));
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
