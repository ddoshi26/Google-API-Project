using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.Places {
    public class Location {
        private Double lat;
        private Double lng;

        public double Lat {
            get => lat; set => lat = value;
        }
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

        public double Height {
            get => height; private set => height = value;
        }
        public double Width {
            get => width; private set => width = value;
        }
        public string Photo_ref {
            get => photo_ref; private set => photo_ref = value;
        }
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

        public string Place_id {
            get => place_id; set => place_id = value;
        }
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

    public class NearbySearchResult {
        private Location location;
        private String iconHTTP;
        private String id;
        private String name;

        private Boolean open_now;

        private List<Photo> photos;

        private String place_id;
        private String scope;

        private List<Id> alternate_ids;

        private String reference;
        private List<String> types;
        private String vicinity;

        public string IconHTTP {
            get => iconHTTP; set => iconHTTP = value;
        }
        public string Id {
            get => id; set => id = value;
        }
        public string Name {
            get => name; set => name = value;
        }
        public bool Open_now {
            get => open_now; set => open_now = value;
        }
        public string Reference {
            get => reference; set => reference = value;
        }
        public List<string> Types {
            get => types; set => types = value;
        }
        public string Vicinity {
            get => vicinity; set => vicinity = value;
        }
        public Location Location {
            get => location; set => location = value;
        }
        public List<Photo> Photos {
            get => photos; set => photos = value;
        }
        public string Place_id {
            get => place_id; set => place_id = value;
        }
        public string Scope {
            get => scope; set => scope = value;
        }
        public List<Id> Alternate_ids {
            get => alternate_ids; set => alternate_ids = value;
        }

        public NearbySearchResult(String iconHTTP, String id, String name, bool open_now, String reference,
            List<String> types, String vicinity, Location location, List<Photo> photos, String place_id,
            String scope, List<Id> alternate_ids) {
            this.IconHTTP = iconHTTP;
            this.Id = id;
            this.Name = name;
            this.Open_now = open_now;
            this.Reference = reference;
            this.Types = types;
            this.Vicinity = vicinity;
            this.Location = location;
            this.Photos = photos;
            this.Place_id = place_id;
            this.Scope = scope;
            this.Alternate_ids = alternate_ids;
        }
    }

    public class NearbySearchResultList {
        private List<NearbySearchResult> results;

        public List<NearbySearchResult> Results {
            get => results; set => results = value;
        }

        public NearbySearchResultList(List<NearbySearchResult> results) {
            this.Results = results;
        }
    }

    public class FindPlaceCandidates {
        private String place_id;
        private String formatted_address;
        private Location location;
        private String name;
        private Boolean open_now;
        private List<Photo> photos;
        private double rating;

        public string Place_id {
            get => place_id; set => place_id = value;
        }

        public string Formatted_address {
            get => formatted_address; set => formatted_address = value;
        }
        public Location Location {
            get => location; set => location = value;
        }
        public string Name {
            get => name; set => name = value;
        }
        public bool Open_now {
            get => open_now; set => open_now = value;
        }
        public List<Photo> Photos {
            get => photos; set => photos = value;
        }
        public double Rating {
            get => rating; set => rating = value;
        }

        public FindPlaceCandidates(String formatted_address, Location location, String name, bool open_now,
            List<Photo> photos, double rating) {
            Formatted_address = formatted_address;
            Location = location;
            Name = name;
            Open_now = open_now;
            Photos = photos;
            Rating = rating;
        }
    }

    public class FindPlacesCandidateList {
        private List<FindPlaceCandidates> candidates;

        public List<FindPlaceCandidates> Candidates { get => candidates; set => candidates = value; }

        public FindPlacesCandidateList(List<FindPlaceCandidates> candidates) {
            Candidates = candidates;
        }
    }
}
