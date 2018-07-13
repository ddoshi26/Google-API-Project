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

        public static readonly ResponseStatus MISSING_API_KEY = new ResponseStatus(401, "Unauthorized: Google API Key is missing");
        public static readonly ResponseStatus INVALID_API_KEY = new ResponseStatus(401, "Unauthorized: The provided API Key is invalid");

        public static readonly ResponseStatus MISSING_QUERY = new ResponseStatus(422, "Unprocessable Entity: Missing query string");
        public static readonly ResponseStatus MISSING_LOCATION = new ResponseStatus(422, "Unprocessable Entity: Missing location parameter");

        public static readonly ResponseStatus INTERNAL_SERVER_ERROR = new ResponseStatus(500, "Internal Server Error");
        public static readonly ResponseStatus DESERIALIZATION_ERROR = new ResponseStatus(500, "Internal Server Error: Endpoint returned an unrecognizable response");

        public static ResponseStatus processErrorMessage(FindPlacesCandidateList candidateList) {
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
                return new ResponseStatus(400, candidateList.Error_message);
            }
        }
    }
}
