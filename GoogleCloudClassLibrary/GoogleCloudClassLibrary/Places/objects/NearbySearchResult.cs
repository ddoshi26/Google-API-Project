using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.Places {
    public class NearbySearchResult {
        private Geometry geometry;
        private String iconHTTP;
        private String id;
        private String name;

        private OpeningHours openingHours;

        private List<Photo> photos;

        private String place_id;
        private String scope;

        private List<Id> alternate_ids;

        private String reference;
        private List<String> types;
        private String vicinity;

        [JsonProperty("icon")]
        public string IconHTTP {
            get => iconHTTP; set => iconHTTP = value;
        }

        [JsonProperty("id")]
        public string Id {
            get => id; set => id = value;
        }

        [JsonProperty("name")]
        public string Name {
            get => name; set => name = value;
        }

        [JsonProperty("opening_hours")]
        public OpeningHours OpeningHours {
            get => openingHours; set => openingHours = value;
        }

        [JsonProperty("reference")]
        public string Reference {
            get => reference; set => reference = value;
        }

        [JsonProperty("types")]
        public List<string> Types {
            get => types; set => types = value;
        }

        [JsonProperty("vicinity")]
        public string Vicinity {
            get => vicinity; set => vicinity = value;
        }

        [JsonProperty("geometry")]
        public Geometry Geometry {
            get => geometry; set => geometry = value;
        }

        [JsonProperty("photos")]
        public List<Photo> Photos {
            get => photos; set => photos = value;
        }

        [JsonProperty("place_id")]
        public string Place_id {
            get => place_id; set => place_id = value;
        }

        [JsonProperty("scope")]
        public string Scope {
            get => scope; set => scope = value;
        }

        [JsonProperty("alt_ids")]
        public List<Id> Alternate_ids {
            get => alternate_ids; set => alternate_ids = value;
        }

        public NearbySearchResult(String iconHTTP, String id, String name, OpeningHours openingHours, String reference,
            List<String> types, String vicinity, Geometry geometry, List<Photo> photos, String place_id,
            String scope, List<Id> alternate_ids) {
            this.IconHTTP = iconHTTP;
            this.Id = id;
            this.Name = name;
            this.OpeningHours = openingHours;
            this.Reference = reference;
            this.Types = types;
            this.Vicinity = vicinity;
            this.Geometry = geometry;
            this.Photos = photos;
            this.Place_id = place_id;
            this.Scope = scope;
            this.Alternate_ids = alternate_ids;
        }
    }
}
