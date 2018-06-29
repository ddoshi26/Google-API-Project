using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.
    Threading.Tasks;
using GC = GoogleCloudClassLibrary;
using NL = GoogleCloudClassLibrary.NaturalLanguageIntelligence;
using VI = GoogleCloudClassLibrary.VideoIntelligence;
using II = GoogleCloudClassLibrary.ImageIntelligence;
using System.Diagnostics;
using Newtonsoft.Json;

namespace GoogleCloudConsole {
    class Program {
        static void Main(string[] args) {
            GC.Places.PlacesSearch placesSearch = new GC.Places.PlacesSearch();
            Task<List<GC.Places.FindPlaceCandidates>> candidates = placesSearch.FindPlacesUsingTextQuery("1234", "720 Northwestern Ave");
            candidates.Wait();
            
            GC.Places.Location location = new GC.Places.Location(-15.5767, 28.9177);
            Task<List<GC.Places.NearbySearchResult>> results = placesSearch.GetNearbySearchResultsRankByProminence("API_KEY", location, 50);
            results.Wait();

            NL.NaturalLanguageIntelligence nli = new NL.NaturalLanguageIntelligence();
            NL.Document document = new NL.Document((NL.DocumentType.PLAIN_TEXT), "en", "abcdefghijk");
            Task<NL.AnalyzeEntitiesResponse> response = nli.AnalyzeEntitySentiment("API_KEY", document, NL.EncodingType.UTF8);
            response.Wait();

            VI.VideoIntelligence videoIntelligence = new VI.VideoIntelligence();
            Task<VI.Operation> operation = 
                videoIntelligence.AnnotateVideoWithLabelDetection("API_KEY", "gs://cloudmleap/video/next/GoogleFiber.mp4", "", null);
            operation.Wait();

            II.ImageIntelligence imageIntelligence = new II.ImageIntelligence();
            II.AnnotateImageRequests request = new II.AnnotateImageRequests(new II.Image(), null, null);
            List<II.AnnotateImageRequests> requestsList = new List<II.AnnotateImageRequests>();
            requestsList.Add(request);

            Task<List<II.AnnotateImageResponse>> responses = imageIntelligence.AnnotateImage("API_KEY", requestsList);
            responses.Wait();

            //NL.AnalyzeEntitiesRequest entitiesRequest = new NL.AnalyzeEntitiesRequest(document, NL.EncodingType.UTF8);
            ////GC.Places.Location location = new GC.Places.Location(-15.5767, 28.9177);
            ////GC.Places.FindPlaceCandidates candidates = new GC.Places.FindPlaceCandidates("720 North", location, "Dhairya", false, null, 5);
            //String output = JsonConvert.SerializeObject(entitiesRequest);
            //Console.WriteLine(output);
            //NL.AnalyzeEntitiesRequest entitiesRequest1 = JsonConvert.DeserializeObject<NL.AnalyzeEntitiesRequest>(output);

            return;
        }
    }
}





