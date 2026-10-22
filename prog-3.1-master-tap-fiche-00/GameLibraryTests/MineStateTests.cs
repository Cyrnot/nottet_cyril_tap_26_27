using GameLibrary.Core;
using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
using GameLibrary.Core.States;
using GameLibrary.Data;

namespace GameLibraryTests
{
    // Fiche 05, partie 2 : le coup de pioche.
    // Les tests jouent comme un joueur, à travers GameManager : ils ne dépendent donc
    // pas du découpage de vos états (un état « mine » et un état « coup en cours »,
    // ou un seul état). Seules les méthodes marquées ADAPTEZ connaissent vos noms.
    //
    // Le monde : WORLD → MINE_LEVEL_1, un seul héros au sac vide.
    public class MineStateTests
    {
        private const float FIXED_DELTA_TIME = 0.02f;
        private const int SEED = 1234567;

        // ADAPTEZ : la durée d'un coup en pas fixes, avec la conversion des magasins.
        // Fiche 03 : 10 minutes de jeu par seconde réelle, pas de 0.02 s → 0.2 minute par pas.
        // 2 minutes par coup → 10 pas.
        private const int MINUTES_PER_STRIKE = 2;
        private const int STRIKE_STEP_COUNT = 10;

        private static readonly string[] ORE_ID_TABLE = new string[] { "CO_SMD", "SO_SMD", "GN_SMD" };

        // ADAPTEZ : remplissez votre GameContentData (noms des champs mines et minerais).
        private GameContentData CreateGameContent(int vein_stock)
        {
            GameContentData game_content_data = new GameContentData();

            game_content_data._locationDataTable = new List<LocationData>();
            game_content_data._locationDataTable.Add(new LocationData("W", "World", "", -1));
            game_content_data._locationDataTable.Add(new LocationData("SMD_W", "Silver Mine Dungeon", "W", 15));
            game_content_data._shopDataTable = new List<ShopData>();
            game_content_data._itemDataTable = new Dictionary<string, ItemData>();
            game_content_data._itemDataTable.Add("CO_SMD", new ItemData("CO_SMD", "Copper ore", 10));
            game_content_data._itemDataTable.Add("SO_SMD", new ItemData("SO_SMD", "Silver ore", 25));
            game_content_data._itemDataTable.Add("GN_SMD", new ItemData("GN_SMD", "Gold nugget", 100));
            game_content_data._inventoryDataTable = new List<InventoryEntryData>();
            game_content_data._heroDataTable = new List<HeroData>();
            game_content_data._heroDataTable.Add(new HeroData("B_P", "Bob", 50, 100));
            game_content_data._mineDataTable = new List<MineData>();
            game_content_data._mineDataTable.Add(new MineData("SMD_W", MINUTES_PER_STRIKE, vein_stock));
            game_content_data._mineOreDataTable = new List<MineOreData>();
            game_content_data._mineOreDataTable.Add(new MineOreData("SMD_W", "CO_SMD", 60));
            game_content_data._mineOreDataTable.Add(new MineOreData("SMD_W", "SO_SMD", 35));
            game_content_data._mineOreDataTable.Add(new MineOreData("SMD_W", "GN_SMD", 5));

            return game_content_data;
        }

        // ADAPTEZ : le constructeur de votre GameManager, et son initialisation s'il en a une.
        private GameManager CreateGameManager(EventManager event_manager, int vein_stock)
        {
            return new GameManager(event_manager, CreateGameContent(vein_stock));
        }

        // ADAPTEZ : ce qu'il faut pour être en exploration à la racine, avec la graine donnée.
        private void StartGame(GameManager game_manager, EventManager event_manager)
        {
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
            event_manager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));

            game_manager.StartNewGame(SEED);

            Assert.That(game_manager.GetCurrentGameFlowState(), Is.InstanceOf<ExploringState>(), "Après le démarrage, le jeu doit être en exploration.");
        }

        // ADAPTEZ : le filon restant de la mine où se trouve le joueur.
        private int GetVeinStock(GameManager game_manager)
        {
            return game_manager.GetCurrentLocation().GetGameObject().GetComponent<MineComponent>().GetStock();
        }

        private int GetOreQuantity(GameManager game_manager, string ore_id)
        {
            return game_manager.GetHeroInventory().GetQuantity(ore_id);
        }

        private int GetTotalOreQuantity(GameManager game_manager)
        {
            int total_quantity = 0;

            for (int ore_index = 0; ore_index < ORE_ID_TABLE.Length; ore_index++)
            {
                total_quantity += GetOreQuantity(game_manager, ORE_ID_TABLE[ore_index]);
            }

            return total_quantity;
        }

        private void RunSteps(GameManager game_manager, EventManager event_manager, int step_count)
        {
            for (int step_index = 0; step_index < step_count; step_index++)
            {
                game_manager.FixedUpdate(FIXED_DELTA_TIME);
                event_manager.ProcessDelayedEvents();
            }
        }

        // Comme une touche pressée : l'action est traitée à la fin du pas suivant.
        private void PlayAction(GameManager game_manager, EventManager event_manager, GameActionType action_type)
        {
            event_manager.TriggerDelayedEvent(new GameActionGameEvent(action_type));
            RunSteps(game_manager, event_manager, 1);
        }

        private GameManager StartGameInMine(EventManager event_manager, int vein_stock)
        {
            GameManager game_manager = CreateGameManager(event_manager, vein_stock);

            StartGame(game_manager, event_manager);
            PlayAction(game_manager, event_manager, GameActionType.NAVIGATE_DOWN);
            PlayAction(game_manager, event_manager, GameActionType.CONFIRM);

            return game_manager;
        }

        private void PlayFullStrike(GameManager game_manager, EventManager event_manager)
        {
            PlayAction(game_manager, event_manager, GameActionType.CONFIRM);
            RunSteps(game_manager, event_manager, STRIKE_STEP_COUNT);
        }

        [Test]
        public void Strike_AddsOneOreToTheHeroAndRemovesOneFromTheVein()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = StartGameInMine(event_manager, 10);

            PlayFullStrike(game_manager, event_manager);

            Assert.That(GetTotalOreQuantity(game_manager), Is.EqualTo(1));
            Assert.That(GetVeinStock(game_manager), Is.EqualTo(9));
        }

        [Test]
        public void Strike_ChangesNothingBeforeItsLastStep()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = StartGameInMine(event_manager, 10);
            PlayAction(game_manager, event_manager, GameActionType.CONFIRM);

            RunSteps(game_manager, event_manager, STRIKE_STEP_COUNT - 1);
            int ore_quantity_before_last_step = GetTotalOreQuantity(game_manager);
            int vein_stock_before_last_step = GetVeinStock(game_manager);
            RunSteps(game_manager, event_manager, 1);

            Assert.That(ore_quantity_before_last_step, Is.EqualTo(0));
            Assert.That(vein_stock_before_last_step, Is.EqualTo(10));
            Assert.That(GetTotalOreQuantity(game_manager), Is.EqualTo(1));
            Assert.That(GetVeinStock(game_manager), Is.EqualTo(9));
        }

        [Test]
        public void Confirm_DuringAStrikeIsIgnored()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = StartGameInMine(event_manager, 10);
            PlayAction(game_manager, event_manager, GameActionType.CONFIRM);
            RunSteps(game_manager, event_manager, 3);

            PlayAction(game_manager, event_manager, GameActionType.CONFIRM);
            RunSteps(game_manager, event_manager, STRIKE_STEP_COUNT);

            Assert.That(GetTotalOreQuantity(game_manager), Is.EqualTo(1));
            Assert.That(GetVeinStock(game_manager), Is.EqualTo(9));
        }

        [Test]
        public void Cancel_DuringAStrikeChangesNeitherTheInventoryNorTheVein()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = StartGameInMine(event_manager, 10);
            PlayAction(game_manager, event_manager, GameActionType.CONFIRM);
            RunSteps(game_manager, event_manager, STRIKE_STEP_COUNT / 2);

            PlayAction(game_manager, event_manager, GameActionType.CANCEL);
            RunSteps(game_manager, event_manager, STRIKE_STEP_COUNT);

            Assert.That(GetTotalOreQuantity(game_manager), Is.EqualTo(0));
            Assert.That(GetVeinStock(game_manager), Is.EqualTo(10));
        }

        // Une partie avec un coup annulé, une partie sans : même graine, mêmes minerais.
        // Si le jet était tiré au début du coup, la suite serait décalée d'un cran.
        [Test]
        public void Cancel_DuringAStrikeDoesNotChangeTheFollowingDraws()
        {
            EventManager cancelled_event_manager = new EventManager();
            GameManager game_with_cancel = StartGameInMine(cancelled_event_manager, 10);
            EventManager reference_event_manager = new EventManager();
            GameManager game_without_cancel = StartGameInMine(reference_event_manager, 10);

            PlayAction(game_with_cancel, cancelled_event_manager, GameActionType.CONFIRM);
            RunSteps(game_with_cancel, cancelled_event_manager, STRIKE_STEP_COUNT / 2);
            PlayAction(game_with_cancel, cancelled_event_manager, GameActionType.CANCEL);

            for (int strike_index = 0; strike_index < 5; strike_index++)
            {
                PlayFullStrike(game_with_cancel, cancelled_event_manager);
                PlayFullStrike(game_without_cancel, reference_event_manager);
            }

            for (int ore_index = 0; ore_index < ORE_ID_TABLE.Length; ore_index++)
            {
                string ore_id = ORE_ID_TABLE[ore_index];

                Assert.That(GetOreQuantity(game_with_cancel, ore_id), Is.EqualTo(GetOreQuantity(game_without_cancel, ore_id)), ore_id);
            }

            Assert.That(GetVeinStock(game_with_cancel), Is.EqualTo(5));
        }

        [Test]
        public void Confirm_OnAnExhaustedVeinChangesNothing()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = StartGameInMine(event_manager, 0);

            PlayFullStrike(game_manager, event_manager);
            RunSteps(game_manager, event_manager, STRIKE_STEP_COUNT);

            Assert.That(GetTotalOreQuantity(game_manager), Is.EqualTo(0));
            Assert.That(GetVeinStock(game_manager), Is.EqualTo(0));
        }

        [Test]
        public void Confirm_IsRefusedOnceTheVeinIsExhausted()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = StartGameInMine(event_manager, 2);

            PlayFullStrike(game_manager, event_manager);
            PlayFullStrike(game_manager, event_manager);
            PlayFullStrike(game_manager, event_manager);

            Assert.That(GetTotalOreQuantity(game_manager), Is.EqualTo(2));
            Assert.That(GetVeinStock(game_manager), Is.EqualTo(0));
        }

        [Test]
        public void Cancel_OutsideAStrikeReturnsToExploration()
        {
            EventManager event_manager = new EventManager();
            GameManager game_manager = StartGameInMine(event_manager, 10);

            PlayAction(game_manager, event_manager, GameActionType.CANCEL);

            Assert.That(game_manager.GetCurrentGameFlowState(), Is.InstanceOf<ExploringState>());
        }
    }
}
