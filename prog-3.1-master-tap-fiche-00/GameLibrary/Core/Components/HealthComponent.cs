using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Components
{
    public class HealthComponent : Component
    {
        private int _health;
        private int _maxHealth;

        public HealthComponent(GameObject game_object, int health) : base(game_object)
        {
            _health = health;
            _maxHealth = health;
        }

        public int GetHealth()
        {
            return _health;
        }

        public void TakeDamage(int damage_count)
        {
            if (damage_count < 0)
            {
                throw new ArgumentException("Damage count cannot be negative");
            }
            _health = Math.Max(_health - damage_count, 0);
        }
    }
}
