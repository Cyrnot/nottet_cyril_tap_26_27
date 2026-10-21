using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Components
{
    public class WalletComponent : Component
    {
        private int _money;

        public WalletComponent(GameObject game_object, int money) : base(game_object)
        {
            _money = money;
        }

        public void AddMoney(int money)
        {
            if (money < 0)
            {
                throw new ArgumentException("The money cannot be negative");
            }

            _money += money;
        }

        public int GetMoney()
        {
            return _money;
        }

        public bool UseMoney(int money)
        {
            bool is_money_use = false;

            if (money < 0)
            {
                throw new ArgumentException("The money cannot be negative");
            }

            if (money <= _money)
            {
                _money -= money;
                is_money_use = true;
            }

            return is_money_use;
        }
    }
}
