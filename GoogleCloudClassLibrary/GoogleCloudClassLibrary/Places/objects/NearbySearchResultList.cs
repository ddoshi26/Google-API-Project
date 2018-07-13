using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.Places {
    public class NearbySearchResultList {
        private String nextPageToken;
        private List<String> html_attributions;
        private List<NearbySearchResult> results;

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

        public NearbySearchResultList(List<NearbySearchResult> results, List<String> html_attributions) {
            this.Results = results;
            this.Html_attributions = html_attributions;
        }
    }
}
