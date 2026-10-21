using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Components
{
    public class HeroComponent : Component
    {
        private string _name;
        private string _id;

        public HeroComponent(GameObject game_object, string name, string id) : base (game_object)
        {
            _name = name;
            _id = id;
        }

        public string GetName()
        {
            return _name;
        }
    }
}
