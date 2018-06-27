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
        private String[] html_attributions;

        public double Height {
            get => height; private set => height = value;
        }
        public double Width {
            get => width; private set => width = value;
        }
        public string Photo_ref {
            get => photo_ref; private set => photo_ref = value;
        }
        public string[] Html_attributions {
            get => html_attributions; private set => html_attributions = value;
        }

        public Photo() {
            this.Height = 0.00;
            this.Width = 0.00;
            this.Photo_ref = "";
            this.Html_attributions = null;
        }

        public Photo(Double height, Double width, String photo_ref, String[] html_atts) {
            this.Height = height;
            this.Width = width;
            this.Photo_ref = photo_ref;
            this.Html_attributions = html_atts;
        }
    }

    class Id {
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

        private Photo[] photos;

        private Id place_id;
        private Id[] alternate_ids;

        private String reference;
        private String[] types;
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
        public string[] Types {
            get => types; set => types = value;
        }
        public string Vicinity {
            get => vicinity; set => vicinity = value;
        }
        internal Location Location {
            get => location; set => location = value;
        }
        internal Photo[] Photos {
            get => photos; set => photos = value;
        }
        internal Id Place_id {
            get => place_id; set => place_id = value;
        }
        internal Id[] Alternate_ids {
            get => alternate_ids; set => alternate_ids = value;
        }
    }

    public class FindPlaceCandidates {
        private String formatted_address;
        private Location location;
        private String name;
        private Boolean open_now;
        private Photo[] photos;
        private double rating;

        public string Formatted_address {
            get => formatted_address; set => formatted_address = value;
        }
        internal Location Location {
            get => location; set => location = value;
        }
        public string Name {
            get => name; set => name = value;
        }
        public bool Open_now {
            get => open_now; set => open_now = value;
        }
        internal Photo[] Photos {
            get => photos; set => photos = value;
        }
        public double Rating {
            get => rating; set => rating = value;
        }

        public FindPlaceCandidates(String formatted_address, Location location, String name, bool open_now,
            Photo[] photos, double rating) {
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
