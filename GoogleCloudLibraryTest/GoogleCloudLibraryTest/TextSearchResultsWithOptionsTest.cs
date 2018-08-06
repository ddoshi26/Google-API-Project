using System;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using ResponseStatus = GoogleCloudClassLibrary.ResponseStatus;
using PlacesSearch = GoogleCloudClassLibrary.Places.PlacesSearch;
using BasicFunctions = GoogleCloudClassLibrary.BasicFunctions;

namespace GoogleClassLibraryTest {

    [TestFixture]
    public class TextSearchResultsWithOptionsTest {
        private static PlacesSearch placesSearch;

        private GC.GoogleCloudClassSetup VALID_SETUP;
    
        private String PIZZA_QUERY = "pizza near purdue";

        private Places.Location LOCATION_PURDUE = new Places.Location(40.428049, -86.915368);
        private double RADIUS = 100.00;

        public TextSearchResultsWithOptionsTest() {
            String dir = Environment.GetEnvironmentVariable("GC_LIBRARY_TEST", EnvironmentVariableTarget.User);
            VALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\APIKEY.txt");

            placesSearch = new PlacesSearch(VALID_SETUP);
        }

        [Test]
        public void TextSearchResultsWithOptions_ValidQueryWithLocationAndRadius() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> textResults =
                placesSearch.GetTextSearchResultsWithOptions(PIZZA_QUERY, location: LOCATION_PURDUE, radius: RADIUS);
            textResults.Wait();

            Places.NearbySearchResultList resultsList = textResults.Result.Item1;
            ResponseStatus response = textResults.Result.Item2;

            Assert.IsNotNull(resultsList);
            Assert.GreaterOrEqual(resultsList.Results.Count, 1);

            Assert.AreSame(response, Places.PlacesStatus.OK);

            for (int i = 0; i < resultsList.Results.Count; i++) {
                Places.NearbySearchResult result = resultsList.Results[i];

                // Verifying Place_id
                Assert.IsNotNull(result.Place_id);
                Assert.IsNotEmpty(result.Place_id);

                // Verifying Name
                Assert.IsNotNull(result.Name);
                Assert.IsNotEmpty(result.Name);

                double dist = BasicFunctions.distanceBetweenLocations(result.Geometry.Location, LOCATION_PURDUE) / 1000.00;
                Assert.LessOrEqual(dist, RADIUS);
            }
        }

        [Test]
        public void TextSearchResultsWithOptions_ValidQueryWithOpenNow() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> textResults =
                placesSearch.GetTextSearchResultsWithOptions(PIZZA_QUERY, open_now: true);
            textResults.Wait();

            Places.NearbySearchResultList resultsList = textResults.Result.Item1;
            ResponseStatus response = textResults.Result.Item2;

            Assert.IsNotNull(resultsList);
            Assert.GreaterOrEqual(resultsList.Results.Count, 1);

            Assert.AreSame(response, Places.PlacesStatus.OK);

            for (int i = 0; i < resultsList.Results.Count; i++) {
                Places.NearbySearchResult result = resultsList.Results[i];

                // Verifying Place_id
                Assert.IsNotNull(result.Place_id);
                Assert.IsNotEmpty(result.Place_id);

                // Verifying Name
                Assert.IsNotNull(result.Name);
                Assert.IsNotEmpty(result.Name);

                Assert.IsNotNull(result.OpeningHours);
            }
        }

        [Test]
        public void TextSearchResultsWithOptions_ValidQueryWithMinMaxPrice() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> textResults =
                placesSearch.GetTextSearchResultsWithOptions(PIZZA_QUERY, min_price: 1, max_price: 2);
            textResults.Wait();

            Places.NearbySearchResultList resultsList = textResults.Result.Item1;
            ResponseStatus response = textResults.Result.Item2;

            Assert.IsNotNull(resultsList);
            Assert.GreaterOrEqual(resultsList.Results.Count, 1);

            Assert.AreSame(response, Places.PlacesStatus.OK);

            for (int i = 0; i < resultsList.Results.Count; i++) {
                Places.NearbySearchResult result = resultsList.Results[i];

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
        public void TextSearchResultsWithOptions_ValidQueryWithTypes() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> textResults =
                placesSearch.GetTextSearchResultsWithOptions(PIZZA_QUERY, type: Places.NearbySearchTypes.MEAL_TAKEAWAY);
            textResults.Wait();

            Places.NearbySearchResultList resultsList = textResults.Result.Item1;
            ResponseStatus response = textResults.Result.Item2;

            Assert.IsNotNull(resultsList);
            Assert.GreaterOrEqual(resultsList.Results.Count, 1);

            Assert.AreSame(response, Places.PlacesStatus.OK);

            for (int i = 0; i < resultsList.Results.Count; i++) {
                Places.NearbySearchResult result = resultsList.Results[i];

                // Verifying Place_id
                Assert.IsNotNull(result.Place_id);
                Assert.IsNotEmpty(result.Place_id);

                // Verifying Name
                Assert.IsNotNull(result.Name);
                Assert.IsNotEmpty(result.Name);

                // Verifying Types
                Assert.IsNotNull(result.Types);
                Assert.GreaterOrEqual(result.Types.Count, 1);
                Assert.IsTrue(result.Types.Contains(Places.NearbySearchTypes.MEAL_TAKEAWAY.ToString().ToLower()));
            }
        }

        [Test]
        public void TextSearchResultsWithOptions_ValidQueryWithAllOptions() {
            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> textResults =
                placesSearch.GetTextSearchResultsWithOptions(PIZZA_QUERY, location: LOCATION_PURDUE,
                radius: RADIUS, type: Places.NearbySearchTypes.MEAL_TAKEAWAY, open_now: true, min_price: 2, max_price: 3);
            textResults.Wait();

            Places.NearbySearchResultList resultsList = textResults.Result.Item1;
            ResponseStatus response = textResults.Result.Item2;

            Assert.IsNotNull(resultsList);
            Assert.GreaterOrEqual(resultsList.Results.Count, 1);

            Assert.AreSame(response, Places.PlacesStatus.OK);

            for (int i = 0; i < resultsList.Results.Count; i++) {
                Places.NearbySearchResult result = resultsList.Results[i];

                // Verifying Place_id
                Assert.IsNotNull(result.Place_id);
                Assert.IsNotEmpty(result.Place_id);

                // Verifying Name
                Assert.IsNotNull(result.Name);
                Assert.IsNotEmpty(result.Name);

                Assert.IsNotNull(result.OpeningHours);

                Assert.GreaterOrEqual(result.PriceLevel, 2);
                Assert.LessOrEqual(result.PriceLevel, 3);

                double dist = BasicFunctions.distanceBetweenLocations(result.Geometry.Location, LOCATION_PURDUE) / 1000.00;
                Assert.LessOrEqual(dist, RADIUS);

                // Verifying Types
                Assert.IsNotNull(result.Types);
                Assert.GreaterOrEqual(result.Types.Count, 1);
                Assert.IsTrue(result.Types.Contains(Places.NearbySearchTypes.MEAL_TAKEAWAY.ToString().ToLower()));
            }
        }
    }
}
