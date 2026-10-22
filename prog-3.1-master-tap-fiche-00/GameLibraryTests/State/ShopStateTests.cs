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
        private EventManager _event_manager;
        private StateMachine _state_machine;
        private ShopComponent _shop_component;
        private InventoryComponent _inventory_component;
        private ShopOpenState _open_state;

        private void InitializeShop(bool include_item = false)
        {
            _event_manager = new EventManager();
            _state_machine = new StateMachine(_event_manager);

            GameContentData game_content_data = new GameContentData
            {
                _locationDataTable = new List<LocationData> {new LocationData("W", "World", "", 0),new LocationData("GS_DT", "Gun Shop", "W", 10)},
                _shopDataTable = new List<ShopData>{new ShopData("GS_DT", 8, 9)},
                _inventoryDataTable = new List<InventoryEntryData>(),
                _itemDataTable = new Dictionary<string, ItemData>(),
                _heroDataTable = new List<HeroData>()
            };

            GameManager game_manager = new GameManager(_event_manager, game_content_data);
            game_manager.StartNewGame(1234567);

            GameObject shop_object = game_manager.GetCurrentLocation().GetChildLocationAtIndex(0).GetDestinationLocation().GetGameObject();

            _shop_component = shop_object.GetComponent<ShopComponent>();
            _inventory_component = new InventoryComponent(shop_object);

            if (include_item)
            {
                _inventory_component.Add(new ItemData("CP_GH", "Colt Peacemaker", 50), 1);
            }

            _open_state = new ShopOpenState(_state_machine,game_manager,_event_manager,_shop_component,_inventory_component);

            _state_machine.SetInitialState(_open_state);
        }

        [Test]
        public void FixedUpdate_CommingInShopOpenAndClosed()
        {
            InitializeShop(include_item: true);

            _state_machine.FixedUpdate(0f);
            Assert.That(_state_machine.GetCurrentState().GetType(), Is.EqualTo(typeof(ShopOpenState)));

            _shop_component.FixedUpdate(6.0f);
            _state_machine.FixedUpdate(0f);

            Assert.That(_state_machine.GetCurrentState().GetType(), Is.EqualTo(typeof(ShopClosedState)));
        }

        [Test]
        public void FixedUpdate_ShopClosedDuringVisit()
        {
            InitializeShop(include_item: true);

            _shop_component.FixedUpdate(6.0f);
            _state_machine.FixedUpdate(0f);

            Assert.That(_state_machine.GetCurrentState().GetType(), Is.EqualTo(typeof(ShopClosedState)));
        }

        [Test]
        public void selectedItemIndex_ReturnMinusOneAfterConstruction()
        {
            InitializeShop();

            Assert.That(_open_state.GetSelectedItemIndex(), Is.EqualTo(-1));
        }

        [Test]
        public void OnGameActionGameEvent_DecreaseQuantityOutOfStock()
        {
            InitializeShop(include_item: true);

            _event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            _event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(_inventory_component.GetEntryAtIndex(0).GetQuantity(), Is.EqualTo(0));
        }

        [Test]
        public void Exit_ChangeStateToExploringState()
        {
            InitializeShop();

            _event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.QUIT));

            Assert.That(_state_machine.GetCurrentState().GetType(), Is.EqualTo(typeof(ExploringState)));
        }
    }

}