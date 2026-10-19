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
            WorldBuilder builder = new WorldBuilder(new List<LocationData>() { new LocationData("W", "World", "", 0) }, null, null);
            GameManager game_manager = new GameManager(event_manager, builder);

            Assert.That(game_manager.GetSelectedDestinationIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void GetSelectedDestinationIndex_ReturnMinusOneAfterConfirmCommandInMainMenu()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = new GameManager(event_manager, new WorldBuilder(new List<LocationData>(), new List<ShopData>(), null));

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetSelectedDestinationIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void GetSelectedDestinationIndex_KeepMinusOneAfterNavigateDownCommandInMainMenu()
        {
            EventManager event_manager = new EventManager();
            WorldBuilder builder = new WorldBuilder(new List<LocationData>() { new LocationData("W", "World", "", 0) }, null, null);
            GameManager game_manager = new GameManager(event_manager, builder);

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));

            Assert.That(game_manager.GetSelectedDestinationIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void GetCurrentLocation_ConfirmEnterExploring()
        {
            List<LocationData> location_data_table = new List<LocationData>();
            List<ShopData> shop_data_table = new List<ShopData>();
            EventManager event_manager = new EventManager();
            WorldBuilder builder = new WorldBuilder(location_data_table, shop_data_table, null);
            GameManager game_manager = new GameManager(event_manager, builder);

            location_data_table.Add(new LocationData("DT_W", "Daisy Town", "W", 5));
            location_data_table.Add(new LocationData("W", "World", "", 0));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetCurrentLocation(), Is.Not.Null);
        }

        [Test]
        public void GetSelectedDestinationIndex_CancelResetsIndex()
        {
            List<LocationData> location_data_table = new List<LocationData>();
            List<ShopData> shop_data_table = new List<ShopData>();
            EventManager event_manager = new EventManager();
            WorldBuilder builder = new WorldBuilder(location_data_table, shop_data_table, null);
            GameManager game_manager = new GameManager(event_manager, builder);

            location_data_table.Add(new LocationData("DT_W", "Daisy Town", "W", 5));
            location_data_table.Add(new LocationData("W", "World", "", 0));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CANCEL));

            Assert.That(game_manager.GetSelectedDestinationIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void GetCurrentLocation_ConfirmMovesToChildLocation()
        {
            List<LocationData> location_data_table = new List<LocationData>();
            List<ShopData> shop_data_table = new List<ShopData>();
            EventManager event_manager = new EventManager();
            WorldBuilder builder = new WorldBuilder(location_data_table, shop_data_table, null);
            GameManager game_manager = new GameManager(event_manager, builder);

            location_data_table.Add(new LocationData("DT_W", "Daisy Town", "W", 5));
            location_data_table.Add(new LocationData("W", "World", "", 0));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));
            LocationComponent old_Location = game_manager.GetCurrentLocation();
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetCurrentLocation(), Is.Not.EqualTo(old_Location));
        }
    }
}
