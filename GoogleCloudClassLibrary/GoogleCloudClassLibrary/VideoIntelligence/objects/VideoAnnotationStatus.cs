using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class VideoAnnotationStatus {
        public static readonly ResponseStatus OK = new ResponseStatus(200, "OK: Request was ok");
        public static readonly ResponseStatus ZERO_RESULTS = new ResponseStatus(204, "No Content: Zero results found for the given query");

        public static readonly ResponseStatus INVALID_REQUEST = new ResponseStatus(400, "Invalid Request");
        public static readonly ResponseStatus TOO_FEW_PARAMETERS = new ResponseStatus(400, "Invalid Request: Either inputUri or inputContent must be provided for this query");
        public static readonly ResponseStatus TOO_MANY_PARAMETERS = new ResponseStatus(400, "Invalid Request: Only one of inputUri or inputContent must be provided for this query");
        public static readonly ResponseStatus MISSING_ANNOTATION_FIELD = new ResponseStatus(400, "Invalid Request: None of the three annotations fields was set to true. At least one must be set to true");

        public static readonly ResponseStatus MISSING_API_KEY = new ResponseStatus(401, "Unauthorized: Google API Key is missing");
        public static readonly ResponseStatus INVALID_API_KEY = new ResponseStatus(401, "Unauthorized: The provided API Key is invalid");

        public static readonly ResponseStatus INTERNAL_SERVER_ERROR = new ResponseStatus(500, "Internal Server Error");
        public static readonly ResponseStatus DESERIALIZATION_ERROR = new ResponseStatus(500, "Internal Server Error: Endpoint returned an unrecognizable response");

        public static ResponseStatus processErrorMessage(string status, string message) {
            if (status.Equals("OK")) {
                return OK;
            }
            else if (status.Equals("REQUEST_DENIED")) {
                if (message.Contains("API key is invalid")) {
                    return INVALID_API_KEY;
                }
                return null;
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
