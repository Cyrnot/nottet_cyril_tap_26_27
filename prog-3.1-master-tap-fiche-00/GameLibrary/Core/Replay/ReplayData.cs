using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Replay
{
    public class ReplayData
    {
        public int _seed;
        public int _endFixedStepIndex;
        public List<ReplayActionData> _actionTable;

        public ReplayData(int seed, List<ReplayActionData> action_table, int end_fixed_step_index)
        {
            _seed = seed;
            _actionTable = action_table;
            _endFixedStepIndex = end_fixed_step_index;
        }
    }
}
