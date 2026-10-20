namespace GameLibrary.Core.Events
{
    public class GameActionGameEvent : IGameEvent
    {
        public GameActionType _gameActionType;
        public int _fixedStepIndex;

        public GameActionGameEvent(GameActionType gameActionType)
        {
            _gameActionType = gameActionType;
            _fixedStepIndex = -1;
        }

        public GameActionGameEvent(GameActionType gameActionType, int fixed_step_index)
        {
            _gameActionType = gameActionType;
            _fixedStepIndex = fixed_step_index;
        }
    }
}
