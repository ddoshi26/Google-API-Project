using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.Places {
    public class PlacesDetail {
        private static HttpClient httpClient;
        private static String APIKey;

        public PlacesDetail(GoogleCloudClassSetup setup) {
            httpClient = new HttpClient();

            // Since this is the first time we use the httpClient, we need to intialize its base address
            httpClient.BaseAddress = new Uri(setup.getAPIUrl("PLACES_API_URL").ToString());
            APIKey = setup.getAPIKey();
        }

        public void UpdateURL(GoogleCloudClassSetup setup) {
            httpClient.BaseAddress = new Uri(setup.getAPIUrl("PLACES_API_URL").ToString());
        }

        public void UpdateKey(GoogleCloudClassSetup setup) {
            APIKey = setup.getAPIKey();
        }

        /*
         * Method: GetPlacesDetail
         * 
         * Description: This method can be used to query the Places API for details regarding places with a
         *   specific place_id. The place_id is usually obtained from the response of a Place Search function.
         * 
         * Parameters:
         *   - place_id (String): A String identifier that uniquely identifies a place. This is returned as part
         *       of the response from the Place Search functions. For more details about place_id: 
         *       https://developers.google.com/places/web-service/place-id
         * 
         *   - APIKey (String): Implicity required paramter which should be set through the constructor when
         *       creating an object of this class. For more details about the Google API Key please see:
         *       https://developers.google.com/places/web-service/get-api-key
         * 
         * Return: The method returns a tuple of two items. The first is an object of PlacesDetailResponse
         *   which contains all available details for the place. The second element is a ResponseStatus object
         *   indicating the status of the query along with the appropiate HTTP code. The tuple wrapped in a Task<>
         *   because the method makes Asynchronous HTTP requests to the Places API.
         */
        public async Task<Tuple<PlacesDetailResponse, ResponseStatus>> GetPlaceDetails(String place_id) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<PlacesDetailResponse, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (BasicFunctions.isEmpty(place_id)) {
                return new Tuple<PlacesDetailResponse, ResponseStatus>(null, PlacesStatus.MISSING_PLACE_ID);
            }

            // Creating the HTTP query url
            String HTTP_query = $"details/json?placeid={place_id}&key={APIKey}";

            // Setting up the request header to indicate that the request body will be in json
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Making an asynchronous HTTP GET request to the Places API and collecting the output
            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            Console.WriteLine(response_str);

            /* 
             * Here we do a two-step hop again to achieve the appropriate return value:
             *   We use the response string (response_str) from above and attempt to convert it back from json to 
             *   NearbySearchResultList, the expected return object for a successful query. This produces one of 
             *   two possibilities:
             *   1. If the response string is not a json of the NearbySearchResultList class, then we either get 
             *      a JsonSerializationException or an empty list. In this case we print out the response and 
             *      return null (Will improve this to return an appropriate error code).
             *   2. If the response string is as expected a json of NearbySearchResultList, then things go 
             *      smoothly and we return that.
             */
            if (response.IsSuccessStatusCode) {
                try {
                    PlacesDetailResponse resultList = JsonConvert.DeserializeObject<PlacesDetailResponse>(response_str);
                    if (!resultList.Status.Equals("OK")) {
                        // If the response status from the API is not OK, then we try to return the most appropriate Error
                        ResponseStatus status = PlacesStatus.ProcessErrorMessage(resultList.Status, resultList.Error_message);
                        return new Tuple<PlacesDetailResponse, ResponseStatus>(null, status);
                    }
                    else if (resultList.Result == null) {
                        // If the response provides an empty response set, then we return the ZERO_RESULTS (204) error
                        return new Tuple<PlacesDetailResponse, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<PlacesDetailResponse, ResponseStatus>(resultList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    // If the deserialization of the response fails, then we return an error
                    Console.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<PlacesDetailResponse, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                // If the response status from the API is not a success, then we return an error using the data returned
                return new Tuple<PlacesDetailResponse, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }


        /*
         * Method: GetPlacesDetailWithOptions
         * 
         * Description: This method can be used to query the Places API for details regarding places with a
         *   specific place_id. The optional parameters can be used to determine the language of the response
         *   as well as what infromation is returned. If no fields are provided, then the query returns all
         *   available fields.
         * 
         * Parameters:
         *   - place_id (String): A String identifier that uniquely identifies a place. This is returned as part
         *       of the response from the Place Search functions. For more details about place_id: 
         *       https://developers.google.com/places/web-service/place-id
         * 
         *   - region_code (String): This is an OPTIONAL parameter, that indicates the region code, specified as a
         *       ccTLD format. This is used to influence the query's results but relevant results outside the
         *       region may also be included.
         *   - fields (List<PlacesDetailFields>): OPTIONAL parameter. This is a list of details you wish to get
         *       about the places that match the query. If the list is empty or null, then the Places API will
         *       return all available details by default.
         *   - language_code (String): OPTIONAL parameter indicating the language in which results will be returned.
         *       By default this is set to English. List of supported languages and their codes: 
         *       https://developers.google.com/maps/faq#languagesupport
         *       
         *   - APIKey (String): Implicity required paramter which should be set through the constructor when
         *       creating an object of this class. For more details about the Google API Key please see:
         *       https://developers.google.com/places/web-service/get-api-key
         * 
         * Return: The method returns a tuple of two items. The first is an object of PlacesDetailResponse
         *   which contains all available details for the place. The second element is a ResponseStatus object
         *   indicating the status of the query along with the appropiate HTTP code. The tuple wrapped in a Task<>
         *   because the method makes Asynchronous HTTP requests to the Places API.
         */
        public async Task<Tuple<PlacesDetailResponse, ResponseStatus>> GetPlaceDetailsWithOptions(String place_id,
            String region_code = "", String language_code = "", String session_token = "", List<PlacesDetailFields> fields = null) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<PlacesDetailResponse, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (BasicFunctions.isEmpty(place_id)) {
                return new Tuple<PlacesDetailResponse, ResponseStatus>(null, PlacesStatus.MISSING_PLACE_ID);
            }

            // Creating the HTTP query url
            String HTTP_query = $"details/json?placeid={place_id}";

            // Appending any optional fields that are set
            if (!BasicFunctions.isEmpty(region_code)) {
                HTTP_query += $"&region={region_code}";
            }
            if (!BasicFunctions.isEmpty(language_code)) {
                HTTP_query += $"&language={language_code}";
            }
            if (!BasicFunctions.isEmpty(session_token)) {
                HTTP_query += $"&sessiontoken={session_token}";
            }
            if (fields != null && fields.Count != 0) {
                HTTP_query += $"&fields={BasicFunctions.getPlacesDetailFieldsListString(fields)}";
            }
            HTTP_query += $"&key={APIKey}";

            // Setting up the request header to indicate that the request body will be in json
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Making an asynchronous HTTP GET request to the Places API and collecting the output
            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();

            // Similar two-step hop as in prior functions
            if (response.IsSuccessStatusCode) {
                try {
                    PlacesDetailResponse resultList = JsonConvert.DeserializeObject<PlacesDetailResponse>(response_str);
                    if (!resultList.Status.Equals("OK")) {
                        // If the response status from the API is not OK, then we try to return the most appropriate Error
                        ResponseStatus status = PlacesStatus.ProcessErrorMessage(resultList.Status, resultList.Error_message);
                        return new Tuple<PlacesDetailResponse, ResponseStatus>(null, status);
                    }
                    else if (resultList.Result == null) {
                        // If the response provides an empty response set, then we return the ZERO_RESULTS (204) error
                        return new Tuple<PlacesDetailResponse, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<PlacesDetailResponse, ResponseStatus>(resultList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    // If the deserialization of the response fails, then we return an error
                    Debug.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<PlacesDetailResponse, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                // If the response status from the API is not a success, then we return an error using the data returned
                return new Tuple<PlacesDetailResponse, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }
    }
}
