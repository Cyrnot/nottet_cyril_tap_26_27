using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Replay
{
    public class ReplayActionData
    {
        public int _fixedStepIndex;
        public GameActionType _action;

        public ReplayActionData(int fixed_step_index, GameActionType action)
        {
            _fixedStepIndex = fixed_step_index;
            _action = action;
        }
    }
}
