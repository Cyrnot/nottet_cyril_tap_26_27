using NUnit.Framework;
using GameLibrary.Core;
using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
using GameLibrary.Core.States;
using GameLibrary.Data;
using System.Collections.Generic;

namespace GameLibraryTests.State
{
    public class ExploringStateTests
    {
        [Test]
        public void OnGameActionGameEvent_SwitchToShopStateWhenEntersShop()
        {
            EventManager game_event_manager = new EventManager();
            EventManager test_event_manager = new EventManager();
            StateMachine state_machine = new StateMachine(test_event_manager);
            GameManager game_manager = new GameManager(game_event_manager, null);
            GameObject destination_game_object = new GameObject("Destination");
            ShopData shop_data = new ShopData("TS_RT", 8, 20);
            List<ItemData> item_data_table = new List<ItemData>();
            ShopComponent shop_component = new ShopComponent(shop_data, destination_game_object);
            LocationComponent destination_location = new LocationComponent("Shop", destination_game_object);
            GameObject current_game_object = new GameObject("Current");
            LocationComponent current_location = new LocationComponent("Current", current_game_object);
            ExploringState exploringstate = new ExploringState(state_machine, game_manager, test_event_manager);

            destination_game_object.AddComponent(shop_component);
            current_location.AddLocation(destination_location, 1.0f);
            game_manager.SetCurrentLocation(current_location);
            game_manager.SetSelectedDestinationIndex(0);
            state_machine.SetInitialState(exploringstate);
            test_event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.IsInstanceOf<ShopOpenState>(state_machine.GetCurrentState());
        }
    }
}
