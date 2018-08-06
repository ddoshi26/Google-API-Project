using System;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using ResponseStatus = GoogleCloudClassLibrary.ResponseStatus;
using PlacesSearch = GoogleCloudClassLibrary.Places.PlacesSearch;

namespace GoogleClassLibraryTest {

    [TestFixture]
    public class NearbySearchResultsRBDWithOptionsTest {
        private static PlacesSearch placesSearch;

        private GC.GoogleCloudClassSetup VALID_SETUP;
        
        private Places.Location GENERIC_LOCATION = new Places.Location(0, 0);
        private Places.Location VALID_LOCATION = new Places.Location(40.757870, -73.983996);

        private String PIZZA_KEYWORD = "pizza";

        public NearbySearchResultsRBDWithOptionsTest() {
            String dir = Environment.GetEnvironmentVariable("GC_LIBRARY_TEST", EnvironmentVariableTarget.User);
            VALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\APIKEY.txt");

            placesSearch = new PlacesSearch(VALID_SETUP);
        }

        [Test]
        public void NearbySearchResultsRBDWthOptions_ValidQueryWithOpenNow() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByDistanceWithOptions(VALID_LOCATION,
                    keyword: PIZZA_KEYWORD, open_now: true);
            searchResults.Wait();

            Places.NearbySearchResultList resultList = searchResults.Result.Item1;
            ResponseStatus responseStatus = searchResults.Result.Item2;

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
        public void NearbySearchResultsRBDWthOptions_ValidQueryWithMinMaxPrice() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByDistanceWithOptions(VALID_LOCATION,
                    keyword: PIZZA_KEYWORD, min_price: 1, max_price: 2);
            searchResults.Wait();

            Places.NearbySearchResultList resultList = searchResults.Result.Item1;
            ResponseStatus responseStatus = searchResults.Result.Item2;

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

                Assert.GreaterOrEqual(result.PriceLevel, 1);
                Assert.LessOrEqual(result.PriceLevel, 2);
            }
        }

        [Test]
        public void NearbySearchResultsRBDWthOptions_ValidQueryWithKeywordAndAllOptions() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByDistanceWithOptions(VALID_LOCATION,
                    keyword: PIZZA_KEYWORD, open_now: true, min_price: 3, max_price: 4);
            searchResults.Wait();

            Places.NearbySearchResultList resultList = searchResults.Result.Item1;
            ResponseStatus responseStatus = searchResults.Result.Item2;

            Assert.AreSame(responseStatus, Places.PlacesStatus.OK);

            Assert.IsNotNull(resultList);
            Assert.GreaterOrEqual(resultList.Results.Count, 1);

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
