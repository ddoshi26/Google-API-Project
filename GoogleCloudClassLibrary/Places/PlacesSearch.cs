using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net = System.Net;
using Web = System.Web;
using System.IO;
using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.Places {
    class PlacesSearch {
        // Find Places

        // Replace int with Status class
        public List<FindPlaceCandidates> FindPlacesUsingTextQuery(String APIKey, String query) {
            if (BasicFunctions.isEmpty(APIKey) || BasicFunctions.isEmpty(query)) {
                return null;
            }

            String processedQuery = BasicFunctions.processTextQuery(query);

            String HTTP_request = $"https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input={processedQuery}" +
                "&inputtype=textquery&" + $"key={APIKey}";

            Console.WriteLine(HTTP_request);

            // Creating the HTTP WebRequest for this url
            Net.HttpWebRequest request = (Net.HttpWebRequest) Net.WebRequest.Create(HTTP_request);
            Net.HttpWebResponse response = (Net.HttpWebResponse) request.GetResponse();
            Net.HttpStatusCode status_code = response.StatusCode;
            Stream stream = response.GetResponseStream();

            // Function call to extract 
            String result = BasicFunctions.processResponseStream(stream);
            Console.WriteLine(result);

            // Function call to process json
            FindPlacesCandidateList candidatesList;
            try {
                candidatesList = JsonConvert.DeserializeObject<FindPlacesCandidateList>(result);
            } catch (JsonSerializationException e) {
                Console.WriteLine(e.StackTrace);
                return null;
            }

            return candidatesList.Candidates;
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

        public List<NearbySearchResult> GetNearbySearchResultsRankByProminence(String APIKey, Location location,
            double radius) {
            if (BasicFunctions.isEmpty(APIKey) || location == null || radius <= 0)
                return null;

            if (radius > 50000)
                return null;

            String Http_request = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?" +
                $"location={location.Lat},{location.Lng}&radius={radius}";

            Console.WriteLine(Http_request);

            Net.HttpWebRequest request = (Net.HttpWebRequest) Net.WebRequest.Create(Http_request);
            Net.HttpWebResponse response = (Net.HttpWebResponse) request.GetResponse();
            Net.HttpStatusCode status_code = response.StatusCode;
            Stream stream = response.GetResponseStream();

            // Function call to extract 
            String result = BasicFunctions.processResponseStream(stream);
            Console.WriteLine(result);

            ////NearbySearchResults searchResults = JsonConvert.DeserializeObject(result);

            return null;
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
