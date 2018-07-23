using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using PlacesSearch = GoogleCloudClassLibrary.Places.PlacesSearch;

namespace GoogleClassLibraryTest {
    [TestFixture]
    public class FindPlacesUsingTextQueryTest {
        static PlacesSearch placesSearch;

        GC.GoogleCloudClassSetup VALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\APIKEY.txt");
        GC.GoogleCloudClassSetup INVALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\INVALID_APIKEY.txt");

        private String EMPTY_QUERY = "";
        private String BAD_QUERY = "BAD_QUERY";
        private String PIZZA_QUERY = "pizza";
        private String VALID_QUERY = "720 Northwestern Ave, West Lafayette";

        private Places.Location GENERIC_LOCATION = new Places.Location(0, 0);
        private Places.Location VALID_LOCATION = new Places.Location(40.757870, -73.983996);

        private Places.Location NE_LOCATION = new Places.Location(41.740976, -84.852384);
        private Places.Location SW_LOCATION = new Places.Location(37.827563, -88.008207);

        private static List<Places.Fields> FIELDS_LIST = new List<Places.Fields>();

        public FindPlacesUsingTextQueryTest() {
            placesSearch = new PlacesSearch(VALID_SETUP);
        }

        // Tests for FindPlacesUsingTextQuery() method

        [Test]
        public void FindPlacesUsingTextQuery_MissingQuery() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesUsingTextQuery(EMPTY_QUERY);
            candidates.Wait();

            Assert.IsNull(candidates.Result.Item1);
            Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.MISSING_QUERY);
        }

        [Test]
        public void FindPlacesUsingTextQuery_InvalidAPIKey() {

            // Temporarily modify the class to have an invalid API Key
            placesSearch.UpdateKey(INVALID_SETUP);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesUsingTextQuery(PIZZA_QUERY);
            candidates.Wait();

            /*
             * Revert the setup to a valid one before running any assert. This is important because doing so
             * ensures that a failed assert in this test will not cause fall through error in the following tests.
             */
            placesSearch.UpdateKey(VALID_SETUP);

            // Verifying that the function returned the expected error
            Assert.IsNull(candidates.Result.Item1);
            Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.INVALID_API_KEY);
        }

        [Test]
        public void FindPlacesUsingTextQuery_ZeroResults() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesUsingTextQuery(BAD_QUERY);
            candidates.Wait();

            Assert.IsNull(candidates.Result.Item1);
            Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.ZERO_RESULTS);
        }

        [Test]
        public void FindPlacesUsingTextQuery_ValidRequest() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                    placesSearch.FindPlacesUsingTextQuery(VALID_QUERY);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.AreEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            Assert.IsNotNull(candidate.Place_id);
            Assert.IsNotEmpty(candidate.Place_id);
            Assert.IsNull(candidate.Name);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);
        }
    }
}
