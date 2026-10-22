using GameLibrary.Core;
using GameLibrary.Core.Events;
using GameLibrary.Core.States;
using GameLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibraryTests.State
{
    public class MainMenuStateTests
    {
        private static GameManager CreateGameManager()
        {
            EventManager event_manager = new EventManager();
            GameContentData game_content_data = new GameContentData
            {
                _locationDataTable = new List<LocationData>{new LocationData("W", "World", "", 0)},
                _shopDataTable = new List<ShopData>(),
                _inventoryDataTable = new List<InventoryEntryData>(),
                _itemDataTable = new Dictionary<string, ItemData>(),
                _heroDataTable = new List<HeroData>(),
                _mineDataTable = new List<MineData>()
            }; 
            return new GameManager(event_manager, game_content_data);
        }

        private static string CreateFakeSave()
        {
            return string.Join(Environment.NewLine, "Step;Type;Value", "0;SEED;1234567", "1;ACTION;CONFIRM", "2;ACTION;QUIT", "2;END;");
        }

        [Test]
        public void AddSave_ValidSaveAppearsInMainMenu()
        {
            GameManager game_manager = CreateGameManager();
            MainMenuState main_menu_state = (MainMenuState)game_manager.GetGameFlowStateMachine();

            game_manager.AddSave("fake-save.csv", CreateFakeSave());
            
            Assert.That(main_menu_state.GetMenuItemTable().Any(menu_entry => menu_entry.GetText().Contains("fake-save.csv")),Is.True);
        }

        [Test]
        public void AddSave_InvalidSaveAppearsAsDamaged()
        {
            GameManager game_manager = CreateGameManager();
            MainMenuState main_menu_state = (MainMenuState)game_manager.GetGameFlowStateMachine();

            game_manager.AddSave("invalid-save.csv", "invalid content");

            Assert.That(main_menu_state.GetMenuItemTable().Any(menu_entry => menu_entry.GetText().Contains("(Damaged)")),Is.True);
        }

        [Test]
        public void Confirm_DamagedSaveDoesNotChangeState()
        {
            GameManager game_manager = CreateGameManager();
            EventManager event_manager = game_manager.GetEventManager();
            IState initial_state = game_manager.GetGameFlowStateMachine();

            game_manager.AddSave("invalid-save.csv", "invalid content");
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetGameFlowStateMachine(),Is.SameAs(initial_state));
            Assert.That(game_manager.GetGameFlowStateMachine(),Is.TypeOf<MainMenuState>());
        }

        [Test]
        public void Confirm_ReplaySpeedChangesFrom20To100ThenTo1()
        {
            GameManager game_manager = CreateGameManager();
            EventManager event_manager = game_manager.GetEventManager();
            MainMenuState main_menu_state =(MainMenuState)game_manager.GetGameFlowStateMachine();

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));

            Assert.That(main_menu_state.GetMenuItemTable()[1].GetText(),Is.EqualTo("Replay speed: 20x"));

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetReplaySpeed(), Is.EqualTo(100));
            Assert.That(main_menu_state.GetMenuItemTable()[1].GetText(), Is.EqualTo("Replay speed: 100x"));

            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetReplaySpeed(), Is.EqualTo(1));
            Assert.That(main_menu_state.GetMenuItemTable()[1].GetText(), Is.EqualTo("Replay speed: 1x"));
        }

        [Test]
        public void Confirm_ValidSaveStartsExplorationAndReplay()
        {
            GameManager game_manager = CreateGameManager();
            EventManager event_manager = game_manager.GetEventManager();
            game_manager.AddSave("fake-save.csv", CreateFakeSave());
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            Assert.That(game_manager.GetGameFlowStateMachine(),Is.TypeOf<ExploringState>());
            Assert.That(((ExploringState)game_manager.GetGameFlowStateMachine()).GetIsReplaying(), Is.True);
        }
    }
}
