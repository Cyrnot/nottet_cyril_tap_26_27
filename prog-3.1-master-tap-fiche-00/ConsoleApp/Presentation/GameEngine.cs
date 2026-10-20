using GameConsole.Render;
using GameConsole.Save;
using GameLibrary.Core;
using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
using GameLibrary.Core.States;
using GameLibrary.Csv;
using GameLibrary.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GameConsole.Presentation
{
    public class GameEngine
    {
        private const float FIXED_FRAME_TIME = 20 / 1000.0f;

        private readonly Stopwatch _stopwatch = new Stopwatch();

        private readonly ConsoleRenderManager _renderManager = new ConsoleRenderManager();

        private EventManager _eventManager = new EventManager();
        private GameManager _gameManager;
        private WorldBuilder _worldBuilder;
        private LogManager _logManager;

        private readonly Dictionary<Type, IScreenRenderer> _screenRendererTable = new Dictionary<Type, IScreenRenderer>();

        private static readonly string DIRECTORY_PATH = "../../../";
        private static readonly string SAVES_DIRECTORY_PATH = $"{DIRECTORY_PATH}Saves/";
        private static readonly string DATA_DIRECTORY_PATH = $"{DIRECTORY_PATH}Data/";
        private static readonly string LOG_FILE_PATH = $"{DIRECTORY_PATH}debug.log";

        private SaveFileWriter _saveFileWriter;
        private SaveFileLoader _saveFileLoader;

        public GameEngine()
        {
            _logManager = new LogManager(new LogFileWriter(LOG_FILE_PATH), _eventManager);
        }

        public void Run()
        {
            _stopwatch.Start();

            float lag = 0.0f;

            float last_time = GetCurrentTime();

            ContentReport report = new ContentReport();

            GameContentData game_content_data = new GameContentData();

            string locations_content = GetFileContent("locations.csv", report);
            string items_content = GetFileContent("Item.csv", report);
            string shops_content = GetFileContent("Shop.csv", report);
            string inventory_content = GetFileContent("Inventory.csv", report);
            string heroes_content = GetFileContent("Hero.csv", report);


            if (locations_content != null)
            {
                CsvTable location_csv_table = CsvReader.Read("locations.csv", locations_content, report);

                if (location_csv_table != null)
                {
                    game_content_data._locationDataTable = LocationReader.Read(location_csv_table, report);
                }
            }

            if (shops_content != null)
            {
                CsvTable shop_csv_table = CsvReader.Read("Shop.csv", shops_content, report);

                if (shop_csv_table != null)
                {
                    game_content_data._shopDataTable = ShopReader.Read(shop_csv_table, game_content_data._locationDataTable, report);
                }
            }

            if (items_content != null)
            {
                CsvTable item_csv_table = CsvReader.Read("Item.csv", items_content, report);

                if (item_csv_table != null)
                {
                    game_content_data._itemDataTable = ItemReader.Read(item_csv_table, report);
                }
            }

            if (heroes_content != null)
            {
                CsvTable hero_csv_table = CsvReader.Read("Hero.csv", heroes_content, report);
                if (hero_csv_table != null)
                {
                    game_content_data._heroDataTable = HeroReader.Read(hero_csv_table, game_content_data._locationDataTable, report);
                }
            }

            if (inventory_content != null)
            {
                CsvTable inventory_csv_table = CsvReader.Read("Inventory.csv", inventory_content, report);

                if (inventory_csv_table != null)
                {
                    game_content_data._inventoryDataTable = InventoryReader.Read(inventory_csv_table, game_content_data._shopDataTable, game_content_data._heroDataTable, game_content_data._itemDataTable, report);
                }
            }

            if (report.GetErrorCount() > 0)
            {
                for (int error_index = 0; error_index < report.GetErrorCount(); error_index++)
                {
                    _eventManager.TriggerEvent(new LogMessageGameEvent(report.GetErrorTextAtIndex(error_index)));
                    Console.WriteLine(report.GetErrorTextAtIndex(error_index));
                }
            }
            else
            {
                _gameManager = new GameManager(_eventManager, game_content_data);

                _saveFileWriter = new SaveFileWriter(SAVES_DIRECTORY_PATH, _gameManager, _eventManager);
                _saveFileLoader = new SaveFileLoader(SAVES_DIRECTORY_PATH, _gameManager, _eventManager);
                _saveFileLoader.LoadAllSaves();

                _screenRendererTable.Add(typeof(MainMenuState), new MainMenuRenderer());
                _screenRendererTable.Add(typeof(ExploringState), new ExploringRenderer());
                _screenRendererTable.Add(typeof(ShopClosedState), new ShopClosedRenderer());
                _screenRendererTable.Add(typeof(ShopOpenState), new ShopOpenRenderer());

                try
                {
                    while (!_gameManager.GetShouldQuit())
                    {
                        float loop_start_time = GetCurrentTime();
                        float elapsed_time = loop_start_time - last_time;

                        lag += elapsed_time;

                        ProcessInput();

                        while (lag >= FIXED_FRAME_TIME)
                        {
                            FixedUpdate(FIXED_FRAME_TIME);

                            _eventManager.ProcessDelayedEvents();

                            lag -= FIXED_FRAME_TIME;
                        }

                        Update(elapsed_time);

                        Render();

                        last_time = loop_start_time;
                    }

                    _eventManager.TriggerEvent(new LogMessageGameEvent("Goodbye !"));
                }
                catch (Exception exception)
                {
                    _saveFileWriter.WriteCrashSave(_gameManager.GetSaveText());
                    _eventManager.TriggerEvent(new LogMessageGameEvent($"The game crashed: {exception.Message}"));
                    throw;
                }
            }
        }
        private static string GetFileContent(string file_name, ContentReport report)
        {
            string file_content = null;
            string file_path = $"{DATA_DIRECTORY_PATH}{file_name}";

            try
            {
                file_content = File.ReadAllText(file_path);
            }
            catch (FileNotFoundException exception)
            {
                report.AddError(file_name, -1, $"File not found: {exception.Message} at location {file_path}");
            }
            catch (Exception exception)
            {
                report.AddError(file_name, -1, $"An unexpected error happened: {exception.Message}");
            }

            return file_content;
        }


        private void ProcessInput()
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo player_command = Console.ReadKey(true);

                if (player_command.Key == ConsoleKey.UpArrow)
                {
                    _eventManager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_UP));
                }
                else if (player_command.Key == ConsoleKey.DownArrow)
                {
                    _eventManager.TriggerEvent(new GameActionGameEvent(GameActionType.NAVIGATE_DOWN));
                }
                else if (player_command.Key == ConsoleKey.Enter)
                {
                    _eventManager.TriggerEvent(new GameActionGameEvent(GameActionType.CONFIRM));
                }
                else if (player_command.Key == ConsoleKey.Backspace)
                {
                    _eventManager.TriggerEvent(new GameActionGameEvent(GameActionType.CANCEL));
                }
                else if (player_command.Key == ConsoleKey.Escape)
                {
                    _eventManager.TriggerEvent(new GameActionGameEvent(GameActionType.QUIT));
                }

            }
        }

        private void FixedUpdate(float fixed_elapsed_time)
        {
            _gameManager.FixedUpdate(fixed_elapsed_time);
        }

        private void Update(float elapsed_time)
        {
            _gameManager.Update(elapsed_time);
        }

        private void Render()
        {
            IState current_game_flow_state = _gameManager.GetGameFlowStateMachine();

            _screenRendererTable[current_game_flow_state.GetType()].Render(current_game_flow_state, _renderManager);

            _renderManager.Render();
        }

        private float GetCurrentTime()
        {
            return _stopwatch.ElapsedMilliseconds / 1000.0f;
        }
    }
}