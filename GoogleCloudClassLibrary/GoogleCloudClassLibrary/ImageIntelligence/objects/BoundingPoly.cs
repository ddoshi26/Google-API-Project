using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class BoundingPoly {
        private List<Vertex> vertices;

        [JsonProperty("vertices")]
        public List<Vertex> Vertices {
            get => vertices; set => vertices = value;
        }

        public BoundingPoly(List<Vertex> vertices) {
            this.Vertices = vertices;
        }
    }
}
