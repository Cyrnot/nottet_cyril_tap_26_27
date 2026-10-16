namespace GameLibrary.Core.Events
{
    public class RegisterGameObjectGameEvent : IGameEvent
    {
        public readonly GameObject _gameObject;
        public RegisterGameObjectGameEvent(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
    }
}
