using System;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using PlacesSearch = GoogleCloudClassLibrary.Places.PlacesSearch;
using BasicFunctions = GoogleCloudClassLibrary.BasicFunctions;

namespace GoogleClassLibraryTest {

    [TestFixture]
    public class TextSearchAdditionalResultsTest {
        private static PlacesSearch placesSearch;

        GC.GoogleCloudClassSetup VALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\APIKEY.txt");
        GC.GoogleCloudClassSetup INVALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\INVALID_APIKEY.txt");

        private String FOOD_QUERY = "mexican restuarants in manhattan, ny";

        private Places.Location LOCATION_NYC = new Places.Location(40.747207, -73.990691);
        private double RADIUS = 10000.00;

        private String EMPTY_PAGE_TOKEN = "";
        private String GENERIC_PAGE_TOKEN = "PAGE_TOKEN";

        public TextSearchAdditionalResultsTest() {
            placesSearch = new PlacesSearch(VALID_SETUP);
        }

        [Test]
        public void TextSearchAdditionalResults_MissingPageToken() {
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> searchResults =
                placesSearch.GetAdditionalTextSearchResults(EMPTY_PAGE_TOKEN);
            searchResults.Wait();

            Assert.IsNull(searchResults.Result.Item1);
            Assert.AreEqual(searchResults.Result.Item2, Places.PlacesStatus.MISSING_PAGE_TOKEN);
        }

        [Test]
        public void TextSearchAdditionalResults_InvalidAPIKey() {
            placesSearch.UpdateKey(INVALID_SETUP);

            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> searchResults =
                placesSearch.GetAdditionalTextSearchResults(GENERIC_PAGE_TOKEN);
            searchResults.Wait();

            // Reverting to the Valid API Key before we run any tests
            placesSearch.UpdateKey(VALID_SETUP);

            Assert.IsNull(searchResults.Result.Item1);
            Assert.AreEqual(searchResults.Result.Item2, Places.PlacesStatus.INVALID_API_KEY);
        }

        [Test]
        public void TextSearchAdditionalResults_InvalidPageToken() {
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> searchResults =
                placesSearch.GetAdditionalTextSearchResults(GENERIC_PAGE_TOKEN);
            searchResults.Wait();

            Assert.IsNull(searchResults.Result.Item1);
            Assert.AreEqual(searchResults.Result.Item2, Places.PlacesStatus.INVALID_REQUEST);
        }

        [Test]
        public void TextSearchAdditionalResults_ValidQuery() {
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> textResults =
                placesSearch.GetTextSearchResultsWithOptions(FOOD_QUERY, location: LOCATION_NYC,
                radius: RADIUS, type: Places.NearbySearchTypes.MEAL_DELIVERY, open_now: true, min_price: 0, max_price: 4);
            textResults.Wait();

            Places.NearbySearchResultList resultsList = textResults.Result.Item1;
            Places.ResponseStatus response = textResults.Result.Item2;

            Assert.IsNotNull(resultsList);
            Assert.GreaterOrEqual(resultsList.Results.Count, 1);

            Assert.AreSame(response, Places.PlacesStatus.OK);

            // Get the next page of results
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> searchResults2 =
                placesSearch.GetAdditionalTextSearchResults(resultsList.NextPageToken);
            searchResults2.Wait();

            Boolean photosSet = false;
            Boolean geometrySet = false;
            Boolean plusCodeSet = false;

            for (int i = 0; i < resultsList.Results.Count; i++) {
                Places.NearbySearchResult result = resultsList.Results[i];

                // Verifying Place_id
                Assert.IsNotNull(result.Place_id);
                Assert.IsNotEmpty(result.Place_id);

                // Verifying Name
                Assert.IsNotNull(result.Name);
                Assert.IsNotEmpty(result.Name);

                Assert.GreaterOrEqual(result.PriceLevel, 0);
                Assert.LessOrEqual(result.PriceLevel, 4);

                double dist = BasicFunctions.distanceBetweenLocations(result.Geometry.Location, LOCATION_NYC) / 1000.00;
                Assert.LessOrEqual(dist, RADIUS);

                // Verifying Types
                Assert.IsNotNull(result.Types);
                Assert.GreaterOrEqual(result.Types.Count, 1);
                Assert.IsTrue(result.Types.Contains(Places.NearbySearchTypes.MEAL_DELIVERY.ToString().ToLower()));

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

                // Verifying Rating
                Assert.GreaterOrEqual(result.Rating, 0);

                // Verifying References
                Assert.IsNotNull(result.Reference);
                Assert.IsNotEmpty(result.Reference);

                // Verifying Types
                Assert.IsNotNull(result.Types);
                Assert.GreaterOrEqual(result.Types.Count, 1);
                Assert.IsTrue(result.Types.Contains(Places.NearbySearchTypes.MEAL_DELIVERY.ToString().ToLower()));
            }

            // Verifying that at least one result has each of Photos, PlusCode, and Geometry is set
            Assert.IsTrue(photosSet && plusCodeSet && geometrySet);
        }
    }
}
