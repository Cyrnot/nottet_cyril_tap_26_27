namespace GameLibrary.Core.Events
{
    internal class UnregisterGameObjectGameEvent : IGameEvent
    {
        public readonly GameObject _gameObject;

        public UnregisterGameObjectGameEvent(GameObject game_object)
        {
            _gameObject = game_object;
        }
    }
}
