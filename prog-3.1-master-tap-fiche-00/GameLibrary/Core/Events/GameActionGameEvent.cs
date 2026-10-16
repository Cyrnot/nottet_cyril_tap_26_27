namespace GameLibrary.Core.Events
{
    public class GameActionGameEvent : IGameEvent
    {
        public GameActionType _gameActionType;

        public GameActionGameEvent(GameActionType gameActionType)
        {
            _gameActionType = gameActionType;
        }
    }
}
