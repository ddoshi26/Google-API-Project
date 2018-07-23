using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.Places {
    public class AspectRating {
        private AspectRatingType type;
        private int rating;

        [JsonProperty("type")]
        public AspectRatingType Type {
            get => type;
            set => type = value;
        }

        [JsonProperty("rating")]
        public int Rating {
            get => rating;
            set => rating = value;
        }

        public AspectRating(AspectRatingType type, int rating) {
            Type = type;
            Rating = rating;
        }
    }
}
