using GameLibrary.Core.Components;
using GameLibrary.Core;

namespace GameLibraryTests
{
    internal class ConnectionTests
    {
        [Test]
        public void GetDestination_ReturnsConstructionDestination()
        {
            LocationComponent destination = CreateLocation("Daisy Town");

            Connection connection = new Connection(destination, 3.0f);

            Assert.That(connection.GetDestinationLocation(), Is.EqualTo(destination));
            Assert.That(connection.GetTimeToReachDestination(), Is.EqualTo(3.0f).Within(0.001f));
        }

        private LocationComponent CreateLocation(string locationName)
        {
            GameObject game_object = new GameObject(locationName);
            LocationComponent location = new LocationComponent(locationName);
            game_object.AddComponent(location);
            return location;
        }

    }
}
