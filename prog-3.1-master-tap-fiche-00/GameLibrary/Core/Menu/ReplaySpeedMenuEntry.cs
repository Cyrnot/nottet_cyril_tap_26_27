using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Menu
{
    public class ReplaySpeedMenuEntry : IMenuEntry
    {
        private const string TEXT = "Replay speed: ";

        private int _replaySpeedIndex = 2;
        private readonly int[] _replaySpeedTable = { 1, 5, 20, 100 };

        private GameManager _gameManager;

        public ReplaySpeedMenuEntry(GameManager game_manager)
        {
            _gameManager = game_manager;
        }

        public void Execute()
        {
            _replaySpeedIndex = (_replaySpeedIndex + 1) % _replaySpeedTable.Length;
            _gameManager.SetReplaySpeed(_replaySpeedTable[_replaySpeedIndex]);
        }

        public string GetText()
        {
            return $"{TEXT}{_replaySpeedTable[_replaySpeedIndex]}x";
        }
    }
}
