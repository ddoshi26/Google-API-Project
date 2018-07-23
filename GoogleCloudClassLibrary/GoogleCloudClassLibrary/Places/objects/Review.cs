using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.Places {
    public class Review {
        private String authorName;
        private String authorUrl;
        private String language;
        private String profilePhotoUrl;
        private double rating;
        private String relativeTimeDescription;
        private String text;
        private long time;
        private List<AspectRating> aspects;

        [JsonProperty("author_name")]
        public string AuthorName {
            get => authorName;
            set => authorName = value;
        }

        [JsonProperty("author_url")]
        public string AuthorUrl {
            get => authorUrl;
            set => authorUrl = value;
        }

        [JsonProperty("language")]
        public string Language {
            get => language;
            set => language = value;
        }

        [JsonProperty("profile_photo_url")]
        public string ProfilePhotoUrl {
            get => profilePhotoUrl;
            set => profilePhotoUrl = value;
        }

        [JsonProperty("rating")]
        public double Rating {
            get => rating;
            set => rating = value;
        }

        [JsonProperty("relative_time_description")]
        public string RelativeTimeDescription {
            get => relativeTimeDescription;
            set => relativeTimeDescription = value;
        }

        [JsonProperty("text")]
        public string Text {
            get => text;
            set => text = value;
        }

        [JsonProperty("time")]
        public long Time {
            get => time;
            set => time = value;
        }

        [JsonProperty("aspects")]
        public List<AspectRating> Aspects {
            get => aspects;
            set => aspects = value;
        }

        public Review(string authorName, string authorUrl, string language, string profilePhotoUrl, double rating, string relativeTimeDescription, string text) {
            AuthorName = authorName;
            AuthorUrl = authorUrl;
            Language = language;
            ProfilePhotoUrl = profilePhotoUrl;
            Rating = rating;
            RelativeTimeDescription = relativeTimeDescription;
            Text = text;
        }
    }
}
