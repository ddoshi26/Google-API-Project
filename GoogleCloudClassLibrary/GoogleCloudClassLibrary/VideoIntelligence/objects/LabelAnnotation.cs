using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class LabelAnnotation {
        private Entity entity;
        private List<Entity> categoryEntities;
        private List<LabelSegment> segments;
        private List<LabelFrame> frames;

        [JsonProperty("entity")]
        public Entity Entity {
            get => entity;
            set => entity = value;
        }

        [JsonProperty("categoryEntities")]
        public List<Entity> CategoryEntities {
            get => categoryEntities;
            set => categoryEntities = value;
        }

        [JsonProperty("segments")]
        public List<LabelSegment> Segments {
            get => segments;
            set => segments = value;
        }

        [JsonProperty("frames")]
        public List<LabelFrame> Frames {
            get => frames;
            set => frames = value;
        }

        public LabelAnnotation(Entity entity, List<Entity> categoryEntities, List<LabelSegment> segments, List<LabelFrame> frame) {
            this.Entity = entity;
            this.CategoryEntities = categoryEntities;
            this.Segments = segments;
            this.Frames = frame;
        }
    }
}
