using GameLibrary.Core;
using GameLibrary.Core.Menu;
using GameLibrary.Core.Events;

namespace GameLibraryTests
{
    public class QuitMenuEntryTests
    {
        // ADAPTEZ : passez les dépendances de votre GameManager.
        private GameManager CreateGameManager()
        {
            return new GameManager(new EventManager(), null);
        }

        [Test]
        public void GetText_ReturnsQuit()
        {
            QuitMenuEntry quit_entry = new QuitMenuEntry(CreateGameManager());

            string text = quit_entry.GetText();

            Assert.That(text, Does.Contain("Quit"));
        }

        [Test]
        public void Execute_AsksTheGameToQuit()
        {
            GameManager game_manager = CreateGameManager();
            QuitMenuEntry quit_entry = new QuitMenuEntry(game_manager);

            quit_entry.Execute();

            Assert.That(game_manager.GetShouldQuit(), Is.True);
        }
    }
}
