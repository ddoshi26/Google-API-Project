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

            // Google Places Search
            // Funtion 1 - FindPlacesUsingTextQuery
            GC.Places.PlacesSearch placesSearch = new GC.Places.PlacesSearch();
            Task<List<GC.Places.FindPlaceCandidates>> candidates = placesSearch.FindPlacesUsingTextQuery("1234", "720 Northwestern Ave");
            candidates.Wait();
            
            // Function 2 - GetNearbySearchResultsRankByProminence
            GC.Places.Location location = new GC.Places.Location(-15.5767, 28.9177);
            Task<List<GC.Places.NearbySearchResult>> results = placesSearch.GetNearbySearchResultsRankByProminence("API_KEY", location, 50);
            results.Wait();

            // Natural Language Intelligence
            // Function 3 - AnalyzeEntitySentiment 
            NL.NaturalLanguageIntelligence nli = new NL.NaturalLanguageIntelligence();
            NL.Document document = new NL.Document((NL.DocumentType.PLAIN_TEXT), "en", "abcdefghijk");
            Task<NL.AnalyzeEntitiesResponse> response = nli.AnalyzeEntitySentiment("API_KEY", document, NL.EncodingType.UTF8);
            response.Wait();

            // Video Intelligence
            // Function 4 - AnnotateVideoWithLabelDetection 
            VI.VideoIntelligence videoIntelligence = new VI.VideoIntelligence();
            Task<VI.Operation> operation = 
                videoIntelligence.AnnotateVideoWithLabelDetection("API_KEY", "gs://cloudmleap/video/next/GoogleFiber.mp4", "", null);
            operation.Wait();

            // Image Intelligence
            // Function 5 - AnnotateImage 
            II.ImageIntelligence imageIntelligence = new II.ImageIntelligence();
            II.AnnotateImageRequests request = new II.AnnotateImageRequests(new II.Image(), null, null);
            List<II.AnnotateImageRequests> requestsList = new List<II.AnnotateImageRequests>();
            requestsList.Add(request);
            Task<List<II.AnnotateImageResponse>> responses = imageIntelligence.AnnotateImage("API_KEY", requestsList);
            responses.Wait();
        }
    }
}





