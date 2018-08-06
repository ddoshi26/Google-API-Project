using System;
using System.Threading.Tasks;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using GoogleCloudClassLibrary.VideoIntelligence;
using ResponseStatus = GoogleCloudClassLibrary.ResponseStatus;

namespace GoogleClassLibraryTest {
    public class VideoAnnotationShotChangeDetectionTest {
        private static VideoIntelligence videoIntelligence;

        private GC.GoogleCloudClassSetup VALID_SETUP;
        private GC.GoogleCloudClassSetup INVALID_SETUP;

        private static readonly String VALID_VIDEO_URL = "gs://gccl_dd_01/Video1";
        private static readonly String VIDEO_URL = "VIDEO_URL";

        private static readonly String VIDEO_DATA = "VIDEO_DATA";
        
        public VideoAnnotationShotChangeDetectionTest() {
            String dir = Environment.GetEnvironmentVariable("GC_LIBRARY_TEST", EnvironmentVariableTarget.User);
            VALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\APIKEY.txt");
            INVALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\INVALID_APIKEY.txt");

            videoIntelligence = new VideoIntelligence(VALID_SETUP);
        }

        [Test]
        public void VideoAnnotationShotChangeDetection_InvalidKey() {
            videoIntelligence.UpdateKey(INVALID_SETUP);

            Task<Tuple<VideoAnnotationResponse, ResponseStatus>> response =
                videoIntelligence.AnnotateVideoWithShotChangeDetection(inputUri: VIDEO_URL);
            response.Wait();

            videoIntelligence.UpdateKey(VALID_SETUP);

            Assert.IsNull(response.Result.Item1);
            Assert.IsNotNull(response.Result.Item2);
            Assert.AreEqual(response.Result.Item2.Code, 400);
        }

        [Test]
        public void VideoAnnotationShotChangeDetection_MissingInputParameters() {
            Task<Tuple<VideoAnnotationResponse, ResponseStatus>> response =
                videoIntelligence.AnnotateVideoWithShotChangeDetection();
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, VideoAnnotationStatus.TOO_FEW_PARAMETERS);
        }

        [Test]
        public void VideoAnnotationShotChangeDetection_TooManyInputParameters() {
            Task<Tuple<VideoAnnotationResponse, ResponseStatus>> response =
                videoIntelligence.AnnotateVideoWithShotChangeDetection(inputUri: VIDEO_URL, inputContent: VIDEO_DATA);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, VideoAnnotationStatus.TOO_MANY_PARAMETERS);
        }

        [Test]
        public void VideoAnnotationShotChangeDetection_ValidQuery() {
            Task<Tuple<VideoAnnotationResponse, ResponseStatus>> response =
                videoIntelligence.AnnotateVideoWithShotChangeDetection(inputUri: VALID_VIDEO_URL);
            response.Wait();

            VideoAnnotationResponse annotationResponse = response.Result.Item1;
            ResponseStatus status = response.Result.Item2;

            Assert.AreSame(status, VideoAnnotationStatus.OK);

            Assert.IsNotNull(annotationResponse);
            Assert.IsTrue(annotationResponse.Done);

            Assert.IsNotNull(annotationResponse.Metadata);
            Assert.AreEqual(annotationResponse.Metadata.Type, "type.googleapis.com/google.cloud.videointelligence.v1.AnnotateVideoProgress");
            Assert.GreaterOrEqual(annotationResponse.Metadata.annotationProgress.Count, 1);

            Boolean foundCompletedProgress = false;

            for (int i = 0; i < annotationResponse.Metadata.annotationProgress.Count; i++) {
                AnnotationProgress annotationProgress = annotationResponse.Metadata.annotationProgress[i];

                if (annotationProgress.ProgressPercent == 100) {
                    foundCompletedProgress = true;
                    break;
                }
            }
            Assert.IsTrue(foundCompletedProgress);

            Assert.IsNotNull(annotationResponse.Name);

            Assert.IsNotNull(annotationResponse.Response);
            Assert.AreEqual(annotationResponse.Response.Type, "type.googleapis.com/google.cloud.videointelligence.v1.AnnotateVideoResponse");

            Assert.AreEqual(annotationResponse.Response.AnnotationResults.Count, 1);
            Assert.IsTrue(VALID_VIDEO_URL.Contains(annotationResponse.Response.AnnotationResults[0].InputUri));

            Assert.GreaterOrEqual(annotationResponse.Response.AnnotationResults[0].ShotAnnotations.Count, 1);
        }
    }
}
