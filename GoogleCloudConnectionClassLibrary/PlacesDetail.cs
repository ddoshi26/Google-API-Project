using System;
using GC = GoogleCloudClassLibrary.PlacesHeader;

namespace GoogleCloudClassLibrary.Places {
	class PlacesDetails {
		public GC.NearbySearchResult[] GetPlaceDetails(String APIKey, String place_id) {
            return null;
        }

        public GC.NearbySearchResult[] GetPlaceDetailsWithOptions(String APIKey, String place_id, String region = "",
            String language_code = "", String session_token = "", String fields = "") {
            return null;
        }
    }
}