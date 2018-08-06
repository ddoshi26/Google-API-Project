using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class ImageAnnotationStatus {
        public static readonly ResponseStatus OK = new ResponseStatus(200, "OK: Request was ok");

        public static readonly ResponseStatus ZERO_RESULTS = new ResponseStatus(204, "No Content: Zero results found for the given query");

        public static readonly ResponseStatus INVALID_REQUEST = new ResponseStatus(400, "Invalid Request");
        public static readonly ResponseStatus MISSING_REQUEST_LIST = new ResponseStatus(400, "Invalid Request: AnnotateImageRequestList is empty or null. It should have at least one request");

        public static readonly ResponseStatus MISSING_API_KEY = new ResponseStatus(401, "Unauthorized: Google API Key is missing");
        public static readonly ResponseStatus INVALID_API_KEY = new ResponseStatus(401, "Unauthorized: The provided API Key is invalid");

        public static readonly ResponseStatus INTERNAL_SERVER_ERROR = new ResponseStatus(500, "Internal Server Error");
        public static readonly ResponseStatus DESERIALIZATION_ERROR = new ResponseStatus(500, "Internal Server Error: Endpoint returned an unrecognizable response");

        public static ResponseStatus ProcessErrorMessage(string status, string message) {
            message = message.ToLower();
            if (status.Equals("OK")) {
                return OK;
            }
            else if (status.Equals("REQUEST_DENIED") || message.Contains("api key not valid")) {
                return INVALID_API_KEY;
            }
            else if (status.Equals("INVALID_REQUEST")) {
                return INVALID_REQUEST;
            }
            else if (status.Equals("ZERO_RESULTS")) {
                return ZERO_RESULTS;
            }
            else {
                return new ResponseStatus(400, message);
            }
        }
    }
}
