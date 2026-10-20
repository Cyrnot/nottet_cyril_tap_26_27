using GameLibrary.Core;
using GameLibrary.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibraryTests.Components
{
    public class LocationComponentTests
    {
        private LocationComponent CreateLocation(string location_name)
        {
            GameObject location_game_object = new GameObject(location_name);
            LocationComponent location_component = new LocationComponent(location_name, null);

            location_game_object.AddComponent(location_component);

            return location_component;
        }

        [Test]
        public void GetDestinationAtIndex_ReturnChildLocation()
        {
            LocationComponent town_location = CreateLocation("Daisy Town");
            LocationComponent saloon_location = CreateLocation("Saloon");

            town_location.AddLocation(saloon_location, 5.0f);

            Assert.That(town_location.GetChildLocationAtIndex(0).GetDestinationLocation(), Is.EqualTo(saloon_location));
        }

        [Test]
        public void GetParentLocation_ReturnNullIfNoParent()
        {
            LocationComponent town_location = CreateLocation("Daisy Town");
            LocationComponent saloon_location = CreateLocation("Saloon");

            town_location.AddLocation(saloon_location, 5.0f);

            Assert.That(town_location.GetParentLocation(), Is.Null);
        }

        [Test]
        public void GetParentLocation_ReturnParentLocation()
        {
            LocationComponent town_location = CreateLocation("Daisy Town");
            LocationComponent saloon_location = CreateLocation("Saloon");

            town_location.AddLocation(saloon_location, 5.0f);

            Assert.That(saloon_location.GetParentLocation(), Is.EqualTo(town_location));
        }
    }
}
