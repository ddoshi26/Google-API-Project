using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.Places {
    public class FindPlaceCandidates {
        private String place_id;
        private String formatted_address;
        private Geometry geometry;
        private String id;
        private String icon;
        private String name;
        private PlusCode plusCode;
        private List<String> types;
        private Boolean permanentlyClosed;
        private OpeningHours openingHours;
        private List<Photo> photos;
        private double rating;
        private int priceLevel;

        [JsonProperty("place_id")]
        public string Place_id {
            get => place_id; set => place_id = value;
        }

        [JsonProperty("formatted_address")]
        public string Formatted_address {
            get => formatted_address; set => formatted_address = value;
        }

        [JsonProperty("geometry")]
        public Geometry Geometry {
            get => geometry; set => geometry = value;
        }

        [JsonProperty("icon")]
        public string Icon {
            get => icon;
            set => icon = value;
        }

        [JsonProperty("id")]
        public string Id {
            get => id;
            set => id = value;
        }

        [JsonProperty("name")]
        public string Name {
            get => name; set => name = value;
        }

        [JsonProperty("plus_code")]
        public PlusCode PlusCode {
            get => plusCode;
            set => plusCode = value;
        }

        [JsonProperty("types")]
        public List<string> Types {
            get => types;
            set => types = value;
        }

        [JsonProperty("permanently_closed")]
        public bool PermanentlyClosed {
            get => permanentlyClosed;
            set => permanentlyClosed = value;
        }

        [JsonProperty("opening_hours")]
        public OpeningHours OpeningHours {
            get => openingHours; set => openingHours = value;
        }

        [JsonProperty("photos")]
        public List<Photo> Photos {
            get => photos; set => photos = value;
        }

        [JsonProperty("rating")]
        public double Rating {
            get => rating; set => rating = value;
        }

        [JsonProperty("price_level")]
        public int PriceLevel {
            get => priceLevel; set => priceLevel = value;
        }

        public FindPlaceCandidates(String formatted_address, Geometry geometry, String icon, String id,
            String name, PlusCode plusCode, List<String> types, OpeningHours openingHours, List<Photo> photos,
            double rating, Boolean permanentlyClosed, int priceLevel) {
            Formatted_address = formatted_address;
            Geometry = geometry;
            Icon = icon;
            Id = id;
            Name = name;
            PlusCode = plusCode;
            Types = types;
            OpeningHours = openingHours;
            Photos = photos;
            Rating = rating;
            PermanentlyClosed = permanentlyClosed;
            PriceLevel = priceLevel;
        }
    }
}
