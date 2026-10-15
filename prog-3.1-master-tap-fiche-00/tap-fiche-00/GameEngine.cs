using activity_00_tap_26_27.Components;
using activity_00_tap_26_27.Events;
using Game;
using Game.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace activity_00_tap_26_27
{
    public class GameEngine
    {
        private const float FIXED_FRAME_TIME = 20 / 1000.0f;

        private readonly Stopwatch _stopwatch = new Stopwatch();

        private readonly List<GameObject> _gameObjectTable = new List<GameObject>();

        private readonly ConsoleRenderManager _renderManager = new ConsoleRenderManager();

        private bool _shouldQuit = false;

        private EventManager _eventManager = new EventManager();
        private LogManager _logManager = new LogManager();

        public void Run()
        {
            _stopwatch.Start();

            float lag = 0.0f;

            float last_time = GetCurrentTime();

            GameObject game_object_test = new GameObject("game_object_test");

            _eventManager.RegisterToEvent<RegisterGameObjectGameEvent> (OnRegisterGameObjectGameEvent);
            _eventManager.UnregisterFromEvent<UnregisterGameObjectGameEvent>(OnUnregisterGameObjectGameEvent);

            _eventManager.TriggerDelayedEvent(new RegisterGameObjectGameEvent(game_object_test));

            _logManager.InitialLog(_eventManager);
            if (File.Exists("Debug.txt"))
            {
                _eventManager.TriggerDelayedEvent(new LogMessageGameEvent("File Found at \"C:\\\\Users\\\\cyril\\\\Fichiers\\\\GitHub\\\\nottet_cyril_tap_26_27\\\\prog-3.1-master-tap-fiche-00 ! Text Add !"));
            }
            else
            {
                _eventManager.TriggerEvent(new LogMessageGameEvent("File Not Found at \"C:\\\\Users\\\\cyril\\\\Fichiers\\\\GitHub\\\\nottet_cyril_tap_26_27\\\\prog-3.1-master-tap-fiche-00 ! File Create !"));
            }

            while (!_shouldQuit)
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

            _eventManager.TriggerDelayedEvent(new LogMessageGameEvent("Goodbye !"));
        }

        private void OnRegisterGameObjectGameEvent(IGameEvent game_event)
        {
            RegisterGameObjectGameEvent register_game_object_game_event = game_event as RegisterGameObjectGameEvent;

            _gameObjectTable.Add(register_game_object_game_event._gameObject);
        }

        private void OnUnregisterGameObjectGameEvent(IGameEvent game_event)
        {
            UnregisterGameObjectGameEvent unregister_game_object_game_event = game_event as UnregisterGameObjectGameEvent;

            _gameObjectTable.Remove(unregister_game_object_game_event._gameObject);
        }

        private void ProcessInput()
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo player_command = Console.ReadKey(true);

                if (player_command.Key == ConsoleKey.Escape)
                {
                    _shouldQuit = true;
                }
            }
        }

        private void FixedUpdate(float fixed_elapsed_time)
        {
            for (int object_index = 0; object_index < _gameObjectTable.Count; object_index++)
            {
                GameObject game_object = _gameObjectTable[object_index];

                if (game_object.GetIsActive())
                {
                    game_object.FixedUpdate(fixed_elapsed_time);
                }
            }
        }

        private void Update(float elapsed_time)
        {
            for (int object_index = 0; object_index < _gameObjectTable.Count; object_index++)
            {
                GameObject game_object = _gameObjectTable[object_index];

                if (game_object.GetIsActive())
                {
                    game_object.Update(elapsed_time);
                }
            }
        }

        private void Render()
        {
            _renderManager.Draw(0,0, "Game in progress...\n", ConsoleColor.Magenta);
            _renderManager.Render();
        }

        private float GetCurrentTime()
        {
            return _stopwatch.ElapsedMilliseconds / 1000.0f;
        }
    }
}