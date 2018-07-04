﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.Places {
    public class Location {
        private Double lat;
        private Double lng;

        [JsonProperty("lat")]
        public double Lat {
            get => lat; set => lat = value;
        }

        [JsonProperty("lng")]
        public double Lng {
            get => lng; set => lng = value;
        }

        public Location() {
            this.Lat = 0.00;
            this.Lng = 0.00;
        }

        public Location(Double lat, Double lng) {
            this.Lat = lat;
            this.Lng = lng;
        }
    }

    public class Photo {
        private Double height;
        private Double width;
        private String photo_ref;
        private List<String> html_attributions;

        [JsonProperty("height")]
        public double Height {
            get => height; private set => height = value;
        }

        [JsonProperty("width")]
        public double Width {
            get => width; private set => width = value;
        }

        [JsonProperty("photo_reference")]
        public string Photo_ref {
            get => photo_ref; private set => photo_ref = value;
        }

        [JsonProperty("html_attributions")]
        public List<string> Html_attributions {
            get => html_attributions; private set => html_attributions = value;
        }

        public Photo() {
            this.Height = 0.00;
            this.Width = 0.00;
            this.Photo_ref = "";
            this.Html_attributions = null;
        }

        public Photo(Double height, Double width, String photo_ref, List<String> html_atts) {
            this.Height = height;
            this.Width = width;
            this.Photo_ref = photo_ref;
            this.Html_attributions = html_atts;
        }
    }

    public class Id {
        private String place_id;
        private String scope;

        [JsonProperty("place_id")]
        public string Place_id {
            get => place_id; set => place_id = value;
        }

        [JsonProperty("scope")]
        public string Scope {
            get => scope; set => scope = value;
        }

        public Id() {
            this.Place_id = "";
            this.Scope = "";
        }

        public Id(String place_id, String scope) {
            this.Place_id = place_id;
            this.Scope = scope;
        }
    }

    public class OpeningHours {
        private Boolean openNow;

        [JsonProperty("open_now")]
        public bool OpenNow {
            get => openNow;
            set => openNow = value;
        }

        public OpeningHours(bool openNow) {
            OpenNow = openNow;
        }
    }

    public class Geometry {
        private Location location;

        [JsonProperty("location")]
        public Location Location {
            get => location;
            set => location = value;
        }

        public Geometry(Location location) {
            Location = location;
        }
    }

    public class Viewport {
        private Location northEast;
        private Location southWest;

        [JsonProperty("northeast")]
        public Location NorthEast {
            get => northEast;
            set => northEast = value;
        }

        [JsonProperty("southwest")]
        public Location SouthWest {
            get => southWest;
            set => southWest = value;
        }

        public Viewport(Location northEast, Location southWest) {
            NorthEast = northEast;
            SouthWest = southWest;
        }
    }

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

    public class NearbySearchResultList {
        private List<String> html_attributions;
        private List<NearbySearchResult> results;

        [JsonProperty("results")]
        public List<NearbySearchResult> Results {
            get => results; set => results = value;
        }

        [JsonProperty("html_attributions")]
        public List<String> Html_attributions {
            get => html_attributions;
            set => html_attributions = value;
        }

        public NearbySearchResultList(List<NearbySearchResult> results, List<String> html_attributions) {
            this.Results = results;
            this.Html_attributions = html_attributions;
        }
    }

    public class FindPlaceCandidates {
        private String place_id;
        private String formatted_address;
        private Geometry geometry;
        private Viewport viewport;
        private String name;
        private OpeningHours openingHours;
        private List<Photo> photos;
        private double rating;

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

        [JsonProperty("viewport")]
        public Viewport Viewport {
            get => viewport; set => viewport = value;
        }

        [JsonProperty("name")]
        public string Name {
            get => name; set => name = value;
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

        public FindPlaceCandidates(String formatted_address, Geometry geometry, String name, OpeningHours openingHours,
            Viewport viewport, List<Photo> photos, double rating) {
            Formatted_address = formatted_address;
            Geometry = geometry;
            Name = name;
            OpeningHours = openingHours;
            Photos = photos;
            Rating = rating;
        }
    }

    public class FindPlacesCandidateList {
        private List<FindPlaceCandidates> candidates;
        private String status;

        [JsonProperty("candidates")]
        public List<FindPlaceCandidates> Candidates { get => candidates; set => candidates = value; }

        [JsonProperty("status")]
        public String Status {
            get => status;
            set => status = value;
        }

        public FindPlacesCandidateList(List<FindPlaceCandidates> candidates, String status) {
            Candidates = candidates;
            Status = status;
        }
    }
}
