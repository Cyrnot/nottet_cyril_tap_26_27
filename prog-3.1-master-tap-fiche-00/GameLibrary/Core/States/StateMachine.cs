using GameLibrary.Core.Events;

namespace GameLibrary.Core.States
{
    public class StateMachine
    {
        private IState _currentState;

        private readonly EventManager _eventManager;

        public StateMachine(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public void SetInitialState(IState initial_state)
        {
            _eventManager.TriggerEvent(new LogMessageGameEvent($"Set initial state to {initial_state.GetType().Name}"));

            _currentState = initial_state;
            _currentState.Enter();
        }

        public void ChangeState(IState new_state)
        {
            _eventManager.TriggerEvent(new LogMessageGameEvent($" {_currentState.GetType().Name} changed to {new_state.GetType().Name}"));

            _currentState.Exit();
            _currentState = new_state;
            _currentState.Enter();
        }

        public void Update(float elapsed_time)
        {
            _currentState.Update(elapsed_time);
        }

        public void FixedUpdate(float fixed_elapsed_time)
        {
            _currentState.FixedUpdate(fixed_elapsed_time);
        }

        public IState GetCurrentState()
        {
            return _currentState;
        }
    }
}