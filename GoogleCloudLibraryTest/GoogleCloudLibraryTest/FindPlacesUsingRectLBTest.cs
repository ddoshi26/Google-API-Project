using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using ResponseStatus = GoogleCloudClassLibrary.ResponseStatus;
using PlacesSearch = GoogleCloudClassLibrary.Places.PlacesSearch;

namespace GoogleClassLibraryTest {
    [TestFixture]
    public class FindPlacesUsingRectLBTest {
        private static PlacesSearch placesSearch;

        private GC.GoogleCloudClassSetup VALID_SETUP;
        private GC.GoogleCloudClassSetup INVALID_SETUP;

        private String EMPTY_QUERY = "";
        private String BAD_QUERY = "BAD_QUERY";
        private String PIZZA_QUERY = "pizza";

        private Places.Location NULL_LOCATION = null;
        private Places.Location GENERIC_LOCATION = new Places.Location(0, 0);
        private Places.Location NE_LOCATION = new Places.Location(41.740976, -84.852384);
        private Places.Location SW_LOCATION = new Places.Location(37.827563, -88.008207);

        private static List<Places.Fields> NULL_LIST = null;
        private static List<Places.Fields> FIELDS_LIST = new List<Places.Fields>();

        public FindPlacesUsingRectLBTest() {
            String dir = Environment.GetEnvironmentVariable("GC_LIBRARY_TEST", EnvironmentVariableTarget.User);
            VALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\APIKEY.txt");
            INVALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\INVALID_APIKEY.txt");

            placesSearch = new PlacesSearch(VALID_SETUP);
        }

        [Test]
        public void FindPlacesWRLB_MissingQuery() {
            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
               placesSearch.FindPlacesWithRectLocationBias(EMPTY_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, NULL_LIST);
            candidates.Wait();

            Assert.IsNull(candidates.Result.Item1);
            Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.MISSING_QUERY);
        }

        [Test]
        public void FindPlacesWRLB_MissingLocation() {
            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
               placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, NULL_LOCATION, NE_LOCATION, NULL_LIST);
            candidates.Wait();

            Assert.IsNull(candidates.Result.Item1);
            Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.MISSING_LOCATION);
        }

        [Test]
        public void FindPlacesWRLB_InvalidAPIKey() {
            placesSearch.UpdateKey(INVALID_SETUP);

            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, NULL_LIST);
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
        public void FindPlacesWRLB_InvalidQuery() {
            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                placesSearch.FindPlacesWithRectLocationBias(BAD_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, NULL_LIST);
            candidates.Wait();

            Assert.IsNull(candidates.Result.Item1);
            Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.ZERO_RESULTS);
        }

        [Test]
        public void FindPlacesWRLB_ValidQuery() {
            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, NULL_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.GreaterOrEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            Assert.IsNotNull(candidate.Place_id);
            Assert.IsNotEmpty(candidate.Place_id);
            Assert.IsNull(candidate.Name);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);
        }

        [Test]
        public void FindPlacesWRLB_ValidQueryWithName() {
            FIELDS_LIST.Add(Places.Fields.NAME);

            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.GreaterOrEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            for (int i = 0; i < candidateList.Candidates.Count; i++) {
                candidate = candidateList.Candidates[i];

                Assert.IsNull(candidate.Place_id);

                Assert.IsNotNull(candidate.Name);
                Assert.IsNotEmpty(candidate.Name);
            }

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWRLB_ValidQueryWithPhotos() {
            FIELDS_LIST.Add(Places.Fields.PHOTOS);

            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                 placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.GreaterOrEqual(candidateList.Candidates.Count, 1);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            // Verify that the place candidate result is as expected
            for (int i = 0; i < candidateList.Candidates.Count; i++) {
                candidate = candidateList.Candidates[i];

                Assert.IsNull(candidate.Place_id);
                Assert.IsNull(candidate.Name);

                if (candidate.Photos != null) {
                    Assert.GreaterOrEqual(candidate.Photos.Count, 1);

                    FIELDS_LIST.Clear();

                    Assert.Pass("Found at least one item with the Photos field set");
                }
            }

            Assert.Fail("None of the Photo fields were set. Please verify the query");
        }

        [Test]
        public void FindPlacesWRLB_ValidQueryWithFormattedAddress() {
            FIELDS_LIST.Add(Places.Fields.FORMATTED_ADDRESS);

            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.GreaterOrEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            for (int i = 0; i < candidateList.Candidates.Count; i++) {
                candidate = candidateList.Candidates[i];

                Assert.IsNull(candidate.Place_id);
                Assert.IsNull(candidate.Name);

                Assert.IsNotNull(candidate.Formatted_address);
            }

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWRLB_ValidQueryWithGeometry() {
            FIELDS_LIST.Add(Places.Fields.GEOMETRY);

            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            ResponseStatus status = candidates.Result.Item2;

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.GreaterOrEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            for (int i = 0; i < candidateList.Candidates.Count; i++) {
                candidate = candidateList.Candidates[i];

                Assert.IsNull(candidate.Place_id);
                Assert.IsNull(candidate.Name);

                if (candidate.Geometry != null) {
                    Assert.IsNotNull(candidate.Geometry.Viewport);
                    Assert.IsNotNull(candidate.Geometry.Location);

                    FIELDS_LIST.Clear();

                    Assert.Pass("Found at least one item with the Geometry field set");
                }
            }

            Assert.Fail("None of the candidates have the Geometry fields were set. Please verify the query");
        }

        [Test]
        public void FindPlacesWRLB_ValidQueryWithIcon() {
            FIELDS_LIST.Add(Places.Fields.ICON);

            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.GreaterOrEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            for (int i = 0; i < candidateList.Candidates.Count; i++) {
                candidate = candidateList.Candidates[i];

                Assert.IsNull(candidate.Place_id);
                Assert.IsNull(candidate.Name);

                Assert.IsNotNull(candidate.Icon);
                Assert.IsNotEmpty(candidate.Icon);
            }

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWRLB_ValidQueryWithId() {
            FIELDS_LIST.Add(Places.Fields.ID);

            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.GreaterOrEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            for (int i = 0; i < candidateList.Candidates.Count; i++) {
                candidate = candidateList.Candidates[i];

                Assert.IsNull(candidate.Place_id);
                Assert.IsNull(candidate.Name);

                Assert.IsNotNull(candidate.Id);
                Assert.IsNotEmpty(candidate.Id);
            }

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWRLB_ValidQueryWithPlusCode() {
            FIELDS_LIST.Add(Places.Fields.PLUS_CODE);

            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            ResponseStatus status = candidates.Result.Item2;

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.GreaterOrEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            for (int i = 0; i < candidateList.Candidates.Count; i++) {
                candidate = candidateList.Candidates[i];

                Assert.IsNull(candidate.Place_id);
                Assert.IsNull(candidate.Name);

                if (candidate.PlusCode != null) {
                    Assert.IsNotNull(candidate.PlusCode.CompoundCode);
                    Assert.IsNotEmpty(candidate.PlusCode.CompoundCode);
                    Assert.IsNotNull(candidate.PlusCode.GlobalCode);
                    Assert.IsNotEmpty(candidate.PlusCode.CompoundCode);

                    FIELDS_LIST.Clear();

                    Assert.Pass("Found at least one candidate with the PlusCode field set");
                }
            }

            Assert.Fail("None of the candidates have the Geometry fields were set. Please verify the query");
        }

        [Test]
        public void FindPlacesWRLB_ValidQueryWithTypes() {
            FIELDS_LIST.Add(Places.Fields.TYPES);

            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.GreaterOrEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            for (int i = 0; i < candidateList.Candidates.Count; i++) {
                candidate = candidateList.Candidates[i];

                Assert.IsNull(candidate.Place_id);
                Assert.IsNull(candidate.Name);

                Assert.IsNotNull(candidate.Types);
                Assert.GreaterOrEqual(candidate.Types.Count, 1);
            }

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWRLB_ValidQueryWithOpeningHours() {
            FIELDS_LIST.Add(Places.Fields.OPENING_HOURS);

            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            ResponseStatus status = candidates.Result.Item2;

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.GreaterOrEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            for (int i = 0; i < candidateList.Candidates.Count; i++) {
                candidate = candidateList.Candidates[i];

                Assert.IsNull(candidate.Place_id);
                Assert.IsNull(candidate.Name);

                if (candidate.OpeningHours != null) {
                    Assert.Pass("Found at least one item with the OpeningHours field set");
                }
            }

            Assert.Fail("None of the candidates have the OpeningHours field were set. Please verify the query");
        }

        [Test]
        public void FindPlacesWRLB_ValidQueryWithRating() {
            FIELDS_LIST.Add(Places.Fields.RATING);

            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.GreaterOrEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            for (int i = 0; i < candidateList.Candidates.Count; i++) {
                candidate = candidateList.Candidates[i];

                Assert.IsNull(candidate.Place_id);
                Assert.IsNull(candidate.Name);

                Assert.GreaterOrEqual(candidate.Rating, 0);
            }

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWRLB_ValidQueryWithPriceLevel() {
            FIELDS_LIST.Add(Places.Fields.PRICE_LEVEL);

            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.GreaterOrEqual(candidateList.Candidates.Count, 1);

            // Verify that the place candidate result is as expected
            for (int i = 0; i < candidateList.Candidates.Count; i++) {
                candidate = candidateList.Candidates[i];

                Assert.IsNull(candidate.Place_id);
                Assert.IsNull(candidate.Name);

                Assert.GreaterOrEqual(candidate.PriceLevel, 0);
            }

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void FindPlacesWRLB_ValidQueryWithAllFields() {
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

            Task<Tuple<Places.FindPlacesCandidateList, ResponseStatus>> candidates =
                placesSearch.FindPlacesWithRectLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, SW_LOCATION, NE_LOCATION, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            ResponseStatus status = candidates.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(candidateList);
            Assert.GreaterOrEqual(candidateList.Candidates.Count, 1);

            Boolean photosSet = false;
            Boolean geometrySet = false;
            Boolean plusCodeSet = false;
            Boolean openingHoursSet = false;

            // Verify that the place candidate result is as expected

            for (int i = 0; i < candidateList.Candidates.Count; i++) {
                candidate = candidateList.Candidates[i];

                // Verifying Place_id
                Assert.IsNotNull(candidate.Place_id);
                Assert.IsNotEmpty(candidate.Place_id);

                // Verifying Name
                Assert.IsNotNull(candidate.Name);
                Assert.IsNotEmpty(candidate.Name);

                // Verifying Photos
                if (candidate.Photos != null && candidate.Photos.Count >= 1) {
                    photosSet = true;
                }

                // Verifying Formatted_address
                Assert.IsNotNull(candidate.Formatted_address);

                //Verifying Geometry
                if (candidate.Geometry != null && candidate.Geometry.Viewport != null && candidate.Geometry.Location != null) {
                    geometrySet = true;
                }

                // Verifying Icon
                Assert.IsNotNull(candidate.Icon);
                Assert.IsNotEmpty(candidate.Icon);

                // Verifying Id
                Assert.IsNotNull(candidate.Id);
                Assert.IsNotEmpty(candidate.Id);

                // Verifying PlusCode
                if (candidate.PlusCode != null && candidate.PlusCode.CompoundCode != null && candidate.PlusCode.GlobalCode != null) {
                    plusCodeSet = true;
                }

                // Verifying Types
                Assert.IsNotNull(candidate.Types);
                Assert.GreaterOrEqual(candidate.Types.Count, 1);

                // Verifying Rating
                Assert.GreaterOrEqual(candidate.Rating, 0);

                // Verifying OpeningHours
                if (candidate.OpeningHours != null) {
                    openingHoursSet = true;
                }

                // Verifying PriceLevel
                Assert.GreaterOrEqual(candidate.PriceLevel, 0);
            }

            Assert.IsTrue(openingHoursSet && plusCodeSet && photosSet && geometrySet);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }
    }
}
