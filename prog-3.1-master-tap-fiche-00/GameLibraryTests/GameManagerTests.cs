using GameLibrary.Core;
using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibraryTests
{
    public class GameManagerTests
    {
        [Test]
        public void GetSelectedDestinationIndex_ReturnMinusOneAfterConstruction()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = new GameManager(event_manager);

            Assert.That(game_manager.GetSelectedDestinationIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void GetSelectedDestinationIndex_ReturnMinusOneAfterConfirmCommandInMainMenu()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = new GameManager(event_manager);

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetSelectedDestinationIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void GetSelectedDestinationIndex_KeepMinusOneAfterNavigateDownCommandInMainMenu()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = new GameManager(event_manager);

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));

            Assert.That(game_manager.GetSelectedDestinationIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void GetCurrentLocation_ConfirmEnterExploring()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = new GameManager(event_manager);

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetCurrentLocation(), Is.Not.Null);
        }

        [Test]
        public void GetSelectedDestinationIndex_CancelResetsIndex()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = new GameManager(event_manager);

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CANCEL));

            Assert.That(game_manager.GetSelectedDestinationIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void GetCurrentLocation_ConfirmMovesToChildLocation()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = new GameManager(event_manager);

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));
            LocationComponent old_Location = game_manager.GetCurrentLocation();
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetCurrentLocation(), Is.Not.EqualTo(old_Location));
        }
    }
}
