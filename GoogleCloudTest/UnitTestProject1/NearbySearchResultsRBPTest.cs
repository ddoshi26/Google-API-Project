using System;
using System.Threading.Tasks;
using NUnit.Framework;
using NU = NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using PlacesSearch = GoogleCloudClassLibrary.Places.PlacesSearch;

namespace GoogleClassLibraryTest {
    [TestFixture]
    public class NearbySearchResultsRBPTest {
        private static PlacesSearch placesSearch;

        GC.GoogleCloudClassSetup VALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\APIKEY.txt");
        GC.GoogleCloudClassSetup INVALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\INVALID_APIKEY.txt");

        private Places.Location NULL_LOCATION = null;
        private Places.Location GENERIC_LOCATION = new Places.Location(0, 0);
        private Places.Location VALID_LOCATION = new Places.Location(40.757870, -73.983996);

        private static double VALID_RADIUS = 100.00;
        private static double NEGATIVE_RADIUS = -1.23;
        private static double TOO_LARGE_RADIUS = 500000.00;

        public NearbySearchResultsRBPTest() {
            placesSearch = new PlacesSearch(VALID_SETUP);
        }

        [Test]
        public void NearbySearchResultsRBP_MissingLocation() {
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByProminence(NULL_LOCATION, VALID_RADIUS);
            searchResults.Wait();

            Assert.IsNull(searchResults.Result.Item1);
            Assert.AreSame(searchResults.Result.Item2, Places.PlacesStatus.MISSING_LOCATION);
        }

        [Test]
        public void NearbySearchResultsRBP_InvalidAPIKey() {
            placesSearch.UpdateKey(INVALID_SETUP);

            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByProminence(GENERIC_LOCATION, VALID_RADIUS);
            searchResults.Wait();

            // Reverting to a valid API Key before we run any tests on the data returned.
            placesSearch.UpdateKey(VALID_SETUP);

            Assert.IsNull(searchResults.Result.Item1);
            Assert.AreSame(searchResults.Result.Item2, Places.PlacesStatus.INVALID_API_KEY);
        }

        [Test]
        public void NearbySearchResultsRBP_InvalidRadius() {
            // Test for negative radius
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByProminence(GENERIC_LOCATION, NEGATIVE_RADIUS);
            searchResults.Wait();

            Assert.IsNull(searchResults.Result.Item1);
            Assert.AreSame(searchResults.Result.Item2, Places.PlacesStatus.INVALID_RADIUS);

            // Test for radius which exceeds the 50,000 meters limit
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> searchResults2 =
                placesSearch.GetNearbySearchResultsRankByProminence(GENERIC_LOCATION, TOO_LARGE_RADIUS);
            searchResults2.Wait();

            Assert.IsNull(searchResults2.Result.Item1);
            Assert.AreSame(searchResults2.Result.Item2, Places.PlacesStatus.INVALID_RADIUS);
        }

        [Test]
        public void NearbySearchResultsRBP_ValidQuery() {
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByProminence(VALID_LOCATION, VALID_RADIUS);
            searchResults.Wait();

            Places.NearbySearchResultList resultList = searchResults.Result.Item1;
            Places.ResponseStatus responseStatus = searchResults.Result.Item2;

            Assert.AreSame(responseStatus, Places.PlacesStatus.OK);

            Assert.IsNotNull(resultList);
            Assert.GreaterOrEqual(resultList.Results.Count, 1);

            Boolean photosSet = false;
            Boolean geometrySet = false;
            Boolean openingHoursSet = false;

            for (int i = 0; i < resultList.Results.Count; i++) {
                Places.NearbySearchResult result = resultList.Results[i];

                // Verifying Place_id
                Assert.IsNotNull(result.Place_id);
                Assert.IsNotEmpty(result.Place_id);

                // Verifying Name
                Assert.IsNotNull(result.Name);
                Assert.IsNotEmpty(result.Name);

                // Verifying Photos
                if (result.Photos != null && result.Photos.Count >= 1) {
                    photosSet = true;
                }

                //Verifying Geometry
                if (result.Geometry != null && result.Geometry.Viewport != null && result.Geometry.Location != null) {
                    geometrySet = true;
                }

                // Verifying Icon
                Assert.IsNotNull(result.IconHTTP);
                Assert.IsNotEmpty(result.IconHTTP);

                // Verifying Id
                Assert.IsNotNull(result.Id);
                Assert.IsNotEmpty(result.Id);

                // Verifying Types
                Assert.IsNotNull(result.Types);
                Assert.GreaterOrEqual(result.Types.Count, 1);

                // Verifying OpeningHours
                if (result.OpeningHours != null) {
                    openingHoursSet = true;
                }
            }

            Assert.IsTrue(openingHoursSet && photosSet && geometrySet);
        }
    }
}
