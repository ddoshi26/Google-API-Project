using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using Places = GoogleCloudClassLibrary.Places;
using ResponseStatus = GoogleCloudClassLibrary.ResponseStatus;
using PlacesPhotos = GoogleCloudClassLibrary.Places.PlacesPhotos;

namespace GoogleClassLibraryTest {

    [TestFixture]
    public class PlacesPhotosTest {
        private static PlacesPhotos placesPhotos;

        private GC.GoogleCloudClassSetup VALID_SETUP;
        private GC.GoogleCloudClassSetup INVALID_SETUP;

        private static readonly String MISSING_PHOTO_REF = "";
        private static readonly String VALID_PHOTO_REF = "CnRvAAAAwMpdHeWlXl-lH0vp7lez4znKPIWSWvgvZFISdKx45AwJVP1Qp37YOrH7sqHMJ8C-vBDC546decipPHchJhHZL94RcTUfPa1jWzo-rSHaTlbNtjh-N68RkcToUCuY9v2HNpo5mziqkir37WU8FJEqVBIQ4k938TI3e7bf8xq-uwDZcxoUbO_ZJzPxremiQurAYzCTwRhE_V0";
        private static readonly String INVALID_PHOTO_REF = "INVALID_PHOTO_REF";

        private static String INVALID_FILE_LOCATION;
        private static String VALID_FILE_LOCATION;
        private String EDITABLE_VALID_FILE_LOCATION;

        public PlacesPhotosTest() {
            String dir = Environment.GetEnvironmentVariable("GC_LIBRARY_TEST", EnvironmentVariableTarget.User);
            VALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\APIKEY.txt");
            INVALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\INVALID_APIKEY.txt");

            INVALID_FILE_LOCATION = dir + "INVALID_FILE_LOCATION\\photo.jpg";
            VALID_FILE_LOCATION = dir + "\\placesPhoto.jpg";
            EDITABLE_VALID_FILE_LOCATION = dir + "placesPhoto";

            placesPhotos = new PlacesPhotos(VALID_SETUP);
        }

        [Test]
        public void PlacesPhotos_MissingPhotoReference() {
            Task<Tuple<String, ResponseStatus>> response = placesPhotos.GetPlacesPhotos(MISSING_PHOTO_REF, VALID_FILE_LOCATION);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, Places.PlacesStatus.MISSING_PHOTO_REFERENCE);
        }

        [Test]
        public void PlacesPhotos_InvalidFileDestination() {
            Task<Tuple<String, ResponseStatus>> response = placesPhotos.GetPlacesPhotos(VALID_PHOTO_REF, INVALID_FILE_LOCATION, maxHeight: 400);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, Places.PlacesStatus.INVALID_FILE_LOCATION);
        }

        [Test]
        public void PlacesPhotos_MissingHeightWidth() {
            Task<Tuple<String, ResponseStatus>> response = placesPhotos.GetPlacesPhotos(VALID_PHOTO_REF, VALID_FILE_LOCATION);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, Places.PlacesStatus.MISSING_HEIGHT_WIDTH);
        }

        [Test]
        public void PlacesPhotos_InvalidPhotoReference() {
            Task<Tuple<String, ResponseStatus>> response = placesPhotos.GetPlacesPhotos(INVALID_PHOTO_REF, VALID_FILE_LOCATION, maxWidth: 500);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2.Code, 400);
            Assert.IsTrue(response.Result.Item2.Description.ToLower().Contains("bad request"));
        }

        [Test]
        public void PlacesPhotos_ValidQueryWithWidth() {
            Task<Tuple<String, ResponseStatus>> response = placesPhotos.GetPlacesPhotos(VALID_PHOTO_REF, VALID_FILE_LOCATION, maxWidth: 500);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item1, VALID_FILE_LOCATION);
            Assert.IsTrue(System.IO.File.Exists(response.Result.Item1));

            Assert.AreEqual(response.Result.Item2, Places.PlacesStatus.OK);
            
            System.IO.File.Delete(VALID_FILE_LOCATION);
        }

        [Test]
        public void PlacesPhotos_ValidQueryWithHeight() {
            Task<Tuple<String, ResponseStatus>> response = placesPhotos.GetPlacesPhotos(VALID_PHOTO_REF, VALID_FILE_LOCATION, maxHeight: 500);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item1, VALID_FILE_LOCATION);
            Assert.IsTrue(System.IO.File.Exists(response.Result.Item1));

            Assert.AreEqual(response.Result.Item2, Places.PlacesStatus.OK);

            System.IO.File.Delete(VALID_FILE_LOCATION);
        }

        [Test]
        public void PlacesPhotos_ValidQuery() {
            Places.PlacesSearch placesSearch = new Places.PlacesSearch(VALID_SETUP);
            Places.Location VALID_LOCATION = new Places.Location(40.757870, -73.983996);

            Task<Tuple<Places.NearbySearchResultList, ResponseStatus>> searchResults =
                placesSearch.GetNearbySearchResultsRankByProminenceWithOptions(VALID_LOCATION, 10000.00, type: Places.NearbySearchTypes.MUSEUM);
            searchResults.Wait();

            Places.NearbySearchResultList resultList = searchResults.Result.Item1;

            int photoCounter = 0;

            for (int i = 0; i < resultList.Results.Count; i++) {
                if (resultList.Results[i].Photos != null && resultList.Results[i].Photos.Count > 0) {
                    List<Places.Photo> photos = resultList.Results[i].Photos;

                    for (int j = 0; j < photos.Count; j++) {
                        Places.Photo photo = photos[j];

                        Task<Tuple<String, ResponseStatus>> response = placesPhotos.GetPlacesPhotos(photo.Photo_ref, EDITABLE_VALID_FILE_LOCATION + $"_{photoCounter}.jpg", maxHeight: 500);
                        response.Wait();

                        Assert.IsNotNull(response.Result.Item1);
                        Assert.AreEqual(response.Result.Item1, EDITABLE_VALID_FILE_LOCATION + $"_{photoCounter}.jpg");
                        Assert.IsTrue(System.IO.File.Exists(response.Result.Item1));

                        Assert.AreEqual(response.Result.Item2, Places.PlacesStatus.OK);

                        photoCounter++;
                    }
                }
            }

            for (int i = 0; i < photoCounter; i++) {
                System.IO.File.Delete(EDITABLE_VALID_FILE_LOCATION + $"_{i}.jpg");
            }
        }
    }
}
