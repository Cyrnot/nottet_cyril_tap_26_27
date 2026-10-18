using GameLibrary.Core.Events;
namespace GameLibrary.Core.States
{
    public class MainMenuState : IState
    {

        private StateMachine _stateMachine;
        private GameManager _gameManager;
        private EventManager _eventManager;

        public MainMenuState(StateMachine state_machine, GameManager game_manager, EventManager event_manager)
        {
            _eventManager = event_manager;
            _gameManager = game_manager;
            _stateMachine = state_machine;
        }

        public void Enter()
        {
            _gameManager.SetIsMenuStateActive(true);
            _eventManager.RegisterToEvent<GameActionGameEvent>(OnGameActionGameEvent);
        }

        private void OnGameActionGameEvent(IGameEvent game_event)
        {
            GameActionGameEvent game_action_game_event = game_event as GameActionGameEvent;

            if (game_action_game_event._gameActionType == GameActionType.CONFIRM)
            {
                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));

                _gameManager.BeginExploration();

                _stateMachine.ChangeState(new ExploringState(_stateMachine, _gameManager, _eventManager));
            }
            else if (game_action_game_event._gameActionType == GameActionType.QUIT)
            {
                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));

                _gameManager.RequestQuit();
            }
        }

        public void Exit()
        {
            _gameManager.SetIsMenuStateActive(false);
            _eventManager.UnregisterFromEvent<GameActionGameEvent>(OnGameActionGameEvent);
        }

        public void FixedUpdate(float fixed_elapsed_time)
        {

        }

        public void Update(float elapsed_time)
        {

        }
    }
}
