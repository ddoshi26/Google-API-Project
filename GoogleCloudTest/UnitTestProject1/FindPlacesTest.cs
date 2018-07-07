using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NUnit.Framework;
using NU = NUnit.Framework;
using Places = GoogleCloudClassLibrary.Places;
using PlacesSearch = GoogleCloudClassLibrary.Places.PlacesSearch;

namespace GoogleClassLibraryTest {

    [TestFixture]
    public class FindPlacesTest {
        static PlacesSearch placesSearch = placesSearch = new PlacesSearch();
        private String MISSING_API_KEY = "";
        private String INVALID_API_KEY = "INVALID_API_KEY";
        private String VALID_API_KEY = System.IO.File.ReadAllText("C:\\Users\\Dhairya\\Desktop\\APIKEY.txt");

        private String PIZZA_QUERY = "pizza";
        private String BAD_QUERY = "BAD_QUERY";
        private String VALID_QUERY = "720 Northwestern Ave";

        // Tests for FindPlacesUsingTextQuery() method

        [Test]
        public void FindPlacesUsingTextQueryMissingAPIKey() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.Status>> candidates =
                placesSearch.FindPlacesUsingTextQuery(MISSING_API_KEY, PIZZA_QUERY);
            candidates.Wait();

            NU.Assert.AreEqual(candidates.Result.Item1, null);
            NU.Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.MISSING_API_KEY);
        }

        [Test]
        public void FindPlacesUsingTextQueryInvalidAPIKey() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.Status>> candidates =
                placesSearch.FindPlacesUsingTextQuery(INVALID_API_KEY, PIZZA_QUERY);
            candidates.Wait();

            NU.Assert.AreEqual(candidates.Result.Item1, null);
            NU.Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.INVALID_API_KEY);
        }

        [Test]
        public void FindPlacesUsingTextQueryZeroResults() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.Status>> candidates =
                placesSearch.FindPlacesUsingTextQuery(VALID_API_KEY, BAD_QUERY);
            candidates.Wait();

            NU.Assert.AreEqual(candidates.Result.Item1, null);
            NU.Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.ZERO_RESULTS);
        }

        [Test]
        public void FindPlacesUsingTextQueryValidRequest() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.Status>> candidates =
                    placesSearch.FindPlacesUsingTextQuery(VALID_API_KEY, VALID_QUERY);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.Status status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            NU.Assert.AreNotEqual(candidateList, null);
            NU.Assert.AreEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            NU.Assert.AreNotEqual(candidate.Place_id, null);
            NU.Assert.AreNotEqual(candidate.Place_id, "");
            NU.Assert.AreEqual(candidate.Name, null);

            // Verify that the Status returned for the request is OK
            NU.Assert.AreSame(status, Places.PlacesStatus.OK);
        }
    }
}
