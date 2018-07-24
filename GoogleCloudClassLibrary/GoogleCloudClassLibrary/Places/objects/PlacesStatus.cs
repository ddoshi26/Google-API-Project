using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.Places {
    public class PlacesStatus {
        public static readonly ResponseStatus OK = new ResponseStatus(200, "OK: Request was ok");
        public static readonly ResponseStatus ZERO_RESULTS = new ResponseStatus(204, "No Content: Zero results found for the given query");

        public static readonly ResponseStatus INVALID_REQUEST = new ResponseStatus(400, "Invalid Request");
        public static readonly ResponseStatus INVALID_RADIUS = new ResponseStatus(400, "Invalid Request: Radius should be greater than 0 and less than 50,000 meters");
        public static readonly ResponseStatus TOO_FEW_PARAMETERS = new ResponseStatus(400, "Invalid Request: At least one of keyword or type must be provided for this query");
        public static readonly ResponseStatus MISSING_HEIGHT_WIDTH = new ResponseStatus(400, "Invalid Request: At least one of maximum height or maximum width should be provided");
        public static readonly ResponseStatus INVALID_FILE_LOCATION = new ResponseStatus(400, "Invalid Request: File location provided was invalid");

        public static readonly ResponseStatus MISSING_API_KEY = new ResponseStatus(401, "Unauthorized: Google API Key is missing");
        public static readonly ResponseStatus INVALID_API_KEY = new ResponseStatus(401, "Unauthorized: The provided API Key is invalid");

        public static readonly ResponseStatus MISSING_QUERY = new ResponseStatus(422, "Unprocessable Entity: Missing query string");
        public static readonly ResponseStatus MISSING_PLACE_ID = new ResponseStatus(422, "Unprocessable Entity: Missing place_id parameter");
        public static readonly ResponseStatus MISSING_LOCATION = new ResponseStatus(422, "Unprocessable Entity: Missing location parameter");
        public static readonly ResponseStatus MISSING_PAGE_TOKEN = new ResponseStatus(422, "Unprocessable Entity: Missing next page token");
        public static readonly ResponseStatus MISSING_PHOTO_REFERENCE = new ResponseStatus(422, "Unprocessable Entity: Missing photo reference");
        public static readonly ResponseStatus MISSING_FILE_DESTINATION = new ResponseStatus(422, "Unprocessable Entity: Missing destination file path");

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
