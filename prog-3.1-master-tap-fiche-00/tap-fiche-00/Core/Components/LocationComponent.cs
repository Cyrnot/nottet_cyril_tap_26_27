using System;
using System.Collections.Generic;

namespace Game.Core.Components
{
    internal class LocationComponent : Component
    {
        private string _locationName;

        private readonly List<Connection> _destinationWayTable = new List<Connection>();

        public LocationComponent(string locationName)
        {
            _locationName = locationName;
        }

        public Connection GetChildLocationAtIndex(int location_index)
        {
            return _destinationWayTable[location_index];
        }

        public int GetChildLocationCount()
        {
            return _destinationWayTable.Count;
        }

        public string GetName()
        {
            return _locationName;
        }

        public void AddLocation(LocationComponent location_component, float travel_duration)
        {
            _destinationWayTable.Add(new Connection(location_component, travel_duration));
        }
    }
}