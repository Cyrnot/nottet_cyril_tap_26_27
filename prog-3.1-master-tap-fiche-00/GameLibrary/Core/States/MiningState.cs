using GameLibrary.Core.Components;
using GameLibrary.Core.Events;

namespace GameLibrary.Core.States
{
    public class MiningState : IState
    {
        private const float GAME_MINUTES_PER_SECOND = 10.0f;

        private readonly StateMachine _stateMachine;
        private readonly GameManager _gameManager;
        private readonly EventManager _eventManager;
        private readonly MineComponent _mineComponent;

        private bool _strikeInProgress;
        private float _elapsedStrikeMinutes;

        public MiningState(StateMachine state_machine, GameManager game_manager, EventManager event_manager, MineComponent mine_component)
        {
            _stateMachine = state_machine;
            _gameManager = game_manager;
            _eventManager = event_manager;
            _mineComponent = mine_component;
        }

        public void Enter()
        {
            _eventManager.RegisterToEvent<GameActionGameEvent>(OnGameActionGameEvent);
        }

        private void OnGameActionGameEvent(IGameEvent game_event)
        {
            GameActionGameEvent game_action_game_event = game_event as GameActionGameEvent;

            if (game_action_game_event._gameActionType == GameActionType.CONFIRM)
            {
                if (!_strikeInProgress && _mineComponent.GetStock() > 0)
                {
                    _strikeInProgress = true;
                    _elapsedStrikeMinutes = 0.0f;
                }
            }
            else if (game_action_game_event._gameActionType == GameActionType.CANCEL)
            {
                if (_strikeInProgress)
                {
                    _strikeInProgress = false;
                    _elapsedStrikeMinutes = 0.0f;
                }
                else
                {
                    _stateMachine.ChangeState(new ExploringState(_stateMachine, _gameManager, _eventManager));
                }
            }
        }

        public void Exit()
        {
            _eventManager.UnregisterFromEvent<GameActionGameEvent>(OnGameActionGameEvent);
        }

        public void FixedUpdate(float fixed_elapsed_time)
        {
            if (!_strikeInProgress)
            {
                return;
            }

            _elapsedStrikeMinutes += fixed_elapsed_time * GAME_MINUTES_PER_SECOND;

            if (_elapsedStrikeMinutes + 0.0001f >= _mineComponent.GetMinutesPerStrike())
            {
                _strikeInProgress = false;
                _elapsedStrikeMinutes = 0.0f;
                _gameManager.CompleteMineStrike(_mineComponent);
            }
        }

        public void Update(float elapsed_time)
        {
        }
    }
}
