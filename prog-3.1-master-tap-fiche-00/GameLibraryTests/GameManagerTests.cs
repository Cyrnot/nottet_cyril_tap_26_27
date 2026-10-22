using GameLibrary.Core;
using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
using GameLibrary.Data;
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
            GameContentData game_content_data = new GameContentData();
            game_content_data._locationDataTable = new List<LocationData>() { new LocationData("W", "World", "", 0) };
            GameManager game_manager = new GameManager(event_manager, game_content_data);

            Assert.That(game_manager.GetSelectedDestinationIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void GetSelectedDestinationIndex_ReturnMinusOneAfterConfirmCommandInMainMenu()
        {
            EventManager event_manager = new EventManager();
            GameContentData game_content_data = new GameContentData { _locationDataTable = new List<LocationData>(), _shopDataTable = new List<ShopData>() };
            game_content_data._locationDataTable = new List<LocationData>() { new LocationData("W", "World", "", 0) };
            GameManager game_manager = new GameManager(event_manager, game_content_data);

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetSelectedDestinationIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void GetSelectedDestinationIndex_KeepMinusOneAfterNavigateDownCommandInMainMenu()
        {
            EventManager event_manager = new EventManager();
            GameContentData game_content_data = new GameContentData();
            GameManager game_manager = new GameManager(event_manager, game_content_data);

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));

            Assert.That(game_manager.GetSelectedDestinationIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void GetCurrentLocation_ConfirmEnterExploring()
        {
            EventManager event_manager = new EventManager();
            GameContentData game_content_data = new GameContentData { _locationDataTable = new List<LocationData>(), _shopDataTable = new List<ShopData>(),_heroDataTable = new List<HeroData>() };
            GameManager game_manager = new GameManager(event_manager, game_content_data);

            game_content_data._locationDataTable.Add(new LocationData("DT_W", "Daisy Town", "W", 5));
            game_content_data._locationDataTable.Add(new LocationData("W", "World", "", 0));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetCurrentLocation(), Is.Not.Null);
        }

        [Test]
        public void GetSelectedDestinationIndex_CancelResetsIndex()
        {
            EventManager event_manager = new EventManager();
            GameContentData game_content_data = new GameContentData { _locationDataTable = new List<LocationData>(), _shopDataTable = new List<ShopData>() };
            GameManager game_manager = new GameManager(event_manager, game_content_data);

            game_content_data._locationDataTable.Add(new LocationData("DT_W", "Daisy Town", "W", 5));
            game_content_data._locationDataTable.Add(new LocationData("W", "World", "", 0));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CANCEL));

            Assert.That(game_manager.GetSelectedDestinationIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void GetCurrentLocation_ConfirmMovesToChildLocation()
        {
            EventManager event_manager = new EventManager();
            GameContentData game_content_data = new GameContentData { _locationDataTable = new List<LocationData>(), _shopDataTable = new List<ShopData>(), _heroDataTable = new List<HeroData>() };
            GameManager game_manager = new GameManager(event_manager, game_content_data);

            game_content_data._locationDataTable.Add(new LocationData("DT_W", "Daisy Town", "W", 5));
            game_content_data._locationDataTable.Add(new LocationData("W", "World", "", 0));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));
            LocationComponent old_Location = game_manager.GetCurrentLocation();
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetCurrentLocation(), Is.Not.EqualTo(old_Location));
        }
    }
}
