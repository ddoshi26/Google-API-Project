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

            // Since this is the first time we use the httpClient, we need to intialize its base address
            httpClient.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/place/");
        }

        // Find Places

        /*
         * Method: FindPlacesUsingTextQuery
         * 
         * Description: This method can be used to query the Places API for places with some specific matching 
         *   paramter. For phone numbers, please use the FindPlacesUsingPhoneNumber() method.
         * 
         * Parameters:
         *   - APIKey (String): String representing the Gooogle Cloud Services API access key. For more details:
         *       https://developers.google.com/places/web-service/get-api-key
         *   - query (String): A string parameter that will be used to search through the Places API and find 
         *       places that contain matching information. This could be any collection of keywords that can 
         *       appropriately describe the information sought. 
         * 
         * Return: The method returns a list of all the candidates that match the query provided. The list is 
         *   wrapped in a Task<> because the method makes Asynchronous HTTP requests to the Places API.
         */
        public async Task<List<FindPlaceCandidates>> FindPlacesUsingTextQuery(String APIKey, String query) {
            if (BasicFunctions.isEmpty(APIKey) || BasicFunctions.isEmpty(query)) {
                return null;
            }

            // Converting the query into a URL friendly version
            String processedQuery = BasicFunctions.processTextQuery(query);
            String HTTP_query = $"findplacefromtext/json?input={processedQuery}&inputtype=textquery&key={APIKey}";

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Making the asynchronous GET request to Places API and colleting the response
            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            Console.WriteLine(response_str);

            /* 
             * Here we do a two-step hop to achieve the appropriate return value:
             *   We use the response string (response_str) from above and attempt to convert it back from json to 
             *   FindPlacesCandidateList, the expected return object for a successful query. This produces one of 
             *   two possibilities:
             *   1. If the response string is not a json of the FindPlacesCandidateList class, then we either get 
             *      a JsonSerializationException or an empty list. In this case we print out the response and 
             *      return null (Will improve this to return an appropriate error code).
             *   2. If the response string is as expected a json of FindPlacesCandidateList, then things go 
             *      smoothly and we return that.
             */
            try {
                FindPlacesCandidateList candidateList = JsonConvert.DeserializeObject<FindPlacesCandidateList>(response_str);
                if (candidateList.Candidates.Count == 0) {
                    return null;
                }
                else {
                    return candidateList.Candidates;
                }
            } catch (JsonSerializationException e) {
                Console.WriteLine("Exception: " + e.StackTrace);
                return null;
            }
        }

        public List<FindPlaceCandidates> FindPlacesUsingPhoneNumber(String APIKey, String phone_no) {
            return null;
        }

        public List<FindPlaceCandidates> FindPlacesWithPointLocationBias(String APIKey, String query, String input_type,
            Location location, String language_code = "en", String fields = "") {
            return null;
        }

        public List<FindPlaceCandidates> FindPlacesWithCircularLocationBias(String APIKey, String query,
            String input_type, Location location, double radius, String language_code = "en",
            String fields = "") {
            return null;
        }

        public List<FindPlaceCandidates> FindPlacesWithRectLocationBias(String APIKey, String query, String input_type,
            Location southWestCorner, Location northEastCorner, String language_code = "en", String fields = "") {
            return null;
        }

        // Nearby Search Requests
        // TODO: Create Custom error messages for validation errors

        /*
         * Method: GetNearbySearchResultsRankByProminence
         * 
         * Description: This method can be used to find places, ranked by prominence, within a circular region
         *   described by the parameters. The prominence based ranking is determined through a combination of factors 
         *   such as the popularity, frequency of search, and ranking in Google's index.
         * 
         * Parameters:
         *  - APIKey (String): String representing the Gooogle Cloud Services API access key. For more details:
         *      https://developers.google.com/places/web-service/get-api-key
         *  - location (Location): This is the representation of a point (latitude, longtitude). This along with
         *      the radius will be used to create a circular search region.
         *  - radius (double): Radius of the circular search region from the location point. This is in meters
         *      and the allowable value is between 0 and 50,000 meters (inclusive).
         * 
         * Return: The method returns a list of all the places within the given circular search region. The 
         *   list is sorted based on descending order of prominence (importance) with the most prominant 
         *   places at the head of the list. The list is wrapped in a Task<> because the method makes 
         *   Asynchronous HTTP requests to the Places API.
         */
        public async Task<List<NearbySearchResult>> GetNearbySearchResultsRankByProminence(String APIKey, Location location,
            double radius) {
            if (BasicFunctions.isEmpty(APIKey) || location == null)
                return null;

            if (radius < 0 || radius > 50000)
                return null;

            // Creating the HTTP query url
            String HTTP_query = $"nearbysearch/json?location={location.Lat},{location.Lng}&radius={radius}&key={APIKey}";

            // Setting up the request header to indicate that the request body will be in json
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Making an asynchronous HTTP GET request to the Places API and collecting the output
            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            Console.WriteLine(response_str);

            /* 
             * Here we do a two-step hop again to achieve the appropriate return value:
             *   We use the response string (response_str) from above and attempt to convert it back from json to 
             *   NearbySearchResultList, the expected return object for a successful query. This produces one of 
             *   two possibilities:
             *   1. If the response string is not a json of the NearbySearchResultList class, then we either get 
             *      a JsonSerializationException or an empty list. In this case we print out the response and 
             *      return null (Will improve this to return an appropriate error code).
             *   2. If the response string is as expected a json of NearbySearchResultList, then things go 
             *      smoothly and we return that.
             */
            try {
                NearbySearchResultList searchResultList = JsonConvert.DeserializeObject<NearbySearchResultList>(response_str);
                if (searchResultList.Results.Count == 0) {
                    return null;
                }
                else {
                    return searchResultList.Results;
                }
            } catch (JsonSerializationException e) {
                Console.WriteLine("Exception: " + e.StackTrace);
                return null;
            }
        }

        public List<NearbySearchResult> GetNearbySearchResultsRankByProminenceWithOptions(String APIKey,
            Location location, double radius, Boolean open_now, String keyword = "", String language_code = "en", int min_price = -1,
            int max_price = -1, String type = "") {
            return null;
        }

        public List<NearbySearchResult> GetNearbySearchResultsRankByDistance(String APIKey, Location location,
            String keyword = "", String type = "") {
            return null;
        }

        public List<NearbySearchResult> GetNearbySearchResultsRankByDistanceWithOptions(String APIKey,
            Location location, Boolean open_now, String keyword = "", String type = "",
            String language_code = "en", int min_price = -1, int max_price = -1) {
            return null;
        }

        public List<NearbySearchResult> GetAdditionalNearbySearchResults(String APIKey) {
            return null;
        }

        // Text Search Requests

        public List<NearbySearchResult> GetTextSearchResults(String APIKey, String query) {
            return null;
        }

        public List<NearbySearchResult> GetTextSearchResultsWithOptions(String APIKey, String query, Location location,
            Boolean open_now, double radius = -1, String region_code = "", String language_code = "en", String type = "",
            int min_price = -1, int max_price = -1) {
            return null;
        }

        public List<NearbySearchResult> GetAdditionalTextSearchResults(String APIKey) {
            return null;
        }
    }
}
