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
        public void GetSelectedDestinationIndex_ReturnMinusOneAfterConfirmCommand()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = new GameManager(event_manager);

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetSelectedDestinationIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void GetSelectedDestinationIndex_ReturnZeroAfterNavigateDownCommand()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = new GameManager(event_manager);

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));

            Assert.That(game_manager.GetSelectedDestinationIndex(), Is.EqualTo(0));
        }

        [Test]
        public void GetCurrentLocation_ReturnChildAfterNavigateDownAndConfirmCommand()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = new GameManager(event_manager);
            LocationComponent initial_location = game_manager.GetCurrentLocation();

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetCurrentLocation(), Is.Not.EqualTo(initial_location));
        }
    }
}
