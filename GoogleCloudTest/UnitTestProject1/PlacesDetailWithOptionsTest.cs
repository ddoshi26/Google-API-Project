using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using PlacesDetail = GoogleCloudClassLibrary.Places.PlacesDetail;

namespace GoogleClassLibraryTest {

    [TestFixture]
    public class PlacesDetailWithOptionsTest {
        private static PlacesDetail placesDetail;

        GC.GoogleCloudClassSetup VALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\APIKEY.txt");
        GC.GoogleCloudClassSetup INVALID_SETUP = new GC.GoogleCloudClassSetup("C:\\Users\\Dhairya\\Desktop\\INVALID_APIKEY.txt");

        private static String MISSING_PLACE_ID = "";
        private static String INVALID_PLACE_ID = "INVALID PLACE ID";
        private static String PLACE_ID_WL = "ChIJleKUHU39EogRYHZBk7mGkTA";
        private static String PLACE_ID_NYC = "ChIJb8Jg9pZYwokR-qHGtvSkLzs";

        private static List<Places.PlacesDetailFields> NULL_LIST = null;
        private static List<Places.PlacesDetailFields> FIELDS_LIST = new List<Places.PlacesDetailFields>();

        public PlacesDetailWithOptionsTest() {
            placesDetail = new PlacesDetail(VALID_SETUP);
        }

        [Test]
        public void PlacesDetailWithOptions_MissingPlaceId() {
            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                placesDetail.GetPlaceDetailsWithOptions(MISSING_PLACE_ID);
            details.Wait();

            Assert.IsNull(details.Result.Item1);
            Assert.AreEqual(details.Result.Item2, Places.PlacesStatus.MISSING_PLACE_ID);
        }

        [Test]
        public void PlacesDetailWithOptions_InvalidAPIKey() {

            // Temporarily modify the class to have an invalid API Key
            placesDetail.UpdateKey(INVALID_SETUP);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_WL);
            details.Wait();

            /*
             * Revert the setup to a valid one before running any assert. This is important because doing so
             * ensures that a failed assert in this test will not cause fall through error in the following tests.
             */
            placesDetail.UpdateKey(VALID_SETUP);

            // Verifying that the function returned the expected error
            Assert.IsNull(details.Result.Item1);
            Assert.AreEqual(details.Result.Item2, Places.PlacesStatus.INVALID_API_KEY);
        }

        [Test]
        public void PlacesDetailWithOptions_InvalidPlaceId() {
            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(INVALID_PLACE_ID);
            details.Wait();

            Assert.IsNull(details.Result.Item1);
            Assert.AreEqual(details.Result.Item2, Places.PlacesStatus.INVALID_REQUEST);
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequest() {
            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_WL);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            // Verify that the place result result is as expected
            Assert.IsNotNull(detailResult.AddressComponents);
            Assert.GreaterOrEqual(detailResult.AddressComponents.Count, 1);

            Assert.IsNotNull(detailResult.AdrAddress);
            Assert.IsNotEmpty(detailResult.AdrAddress);

            Assert.IsNotNull(detailResult.FormattedAddress);
            Assert.IsNotEmpty(detailResult.FormattedAddress);

            Assert.IsNotNull(detailResult.Geometry);

            Assert.IsNotNull(detailResult.Icon);
            Assert.IsNotEmpty(detailResult.Icon);

            Assert.IsNotNull(detailResult.Name);
            Assert.IsNotEmpty(detailResult.Name);

            Assert.IsNotNull(detailResult.PlaceId);
            Assert.IsNotEmpty(detailResult.PlaceId);

            Assert.IsNotNull(detailResult.PlusCode);

            Assert.IsNotNull(detailResult.Reference);
            Assert.IsNotEmpty(detailResult.Reference);

            Assert.IsNotNull(detailResult.Types);
            Assert.GreaterOrEqual(detailResult.Types.Count, 1);

            Assert.IsNotNull(detailResult.Url);
            Assert.IsNotEmpty(detailResult.Url);

            Assert.IsNotNull(detailResult.Vicinity);
            Assert.IsNotEmpty(detailResult.Vicinity);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithFormattedAddress() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.FORMATTED_ADDRESS);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_WL, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.FormattedAddress);
            Assert.IsNotEmpty(detailResult.FormattedAddress);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithGeometry() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.GEOMETRY);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_WL, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.Geometry);
            Assert.IsNotNull(detailResult.Geometry.Location);
            Assert.IsNotNull(detailResult.Geometry.Viewport);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithIcon() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.ICON);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_WL, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.Icon);
            Assert.IsNotEmpty(detailResult.Icon);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithName() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.NAME);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_WL, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.Name);
            Assert.IsNotEmpty(detailResult.Name);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithId() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.ID);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_WL, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.Id);
            Assert.IsNotEmpty(detailResult.Id);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithOpeningHours() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.OPENING_HOURS);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_NYC, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.OpeningHours);

            Assert.IsNotNull(detailResult.OpeningHours.Periods);
            Assert.GreaterOrEqual(detailResult.OpeningHours.Periods.Count, 1);

            Assert.IsNotNull(detailResult.OpeningHours.WeekdayText);
            Assert.GreaterOrEqual(detailResult.OpeningHours.WeekdayText.Count, 1);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithPhotos() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.PHOTOS);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_NYC, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.Photos);
            Assert.GreaterOrEqual(detailResult.Photos.Count, 1);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithPlaceId() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.PLACE_ID);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_WL, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.PlaceId);
            Assert.IsNotEmpty(detailResult.PlaceId);
            Assert.AreEqual(detailResult.PlaceId, PLACE_ID_WL);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithPlusCode() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.PLUS_CODE);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_WL, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.PlusCode);

            Assert.IsNotNull(detailResult.PlusCode.CompoundCode);
            Assert.IsNotEmpty(detailResult.PlusCode.CompoundCode);

            Assert.IsNotNull(detailResult.PlusCode.GlobalCode);
            Assert.IsNotEmpty(detailResult.PlusCode.GlobalCode);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWitPriceLevel() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.PRICE_LEVEL);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_WL, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.GreaterOrEqual(detailResult.PriceLevel, 0);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithRating() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.RATING);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_WL, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.GreaterOrEqual(detailResult.Rating, 0);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithTypes() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.TYPES);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_WL, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.Types);
            Assert.GreaterOrEqual(detailResult.Types.Count, 1);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithAddressComponent() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.ADDRESS_COMPONENT);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_WL, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.AddressComponents);
            Assert.GreaterOrEqual(detailResult.AddressComponents.Count, 1);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithAdrAddress() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.ADR_ADDRESS);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_WL, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.AdrAddress);
            Assert.IsNotEmpty(detailResult.AdrAddress);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithFormattedPhoneNumber() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.FORMATTED_PHONE_NUMBER);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_NYC, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.FormattedPhoneNumber);
            Assert.IsNotEmpty(detailResult.FormattedPhoneNumber);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithInternationalPhoneNumber() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.INTERNATIONAL_PHONE_NUMBER);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_NYC, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.InternationalPhoneNumber);
            Assert.IsNotEmpty(detailResult.InternationalPhoneNumber);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithReviews() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.REVIEW);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_NYC, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.Reviews);
            Assert.GreaterOrEqual(detailResult.Reviews.Count, 1);

            Assert.IsNotNull(detailResult.Reviews[0].AuthorName);
            Assert.IsNotEmpty(detailResult.Reviews[0].AuthorName);

            Assert.IsNotNull(detailResult.Reviews[0].AuthorUrl);
            Assert.IsNotEmpty(detailResult.Reviews[0].AuthorUrl);

            Assert.IsNotNull(detailResult.Reviews[0].Language);
            Assert.IsNotEmpty(detailResult.Reviews[0].Language);

            Assert.IsNotNull(detailResult.Reviews[0].ProfilePhotoUrl);
            Assert.IsNotEmpty(detailResult.Reviews[0].ProfilePhotoUrl);

            Assert.GreaterOrEqual(detailResult.Reviews[0].Rating, 1);

            Assert.IsNotNull(detailResult.Reviews[0].RelativeTimeDescription);
            Assert.IsNotEmpty(detailResult.Reviews[0].RelativeTimeDescription);

            Assert.IsNotNull(detailResult.Reviews[0].Text);
            Assert.IsNotEmpty(detailResult.Reviews[0].Text);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithScope() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.SCOPE);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_NYC, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.Scope);
            Assert.IsNotEmpty(detailResult.Scope);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithURL() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.URL);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_NYC, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.Url);
            Assert.IsNotEmpty(detailResult.Url);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithUTCOffset() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.UTC_OFFSET);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_NYC, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.UtcOffset);
            Assert.GreaterOrEqual(detailResult.UtcOffset, -12 * 60);
            Assert.LessOrEqual(detailResult.UtcOffset, 12 * 60);
            
            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithVicinity() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.VICINITY);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_NYC, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.Vicinity);
            Assert.IsNotEmpty(detailResult.Vicinity);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithWebsite() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.WEBSITE);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_NYC, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that the result list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            Assert.IsNotNull(detailResult.Website);
            Assert.IsNotEmpty(detailResult.Website);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }

        [Test]
        public void PlacesDetailWithOptions_ValidRequestWithAllOptions() {
            FIELDS_LIST.Add(Places.PlacesDetailFields.ADDRESS_COMPONENT);
            FIELDS_LIST.Add(Places.PlacesDetailFields.ADR_ADDRESS);
            FIELDS_LIST.Add(Places.PlacesDetailFields.FORMATTED_ADDRESS);
            FIELDS_LIST.Add(Places.PlacesDetailFields.FORMATTED_PHONE_NUMBER);
            FIELDS_LIST.Add(Places.PlacesDetailFields.GEOMETRY);
            FIELDS_LIST.Add(Places.PlacesDetailFields.ICON);
            FIELDS_LIST.Add(Places.PlacesDetailFields.ID);
            FIELDS_LIST.Add(Places.PlacesDetailFields.INTERNATIONAL_PHONE_NUMBER);
            FIELDS_LIST.Add(Places.PlacesDetailFields.NAME);
            FIELDS_LIST.Add(Places.PlacesDetailFields.OPENING_HOURS);
            FIELDS_LIST.Add(Places.PlacesDetailFields.PHOTOS);
            FIELDS_LIST.Add(Places.PlacesDetailFields.PLACE_ID);
            FIELDS_LIST.Add(Places.PlacesDetailFields.PLUS_CODE);
            FIELDS_LIST.Add(Places.PlacesDetailFields.PRICE_LEVEL);
            FIELDS_LIST.Add(Places.PlacesDetailFields.RATING);
            FIELDS_LIST.Add(Places.PlacesDetailFields.REVIEW);
            FIELDS_LIST.Add(Places.PlacesDetailFields.SCOPE);
            FIELDS_LIST.Add(Places.PlacesDetailFields.TYPES);
            FIELDS_LIST.Add(Places.PlacesDetailFields.URL);
            FIELDS_LIST.Add(Places.PlacesDetailFields.UTC_OFFSET);
            FIELDS_LIST.Add(Places.PlacesDetailFields.VICINITY);
            FIELDS_LIST.Add(Places.PlacesDetailFields.WEBSITE);

            Task<Tuple<Places.PlacesDetailResponse, Places.ResponseStatus>> details =
                 placesDetail.GetPlaceDetailsWithOptions(PLACE_ID_NYC, fields: FIELDS_LIST);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            Places.ResponseStatus status = details.Result.Item2;

            // Verify that theresult list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            // Verifying AddressComponents
            Assert.IsNotNull(detailResult.AddressComponents);
            Assert.GreaterOrEqual(detailResult.AddressComponents.Count, 1);

            // Verifying AdrAddress
            Assert.IsNotNull(detailResult.AdrAddress);
            Assert.IsNotEmpty(detailResult.AdrAddress);

            // Verifying FormattedAddress
            Assert.IsNotNull(detailResult.FormattedAddress);
            Assert.IsNotEmpty(detailResult.FormattedAddress);

            // Verifying FormattedPhoneNumber
            Assert.IsNotNull(detailResult.FormattedPhoneNumber);
            Assert.IsNotEmpty(detailResult.FormattedPhoneNumber);

            // Verifying Geometry
            Assert.IsNotNull(detailResult.Geometry);
            Assert.IsNotNull(detailResult.Geometry.Location);
            Assert.IsNotNull(detailResult.Geometry.Viewport);

            // Verifying Icon
            Assert.IsNotNull(detailResult.Icon);
            Assert.IsNotEmpty(detailResult.Icon);

            // Verifying InternationalPhoneNumber
            Assert.IsNotNull(detailResult.InternationalPhoneNumber);
            Assert.IsNotEmpty(detailResult.InternationalPhoneNumber);

            // Verifying Name
            Assert.IsNotNull(detailResult.Name);
            Assert.IsNotEmpty(detailResult.Name);

            // Verifying OpeningHours
            Assert.IsNotNull(detailResult.OpeningHours);

            Assert.IsNotNull(detailResult.OpeningHours.Periods);
            Assert.GreaterOrEqual(detailResult.OpeningHours.Periods.Count, 1);

            Assert.IsNotNull(detailResult.OpeningHours.WeekdayText);
            Assert.GreaterOrEqual(detailResult.OpeningHours.WeekdayText.Count, 1);

            // Verifying PlaceId
            Assert.IsNotNull(detailResult.PlaceId);
            Assert.IsNotEmpty(detailResult.PlaceId);
            Assert.AreEqual(detailResult.PlaceId, PLACE_ID_NYC);

            // Verifying PlusCode
            Assert.IsNotNull(detailResult.PlusCode);

            Assert.IsNotNull(detailResult.PlusCode.CompoundCode);
            Assert.IsNotEmpty(detailResult.PlusCode.CompoundCode);

            Assert.IsNotNull(detailResult.PlusCode.GlobalCode);
            Assert.IsNotEmpty(detailResult.PlusCode.GlobalCode);

            // Verifying Photos
            Assert.IsNotNull(detailResult.Photos);
            Assert.GreaterOrEqual(detailResult.Photos.Count, 1);

            // Verifying Reviews
            Assert.IsNotNull(detailResult.Reviews);
            Assert.GreaterOrEqual(detailResult.Reviews.Count, 1);

            Assert.IsNotNull(detailResult.Reviews[0].AuthorName);
            Assert.IsNotEmpty(detailResult.Reviews[0].AuthorName);

            Assert.IsNotNull(detailResult.Reviews[0].AuthorUrl);
            Assert.IsNotEmpty(detailResult.Reviews[0].AuthorUrl);

            Assert.IsNotNull(detailResult.Reviews[0].Language);
            Assert.IsNotEmpty(detailResult.Reviews[0].Language);

            Assert.IsNotNull(detailResult.Reviews[0].ProfilePhotoUrl);
            Assert.IsNotEmpty(detailResult.Reviews[0].ProfilePhotoUrl);

            Assert.GreaterOrEqual(detailResult.Reviews[0].Rating, 1);

            Assert.IsNotNull(detailResult.Reviews[0].RelativeTimeDescription);
            Assert.IsNotEmpty(detailResult.Reviews[0].RelativeTimeDescription);

            Assert.IsNotNull(detailResult.Reviews[0].Text);
            Assert.IsNotEmpty(detailResult.Reviews[0].Text);

            // Verifying Scope
            Assert.IsNotNull(detailResult.Scope);
            Assert.IsNotEmpty(detailResult.Scope);

            // Verifying Types
            Assert.IsNotNull(detailResult.Types);
            Assert.GreaterOrEqual(detailResult.Types.Count, 1);

            // Verifying Url
            Assert.IsNotNull(detailResult.Url);
            Assert.IsNotEmpty(detailResult.Url);

            // Verifying UTC_Offset
            Assert.IsNotNull(detailResult.UtcOffset);
            Assert.GreaterOrEqual(detailResult.UtcOffset, -12 * 60);
            Assert.LessOrEqual(detailResult.UtcOffset, 12 * 60);

            // Verifying Vicinity
            Assert.IsNotNull(detailResult.Vicinity);
            Assert.IsNotEmpty(detailResult.Vicinity);

            // Verifying Website
            Assert.IsNotNull(detailResult.Website);
            Assert.IsNotEmpty(detailResult.Website);

            // Verify that the Status returned for the request is OK
            Assert.AreSame(status, Places.PlacesStatus.OK);

            FIELDS_LIST.Clear();
        }
    }
}
