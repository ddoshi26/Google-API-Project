using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net = System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Web = System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace GoogleCloudClassLibrary.Places {
    public class PlacesSearch {

        private static HttpClient httpClient;

        public PlacesSearch() {
            httpClient = new HttpClient();
        }

        // Find Places

        // Replace int with Status class
        public async Task<List<FindPlaceCandidates>> FindPlacesUsingTextQuery(String APIKey, String query) {
            if (BasicFunctions.isEmpty(APIKey) || BasicFunctions.isEmpty(query)) {
                return null;
            }

            String processedQuery = BasicFunctions.processTextQuery(query);

            String HTTP_base_request = "https://maps.googleapis.com/maps/api/place/";
            String HTTP_query = $"findplacefromtext/json?input={processedQuery}&inputtype=textquery&key={APIKey}";

            Console.WriteLine(HTTP_base_request + HTTP_query);

            httpClient.BaseAddress = new Uri(HTTP_base_request);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();

            try {
                FindPlacesCandidateList candidateList = JsonConvert.DeserializeObject<FindPlacesCandidateList>(response_str);
                if (candidateList.Candidates.Count == 0) {
                    Console.WriteLine(response_str);
                    return null;
                }
                else {
                    return candidateList.Candidates;
                }
            } catch (JsonSerializationException e) {
                Console.WriteLine(response_str);
                Console.WriteLine("Exception: " + e.StackTrace);
                return null;
            }
        }

        public List<FindPlaceCandidates> FindPlacesUsingPhoneNumber(String APIKey, String phone_no) {
            return null;
        }

        public FindPlaceCandidates[] FindPlacesWithPointLocationBias(String APIKey, String query, String input_type,
            Location location, String language_code = "en", String fields = "") {
            return null;
        }

        public FindPlaceCandidates[] FindPlacesWithCircularLocationBias(String APIKey, String query,
            String input_type, Location location, double radius, String language_code = "en",
            String fields = "") {
            return null;
        }

        public FindPlaceCandidates[] FindPlacesWithRectLocationBias(String APIKey, String query, String input_type,
            Location southWestCorner, Location northEastCorner, String language_code = "en", String fields = "") {
            return null;
        }

        // Nearby Search Requests
        // TODO: Create Custom error messages for validation errors

        public async Task<List<NearbySearchResult>> GetNearbySearchResultsRankByProminence(String APIKey, Location location,
            double radius) {
            if (BasicFunctions.isEmpty(APIKey) || location == null || radius <= 0)
                return null;

            if (radius > 50000)
                return null;

            String HTTP_query = $"nearbysearch/json?location={location.Lat},{location.Lng}&radius={radius}&key={APIKey}";
            
            //Console.WriteLine(HTTP_base_request + HTTP_query);

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();

            try {
                NearbySearchResultList searchResultList = JsonConvert.DeserializeObject<NearbySearchResultList>(response_str);
                if (searchResultList.Results.Count == 0) {
                    Console.WriteLine(response_str);
                    return null;
                }
                else {
                    return searchResultList.Results;
                }
            } catch (JsonSerializationException e) {
                Console.WriteLine(response_str);
                Console.WriteLine("Exception: " + e.StackTrace);
                return null;
            }
        }

        public NearbySearchResult[] GetNearbySearchResultsRankByProminenceWithOptions(String APIKey,
            Location location, double radius, Boolean open_now, String keyword = "", String language_code = "en", int min_price = -1,
            int max_price = -1, String type = "") {
            return null;
        }

        public NearbySearchResult[] GetNearbySearchResultsRankByDistance(String APIKey, Location location,
            String keyword = "", String type = "") {
            return null;
        }

        public NearbySearchResult[] GetNearbySearchResultsRankByDistanceWithOptions(String APIKey,
            Location location, Boolean open_now, String keyword = "", String type = "",
            String language_code = "en", int min_price = -1, int max_price = -1) {
            return null;
        }

        public NearbySearchResult[] GetAdditionalNearbySearchResults(String APIKey) {
            return null;
        }

        // Text Search Requests

        public NearbySearchResult[] GetTextSearchResults(String APIKey, String query) {
            return null;
        }

        public NearbySearchResult[] GetTextSearchResultsWithOptions(String APIKey, String query, Location location,
            Boolean open_now, double radius = -1, String region_code = "", String language_code = "en", String type = "",
            int min_price = -1, int max_price = -1) {
            return null;
        }

        public NearbySearchResult[] GetAdditionalTextSearchResults(String APIKey) {
            return null;
        }
    }
}
