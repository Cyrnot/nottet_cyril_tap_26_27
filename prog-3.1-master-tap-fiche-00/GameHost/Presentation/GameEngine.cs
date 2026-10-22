using GameHost.Render;
using GameHost.Save;
using GameLibrary.Core;
using GameLibrary.Core.Events;
using GameLibrary.Core.States;
using GameLibrary.Csv;
using GameLibrary.Data;
using System.Diagnostics;

namespace GameHost.Presentation
{
    public class GameEngine
    {
        private const float FIXED_FRAME_TIME = 20 / 1000.0f;

        private readonly Stopwatch _stopwatch = new Stopwatch();


        private EventManager _eventManager = new EventManager();
        private GameManager _gameManager;
        private WorldBuilder _worldBuilder;
        private LogManager _logManager;

        private readonly Dictionary<Type, IScreenRenderer> _screenRendererTable;
        private readonly string _dataDirectoryPath;
        private readonly string _savesDirectoryPath;

        private SaveFileWriter _saveFileWriter;
        private SaveFileLoader _saveFileLoader;

        public GameEngine(Dictionary<Type, IScreenRenderer> renderer_table, string game_data_path)
        {
            _screenRendererTable = renderer_table;
            _dataDirectoryPath = game_data_path + "Data/";
            _savesDirectoryPath = game_data_path + "Saves/";

            string log_file_path = game_data_path + "debug.log";
            _logManager = new LogManager(new LogFileWriter(log_file_path),_eventManager);
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
            string mine_ores_content = GetFileContent("mine_ores.csv", report);
            string mines_content = GetFileContent("mines.csv", report);


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
                    if (game_content_data._locationDataTable == null)
                    {
                        report.AddError("Shop.csv", 0, "Cannot validate shops because locations.csv was not loaded.");
                    }
                    else
                    {
                        game_content_data._shopDataTable = ShopReader.Read(shop_csv_table, game_content_data._locationDataTable, report);
                    }
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

            if (mines_content != null)
            {
                CsvTable mines_csv_table = CsvReader.Read("mines.csv", mines_content, report);
                if (mines_csv_table != null)
                {
                    game_content_data._mineDataTable = MineReader.Read(mines_csv_table, game_content_data._locationDataTable, game_content_data._shopDataTable, report);
                }
            }

            if (mine_ores_content != null)
            {
                CsvTable mine_ores_csv_table = CsvReader.Read("mine_ores.csv", mine_ores_content, report);
                if (mine_ores_csv_table != null)
                {
                    game_content_data._mineOreDataTable = MineOreReader.Read(mine_ores_csv_table, game_content_data._mineDataTable, game_content_data._itemDataTable, report);
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

                _saveFileWriter = new SaveFileWriter(_savesDirectoryPath, _gameManager, _eventManager);
                _saveFileLoader = new SaveFileLoader(_savesDirectoryPath, _gameManager, _eventManager);
                _saveFileLoader.LoadAllSaves();

                try
                {
                    bool first_render = true;

                    float renderElapsedTime = 0;

                    while (!_gameManager.GetShouldQuit())
                    {
                        float loop_start_time = GetCurrentTime();
                        float elapsed_time = loop_start_time - last_time;

                        if (_gameManager.GetIsReplaying())
                        {
                            elapsed_time *= _gameManager.GetReplaySpeed();
                        }

                        lag += elapsed_time;

                        renderElapsedTime += elapsed_time;

                        bool action_received = ProcessInput();

                        while (lag >= FIXED_FRAME_TIME)
                        {
                            FixedUpdate(FIXED_FRAME_TIME);

                            _eventManager.ProcessDelayedEvents();

                            lag -= FIXED_FRAME_TIME;
                        }

                        Update(elapsed_time);

                        if (first_render || action_received || renderElapsedTime >= 0.1f)
                        {
                            Render();
                            first_render = false;
                            renderElapsedTime = 0;
                        }

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
        private string GetFileContent(string file_name, ContentReport report)
        {
            string file_content = null;
            string file_path = $"{_dataDirectoryPath}{file_name}";

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


        private bool ProcessInput()
        {
            bool action_received = false;

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo player_command = Console.ReadKey(true);

                GameActionType action;

                if (_gameManager.GetIsReplaying())
                {
                    continue;
                }

                switch (player_command.Key)
                {
                    case ConsoleKey.UpArrow:
                        action = GameActionType.NAVIGATE_UP;
                        break;

                    case ConsoleKey.DownArrow:
                        action = GameActionType.NAVIGATE_DOWN;
                        break;

                    case ConsoleKey.Enter:
                        action = GameActionType.CONFIRM;
                        break;

                    case ConsoleKey.Backspace:
                        action = GameActionType.CANCEL;
                        break;

                    case ConsoleKey.Escape:
                        action = GameActionType.QUIT;
                        break;

                    default:
                        continue;

                }
                    _eventManager.TriggerEvent(new GameActionGameEvent(action));
                    action_received = true;
            }

            return action_received;
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

            _screenRendererTable[current_game_flow_state.GetType()].Render(current_game_flow_state);

        }

        private float GetCurrentTime()
        {
            return _stopwatch.ElapsedMilliseconds / 1000.0f;
        }
    }
}