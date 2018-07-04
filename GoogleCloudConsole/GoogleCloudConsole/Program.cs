using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC = GoogleCloudClassLibrary;
using NL = GoogleCloudClassLibrary.NaturalLanguageIntelligence;
using VI = GoogleCloudClassLibrary.VideoIntelligence;
using II = GoogleCloudClassLibrary.ImageIntelligence;
using System.Diagnostics;
using Newtonsoft.Json;

namespace GoogleCloudConsole {
    class Program {
        static void Main(string[] args) {
            String API_KEY = "AIzaSyB72dDD0LrpCvVzkXvXu-qwr9ITqFhFhmU";
            // Google Places Search
            // Funtion 1 - FindPlacesUsingTextQuery - Tested
            GC.Places.PlacesSearch placesSearch = new GC.Places.PlacesSearch();
            Task<List<GC.Places.FindPlaceCandidates>> candidates = placesSearch.FindPlacesUsingTextQuery(API_KEY,
                "720 Northwestern Ave");
            candidates.Wait();

            // Function 2 - GetNearbySearchResultsRankByProminence - Tested
            GC.Places.Location location = new GC.Places.Location(-33.8670522, 151.1957362);
            Task<List<GC.Places.NearbySearchResult>> results = placesSearch.GetNearbySearchResultsRankByProminence(API_KEY, location, 50);
            results.Wait();

            // Natural Language Intelligence
            // Function 3 - AnalyzeEntitySentiment - Tessted
            NL.NaturalLanguageIntelligence nli = new NL.NaturalLanguageIntelligence();
            NL.Document document = new NL.Document((NL.DocumentType.PLAIN_TEXT), "en", "gs://\"gccl_dd_01/Case Response - Masterpiece Cakeshop.pdf\"");
            Task<NL.AnalyzeEntitiesResponse> response = nli.AnalyzeEntitySentiment(API_KEY, document, NL.EncodingType.UTF8);
            response.Wait();

            // Video Intelligence
            // Function 4 - AnnotateVideoWithLabelDetection - Tested
            VI.VideoIntelligence videoIntelligence = new VI.VideoIntelligence();
            Task<VI.VideoAnnotationResponse> annotateResponse =
                videoIntelligence.AnnotateVideoWithLabelDetection(API_KEY, "gs://gccl_dd_01/Video1", "", null);
            annotateResponse.Wait();

            // Image Intelligence
            // Function 5 - AnnotateImage - Tested
            II.ImageIntelligence imageIntelligence = new II.ImageIntelligence();

            II.ImageSource imageSource = new II.ImageSource("gs://gccl_dd_01/Image1");
            II.Image image = new II.Image(source: imageSource);

            List<II.ImageFeatures> imageFeatures = new List<II.ImageFeatures>();
            II.ImageFeatures faceDetection = new II.ImageFeatures(II.ImageType.FACE_DETECTION, 10, "builtin/stable");
            imageFeatures.Add(faceDetection);
            II.ImageFeatures landmarkDetection = new II.ImageFeatures(II.ImageType.LANDMARK_DETECTION, 10, "builtin/stable");
            imageFeatures.Add(landmarkDetection);
            II.ImageFeatures imageProps = new II.ImageFeatures(II.ImageType.IMAGE_PROPERTIES, 10, "builtin/stable");
            imageFeatures.Add(imageProps);

            II.AnnotateImageRequests request = new II.AnnotateImageRequests(image, imageFeatures, null);
            List<II.AnnotateImageRequests> requestsList = new List<II.AnnotateImageRequests>();
            requestsList.Add(request);
            II.AnnotateImageRequestList imageRequestList = new II.AnnotateImageRequestList(requestsList);

            String json = JsonConvert.SerializeObject(imageRequestList);
            Console.WriteLine(json);

            Task<List<II.AnnotateImageResponse>> responses = imageIntelligence.AnnotateImage(API_KEY, imageRequestList);
            responses.Wait();

            //String file = "C:\\Users\\Dhairya\\Desktop\\json_out_NL.txt";
            //String json = System.IO.File.ReadAllText(file);
            //Console.WriteLine(json);

            //II.AnnotateImageResponseList list = JsonConvert.DeserializeObject<II.AnnotateImageResponseList>(json);
            return;
        }
    }
}





