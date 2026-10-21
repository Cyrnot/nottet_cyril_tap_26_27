using NUnit.Framework;
using GameLibrary.Core;
using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
using GameLibrary.Core.States;
using GameLibrary.Data;
using System.Collections.Generic;

namespace GameLibraryTests.State
{
    public class ShopStateTests
    {

        [Test]
        public void FixedUpdate_CommingInShopOpenAndClosed()
        {
            EventManager event_manager = new EventManager();
            StateMachine state_machine = new StateMachine(event_manager);
            GameContentData game_content_data = new GameContentData { _locationDataTable = new List<LocationData>(), _shopDataTable = new List<ShopData>(), _inventoryDataTable = new List<InventoryEntryData>(), _itemDataTable = new Dictionary<string, ItemData>() };
            game_content_data._locationDataTable.Add(new LocationData("W", "World", "", 0));
            game_content_data._locationDataTable.Add(new LocationData("GS_DT", "Gun Shop", "W", 10));
            game_content_data._shopDataTable.Add(new ShopData("GS_DT", 8, 9));
            GameManager game_manager = new GameManager(event_manager, game_content_data);

            game_manager.StartNewGame(1234567);
            GameObject shop_object = game_manager.GetCurrentLocation().GetChildLocationAtIndex(0).GetDestinationLocation().GetGameObject();
            ShopComponent shops_component = shop_object.GetComponent<ShopComponent>();
            InventoryComponent inventory_component = new InventoryComponent(shop_object);
            inventory_component.Add(new ItemData("CP_GH", "Colt Peacemaker", 50), 1);
            ShopOpenState open_state = new ShopOpenState(state_machine, game_manager, event_manager, shops_component, inventory_component);
            state_machine.SetInitialState(open_state);
            state_machine.FixedUpdate(0f);

            Assert.That(state_machine.GetCurrentState().GetType(), Is.EqualTo(typeof(ShopOpenState)));

            shops_component.FixedUpdate(6.0f);
            state_machine.FixedUpdate(0.0f);

            Assert.That(state_machine.GetCurrentState().GetType(), Is.EqualTo(typeof(ShopClosedState)));
        }

        [Test]
        public void FixedUpdate_ShopClosedDuringVisit()
        {
            EventManager event_manager = new EventManager();
            StateMachine state_machine = new StateMachine(event_manager);
            GameContentData game_content_data = new GameContentData { _locationDataTable = new List<LocationData>(), _shopDataTable = new List<ShopData>(), _inventoryDataTable = new List<InventoryEntryData>(), _itemDataTable = new Dictionary<string, ItemData>() };
            game_content_data._locationDataTable.Add(new LocationData("W", "World", "", 0));
            game_content_data._locationDataTable.Add(new LocationData("GS_DT", "Gun Shop", "W", 10));
            game_content_data._shopDataTable.Add(new ShopData("GS_DT", 8, 9));
            GameManager game_manager = new GameManager(event_manager, game_content_data);

            game_manager.StartNewGame(1234567);
            GameObject shop_object = game_manager.GetCurrentLocation().GetChildLocationAtIndex(0).GetDestinationLocation().GetGameObject();
            ShopComponent shops_component = shop_object.GetComponent<ShopComponent>();
            InventoryComponent inventory_component = new InventoryComponent(shop_object);
            inventory_component.Add(new ItemData("CP_GH", "Colt Peacemaker", 50), 1);
            ShopOpenState open_state = new ShopOpenState(state_machine, game_manager, event_manager, shops_component, inventory_component);
            state_machine.SetInitialState(open_state);
            shops_component.FixedUpdate(6.0f);
            state_machine.FixedUpdate(0.0f);

            Assert.That(state_machine.GetCurrentState().GetType(), Is.EqualTo(typeof(ShopClosedState)));
        }

        [Test]
        public void selectedItemIndex_ReturnMinusOneAfterConstruction()
        {
            EventManager event_manager = new EventManager();
            StateMachine state_machine = new StateMachine(event_manager);
            GameContentData game_content_data = new GameContentData { _locationDataTable = new List<LocationData>(), _shopDataTable = new List<ShopData>(), _inventoryDataTable = new List<InventoryEntryData>(), _itemDataTable = new Dictionary<string, ItemData>() };
            game_content_data._locationDataTable.Add(new LocationData("W", "World", "", 0));
            game_content_data._locationDataTable.Add(new LocationData("GS_DT", "Gun Shop", "W", 10));
            game_content_data._shopDataTable.Add(new ShopData("GS_DT", 8, 9));
            GameManager game_manager = new GameManager(event_manager, game_content_data);

            game_manager.StartNewGame(1234567);
            GameObject shop_object = game_manager.GetCurrentLocation().GetChildLocationAtIndex(0).GetDestinationLocation().GetGameObject();
            ShopComponent shops_component = shop_object.GetComponent<ShopComponent>();
            InventoryComponent inventory_component = new InventoryComponent(shop_object);
            ShopOpenState open_state = new ShopOpenState(state_machine, game_manager, event_manager, shops_component, inventory_component);
            state_machine.SetInitialState(open_state);

            Assert.That(open_state.GetSelectedItemIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void OnGameActionGameEvent_DecreaseQuantityOutOfStock()
        {
            EventManager event_manager = new EventManager();
            StateMachine state_machine = new StateMachine(event_manager);
            GameContentData game_content_data = new GameContentData { _locationDataTable = new List<LocationData>(), _shopDataTable = new List<ShopData>(), _inventoryDataTable = new List<InventoryEntryData>(), _itemDataTable = new Dictionary<string, ItemData>() };
            game_content_data._locationDataTable.Add(new LocationData("W", "World", "", 0));
            game_content_data._locationDataTable.Add(new LocationData("GS_DT", "Gun Shop", "W", 10));
            game_content_data._shopDataTable.Add(new ShopData("GS_DT", 8, 9));
            GameManager game_manager = new GameManager(event_manager, game_content_data);

            game_manager.StartNewGame(1234567);
            GameObject shop_object = game_manager.GetCurrentLocation().GetChildLocationAtIndex(0).GetDestinationLocation().GetGameObject();
            ShopComponent shops_component = shop_object.GetComponent<ShopComponent>();
            InventoryComponent inventory_component = new InventoryComponent(shop_object);
            inventory_component.Add(new ItemData("CP_GH", "Colt Peacemaker", 50), 1);
            ShopOpenState open_state = new ShopOpenState(state_machine, game_manager, event_manager, shops_component, inventory_component);
            state_machine.SetInitialState(open_state);
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(inventory_component.GetEntryAtIndex(0).GetQuantity(), Is.EqualTo(0));
        }

        [Test]
        public void Exit_ChangeStateToExploringState()
        {
            EventManager event_manager = new EventManager();
            StateMachine state_machine = new StateMachine(event_manager);
            GameContentData game_content_data = new GameContentData { _locationDataTable = new List<LocationData>(), _shopDataTable = new List<ShopData>(), _inventoryDataTable = new List<InventoryEntryData>(), _itemDataTable = new Dictionary<string, ItemData>() };
            game_content_data._locationDataTable.Add(new LocationData("W", "World", "", 0));
            game_content_data._locationDataTable.Add(new LocationData("GS_DT", "Gun Shop", "W", 10));
            game_content_data._shopDataTable.Add(new ShopData("GS_DT", 8, 9));
            GameManager game_manager = new GameManager(event_manager, game_content_data);

            game_manager.StartNewGame(1234567);
            GameObject shop_object = game_manager.GetCurrentLocation().GetChildLocationAtIndex(0).GetDestinationLocation().GetGameObject();
            ShopComponent shops_component = shop_object.GetComponent<ShopComponent>();
            InventoryComponent inventory_component = new InventoryComponent(shop_object);
            ShopOpenState open_state = new ShopOpenState(state_machine, game_manager, event_manager, shops_component, inventory_component);
            state_machine.SetInitialState(open_state);
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.QUIT));

            Assert.That(state_machine.GetCurrentState().GetType(), Is.EqualTo(typeof(ExploringState)));
        }
    }

}