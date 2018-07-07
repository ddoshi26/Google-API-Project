using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.Places {
    public class Status {
        private int code;
        private String description;

        public int Code {
            get => code;
            set => code = value;
        }

        public String Description {
            get => description;
            set => description = value;
        }

        public Status(int code, String description) {
            this.Code = code;
            this.Description = description;
        }
    }

    public class PlacesStatus {
        public static readonly Status OK = new Status(200, "OK: Request was ok");
        public static readonly Status ZERO_RESULTS = new Status(204, "No Content: Zero results found for the given query");

        public static readonly Status INVALID_REQUEST = new Status(400, "Invalid Request");
        public static readonly Status INVALID_RADIUS = new Status(400, "Invalid Request: Radius should be greater than 0 and less than 50,000 meters");

        public static readonly Status MISSING_API_KEY = new Status(401, "Unauthorized: Google API Key is missing");
        public static readonly Status INVALID_API_KEY = new Status(401, "Unauthorized: The provided API Key is invalid");

        public static readonly Status MISSING_QUERY = new Status(422, "Unprocessable Entity: Missing query string");
        public static readonly Status MISSING_LOCATION = new Status(422, "Unprocessable Entity: Missing location parameter");
        
        public static Status processErrorMessage(FindPlacesCandidateList candidateList) {
            if (candidateList.Status.Equals("OK")) {
                return OK;
            }
            else if (candidateList.Status.Equals("REQUEST_DENIED")) {
                if (candidateList.Error_message.Contains("API key is invalid")) {
                    return INVALID_API_KEY;
                }
                return null;
            }
            else if (candidateList.Status.Equals("INVALID_REQUEST")) {
                return MISSING_QUERY;
            }
            else if (candidateList.Status.Equals("ZERO_RESULTS")) {
                return ZERO_RESULTS;
            }
            else {
                return new Status(400, candidateList.Error_message);
            }
        }
    }
}
