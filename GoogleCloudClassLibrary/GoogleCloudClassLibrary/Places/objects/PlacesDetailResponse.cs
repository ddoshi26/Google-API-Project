using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.Places {
    public class PlacesDetailResponse {
        private List<String> html_attributions;
        private PlacesDetailResult result;
        private String status;
        private String error_message;

        [JsonProperty("html_attributions")]
        public List<String> Html_attributions {
            get => html_attributions;
            set => html_attributions = value;
        }

        [JsonProperty("result")]
        public PlacesDetailResult Result {
            get => result;
            set => result = value;
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

        public PlacesDetailResponse(List<String> html_attributions, PlacesDetailResult result, string status, string error_message) {
            Html_attributions = html_attributions;
            Result = result;
            Status = status;
            Error_message = error_message;
        }
    }
}
