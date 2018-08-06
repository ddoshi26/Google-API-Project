using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class Data {
        private AnnotateImageRequestList requests;

        [JsonProperty("data")]
        public AnnotateImageRequestList Requests {
            get => requests;
            set => requests = value;
        }


        public Data(AnnotateImageRequestList requests) {
            Requests = requests;
        }
    }
}
