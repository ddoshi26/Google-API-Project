using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class AnnotateImageResponseList {
        private List<AnnotateImageResponse> responses;

        [JsonProperty("responses")]
        public List<AnnotateImageResponse> Responses { get => responses; set => responses = value; }

        public AnnotateImageResponseList(List<AnnotateImageResponse> responses) {
            Responses = responses;
        }
    }
}
