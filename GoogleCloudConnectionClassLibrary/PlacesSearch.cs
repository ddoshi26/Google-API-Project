using System;
using GC = GoogleCloudClassLibrary.PlacesHeader;

namespace GoogleCloudClassLibrary.Places {
    class PlacesSearch {

        // Find Places

        public GC.FindPlaceCandidates[] FindPlacesUsingTextQuery(String APIKey, String query) {
            return null;
        }

        public GC.FindPlaceCandidates[] FindPlacesUsingPhoneNumber(String APIKey, String phone_no) {
            return null;
        }

        public GC.FindPlaceCandidates[] FindPlacesWithPointLocationBias(String APIKey, String query, String input_type,
            GC.Location location, String language_code = "en", String fields = "") {
            return null;
        }

        public GC.FindPlaceCandidates[] FindPlacesWithCircularLocationBias(String APIKey, String query, 
            String input_type, GC.Location location, double radius, String language_code = "en", 
            String fields = "") {
            return null;
        }

        public GC.FindPlaceCandidates[] FindPlacesWithRectLocationBias(String APIKey, String query, String input_type, 
            GC.Location southWestCorner, GC.Location northEastCorner, String language_code = "en", String fields = "") {
            return null;
        }

        // Nearby Search Requests

        public GC.NearbySearchResult[] GetNearbySearchResultsRankByProminence(String APIKey, GC.Location location, 
            double radius) {
            
        }

        public GC.NearbySearchResult[] GetNearbySearchResultsRankByProminenceWithOptions(String APIKey, 
            GC.Location location, double radius, String keyword = "", String language_code = "en", int min_price = -1,
            int max_price = -1, String type = "", Boolean open_now) {
                return null;
        }

        public GC.NearbySearchResult[] GetNearbySearchResultsRankByDistance(String APIKey, GC.Location location,
            String keyword = "", String type = "") {
            return null;
        }
        
        public GC.NearbySearchResult[] GetNearbySearchResultsRankByDistanceWithOptions(String APIKey, 
            GC.Location location, String keyword = "", String type = "", String language_code = "en", int min_price = -1,
            int max_price = -1, Boolean open_now) {
            return null;
        }

        public GC.NearbySearchResult[] GetAdditionalNearbySearchResults(String APIKey) {
            return null;
        }

        // Text Search Requests

        public GC.NearbySearchResult[] GetTextSearchResults(String APIKey, String query) {
            return null;
        }

        public GC.NearbySearchResult[] GetTextSearchResultsWithOptions(String APIKey, String query, double radius = -1,
            GC.Location location, String region_code = "", String language_code = "en", String type = "", 
            int min_price = -1, int max_price = -1, Boolean open_now) {
            return null;
        }

        public GC.NearbySearchResult[] GetAdditionalTextSearchResults(String APIKey) {
            return null;
        }
    } 
}