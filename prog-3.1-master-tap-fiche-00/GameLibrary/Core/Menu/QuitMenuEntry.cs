using GameLibrary.Core;

namespace GameLibrary.Core.Menu
{
    public class QuitMenuEntry : IMenuEntry
    {
        private const string TEXT = "Quit (or press Esc)";

        private readonly GameManager _gameManager;

        public QuitMenuEntry(GameManager game_manager)
        {
            _gameManager = game_manager;
        }

        public string GetText()
        {
            return TEXT;
        }

        public void Execute()
        {
            _gameManager.RequestQuit();
        }
    }
}
