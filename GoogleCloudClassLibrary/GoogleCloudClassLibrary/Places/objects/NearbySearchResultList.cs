using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.Places {
    public class NearbySearchResultList {
        private String nextPageToken;
        private List<String> html_attributions;
        private List<NearbySearchResult> results;
        private String status;
        private String error_message;

        [JsonProperty("results")]
        public List<NearbySearchResult> Results {
            get => results; set => results = value;
        }

        [JsonProperty("html_attributions")]
        public List<String> Html_attributions {
            get => html_attributions;
            set => html_attributions = value;
        }

        [JsonProperty("next_page_token")]
        public string NextPageToken {
            get => nextPageToken;
            set => nextPageToken = value;
        }

        [JsonProperty("status")]
        public string Status {
            get => status;
            set => status = value;
        }

        [JsonProperty("error_message")]
        public string Error_message {
            get => error_message;
            set => error_message = value;
        }

        public NearbySearchResultList(List<NearbySearchResult> results, List<String> html_attributions, String status, String error_message) {
            this.Results = results;
            this.Html_attributions = html_attributions;
            this.Status = status;
            this.Error_message = error_message;
        }
    }
}
