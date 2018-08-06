using System;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using ResponseStatus = GoogleCloudClassLibrary.ResponseStatus;
using PlacesSearch = GoogleCloudClassLibrary.Places.PlacesSearch;

namespace GoogleClassLibraryTest {

    [TestFixture]
    public class NearbySearchAdditionalResultsTest {
        private static PlacesSearch placesSearch;

        private GC.GoogleCloudClassSetup VALID_SETUP;
        private GC.GoogleCloudClassSetup INVALID_SETUP;

        private Places.Location GENERIC_LOCATION = new Places.Location(0, 0);
        private Places.Location VALID_LOCATION = new Places.Location(40.757870, -73.983996);

        private String PIZZA_KEYWORD = "pizza";

        private String EMPTY_PAGE_TOKEN = "";
        private String GENERIC_PAGE_TOKEN = "PAGE_TOKEN";

        public NearbySearchAdditionalResultsTest() {
            String dir = Environment.GetEnvironmentVariable("GC_LIBRARY_TEST", EnvironmentVariableTarget.User);
            VALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\APIKEY.txt");
            INVALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\INVALID_APIKEY.txt");

            placesSearch = new PlacesSearch(VALID_SETUP);
        }

        [Test]
        public void NearbySearchAdditionalResults_MissingPageToken() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> searchResults =
                placesSearch.GetAdditionalNearbySearchResults(EMPTY_PAGE_TOKEN);
            searchResults.Wait();

            Assert.IsNull(searchResults.Result.Item1);
            Assert.AreEqual(searchResults.Result.Item2, Places.PlacesStatus.MISSING_PAGE_TOKEN);
        }

        [Test]
        public void NearbySearchAdditionalResults_InvalidAPIKey() {
            placesSearch.UpdateKey(INVALID_SETUP);

            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> searchResults =
                placesSearch.GetAdditionalNearbySearchResults(GENERIC_PAGE_TOKEN);
            searchResults.Wait();

            /*
             * Revert the setup to a valid one before running any assert. This is important because doing so
             * ensures that a failed assert in this test will not cause fall through error in the following tests.
             */
            placesSearch.UpdateKey(VALID_SETUP);

            Assert.IsNull(searchResults.Result.Item1);
            Assert.AreEqual(searchResults.Result.Item2, Places.PlacesStatus.INVALID_API_KEY);
        }

        [Test]
        public void NearbySearchAdditionalResults_InvalidPageToken() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> searchResults =
                placesSearch.GetAdditionalNearbySearchResults(GENERIC_PAGE_TOKEN);
            searchResults.Wait();

            Assert.IsNull(searchResults.Result.Item1);
            Assert.AreEqual(searchResults.Result.Item2, Places.PlacesStatus.INVALID_REQUEST);
        }

        [Test]
        public void NearbySearchAdditionalResults_ValidQuery() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByDistanceWithOptions(VALID_LOCATION,
                    keyword: PIZZA_KEYWORD, open_now: true, min_price: 3, max_price: 4);
            searchResults.Wait();

            // Verifying the first page of results
            Places.NearbySearchResultList resultList = searchResults.Result.Item1;
            ResponseStatus responseStatus = searchResults.Result.Item2;

            Assert.AreSame(responseStatus, Places.PlacesStatus.OK);

            Assert.IsNotNull(resultList);
            Assert.GreaterOrEqual(resultList.Results.Count, 1);

            // Get the next page of results
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> searchResults2 =
                placesSearch.GetAdditionalNearbySearchResults(resultList.NextPageToken);
            searchResults2.Wait();

            Boolean photosSet = false;
            Boolean geometrySet = false;
            Boolean plusCodeSet = false;

            for (int i = 0; i < resultList.Results.Count; i++) {
                Places.NearbySearchResult result = resultList.Results[i];

                // Verifying Place_id
                Assert.IsNotNull(result.Place_id);
                Assert.IsNotEmpty(result.Place_id);

                // Verifying Name
                Assert.IsNotNull(result.Name);
                Assert.IsNotEmpty(result.Name);

                // Verifying Geometry
                if (result.Geometry != null && result.Geometry.Viewport != null && result.Geometry.Location != null) {
                    geometrySet = true;
                }

                // Verifying IconHTTP
                Assert.IsNotNull(result.IconHTTP);
                Assert.IsNotEmpty(result.IconHTTP);

                // Verifying ID
                Assert.IsNotNull(result.Id);
                Assert.IsNotEmpty(result.Id);

                // Verifying OpeningHours
                Assert.IsNotNull(result.OpeningHours);
                Assert.IsTrue(result.OpeningHours.OpenNow);

                // Verifying Photos
                if (result.Photos != null && result.Photos.Count >= 1) {
                    photosSet = true;
                }

                // Verifying PlusCode
                if (result.PlusCode != null && result.PlusCode.CompoundCode != null && result.PlusCode.GlobalCode != null) {
                    plusCodeSet = true;
                }

                // Verifying PriceLevel
                Assert.GreaterOrEqual(result.PriceLevel, 3);
                Assert.LessOrEqual(result.PriceLevel, 4);

                // Verifying Rating
                Assert.GreaterOrEqual(result.Rating, 0);

                // Verifying References
                Assert.IsNotNull(result.Reference);
                Assert.IsNotEmpty(result.Reference);

                // Verifying Scope
                Assert.IsNotNull(result.Scope);
                Assert.IsNotEmpty(result.Scope);

                // Verifying Types
                Assert.IsNotNull(result.Types);
                Assert.GreaterOrEqual(result.Types.Count, 1);

                // Verifying Vicinity
                Assert.IsNotNull(result.Vicinity);
                Assert.IsNotEmpty(result.Vicinity);
            }

            // Verifying that at least one result has each of Photos, PlusCode, and Geometry is set
            Assert.IsTrue(photosSet && plusCodeSet && geometrySet);
        }
    }
}
