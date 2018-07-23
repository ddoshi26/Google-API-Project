using System;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using PlacesSearch = GoogleCloudClassLibrary.Places.PlacesSearch;

namespace GoogleClassLibraryTest {

    [TestFixture]
    public class NearbySearchResultRBPWithOptionsTest {
        private static PlacesSearch placesSearch;

        GC.GoogleCloudClassSetup VALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\APIKEY.txt");
        GC.GoogleCloudClassSetup INVALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\INVALID_APIKEY.txt");

        private Places.Location GENERIC_LOCATION = new Places.Location(0, 0);
        private Places.Location VALID_LOCATION = new Places.Location(40.757870, -73.983996);
        private static double SMALL_RADIUS = 4000.00;
        private static double LARGE_RADIUS = 40000.00;

        private static String THEATER_KEYWORD = "theater";
        private static Places.NearbySearchTypes MUSEUM_TYPE = Places.NearbySearchTypes.MUSEUM;

        public NearbySearchResultRBPWithOptionsTest() {
            placesSearch = new PlacesSearch(VALID_SETUP);
        }

        [Test]
        public void NearbySearchResultsRBPWithOptions_ValidQueryWithOpenNow() {
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByProminenceWithOptions(VALID_LOCATION, SMALL_RADIUS, open_now: true);
            searchResults.Wait();

            Places.NearbySearchResultList resultList = searchResults.Result.Item1;
            Places.ResponseStatus responseStatus = searchResults.Result.Item2;

            Assert.AreSame(responseStatus, Places.PlacesStatus.OK);

            Assert.IsNotNull(resultList);
            Assert.GreaterOrEqual(resultList.Results.Count, 1);

            for (int i = 0; i < resultList.Results.Count; i++) {
                Places.NearbySearchResult result = resultList.Results[i];

                // Verifying Place_id
                Assert.IsNotNull(result.Place_id);
                Assert.IsNotEmpty(result.Place_id);

                // Verifying Name
                Assert.IsNotNull(result.Name);
                Assert.IsNotEmpty(result.Name);

                // Verifying OpeningHours
                Assert.IsNotNull(result.OpeningHours);
                Assert.IsTrue(result.OpeningHours.OpenNow);
            }
        }

        [Test]
        public void NearbySearchResultsRBPWithOptions_ValidQueryWithKeyword() {
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByProminenceWithOptions(VALID_LOCATION, SMALL_RADIUS, keyword: THEATER_KEYWORD);
            searchResults.Wait();

            Places.NearbySearchResultList resultList = searchResults.Result.Item1;
            Places.ResponseStatus responseStatus = searchResults.Result.Item2;

            Assert.AreSame(responseStatus, Places.PlacesStatus.OK);

            Assert.IsNotNull(resultList);
            Assert.GreaterOrEqual(resultList.Results.Count, 1);

            Boolean hasKeyword = false;

            for (int i = 0; i < resultList.Results.Count; i++) {
                Places.NearbySearchResult result = resultList.Results[i];

                // Verifying Place_id
                Assert.IsNotNull(result.Place_id);
                Assert.IsNotEmpty(result.Place_id);

                // Verifying Name
                Assert.IsNotNull(result.Name);
                Assert.IsNotEmpty(result.Name);
                if (result.Name.ToLower().Contains(THEATER_KEYWORD)) {
                    hasKeyword = true;
                }
            }

            Assert.IsTrue(hasKeyword);
        }

        [Test]
        public void NearbySearchResultsRBPWithOptions_ValidQueryWithMinMaxPrice() {
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByProminenceWithOptions(VALID_LOCATION, SMALL_RADIUS, min_price: 1, max_price: 3);
            searchResults.Wait();

            Places.NearbySearchResultList resultList = searchResults.Result.Item1;
            Places.ResponseStatus responseStatus = searchResults.Result.Item2;

            Assert.AreSame(responseStatus, Places.PlacesStatus.OK);

            Assert.IsNotNull(resultList);
            Assert.GreaterOrEqual(resultList.Results.Count, 1);

            for (int i = 0; i < resultList.Results.Count; i++) {
                Places.NearbySearchResult result = resultList.Results[i];

                // Verifying Place_id
                Assert.IsNotNull(result.Place_id);
                Assert.IsNotEmpty(result.Place_id);

                // Verifying Name
                Assert.IsNotNull(result.Name);
                Assert.IsNotEmpty(result.Name);

                // Verifying PriceLevel for each item
                Assert.GreaterOrEqual(result.PriceLevel, 1);
                Assert.LessOrEqual(result.PriceLevel, 3);
            }
        }

        [Test]
        public void NearbySearchResultsRBPWithOptions_ValidQueryWithType() {
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByProminenceWithOptions(VALID_LOCATION, SMALL_RADIUS, type: MUSEUM_TYPE);
            searchResults.Wait();

            Places.NearbySearchResultList resultList = searchResults.Result.Item1;
            Places.ResponseStatus responseStatus = searchResults.Result.Item2;

            Assert.AreSame(responseStatus, Places.PlacesStatus.OK);

            Assert.IsNotNull(resultList);
            Assert.GreaterOrEqual(resultList.Results.Count, 1);

            for (int i = 0; i < resultList.Results.Count; i++) {
                Places.NearbySearchResult result = resultList.Results[i];

                // Verifying Place_id
                Assert.IsNotNull(result.Place_id);
                Assert.IsNotEmpty(result.Place_id);

                // Verifying Name
                Assert.IsNotNull(result.Name);
                Assert.IsNotEmpty(result.Name);

                // Verifying Types
                Assert.IsNotNull(result.Types);
                Assert.GreaterOrEqual(result.Types.Count, 1);
                Assert.IsTrue(result.Types.Contains(MUSEUM_TYPE.ToString().ToLower()));
            }
        }

        [Test]
        public void NearbySearchResultsRBPWithOptions_ValidQueryWithAllOptions() {
            Task<Tuple<Places.NearbySearchResultList, Places.ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByProminenceWithOptions(VALID_LOCATION,
                LARGE_RADIUS, open_now: true, keyword: "mexican", min_price: 1, max_price: 3, type: Places.NearbySearchTypes.RESTAURANT);
            searchResults.Wait();

            Places.NearbySearchResultList resultList = searchResults.Result.Item1;
            Places.ResponseStatus responseStatus = searchResults.Result.Item2;

            Assert.AreSame(responseStatus, Places.PlacesStatus.OK);

            Assert.IsNotNull(resultList);
            Assert.GreaterOrEqual(resultList.Results.Count, 1);

            Boolean hasKeyword = false;

            for (int i = 0; i < resultList.Results.Count; i++) {
                Places.NearbySearchResult result = resultList.Results[i];

                // Verifying Place_id
                Assert.IsNotNull(result.Place_id);
                Assert.IsNotEmpty(result.Place_id);

                // Verifying Name
                Assert.IsNotNull(result.Name);
                Assert.IsNotEmpty(result.Name);

                if (result.Name.ToLower().Contains("mexican")) {
                    hasKeyword = true;
                }

                // Verifying OpeningHours
                Assert.IsNotNull(result.OpeningHours);
                Assert.IsTrue(result.OpeningHours.OpenNow);

                // Verifying PriceLevel for each item
                Assert.GreaterOrEqual(result.PriceLevel, 1);
                Assert.LessOrEqual(result.PriceLevel, 3);

                // Verifying Types
                Assert.IsNotNull(result.Types);
                Assert.GreaterOrEqual(result.Types.Count, 1);
                Assert.IsTrue(result.Types.Contains("restaurant") || result.Types.Contains("food"));
            }

            Assert.IsTrue(hasKeyword);
        }
    }
}
