namespace GameLibrary.Core.Components
{
    public class LocationComponent : Component
    {
        private string _locationName;
        private LocationComponent? _parentLocation;

        private readonly List<Connection> _destinationWayTable = new List<Connection>();

        public LocationComponent(string locationName, GameObject game_object) : base(game_object)
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
            location_component.SetParentLocation(this);
        }

        public LocationComponent GetParentLocation()
        {
            return _parentLocation;
        }

        public void SetParentLocation(LocationComponent parent_location)
        {
            _parentLocation = parent_location;

        }


        public LocationComponent GetChildLocation()
        {
            throw new NotImplementedException();
        }
    }
}