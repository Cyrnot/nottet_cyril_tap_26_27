using NUnit.Framework;
using GameLibrary.Core;
using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
using GameLibrary.Core.States;
using GameLibrary.Data;
using System.Collections.Generic;

namespace GameLibraryTests
{
    public class ShopStateTests
    {
        [Test]
        public void FixedUpdate_CommingInShopOpenAndClosed()
        {
            EventManager event_manager = new EventManager();
            StateMachine state_machine = new StateMachine(event_manager);
            WorldBuilder builder = new WorldBuilder(new List<LocationData>() { new LocationData("W", "World", "", 0) }, new List<ShopData>(), null);
            GameManager game_manager = new GameManager(event_manager, builder);
            GameObject shopObject = new GameObject("shop");
            LocationComponent shop_location = new LocationComponent("shop", shopObject);
            GameObject parent_object = new GameObject("parent");
            LocationComponent parent_location = new LocationComponent("parent", parent_object);
            ShopData shop_data_open = new ShopData("shop", 8, 18);
            List<ItemData> item_data_table = new List<ItemData>() { new ItemData("CP_GH", "Colt Peacemaker", "GS_DT", 50, 3) };
            ShopComponent shop_component = new ShopComponent(shop_data_open, item_data_table, shopObject);
            ShopOpenState open_state = new ShopOpenState(state_machine, game_manager, event_manager, shop_component);

            parent_object.AddComponent(parent_location);
            shopObject.AddComponent(shop_location);
            shop_location.SetParentLocation(parent_location);
            state_machine.SetInitialState(open_state);
            state_machine.FixedUpdate(0f);

            Assert.That(state_machine.GetCurrentState().GetType(), Is.EqualTo(typeof(ShopOpenState)));

            shop_component.FixedUpdate(60.0f);
            state_machine.FixedUpdate(0f);

            Assert.That(state_machine.GetCurrentState().GetType(), Is.EqualTo(typeof(ShopClosedState)));
        }

        [Test]
        public void FixedUpdate_ShopClosedDuringVisit()
        {
            EventManager event_manager = new EventManager();
            StateMachine state_machine = new StateMachine(event_manager);
            WorldBuilder builder = new WorldBuilder(new List<LocationData>() { new LocationData("W", "World", "", 0) }, new List<ShopData>(), null);
            GameManager game_manager = new GameManager(event_manager, builder);
            GameObject shopObject = new GameObject("shop");
            LocationComponent shop_location = new LocationComponent("shop", shopObject);
            GameObject parent_object = new GameObject("parent");
            LocationComponent parent_location = new LocationComponent("parent", parent_object);
            ShopData shop_data = new ShopData("shop", 8, 9);
            List<ItemData> item_data_table = new List<ItemData>() { new ItemData("CP_GH", "Colt Peacemaker", "GS_DT", 50, 3) };
            ShopComponent shop_component = new ShopComponent(shop_data, item_data_table, shopObject);
            ShopOpenState open_state = new ShopOpenState(state_machine, game_manager, event_manager, shop_component);

            parent_object.AddComponent(parent_location);
            shopObject.AddComponent(shop_location);
            shop_location.SetParentLocation(parent_location);
            state_machine.SetInitialState(open_state);
            shop_component.FixedUpdate(6f);
            state_machine.FixedUpdate(0f);

            Assert.That(state_machine.GetCurrentState().GetType(), Is.EqualTo(typeof(ShopClosedState)));
        }

        [Test]
        public void selectedItemIndex_ReturnMinusOneAfterConstruction()
        {
            EventManager event_manager = new EventManager();
            StateMachine state_machine = new StateMachine(event_manager);
            WorldBuilder builder = new WorldBuilder(new List<LocationData>() { new LocationData("W", "World", "", 0) }, new List<ShopData>(), null);
            GameManager game_manager = new GameManager(event_manager, builder);
            GameObject shop_object = new GameObject("shop");
            LocationComponent shop_location = new LocationComponent("shop", shop_object);
            ShopData shop_data = new ShopData("shop", 9, 10);
            List<ItemData> item_data_table = new List<ItemData>() { new ItemData("CP_GH", "Colt Peacemaker", "GS_DT", 50, 3) };
            ShopComponent shop_component = new ShopComponent(shop_data, item_data_table, shop_object);
            ShopOpenState open_state = new ShopOpenState(state_machine, game_manager, event_manager, shop_component);

            shop_object.AddComponent(shop_location);
            state_machine.SetInitialState(open_state);
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(shop_component.GetQuantityAtIndex(0), Is.EqualTo(3));
        }

        [Test]
        public void OnGameActionGameEvent_DecreaseQuantityOutOfStock()
        {
            EventManager event_manager = new EventManager();
            StateMachine state_machine = new StateMachine(event_manager);
            WorldBuilder builder = new WorldBuilder(new List<LocationData>() { new LocationData("W", "World", "", 0) }, new List<ShopData>(), null);
            GameManager game_manager = new GameManager(event_manager, builder);
            GameObject shop_object = new GameObject("shop");
            LocationComponent shop_location = new LocationComponent("shop", shop_object);
            ShopData shop_data = new ShopData("shop", 9, 10);
            List<ItemData> item_data_table = new List<ItemData>() { new ItemData("CP_GH", "Colt Peacemaker", "GS_DT", 50, 0) };
            ShopComponent shop_component = new ShopComponent(shop_data, item_data_table, shop_object);
            ShopOpenState open_state = new ShopOpenState(state_machine, game_manager, event_manager, shop_component);

            shop_object.AddComponent(shop_location);
            state_machine.SetInitialState(open_state);
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(shop_component.GetQuantityAtIndex(0), Is.EqualTo(0));
        }

        [Test]
        public void Exit_ChangeStateToExploringState()
        {
            EventManager event_manager = new EventManager();
            StateMachine state_machine = new StateMachine(event_manager);
            GameManager game_manager = new GameManager(event_manager, null);
            GameObject shop_object = new GameObject("shop");
            LocationComponent shop_location = new LocationComponent("shop", shop_object);
            GameObject parent_object = new GameObject("parent");
            LocationComponent parent_location = new LocationComponent("parent", parent_object);
            ShopData shopData = new ShopData("shop", 9, 10);
            List<ItemData> item_data_table = new List<ItemData>() { new ItemData("CP_GH", "Colt Peacemaker", "GS_DT", 50, 3) };
            ShopComponent shop_component = new ShopComponent(shopData, item_data_table, shop_object);
            ShopClosedState closed_state = new ShopClosedState(state_machine, game_manager, event_manager, shop_component);

            parent_object.AddComponent(parent_location);
            shop_object.AddComponent(shop_location);
            shop_location.SetParentLocation(parent_location);
            state_machine.SetInitialState(closed_state);
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CANCEL));

            Assert.That(state_machine.GetCurrentState().GetType(), Is.EqualTo(typeof(ExploringState)));
            Assert.That(game_manager.GetCurrentLocation(), Is.EqualTo(parent_location));
        }
    }
}