using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using PlacesSearch = GoogleCloudClassLibrary.Places.PlacesSearch;

namespace GoogleClassLibraryTest {
    [TestFixture]
    public class FindPlacesUsingPointLBTest {
        private static PlacesSearch placesSearch;

        GC.GoogleCloudClassSetup VALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\APIKEY.txt");
        GC.GoogleCloudClassSetup INVALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\INVALID_APIKEY.txt");

        private String EMPTY_QUERY = "";
        private String BAD_QUERY = "BAD_QUERY";
        private String PIZZA_QUERY = "pizza";
        private String MUSEUM_QUERY = "museum";

        private Places.Location NULL_LOCATION = null;
        private Places.Location GENERIC_LOCATION = new Places.Location(0, 0);
        private Places.Location VALID_LOCATION = new Places.Location(40.757870, -73.983996);

        private static List<Places.Fields> NULL_LIST = null;
        private static List<Places.Fields> FIELDS_LIST = new List<Places.Fields>();

        public FindPlacesUsingPointLBTest() {
            placesSearch = new PlacesSearch(VALID_SETUP);
        }

        [Test]
        public void FindPlacesWPLB_MissingQuery() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(EMPTY_QUERY, GENERIC_LOCATION, NULL_LIST);
            candidates.Wait();

            Assert.IsNull(candidates.Result.Item1);
            Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.MISSING_QUERY);
        }

        [Test]
        public void FindPlacesWPLB_MissingLocation() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(PIZZA_QUERY, NULL_LOCATION, NULL_LIST);
            candidates.Wait();

            Assert.IsNull(candidates.Result.Item1);
            Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.MISSING_LOCATION);
        }

        [Test]
        public void FindPlacesWPLB_InvalidAPIKey() {
            placesSearch.UpdateKey(INVALID_SETUP);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(PIZZA_QUERY, GENERIC_LOCATION, NULL_LIST);
            candidates.Wait();

            /*
             * Revert the setup to a valid one before running any assert. This is important because doing so
             * ensures that a failed assert in this test will not cause fall through error in the following tests.
             */
            placesSearch.UpdateKey(VALID_SETUP);

            Assert.IsNull(candidates.Result.Item1);
            Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.INVALID_API_KEY);
        }

        [Test]
        public void FindPlacesWPLB_InvalidQuery() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(BAD_QUERY, GENERIC_LOCATION, NULL_LIST);
            candidates.Wait();

            Assert.IsNull(candidates.Result.Item1);
            Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.ZERO_RESULTS);
        }

        [Test]
        public void FindPlacesWPLB_ValidQuery() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(PIZZA_QUERY, VALID_LOCATION, NULL_LIST);
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

        [Test]
        public void FindPlacesWPLB_ValidQueryWithName() {
            FIELDS_LIST.Add(Places.Fields.NAME);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(PIZZA_QUERY, VALID_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.AreEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            Assert.IsNull(candidate.Place_id);

            Assert.IsNotNull(candidate.Name);
            Assert.IsNotEmpty(candidate.Name);
            Assert.IsTrue(candidate.Name.ToLower().Contains("pizza"));

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWPLB_ValidQueryWithPhotos() {
            FIELDS_LIST.Add(Places.Fields.PHOTOS);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(PIZZA_QUERY, VALID_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.AreEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            Assert.IsNull(candidate.Place_id);
            Assert.IsNull(candidate.Name);

            Assert.IsNotNull(candidate.Photos, null);
            Assert.GreaterOrEqual(candidate.Photos.Count, 1);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWPLB_ValidQueryWithFormattedAddress() {
            FIELDS_LIST.Add(Places.Fields.FORMATTED_ADDRESS);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(PIZZA_QUERY, VALID_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.AreEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            Assert.IsNull(candidate.Place_id);
            Assert.IsNull(candidate.Name);

            Assert.IsNotNull(candidate.Formatted_address);
            Assert.IsTrue(candidate.Formatted_address.Contains("New York"));

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWPLB_ValidQueryWithGeometry() {
            FIELDS_LIST.Add(Places.Fields.GEOMETRY);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(PIZZA_QUERY, VALID_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.AreEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            Assert.IsNull(candidate.Place_id);
            Assert.IsNull(candidate.Name);

            Assert.IsNotNull(candidate.Geometry);
            Assert.IsNotNull(candidate.Geometry.Viewport);
            Assert.IsNotNull(candidate.Geometry.Location);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWPLB_ValidQueryWithIcon() {
            FIELDS_LIST.Add(Places.Fields.ICON);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(PIZZA_QUERY, VALID_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.AreEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            Assert.IsNull(candidate.Place_id);
            Assert.IsNull(candidate.Name);

            Assert.IsNotNull(candidate.Icon);
            Assert.IsNotEmpty(candidate.Icon);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWPLB_ValidQueryWithId() {
            FIELDS_LIST.Add(Places.Fields.ID);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(PIZZA_QUERY, VALID_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.AreEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            Assert.IsNull(candidate.Place_id);
            Assert.IsNull(candidate.Name);

            Assert.IsNotNull(candidate.Id);
            Assert.IsNotEmpty(candidate.Id);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWPLB_ValidQueryWithPlusCode() {
            FIELDS_LIST.Add(Places.Fields.PLUS_CODE);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(PIZZA_QUERY, VALID_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.AreEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            Assert.IsNull(candidate.Place_id);
            Assert.IsNull(candidate.Name);

            Assert.IsNotNull(candidate.PlusCode);
            Assert.IsNotNull(candidate.PlusCode.CompoundCode);
            Assert.IsNotEmpty(candidate.PlusCode.CompoundCode);
            Assert.IsNotNull(candidate.PlusCode.GlobalCode);
            Assert.IsNotEmpty(candidate.PlusCode.CompoundCode);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWPLB_ValidQueryWithTypes() {
            FIELDS_LIST.Add(Places.Fields.TYPES);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(PIZZA_QUERY, VALID_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.AreEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            Assert.IsNull(candidate.Place_id);
            Assert.IsNull(candidate.Name);

            Assert.IsNotNull(candidate.Types);
            Assert.GreaterOrEqual(candidate.Types.Count, 1);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWPLB_ValidQueryWithOpeningHours() {
            FIELDS_LIST.Add(Places.Fields.OPENING_HOURS);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(PIZZA_QUERY, VALID_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.AreEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            Assert.IsNull(candidate.Place_id);
            Assert.IsNull(candidate.Name);

            Assert.IsNotNull(candidate.OpeningHours);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWPLB_ValidQueryWithRating() {
            FIELDS_LIST.Add(Places.Fields.RATING);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(PIZZA_QUERY, VALID_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.AreEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            Assert.IsNull(candidate.Place_id);
            Assert.IsNull(candidate.Name);

            Assert.GreaterOrEqual(candidate.Rating, 0);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWPLB_ValidQueryWithPriceLevel() {
            FIELDS_LIST.Add(Places.Fields.PRICE_LEVEL);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(PIZZA_QUERY, VALID_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.AreEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            Assert.IsNull(candidate.Place_id);
            Assert.IsNull(candidate.Name);

            Assert.GreaterOrEqual(candidate.PriceLevel, 0);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWPLB_ValidQueryWithAllFields() {
            FIELDS_LIST.Add(Places.Fields.FORMATTED_ADDRESS);
            FIELDS_LIST.Add(Places.Fields.GEOMETRY);
            FIELDS_LIST.Add(Places.Fields.ICON);
            FIELDS_LIST.Add(Places.Fields.ID);
            FIELDS_LIST.Add(Places.Fields.NAME);
            FIELDS_LIST.Add(Places.Fields.OPENING_HOURS);
            FIELDS_LIST.Add(Places.Fields.PHOTOS);
            FIELDS_LIST.Add(Places.Fields.PLACE_ID);
            FIELDS_LIST.Add(Places.Fields.PLUS_CODE);
            FIELDS_LIST.Add(Places.Fields.PRICE_LEVEL);
            FIELDS_LIST.Add(Places.Fields.RATING);
            FIELDS_LIST.Add(Places.Fields.TYPES);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlaceWithPointLocationBias(MUSEUM_QUERY, VALID_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.AreEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected

            // Verifying Place_id
            Assert.IsNotNull(candidate.Place_id);
            Assert.IsNotEmpty(candidate.Place_id);

            // Verifying Name
            Assert.IsNotNull(candidate.Name);
            Assert.IsNotEmpty(candidate.Name);
            Assert.IsTrue(candidate.Name.ToLower().Contains(MUSEUM_QUERY.ToLower()));

            // Verifying Photos
            Assert.IsNotNull(candidate.Photos);
            Assert.GreaterOrEqual(candidate.Photos.Count, 1);

            // Verifying Formatted_address
            Assert.IsNotNull(candidate.Formatted_address);
            Assert.IsTrue(candidate.Formatted_address.Contains("New York"));

            //Verifying Geometry
            Assert.IsNotNull(candidate.Geometry);
            Assert.IsNotNull(candidate.Geometry.Viewport);
            Assert.IsNotNull(candidate.Geometry.Location);

            // Verifying Icon
            Assert.IsNotNull(candidate.Icon);
            Assert.IsNotEmpty(candidate.Icon);

            // Verifying Id
            Assert.IsNotNull(candidate.Id);
            Assert.IsNotEmpty(candidate.Id);

            // Verifying PlusCode
            Assert.IsNotNull(candidate.PlusCode);
            Assert.IsNotNull(candidate.PlusCode.CompoundCode);
            Assert.IsNotEmpty(candidate.PlusCode.CompoundCode);
            Assert.IsNotNull(candidate.PlusCode.GlobalCode);
            Assert.IsNotEmpty(candidate.PlusCode.CompoundCode);

            // Verifying Types
            Assert.IsNotNull(candidate.Types);
            Assert.GreaterOrEqual(candidate.Types.Count, 1);

            // Verifying Rating
            Assert.GreaterOrEqual(candidate.Rating, 0);

            // Verifying OpeningHours
            Assert.IsNotNull(candidate.OpeningHours);

            // Verifying PriceLevel
            Assert.GreaterOrEqual(candidate.PriceLevel, 0);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }
    }
}
