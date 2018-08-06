using System;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using ResponseStatus = GoogleCloudClassLibrary.ResponseStatus;
using PlacesDetail = GoogleCloudClassLibrary.Places.PlacesDetail;

namespace GoogleClassLibraryTest {

    [TestFixture]
    public class PlacesDetailTest {
        private static PlacesDetail placesDetail;

        private GC.GoogleCloudClassSetup VALID_SETUP;
        private GC.GoogleCloudClassSetup INVALID_SETUP;

        private static String MISSING_PLACE_ID = "";
        private static String INVALID_PLACE_ID = "INVALID PLACE ID";
        private static String PLACE_ID_WL = "ChIJleKUHU39EogRYHZBk7mGkTA";

        public PlacesDetailTest() {
            String dir = Environment.GetEnvironmentVariable("GC_LIBRARY_TEST", EnvironmentVariableTarget.User);
            VALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\APIKEY.txt");
            INVALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\INVALID_APIKEY.txt");

            placesDetail = new PlacesDetail(VALID_SETUP);
        }

        [Test]
        public void PlacesDetail_MissingPlaceId() {
            Task<Tuple<Places.PlacesDetailResponse, ResponseStatus>> details =
                placesDetail.GetPlaceDetails(MISSING_PLACE_ID);
            details.Wait();

            Assert.IsNull(details.Result.Item1);
            Assert.AreEqual(details.Result.Item2, Places.PlacesStatus.MISSING_PLACE_ID);
        }

        [Test]
        public void PlacesDetail_InvalidAPIKey() {

            // Temporarily modify the class to have an invalid API Key
            placesDetail.UpdateKey(INVALID_SETUP);

            Task<Tuple< Places.PlacesDetailResponse, ResponseStatus >> details =
                 placesDetail.GetPlaceDetails(PLACE_ID_WL);
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
        public void PlacesDetail_InvalidPlaceId() {
            Task<Tuple<Places.PlacesDetailResponse, ResponseStatus>> details =
                 placesDetail.GetPlaceDetails(INVALID_PLACE_ID);
            details.Wait();

            Assert.IsNull(details.Result.Item1);
            Assert.AreEqual(details.Result.Item2, Places.PlacesStatus.INVALID_REQUEST);
        }

        [Test]
        public void PlacesDetail_ValidRequest() {
            Task<Tuple<Places.PlacesDetailResponse, ResponseStatus>> details =
                 placesDetail.GetPlaceDetails(PLACE_ID_WL);
            details.Wait();

            Places.PlacesDetailResponse response = details.Result.Item1;
            Places.PlacesDetailResult detailResult = response.Result;
            ResponseStatus status = details.Result.Item2;

            // Verify that the Candidate list is of expected size
            Assert.IsNotNull(response);
            Assert.IsNotNull(detailResult);

            // Verify that the place candidate result is as expected
            Assert.IsNotNull(detailResult.AddressComponents);
            Assert.GreaterOrEqual(detailResult.AddressComponents.Count, 1);

            Assert.IsNotNull(detailResult.AdrAddress);
            Assert.IsNotEmpty(detailResult.AdrAddress);

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
    }
}
