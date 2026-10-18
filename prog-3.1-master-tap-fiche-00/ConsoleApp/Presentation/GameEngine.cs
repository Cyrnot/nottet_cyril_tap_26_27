using GameLibrary.Core;
using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
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

        public GameEngine()
        {
            _logManager = new LogManager(new LogFileWriter("../../../debug.log"), _eventManager);
        }

        public void Run()
        {
            _stopwatch.Start();

            float lag = 0.0f;

            float last_time = GetCurrentTime();

            ContentReport report = new ContentReport();
            List<LocationData> location_data_table = null;

            string locations_content = GetFileContent("locations.csv", report);

            if (locations_content != null)
            {
                CsvTable location_csv_table = CsvReader.Read("locations.csv", locations_content, report);

                if (location_csv_table != null)
                {
                    location_data_table = LocationReader.Read(location_csv_table, report);
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
                _gameManager = new GameManager(_eventManager, new WorldBuilder(location_data_table));
            }

            while (!_gameManager.GetShouldQuit())
            {
                float loop_start_time = GetCurrentTime();
                float elapsed_time = loop_start_time - last_time;

                lag += elapsed_time;

                ProcessInput();

                while (lag >= FIXED_FRAME_TIME)
                {
                    FixedUpdate(FIXED_FRAME_TIME);

                    _eventManager.ProcessDelayEvents();

                    lag -= FIXED_FRAME_TIME;
                }

                Update(elapsed_time);

                Render();

                last_time = loop_start_time;
            }

            _eventManager.TriggerEvent(new LogMessageGameEvent("Goodbye !"));
        }
        private static string GetFileContent(string file_name, ContentReport report)
        {
            string file_content = null;
            string file_path = $"../../../Data/{file_name}";

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
            if (_gameManager.GetIsMenuStateActive())
            {
                _renderManager.Draw(0, 0, "Enter to start", ConsoleColor.Green, ConsoleColor.Black);
                _renderManager.Draw(0, 1, "Esc to quit", ConsoleColor.Green, ConsoleColor.Black);
            }
            else
            {
                _renderManager.Draw(0, 0, "Game in progress...\n", ConsoleColor.Magenta, ConsoleColor.Black);

                LocationComponent current_location = _gameManager.GetCurrentLocation();

                _renderManager.Draw(0, 1, $"Exploring {current_location.GetName()}\n", ConsoleColor.Cyan, ConsoleColor.Black);

                for (int location_index = 0; location_index < current_location.GetChildLocationCount(); location_index++)
                {
                    Connection current_connection = current_location.GetChildLocationAtIndex(location_index);

                    if (location_index == _gameManager.GetSelectedDestinationIndex())
                    {
                        _renderManager.Draw(0, 2 + location_index, $"{location_index + 1}. {current_connection.GetDestinationLocation().GetName()}: Duration: {current_connection.GetTimeToReachDestination()}", ConsoleColor.Black, ConsoleColor.Green);
                    }
                    else
                    {
                        _renderManager.Draw(0, 2 + location_index, $"{location_index + 1}. {current_connection.GetDestinationLocation().GetName()}: Duration: {current_connection.GetTimeToReachDestination()}", ConsoleColor.Green, ConsoleColor.Black);
                    }
                }
            }
            _renderManager.Render();
        }

        private float GetCurrentTime()
        {
            return _stopwatch.ElapsedMilliseconds / 1000.0f;
        }
    }
}