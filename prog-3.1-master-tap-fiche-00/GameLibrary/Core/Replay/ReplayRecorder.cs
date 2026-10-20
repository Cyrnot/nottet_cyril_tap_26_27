using GameLibrary.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Replay
{
    public class ReplayRecorder
    {
        private int _seed;
        private EventManager _eventManager;
        private IFixedStepClock _fixedStepClock;

        private List<ReplayActionData> _replayActionDataTable = new List<ReplayActionData>();

        public ReplayRecorder(int seed, EventManager event_manager, IFixedStepClock fixed_step_clock)
        {
            _seed = seed;
            _eventManager = event_manager;
            _fixedStepClock = fixed_step_clock;

            event_manager.RegisterToEvent<GameActionGameEvent>(OnGameActionGameEvent);
        }

        private void OnGameActionGameEvent(IGameEvent game_event)
        {
            GameActionGameEvent game_action_game_event = game_event as GameActionGameEvent;

            int fixed_step_index;

            if (game_action_game_event._fixedStepIndex >= 0)
            {
                fixed_step_index = game_action_game_event._fixedStepIndex;
            }
            else
            {
                fixed_step_index = _fixedStepClock.GetFixedStepIndex();
            }

            Record(fixed_step_index, game_action_game_event._gameActionType);
        }

        public int GetActionDataCount()
        {
            return _replayActionDataTable.Count();
        }

        public string GetCsvTextEnd(int fixed_step_end)
        {
            StringBuilder csv_text_string_builder = new StringBuilder(GetCsvTextWithoutEnd());

            if (fixed_step_end > -1)
            {
                csv_text_string_builder.AppendLine($"{fixed_step_end};END;");
            }

            return csv_text_string_builder.ToString();
        }

        public string GetCsvTextWithoutEnd()
        {
            StringBuilder csv_text_string_builder = new StringBuilder();
            csv_text_string_builder.AppendLine("Step;Type;Value");
            csv_text_string_builder.AppendLine($"0;SEED;{_seed}");

            for (int replay_action_index = 0; replay_action_index < _replayActionDataTable.Count; replay_action_index++)
            {
                ReplayActionData current_replay_action_data = _replayActionDataTable[replay_action_index];
                csv_text_string_builder.AppendLine($"{current_replay_action_data._fixedStepIndex};ACTION;{current_replay_action_data._action}");
            }

            return csv_text_string_builder.ToString();
        }

        public void Record(int fixed_step, GameActionType action)
        {
            ReplayActionData replay_action_data = new ReplayActionData(fixed_step, action);
            _replayActionDataTable.Add(replay_action_data);
        }

        public void StopRecording()
        {
            _eventManager.UnregisterFromEvent<GameActionGameEvent>(OnGameActionGameEvent);
        }
    }
}
