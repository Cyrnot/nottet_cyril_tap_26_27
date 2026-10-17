using GameLibrary.Core.Events;
using GameLibrary.Core;
using System;
using System.Diagnostics;
using GameLibrary.Core.Components;

namespace GameConsole.Presentation
{
    public class GameEngine
    {
        private const float FIXED_FRAME_TIME = 20 / 1000.0f;

        private readonly Stopwatch _stopwatch = new Stopwatch();

        private readonly ConsoleRenderManager _renderManager = new ConsoleRenderManager();

        private EventManager _eventManager = new EventManager();
        private GameManager _gameManager;

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

            _gameManager = new GameManager(_eventManager);

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