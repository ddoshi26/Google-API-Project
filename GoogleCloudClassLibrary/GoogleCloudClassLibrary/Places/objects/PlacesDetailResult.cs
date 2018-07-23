using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.Places {
    public class PlacesDetailResult {
        private List<AddressComponent> addressComponents;
        private String adrAddress;
        private String formattedAddress;
        private Geometry geometry;
        private String icon;
        private String id;
        private String name;
        private String placeId;
        private PlusCode plusCode;
        private String reference;
        private String scope;
        private List<String> types;
        private String url;
        private double utcOffset;
        private String vicinity;
        private OpeningHours openingHours;
        private Boolean permanentlyClosed;
        private List<Photo> photos;
        private int priceLevel;
        private double rating;
        private String formattedPhoneNumber;
        private String internationalPhoneNumber;
        private List<Review> reviews;
        private String website;

        [JsonProperty("address_components")]
        public List<AddressComponent> AddressComponents {
            get => addressComponents;
            set => addressComponents = value;
        }

        [JsonProperty("adr_address")]
        public string AdrAddress {
            get => adrAddress;
            set => adrAddress = value;
        }

        [JsonProperty("formatted_address")]
        public string FormattedAddress {
            get => formattedAddress;
            set => formattedAddress = value;
        }

        [JsonProperty("geometry")]
        public Geometry Geometry {
            get => geometry;
            set => geometry = value;
        }

        [JsonProperty("icon")]
        public string Icon {
            get => icon;
            set => icon = value;
        }

        [JsonProperty("name")]
        public string Name {
            get => name;
            set => name = value;
        }

        [JsonProperty("place_id")]
        public string PlaceId {
            get => placeId;
            set => placeId = value;
        }

        [JsonProperty("plus_code")]
        public PlusCode PlusCode {
            get => plusCode;
            set => plusCode = value;
        }

        [JsonProperty("reference")]
        public string Reference {
            get => reference;
            set => reference = value;
        }

        [JsonProperty("scope")]
        public string Scope {
            get => scope;
            set => scope = value;
        }

        [JsonProperty("types")]
        public List<string> Types {
            get => types;
            set => types = value;
        }

        [JsonProperty("url")]
        public string Url {
            get => url;
            set => url = value;
        }

        [JsonProperty("utc_offset")]
        public double UtcOffset {
            get => utcOffset;
            set => utcOffset = value;
        }

        [JsonProperty("vicinity")]
        public string Vicinity {
            get => vicinity;
            set => vicinity = value;
        }

        [JsonProperty("id")]
        public string Id {
            get => id;
            set => id = value;
        }

        [JsonProperty("opening_hours")]
        public OpeningHours OpeningHours {
            get => openingHours;
            set => openingHours = value;
        }

        [JsonProperty("permanently_closed")]
        public bool PermanentlyClosed {
            get => permanentlyClosed;
            set => permanentlyClosed = value;
        }

        [JsonProperty("photos")]
        public List<Photo> Photos {
            get => photos;
            set => photos = value;
        }

        [JsonProperty("priceLevel")]
        public int PriceLevel {
            get => priceLevel;
            set => priceLevel = value;
        }

        [JsonProperty("rating")]
        public double Rating {
            get => rating;
            set => rating = value;
        }

        [JsonProperty("formatted_phone_number")]
        public string FormattedPhoneNumber {
            get => formattedPhoneNumber;
            set => formattedPhoneNumber = value;
        }

        [JsonProperty("international_phone_number")]
        public string InternationalPhoneNumber {
            get => internationalPhoneNumber;
            set => internationalPhoneNumber = value;
        }

        [JsonProperty("reviews")]
        public List<Review> Reviews {
            get => reviews;
            set => reviews = value;
        }

        [JsonProperty("website")]
        public string Website {
            get => website;
            set => website = value;
        }

        public PlacesDetailResult(List<AddressComponent> addressComponents, string adrAddress, 
            string formattedAddress, Geometry geometry, string icon, string name, string placeId, String id,
            PlusCode plusCode, string reference, string scope, List<string> types, string url, double utcOffset,
            string vicinity, OpeningHours openingHours, bool permanentlyClosed, List<Photo> photos) {
            AddressComponents = addressComponents;
            AdrAddress = adrAddress;
            FormattedAddress = formattedAddress;
            Geometry = geometry;
            Icon = icon;
            Name = name;
            Id = id;
            PlaceId = placeId;
            PlusCode = plusCode;
            Reference = reference;
            Scope = scope;
            Types = types;
            Url = url;
            UtcOffset = utcOffset;
            Vicinity = vicinity;
            OpeningHours = openingHours;
            PermanentlyClosed = permanentlyClosed;
            Photos = photos;
        }
    }
}
