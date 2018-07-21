using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using PlacesSearch = GoogleCloudClassLibrary.Places.PlacesSearch;

namespace GoogleClassLibraryTest {
    [TestFixture]
    public class FindPlacesUsingCircularLBTest {
        private static PlacesSearch placesSearch;

        GC.GoogleCloudClassSetup VALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\APIKEY.txt");
        GC.GoogleCloudClassSetup INVALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\INVALID_APIKEY.txt");

        private String EMPTY_QUERY = "";
        private String BAD_QUERY = "BAD_QUERY";
        private String PIZZA_QUERY = "pizza";

        private Places.Location NULL_LOCATION = null;
        private Places.Location GENERIC_LOCATION = new Places.Location(0, 0);
        private Places.Location VALID_LOCATION = new Places.Location(40.757870, -73.983996);

        private static List<Places.Fields> NULL_LIST = null;
        private static List<Places.Fields> FIELDS_LIST = new List<Places.Fields>();

        private static double VALID_RADIUS = 1000.00;
        private static double BAD_RADIUS = -1.23;
        private static double TOO_LARGE_RADIUS = 500000.00;

        private static String VALID_PHONE_NO = "2127363100";

        public FindPlacesUsingCircularLBTest() {
            placesSearch = new PlacesSearch(VALID_SETUP);
        }

        [Test]
        public void FindPlacesWCLB_MissingQuery() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(EMPTY_QUERY, Places.InputType.TEXTQUERY, GENERIC_LOCATION, VALID_RADIUS, NULL_LIST);
            candidates.Wait();

            Assert.IsNull(candidates.Result.Item1);
            Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.MISSING_QUERY);
        }

        [Test]
        public void FindPlacesWCLB_MissingLocation() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, NULL_LOCATION, VALID_RADIUS, NULL_LIST);
            candidates.Wait();

            Assert.IsNull(candidates.Result.Item1);
            Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.MISSING_LOCATION);
        }

        [Test]
        public void FindPlacesWCLB_InvalidAPIKey() {
            placesSearch.UpdateKey(INVALID_SETUP);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(VALID_PHONE_NO, Places.InputType.PHONENUMBER, GENERIC_LOCATION, VALID_RADIUS, NULL_LIST);
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
        public void FindPlacesWCLB_InvalidQuery() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(BAD_QUERY, Places.InputType.TEXTQUERY, GENERIC_LOCATION, VALID_RADIUS, NULL_LIST);
            candidates.Wait();

            Assert.IsNull(candidates.Result.Item1);
            Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.ZERO_RESULTS);
        }

        [Test]
        public void FindPlacesWCLB_InvalidRadius() {
            // Test for a negative radius parameter
            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, GENERIC_LOCATION, BAD_RADIUS, NULL_LIST);
            candidates.Wait();

            Assert.IsNull(candidates.Result.Item1);
            Assert.AreEqual(candidates.Result.Item2, Places.PlacesStatus.INVALID_RADIUS);

            // Test for radius greater than 50,000 meters
            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates2 =
                placesSearch.FindPlacesWithCircularLocationBias(PIZZA_QUERY, Places.InputType.TEXTQUERY, GENERIC_LOCATION, TOO_LARGE_RADIUS, NULL_LIST);
            candidates2.Wait();

            Assert.IsNull(candidates2.Result.Item1);
            Assert.AreEqual(candidates2.Result.Item2, Places.PlacesStatus.INVALID_RADIUS);
        }

        [Test]
        public void FindPlacesWCLB_ValidQuery() {
            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(VALID_PHONE_NO,
                Places.InputType.PHONENUMBER, VALID_LOCATION, VALID_RADIUS, NULL_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

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
        public void FindPlacesWCLB_ValidQueryWithName() {
            FIELDS_LIST.Add(Places.Fields.NAME);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(VALID_PHONE_NO,
                Places.InputType.PHONENUMBER, VALID_LOCATION, VALID_RADIUS, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            Places.ResponseStatus status = candidates.Result.Item2;

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
        public void FindPlacesWCLB_ValidQueryWithPhotos() {
            FIELDS_LIST.Add(Places.Fields.PHOTOS);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                 placesSearch.FindPlacesWithCircularLocationBias(VALID_PHONE_NO,
                 Places.InputType.PHONENUMBER, VALID_LOCATION, VALID_RADIUS, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            Places.ResponseStatus status = candidates.Result.Item2;

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
        public void FindPlacesWCLB_ValidQueryWithFormattedAddress() {
            FIELDS_LIST.Add(Places.Fields.FORMATTED_ADDRESS);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(VALID_PHONE_NO,
                Places.InputType.PHONENUMBER, VALID_LOCATION, VALID_RADIUS, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            Places.ResponseStatus status = candidates.Result.Item2;

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
        public void FindPlacesWCLB_ValidQueryWithGeometry() {
            FIELDS_LIST.Add(Places.Fields.GEOMETRY);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(VALID_PHONE_NO,
                Places.InputType.PHONENUMBER, VALID_LOCATION, VALID_RADIUS, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            Places.ResponseStatus status = candidates.Result.Item2;

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
        public void FindPlacesWCLB_ValidQueryWithIcon() {
            FIELDS_LIST.Add(Places.Fields.ICON);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(VALID_PHONE_NO,
                Places.InputType.PHONENUMBER, VALID_LOCATION, VALID_RADIUS, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

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
        public void FindPlacesWCLB_ValidQueryWithId() {
            FIELDS_LIST.Add(Places.Fields.ID);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(VALID_PHONE_NO,
                Places.InputType.PHONENUMBER, VALID_LOCATION, VALID_RADIUS, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

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
        public void FindPlacesWCLB_ValidQueryWithPlusCode() {
            FIELDS_LIST.Add(Places.Fields.PLUS_CODE);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(VALID_PHONE_NO,
                Places.InputType.PHONENUMBER, VALID_LOCATION, VALID_RADIUS, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            Places.ResponseStatus status = candidates.Result.Item2;

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
        public void FindPlacesWCLB_ValidQueryWithTypes() {
            FIELDS_LIST.Add(Places.Fields.TYPES);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(VALID_PHONE_NO,
                Places.InputType.PHONENUMBER, VALID_LOCATION, VALID_RADIUS, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate = candidateList.Candidates[0];
            Places.ResponseStatus status = candidates.Result.Item2;

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
        public void FindPlacesWCLB_ValidQueryWithOpeningHours() {
            FIELDS_LIST.Add(Places.Fields.OPENING_HOURS);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(VALID_PHONE_NO,
                Places.InputType.PHONENUMBER, VALID_LOCATION, VALID_RADIUS, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            Places.ResponseStatus status = candidates.Result.Item2;

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
        public void FindPlacesWCLB_ValidQueryWithRating() {
            FIELDS_LIST.Add(Places.Fields.RATING);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(VALID_PHONE_NO,
                Places.InputType.PHONENUMBER, VALID_LOCATION, VALID_RADIUS, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            Places.ResponseStatus status = candidates.Result.Item2;

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
        public void FindPlacesWCLB_ValidQueryWithPriceLevel() {
            FIELDS_LIST.Add(Places.Fields.PRICE_LEVEL);

            Task<Tuple<Places.FindPlacesCandidateList, Places.ResponseStatus>> candidates =
                placesSearch.FindPlacesWithCircularLocationBias(VALID_PHONE_NO,
                Places.InputType.PHONENUMBER, VALID_LOCATION, VALID_RADIUS, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            Places.ResponseStatus status = candidates.Result.Item2;

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
        public void FindPlacesWCLB_ValidQueryWithAllFields() {
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
                placesSearch.FindPlacesWithCircularLocationBias(VALID_PHONE_NO,
                Places.InputType.PHONENUMBER, VALID_LOCATION, VALID_RADIUS, FIELDS_LIST);
            candidates.Wait();

            Places.FindPlacesCandidateList candidateList = candidates.Result.Item1;
            Places.FindPlaceCandidates candidate;
            Places.ResponseStatus status = candidates.Result.Item2;

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
