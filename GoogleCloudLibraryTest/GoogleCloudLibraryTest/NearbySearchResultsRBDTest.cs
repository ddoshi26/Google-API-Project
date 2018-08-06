using System;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using ResponseStatus = GoogleCloudClassLibrary.ResponseStatus;
using PlacesSearch = GoogleCloudClassLibrary.Places.PlacesSearch;

namespace GoogleClassLibraryTest {

    [TestFixture]
    public class NearbySearchResultsRBDTest {
        private static PlacesSearch placesSearch;

        private GC.GoogleCloudClassSetup VALID_SETUP;
        private GC.GoogleCloudClassSetup INVALID_SETUP;

        private Places.Location NULL_LOCATION = null;
        private Places.Location GENERIC_LOCATION = new Places.Location(0, 0);
        private Places.Location VALID_LOCATION = new Places.Location(40.757870, -73.983996);

        private String PIZZA_KEYWORD = "pizza";

        public NearbySearchResultsRBDTest() {
            String dir = Environment.GetEnvironmentVariable("GC_LIBRARY_TEST", EnvironmentVariableTarget.User);
            VALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\APIKEY.txt");
            INVALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\INVALID_APIKEY.txt");

            placesSearch = new PlacesSearch(VALID_SETUP);
        }

        [Test]
        public void NearbySearchResultsRBD_MissingLocation() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByDistance(NULL_LOCATION, PIZZA_KEYWORD);
            searchResults.Wait();

            Assert.IsNull(searchResults.Result.Item1);
            Assert.AreSame(searchResults.Result.Item2, Places.PlacesStatus.MISSING_LOCATION);
        }

        [Test]
        public void NearbySearchResultsRBD_MissingKeywordOrType() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByDistance(VALID_LOCATION);
            searchResults.Wait();

            Assert.IsNull(searchResults.Result.Item1);
            Assert.AreSame(searchResults.Result.Item2, Places.PlacesStatus.TOO_FEW_PARAMETERS);
        }

        [Test]
        public void NearbySearchResultsRBP_InvalidAPIKey() {
            placesSearch.UpdateKey(INVALID_SETUP);

            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByDistance(GENERIC_LOCATION, PIZZA_KEYWORD);
            searchResults.Wait();

            // Reverting to a valid API Key before we run any tests on the results
            placesSearch.UpdateKey(VALID_SETUP);

            Assert.IsNull(searchResults.Result.Item1);
            Assert.AreSame(searchResults.Result.Item2, Places.PlacesStatus.INVALID_API_KEY);
        }

        [Test]
        public void NearbySearchResultsRBD_ValidQueryWithKeyword() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByDistance(VALID_LOCATION, keyword: PIZZA_KEYWORD);
            searchResults.Wait();

            Places.NearbySearchResultList resultList = searchResults.Result.Item1;
            ResponseStatus responseStatus = searchResults.Result.Item2;

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

        [Test]
        public void NearbySearchResultsRBD_ValidQueryWithType() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByDistance(VALID_LOCATION, type: Places.NearbySearchTypes.LAWYER);
            searchResults.Wait();

            Places.NearbySearchResultList resultList = searchResults.Result.Item1;
            ResponseStatus responseStatus = searchResults.Result.Item2;

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
