using GameLibrary.Core;
using GameLibrary.Core.Components;
using GameLibrary.Core.Events;

namespace GameLibrary.Data
{
    public class WorldBuilder
    {
        private readonly List<LocationData> _locationDataTable;

        public WorldBuilder(List<LocationData> location_data_table)
        {
            _locationDataTable = location_data_table;
        }

        public LocationComponent Build(EventManager event_manager)
        {
            LocationComponent location = null;
            Dictionary<string, GameObject> game_object_table = new Dictionary<string, GameObject>();
            Dictionary<string, LocationComponent> location_component_table = new Dictionary<string, LocationComponent>();

            for (int location_index = 0; location_index < _locationDataTable.Count; location_index++)
            {
                LocationData location_data = _locationDataTable[location_index];
                GameObject location_game_object = new GameObject(location_data._id);
                LocationComponent location_component = new LocationComponent(location_data._name, location_game_object);

                location_game_object.AddComponent(location_component);

                location_component_table.Add(location_data._id, location_component);
                game_object_table.Add(location_data._id, location_game_object);

                event_manager.TriggerEvent(new RegisterGameObjectGameEvent(location_game_object));
                location_game_object.SetIsActive(true);
            }

            for (int location_index = 0; location_index < _locationDataTable.Count; location_index++)
            {
                LocationData location_data = _locationDataTable[location_index];
                LocationComponent location_component = location_component_table[location_data._id];

                if (!string.IsNullOrEmpty(location_data._parent))
                {
                    LocationComponent parent_location = location_component_table[location_data._parent];

                    parent_location.AddLocation(location_component, location_data._timeToTravel);
                }
                else
                {
                    location = location_component;
                }
            }

            return location;
        }
    }
}
