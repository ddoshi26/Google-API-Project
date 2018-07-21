using System;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using PlacesSearch = GoogleCloudClassLibrary.Places.PlacesSearch;

namespace GoogleClassLibraryTest {

    [TestFixture]
    public class TextSearchResultsTest {
        private static PlacesSearch placesSearch;

        GC.GoogleCloudClassSetup VALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\APIKEY.txt");
        GC.GoogleCloudClassSetup INVALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\INVALID_APIKEY.txt");

        private String EMPTY_QUERY = "";
        private String BAD_QUERY = "BAD_QUERY";
        private String PIZZA_QUERY = "pizza near 720 Northwestern Ave";
        private String VALID_QUERY = "museums in washington dc";

        public TextSearchResultsTest() {
            placesSearch = new PlacesSearch(VALID_SETUP);
        }

        [Test]
        public void TextSearchResults_MissingQuery() {
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> textResults =
                placesSearch.GetTextSearchResults(EMPTY_QUERY);
            textResults.Wait();

            Assert.IsNull(textResults.Result.Item1);
            Assert.AreSame(textResults.Result.Item2, Places.PlacesStatus.MISSING_QUERY);
        }

        [Test]
        public void TextSearchResults_InvalidAPIKey() {
            placesSearch.UpdateKey(INVALID_SETUP);

            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> textResults =
                placesSearch.GetTextSearchResults(PIZZA_QUERY);
            textResults.Wait();

            // Reverting to a valid API Key before we run any tests
            placesSearch.UpdateKey(VALID_SETUP);

            Assert.IsNull(textResults.Result.Item1);
            Assert.AreSame(textResults.Result.Item2, Places.PlacesStatus.INVALID_API_KEY);
        }

        [Test]
        public void TextSearchResults_BadQuery() {
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> textResults =
                placesSearch.GetTextSearchResults(BAD_QUERY);
            textResults.Wait();

            Assert.IsNull(textResults.Result.Item1);
            Assert.AreSame(textResults.Result.Item2, Places.PlacesStatus.ZERO_RESULTS);
        }

        [Test]
        public void TextSearchResults_ValidQuery() {
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> textResults =
                placesSearch.GetTextSearchResults(VALID_QUERY);
            textResults.Wait();

            Places.NearbySearchResultList resultsList = textResults.Result.Item1;
            Places.ResponseStatus response = textResults.Result.Item2;

            Assert.IsNotNull(resultsList);
            Assert.GreaterOrEqual(resultsList.Results.Count, 1);

            Assert.AreSame(response, Places.PlacesStatus.OK);

            Boolean photosSet = false;
            Boolean geometrySet = false;
            Boolean openingHoursSet = false;

            for (int i = 0; i < resultsList.Results.Count; i++) {
                Places.NearbySearchResult result = resultsList.Results[i];

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
