﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.Places {
    class PlacesDetails {
        public List<NearbySearchResult> GetPlaceDetails(String APIKey, String place_id) {
            return null;
        }

        public List<NearbySearchResult> GetPlaceDetailsWithOptions(String APIKey, String place_id, String region = "",
            String language_code = "", String session_token = "", String fields = "") {
            return null;
        }
    }
}
