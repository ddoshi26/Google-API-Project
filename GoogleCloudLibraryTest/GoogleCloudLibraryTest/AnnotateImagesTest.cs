using System;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using GoogleCloudClassLibrary.ImageIntelligence;
using ResponseStatus = GoogleCloudClassLibrary.ResponseStatus;
using System.Collections.Generic;

namespace GoogleClassLibraryTest {

    [TestFixture]
    public class AnnotateImagesTest {
        private static ImageIntelligence imageIntelligence;

        private GC.GoogleCloudClassSetup VALID_SETUP;
        private GC.GoogleCloudClassSetup INVALID_SETUP;

        private static readonly String VALID_IMAGE_URL = "gs://gccl_dd_01/Image1";
        private static readonly String FACE_ANNOTATION_URL = "gs://gccl_dd_01/car.png";
        private static readonly String LABEL_ANNOTATION_URL = "gs://gccl_dd_01/faulkner.jpg";
        private static readonly String LANDMARK_ANNOTATION_URL_1 = "gs://gccl_dd_01/purdue_1.jpg";
        private static readonly String LANDMARK_ANNOTATION_URL_2 = "gs://gccl_dd_01/NYC_1.jpg";
        private static readonly String LOGO_DETECTION_URL = "gs://gccl_dd_01/logos.jpg";
        private static readonly String UNSAFE_IMAGE_URL = "gs://gccl_dd_01/logos.jpg";
        private static readonly String TEXT_DETECTION_URL_1 = "gs://gccl_dd_01/OCR_1.jpg";
        private static readonly String TEXT_DETECTION_URL_2 = "gs://gccl_dd_01/OCR_2.jpg";
        private static readonly String IMAGE_URL = "IMAGE_URL";

        private static readonly int MAX_RESULTS = 10;
        private static readonly String VALID_MODEL = "builtin/stable";

        private static List<ImageFeatures> imageFeaturesList = new List<ImageFeatures>();

        public AnnotateImagesTest() {
            String dir = Environment.GetEnvironmentVariable("GC_LIBRARY_TEST", EnvironmentVariableTarget.User);
            VALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\APIKEY.txt");
            INVALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\INVALID_APIKEY.txt");

            imageIntelligence = new ImageIntelligence(VALID_SETUP);
        }

        public AnnotateImageRequest GenerateImageRequest(String imageURL, 
            List<ImageFeatures> imageFeaturesList, ImageContext context = null) {
            ImageSource imageSource = new ImageSource(imageURL);
            Image image = new Image(source: imageSource);
            
            AnnotateImageRequest request = new AnnotateImageRequest(image, imageFeaturesList, null);

            return request;
        }

        [Test]
        public void AnnotateImagesTest_MissingRequestList() {
            Task<Tuple<AnnotateImageResponseList, ResponseStatus>> responses = imageIntelligence.AnnotateImages(null);
            responses.Wait();

            Assert.IsNull(responses.Result.Item1);
            Assert.AreSame(responses.Result.Item2, ImageAnnotationStatus.MISSING_REQUEST_LIST);
        }

        [Test]
        public void AnnotateImagesTest_InvalidAPIKey() {
            ImageFeatures feature = new ImageFeatures(ImageType.LABEL_DETECTION, MAX_RESULTS, VALID_MODEL);
            imageFeaturesList.Add(feature);

            AnnotateImageRequest imageRequest = GenerateImageRequest(VALID_IMAGE_URL, imageFeaturesList);
            AnnotateImageRequestList imageRequestList = new AnnotateImageRequestList(new List<AnnotateImageRequest>() { imageRequest });

            imageIntelligence.UpdateKey(INVALID_SETUP);

            Task<Tuple<AnnotateImageResponseList, ResponseStatus>> responses = imageIntelligence.AnnotateImages(imageRequestList);
            responses.Wait();

            imageIntelligence.UpdateKey(VALID_SETUP);

            Assert.IsNull(responses.Result.Item1);
            Assert.AreSame(responses.Result.Item2, ImageAnnotationStatus.INVALID_API_KEY);

            imageRequestList.Requests.Clear();
        }

        [Test]
        public void AnnotateImagesTest_SingleInvalidImageURL() {
            ImageFeatures feature = new ImageFeatures(ImageType.LABEL_DETECTION, MAX_RESULTS, VALID_MODEL);
            imageFeaturesList.Add(feature);

            AnnotateImageRequest imageRequest = GenerateImageRequest(IMAGE_URL, imageFeaturesList);
            AnnotateImageRequestList imageRequestList = new AnnotateImageRequestList(new List<AnnotateImageRequest>() { imageRequest });

            Task<Tuple<AnnotateImageResponseList, ResponseStatus>> responses = imageIntelligence.AnnotateImages(imageRequestList);
            responses.Wait();

            Assert.IsNotNull(responses.Result.Item1);
            Assert.IsNotNull(responses.Result.Item1.Responses[0].Error);
            Assert.IsNotNull(responses.Result.Item1.Responses[0].Error.Message);
            Assert.IsNotEmpty(responses.Result.Item1.Responses[0].Error.Message);

            imageRequestList.Requests.Clear();
        }

        [Test]
        public void AnnotateImagesTest_OneValidAndOneInvalidImageURL() {
            ImageFeatures feature = new ImageFeatures(ImageType.LANDMARK_DETECTION, MAX_RESULTS, VALID_MODEL);
            imageFeaturesList.Add(feature);

            AnnotateImageRequest imageRequest1 = GenerateImageRequest(IMAGE_URL, imageFeaturesList);
            AnnotateImageRequest imageRequest2 = GenerateImageRequest(LANDMARK_ANNOTATION_URL_1, imageFeaturesList);
            AnnotateImageRequestList imageRequestList = 
                new AnnotateImageRequestList(new List<AnnotateImageRequest>() { imageRequest1, imageRequest2 });

            Task<Tuple<AnnotateImageResponseList, ResponseStatus>> responses = imageIntelligence.AnnotateImages(imageRequestList);
            responses.Wait();

            Assert.IsNotNull(responses.Result.Item1);

            AnnotateImageResponseList responseList = responses.Result.Item1;
            Assert.AreEqual(responseList.Responses.Count, 2);

            Assert.IsNotNull(responseList.Responses[0].Error);
            Assert.IsNotNull(responseList.Responses[0].Error.Message);
            Assert.IsNotEmpty(responseList.Responses[0].Error.Message);

            
            Assert.IsNotNull(responseList.Responses[1].LandmarkAnnotations);
            Assert.GreaterOrEqual(responseList.Responses[1].LandmarkAnnotations.Count, 1);
            
            imageRequestList.Requests.Clear();
        }

        [Test]
        public void AnnotateImagesTest_ValidRequestWithFaceDetection() {
            ImageFeatures feature = new ImageFeatures(ImageType.FACE_DETECTION, MAX_RESULTS, VALID_MODEL);
            imageFeaturesList.Add(feature);

            AnnotateImageRequest imageRequest = GenerateImageRequest(FACE_ANNOTATION_URL, imageFeaturesList);
            AnnotateImageRequestList imageRequestList = new AnnotateImageRequestList(new List<AnnotateImageRequest>() { imageRequest });

            Task<Tuple<AnnotateImageResponseList, ResponseStatus>> response = imageIntelligence.AnnotateImages(imageRequestList);
            response.Wait();

            AnnotateImageResponseList responseList = response.Result.Item1;

            Assert.IsNotNull(responseList);
            Assert.AreSame(response.Result.Item2, ImageAnnotationStatus.OK);

            Assert.GreaterOrEqual(responseList.Responses.Count, 1);

            for (int i = 0; i < responseList.Responses.Count; i++) {
                Assert.IsNotNull(responseList.Responses[i].FaceAnnotations);
                Assert.GreaterOrEqual(responseList.Responses[i].FaceAnnotations.Count, 1);
            }

            imageRequestList.Requests.Clear();
        }

        [Test]
        public void AnnotateImagesTest_ValidRequestWithLabelDetection() {
            ImageFeatures feature = new ImageFeatures(ImageType.LABEL_DETECTION, MAX_RESULTS, VALID_MODEL);
            imageFeaturesList.Add(feature);

            AnnotateImageRequest imageRequest = GenerateImageRequest(LABEL_ANNOTATION_URL, imageFeaturesList);
            AnnotateImageRequestList imageRequestList = new AnnotateImageRequestList(new List<AnnotateImageRequest>() { imageRequest });

            Task<Tuple<AnnotateImageResponseList, ResponseStatus>> response = imageIntelligence.AnnotateImages(imageRequestList);
            response.Wait();

            AnnotateImageResponseList responseList = response.Result.Item1;

            Assert.IsNotNull(responseList);
            Assert.AreSame(response.Result.Item2, ImageAnnotationStatus.OK);

            Assert.GreaterOrEqual(responseList.Responses.Count, 1);

            for (int i = 0; i < responseList.Responses.Count; i++) {
                Assert.IsNotNull(responseList.Responses[i].LabelAnnotations);
                Assert.GreaterOrEqual(responseList.Responses[i].LabelAnnotations.Count, 1);
            }

            imageRequestList.Requests.Clear();
        }

        [Test]
        public void AnnotateImagesTest_ValidRequestWithCropHintsAnnotation() {
            ImageFeatures feature = new ImageFeatures(ImageType.CROP_HINTS, MAX_RESULTS, VALID_MODEL);
            imageFeaturesList.Add(feature);

            AnnotateImageRequest imageRequest = GenerateImageRequest(VALID_IMAGE_URL, imageFeaturesList);
            AnnotateImageRequestList imageRequestList = new AnnotateImageRequestList(new List<AnnotateImageRequest>() { imageRequest });

            Task<Tuple<AnnotateImageResponseList, ResponseStatus>> response = imageIntelligence.AnnotateImages(imageRequestList);
            response.Wait();

            AnnotateImageResponseList responseList = response.Result.Item1;

            Assert.IsNotNull(responseList);
            Assert.AreSame(response.Result.Item2, ImageAnnotationStatus.OK);

            Assert.GreaterOrEqual(responseList.Responses.Count, 1);

            for (int i = 0; i < responseList.Responses.Count; i++) {
                Assert.IsNotNull(responseList.Responses[i].CropHintsAnnotation);
                Assert.IsNotNull(responseList.Responses[i].CropHintsAnnotation.CropHints);
                Assert.GreaterOrEqual(responseList.Responses[i].CropHintsAnnotation.CropHints.Count, 1);
            }

            imageRequestList.Requests.Clear();
        }

        [Test]
        public void AnnotateImagesTest_ValidRequestWithImageProperties() {
            ImageFeatures feature = new ImageFeatures(ImageType.IMAGE_PROPERTIES, MAX_RESULTS, VALID_MODEL);
            imageFeaturesList.Add(feature);

            AnnotateImageRequest imageRequest = GenerateImageRequest(VALID_IMAGE_URL, imageFeaturesList);
            AnnotateImageRequestList imageRequestList = new AnnotateImageRequestList(new List<AnnotateImageRequest>() { imageRequest });

            Task<Tuple<AnnotateImageResponseList, ResponseStatus>> response = imageIntelligence.AnnotateImages(imageRequestList);
            response.Wait();

            AnnotateImageResponseList responseList = response.Result.Item1;

            Assert.IsNotNull(responseList);
            Assert.AreSame(response.Result.Item2, ImageAnnotationStatus.OK);

            Assert.GreaterOrEqual(responseList.Responses.Count, 1);

            for (int i = 0; i < responseList.Responses.Count; i++) {
                Assert.IsNotNull(responseList.Responses[i].ImagePropertiesAnnotation);
                Assert.IsNotNull(responseList.Responses[i].ImagePropertiesAnnotation.DominantColors);
                Assert.GreaterOrEqual(responseList.Responses[i].ImagePropertiesAnnotation.DominantColors.Colors.Count, 1);
            }

            imageRequestList.Requests.Clear();
        }

        [Test]
        public void AnnotateImagesTest_ValidRequestWithLandmarkDetection() {
            ImageFeatures feature = new ImageFeatures(ImageType.LANDMARK_DETECTION, MAX_RESULTS, VALID_MODEL);
            imageFeaturesList.Add(feature);

            AnnotateImageRequest imageRequest1 = GenerateImageRequest(LANDMARK_ANNOTATION_URL_1, imageFeaturesList);
            AnnotateImageRequest imageRequest2 = GenerateImageRequest(LANDMARK_ANNOTATION_URL_2, imageFeaturesList);
            AnnotateImageRequestList imageRequestList = new AnnotateImageRequestList(new List<AnnotateImageRequest>() { imageRequest1, imageRequest2 });

            Task<Tuple<AnnotateImageResponseList, ResponseStatus>> response = imageIntelligence.AnnotateImages(imageRequestList);
            response.Wait();

            AnnotateImageResponseList responseList = response.Result.Item1;

            Assert.IsNotNull(responseList);
            Assert.AreSame(response.Result.Item2, ImageAnnotationStatus.OK);

            Assert.GreaterOrEqual(responseList.Responses.Count, 1);

            for (int i = 0; i < responseList.Responses.Count; i++) {
                Assert.IsNotNull(responseList.Responses[i].LandmarkAnnotations);
                Assert.GreaterOrEqual(responseList.Responses[i].LandmarkAnnotations.Count, 1);
            }

            imageRequestList.Requests.Clear();
        }

        [Test]
        public void AnnotateImagesTest_ValidRequestWithLogoDetection() {
            ImageFeatures feature = new ImageFeatures(ImageType.LOGO_DETECTION, MAX_RESULTS, VALID_MODEL);
            imageFeaturesList.Add(feature);

            AnnotateImageRequest imageRequest = GenerateImageRequest(LOGO_DETECTION_URL, imageFeaturesList);
            AnnotateImageRequestList imageRequestList = new AnnotateImageRequestList(new List<AnnotateImageRequest>() { imageRequest });

            Task<Tuple<AnnotateImageResponseList, ResponseStatus>> response = imageIntelligence.AnnotateImages(imageRequestList);
            response.Wait();

            AnnotateImageResponseList responseList = response.Result.Item1;

            Assert.IsNotNull(responseList);
            Assert.AreSame(response.Result.Item2, ImageAnnotationStatus.OK);

            Assert.GreaterOrEqual(responseList.Responses.Count, 1);

            for (int i = 0; i < responseList.Responses.Count; i++) {
                Assert.IsNotNull(responseList.Responses[i].LogoAnnotations);
                Assert.GreaterOrEqual(responseList.Responses[i].LogoAnnotations.Count, 1);
            }

            imageRequestList.Requests.Clear();
        }

        [Test]
        public void AnnotateImagesTest_ValidRequestWithSafeSearchDetection() {
            ImageFeatures feature = new ImageFeatures(ImageType.SAFE_SEARCH_DETECTION, MAX_RESULTS, VALID_MODEL);
            imageFeaturesList.Add(feature);

            AnnotateImageRequest imageRequest = GenerateImageRequest(UNSAFE_IMAGE_URL, imageFeaturesList);
            AnnotateImageRequestList imageRequestList = new AnnotateImageRequestList(new List<AnnotateImageRequest>() { imageRequest });

            Task<Tuple<AnnotateImageResponseList, ResponseStatus>> response = imageIntelligence.AnnotateImages(imageRequestList);
            response.Wait();

            AnnotateImageResponseList responseList = response.Result.Item1;

            Assert.IsNotNull(responseList);
            Assert.AreSame(response.Result.Item2, ImageAnnotationStatus.OK);

            Assert.GreaterOrEqual(responseList.Responses.Count, 1);

            for (int i = 0; i < responseList.Responses.Count; i++) {
                Assert.IsNotNull(responseList.Responses[i].SafeSearchAnnotations);
            }

            imageRequestList.Requests.Clear();
        }

        [Test]
        public void AnnotateImagesTest_ValidRequestWithTextDetection() {
            ImageFeatures feature = new ImageFeatures(ImageType.TEXT_DETECTION, MAX_RESULTS, VALID_MODEL);
            imageFeaturesList.Add(feature);

            AnnotateImageRequest imageRequest = GenerateImageRequest(TEXT_DETECTION_URL_1, imageFeaturesList);
            AnnotateImageRequestList imageRequestList = new AnnotateImageRequestList(new List<AnnotateImageRequest>() { imageRequest });

            Task<Tuple<AnnotateImageResponseList, ResponseStatus>> response = imageIntelligence.AnnotateImages(imageRequestList);
            response.Wait();

            AnnotateImageResponseList responseList = response.Result.Item1;

            Assert.IsNotNull(responseList);
            Assert.AreSame(response.Result.Item2, ImageAnnotationStatus.OK);

            Assert.GreaterOrEqual(responseList.Responses.Count, 1);

            for (int i = 0; i < responseList.Responses.Count; i++) {
                Assert.IsNotNull(responseList.Responses[i].TextAnnotations);
                Assert.GreaterOrEqual(responseList.Responses[i].TextAnnotations.Count, 1);

                Assert.IsNotNull(responseList.Responses[i].FullTextAnnotations);
                Assert.IsNotNull(responseList.Responses[i].FullTextAnnotations.Text);
                Assert.GreaterOrEqual(responseList.Responses[i].FullTextAnnotations.Pages.Count, 1);
            }

            imageRequestList.Requests.Clear();
        }

        [Test]
        public void AnnotateImagesTest_ValidRequestWithDocumentTextDetection() {
            ImageFeatures feature = new ImageFeatures(ImageType.DOCUMENT_TEXT_DETECTION, MAX_RESULTS, VALID_MODEL);
            imageFeaturesList.Add(feature);

            AnnotateImageRequest imageRequest1 = GenerateImageRequest(TEXT_DETECTION_URL_1, imageFeaturesList);
            AnnotateImageRequest imageRequest2 = GenerateImageRequest(TEXT_DETECTION_URL_2, imageFeaturesList);
            AnnotateImageRequestList imageRequestList = 
                new AnnotateImageRequestList(new List<AnnotateImageRequest>() { imageRequest1, imageRequest2 });

            Task<Tuple<AnnotateImageResponseList, ResponseStatus>> response = imageIntelligence.AnnotateImages(imageRequestList);
            response.Wait();

            AnnotateImageResponseList responseList = response.Result.Item1;

            Assert.IsNotNull(responseList);
            Assert.AreSame(response.Result.Item2, ImageAnnotationStatus.OK);

            Assert.GreaterOrEqual(responseList.Responses.Count, 1);

            for (int i = 0; i < responseList.Responses.Count; i++) {
                Assert.IsNotNull(responseList.Responses[i].TextAnnotations);
                Assert.GreaterOrEqual(responseList.Responses[i].TextAnnotations.Count, 1);

                Assert.IsNotNull(responseList.Responses[i].FullTextAnnotations);
                Assert.IsNotNull(responseList.Responses[i].FullTextAnnotations.Text);
                Assert.GreaterOrEqual(responseList.Responses[i].FullTextAnnotations.Pages.Count, 1);
            }

            imageRequestList.Requests.Clear();
        }

        [Test]
        public void AnnotateImagesTest_ValidRequestWithMultipleAnnotations() {
            ImageFeatures feature = new ImageFeatures(ImageType.LANDMARK_DETECTION, MAX_RESULTS, VALID_MODEL);
            imageFeaturesList.Add(feature);
            AnnotateImageRequest imageRequest1 = GenerateImageRequest(LANDMARK_ANNOTATION_URL_1, imageFeaturesList);

            AnnotateImageRequestList imageRequestList =
                new AnnotateImageRequestList(new List<AnnotateImageRequest>() { imageRequest1 });

            feature = new ImageFeatures(ImageType.FACE_DETECTION, MAX_RESULTS, VALID_MODEL);
            imageFeaturesList.Add(feature);
            AnnotateImageRequest imageRequest2 = GenerateImageRequest(FACE_ANNOTATION_URL, imageFeaturesList);
            imageRequestList.Requests.Add(imageRequest2);

            Task<Tuple<AnnotateImageResponseList, ResponseStatus>> response = imageIntelligence.AnnotateImages(imageRequestList);
            response.Wait();

            AnnotateImageResponseList responseList = response.Result.Item1;

            Assert.IsNotNull(responseList);
            Assert.AreSame(response.Result.Item2, ImageAnnotationStatus.OK);
            Assert.AreEqual(responseList.Responses.Count, 2);

            Assert.IsNotNull(responseList.Responses[0].LandmarkAnnotations);
            Assert.GreaterOrEqual(responseList.Responses[0].LandmarkAnnotations.Count, 1);

            Assert.IsNotNull(responseList.Responses[1].FaceAnnotations);
            Assert.GreaterOrEqual(responseList.Responses[1].FaceAnnotations.Count, 1);
            
            imageRequestList.Requests.Clear();
        }
    }
}
