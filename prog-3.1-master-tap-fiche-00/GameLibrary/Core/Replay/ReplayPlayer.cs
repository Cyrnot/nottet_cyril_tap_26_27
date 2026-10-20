using GameLibrary.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Replay
{
    public class ReplayPlayer
    {
        private ReplayData _replayData;
        private EventManager _eventManager;
        private IFixedStepClock _fixedStepClock;
        private int _currentReplayIndex;

        public ReplayPlayer(ReplayData replayData, EventManager event_manager, IFixedStepClock fixed_step_clock)
        {
            _replayData = replayData;
            _eventManager = event_manager;
            _fixedStepClock = fixed_step_clock;
        }

        public void FixedUpdate(float fixed_elapsed_time)
        {
            int fixed_step_index = _fixedStepClock.GetFixedStepIndex();

            if (!GetIsFinished(fixed_step_index))
            {
                TriggerActionsOfStep(fixed_step_index, _eventManager);
            }
        }

        public bool GetIsFinished(int fixed_step_index)
        {
            bool is_finished = false;

            if (_replayData._endFixedStepIndex >= 0)
            {
                is_finished = _replayData._endFixedStepIndex <= fixed_step_index;
            }
            else
            {
                is_finished = _replayData._actionTable.Count <= 0 || _replayData._actionTable[_replayData._actionTable.Count - 1]._fixedStepIndex < fixed_step_index;
            }

            return is_finished;
        }

        public void TriggerActionsOfStep(int fixed_step_index, EventManager event_manager)
        {
            while ((_replayData._actionTable.Count > _currentReplayIndex) && (_replayData._actionTable[_currentReplayIndex]._fixedStepIndex <= fixed_step_index))
            {
                GameActionType game_action_type = _replayData._actionTable[_currentReplayIndex]._action;

                if (_replayData._endFixedStepIndex != fixed_step_index)
                {
                    event_manager.TriggerDelayedEvent(new GameActionGameEvent(game_action_type, _replayData._actionTable[_currentReplayIndex]._fixedStepIndex));
                }

                ++_currentReplayIndex;
            }
        }
    }
}
