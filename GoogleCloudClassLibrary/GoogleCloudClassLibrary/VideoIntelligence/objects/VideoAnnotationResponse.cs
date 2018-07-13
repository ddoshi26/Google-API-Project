using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class VideoAnnotationResponse {
        private String name;
        private Metadata metadata;
        private Boolean done;
        private AnnotateResponse response;

        [JsonProperty("name")]
        public string Name {
            get => name;
            set => name = value;
        }

        [JsonProperty("metadata")]
        public Metadata Metadata {
            get => metadata;
            set => metadata = value;
        }

        [JsonProperty("done")]
        public bool Done {
            get => done;
            set => done = value;
        }

        [JsonProperty("response")]
        public AnnotateResponse Response {
            get => response;
            set => response = value;
        }

        public VideoAnnotationResponse(string name, Metadata metadata, bool done, AnnotateResponse response) {
            this.Name = name;
            this.Metadata = metadata;
            this.Done = done;
            this.Response = response;
        }
    }
}
