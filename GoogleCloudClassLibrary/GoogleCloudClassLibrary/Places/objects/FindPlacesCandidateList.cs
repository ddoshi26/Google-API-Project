using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.Places {
    public class FindPlacesCandidateList {
        private List<FindPlaceCandidates> candidates;
        private String status;
        private String error_message;

        [JsonProperty("candidates")]
        public List<FindPlaceCandidates> Candidates { get => candidates; set => candidates = value; }

        [JsonProperty("status")]
        public String Status {
            get => status;
            set => status = value;
        }

        [JsonProperty("error_message")]
        public string Error_message {
            get => error_message;
            set => error_message = value;
        }

        public FindPlacesCandidateList(List<FindPlaceCandidates> candidates, String status, String error_message) {
            Candidates = candidates;
            Status = status;
            Error_message = error_message;
        }
    }
}
