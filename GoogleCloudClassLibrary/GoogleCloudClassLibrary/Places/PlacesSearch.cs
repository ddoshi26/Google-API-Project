using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net = System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Web = System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace GoogleCloudClassLibrary.Places {
    public class PlacesSearch {
        private static HttpClient httpClient;
        private static String APIKey;

        public PlacesSearch(GoogleCloudClassSetup setup) {
            httpClient = new HttpClient();

            // Since this is the first time we use the httpClient, we need to intialize its base address
            httpClient.BaseAddress = new Uri(setup.getAPIUrl("PLACES_SEARCH_API_URL").ToString());
            APIKey = setup.getAPIKey();
        }

        public void UpdateURL(GoogleCloudClassSetup setup) {
            httpClient.BaseAddress = new Uri(setup.getAPIUrl("PLACES_SEARCH_API_URL").ToString());
        }

        public void UpdateKey(GoogleCloudClassSetup setup) {
            APIKey = setup.getAPIKey();
        }

        // Find Places

        /*
         * Method: FindPlacesUsingTextQuery
         * 
         * Description: This method can be used to query the Places API for places with some specific matching 
         *   paramter. For phone numbers, please use the FindPlacesUsingPhoneNumber() method.
         * 
         * Parameters:
         *   - query (String): A string parameter that will be used to search through the Places API and find 
         *       places that contain matching information. This could be any collection of keywords that can 
         *       appropriately describe the information sought.
         *       
         *   - APIKey (String): Implicity required paramter which should be set through the constructor when
         *       creating an object of this class. For more details about the Google API Key please see:
         *       https://developers.google.com/places/web-service/get-api-key
         * 
         * Return: The method returns a tuple of two items. The first is a list of all the candidates that match
         *   the query provided. The second element is a ResponseStatus object indicating the status of the
         *   query along with the appropiate HTTP code. The tuple wrapped in a Task<> because the method makes
         *   Asynchronous HTTP requests to the Places API.
         */
        public async Task<Tuple<FindPlacesCandidateList, ResponseStatus>> FindPlacesUsingTextQuery(String query) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (BasicFunctions.isEmpty(query)) {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.MISSING_QUERY);
            }

            // Converting the query into a URL friendly version
            String processedQuery = BasicFunctions.processTextQuery(query);
            String HTTP_query = $"findplacefromtext/json?input={processedQuery}&inputtype=textquery&key={APIKey}";

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Making the asynchronous GET request to Places API and colleting the response
            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            
            /* 
             * Here we do a two-step hop to achieve the appropriate return value:
             *   We use the response string (response_str) from above and attempt to convert it back from json to 
             *   FindPlacesCandidateList, the expected return object for a successful query. This produces one of 
             *   two possibilities:
             *   1. If the response string is not a json of the FindPlacesCandidateList class, then we either get 
             *      a JsonSerializationException or an empty list. In this case we print out the response and 
             *      return null (Will improve this to return an appropriate error code).
             *   2. If the response string is as expected a json of FindPlacesCandidateList, then things go 
             *      smoothly and we return that.
             */
            if (response.IsSuccessStatusCode) {
                try {
                    FindPlacesCandidateList candidateList = JsonConvert.DeserializeObject<FindPlacesCandidateList>(response_str);
                    if (!candidateList.Status.Equals("OK")) {
                        // If the response status from the API is not OK, then we try to return the most appropriate Error
                        ResponseStatus status = PlacesStatus.processErrorMessage(candidateList.Status, candidateList.Error_message);
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, status);
                    }
                    else if (candidateList.Candidates.Count == 0) {
                        // If the response provides an empty response set, then we return the ZERO_RESULTS (204) error
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(candidateList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    // If the deserialization of the response fails, then we throw an error
                    Debug.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR); 
                }
            }
            else {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, new ResponseStatus((int)response.StatusCode, response.ReasonPhrase));
            }
        }

        /*
         * Method: FindPlaceWithPointLocationBias
         * 
         * Description: This method can be used to query the Places API for places with some specific matching 
         *   parameter located at a given location. This only works for text query. For phone number queries,
         *   please use Circular or Rectangular FindPlaces methods below. The API will prefer results that are
         *   closer to the given location.
         * 
         * Parameters:
         *   - query (String): A string parameter that will be used to search through the Places API and find 
         *       places that contain matching information. This could be any collection of keywords that can 
         *       appropriately describe the place sought.
         *   - location (Location): The coordinates of the point which will be used by the Places API to bias results.
         *   - fields (List<Fields>): OPTIONAL parameter. This is a list of details you wish to get about the places
         *       that match the query. If the list is empty or null, then the Places API will only return the place_id
         *       by default.
         *   - language_code (String): OPTIONAL parameter indicating the language in which results will be returned.
         *       By default this is set to English. List of supported languages and their codes: 
         *       https://developers.google.com/maps/faq#languagesupport
         *       
         *   - APIKey (String): Implicity required paramter which should be set through the constructor when
         *       creating an object of this class. For more details about the Google API Key please see:
         *       https://developers.google.com/places/web-service/get-api-key
         * 
         * Return: The method returns a tuple of two items. The first is an object of FindPlacesCandidateList
         *   which contains the candidates returned based on the query provided. The second element is a
         *   ResponseStatus object indicating the status of the query along with the appropiate HTTP code. The
         *   tuple wrapped in a Task<> because the method makes Asynchronous HTTP requests to the Places API.
         */
        public async Task<Tuple<FindPlacesCandidateList, ResponseStatus>> FindPlaceWithPointLocationBias(String query,
            Location location, List<Fields> fields = null, String language_code = "") {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (BasicFunctions.isEmpty(query)) {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.MISSING_QUERY);
            }
            if (location == null) {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.MISSING_LOCATION);
            }

            String processed_query = BasicFunctions.processTextQuery(query);

            // Converting the query into a URL friendly version
            String HTTP_query = $"findplacefromtext/json?input={processed_query}&inputtype={InputType.TEXTQUERY.ToString().ToLower()}" +
                $"&locationbias=point:{location.Lat},{location.Lng}";

            // We append the fields and language code if they are provided
            if (fields != null && fields.Count != 0) {
                HTTP_query += $"&fields={BasicFunctions.getFieldsListString(fields)}";
            }
            if (!BasicFunctions.isEmpty(language_code)) {
                HTTP_query += $"&language={language_code}";
            }
            HTTP_query += $"&key={APIKey}";
            
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Making the asynchronous GET request to Places API and colleting the response
            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            Console.WriteLine(response_str);

            if (response.IsSuccessStatusCode) {
                // Similar two-step hop as we have seen in prior functions
                try {
                    FindPlacesCandidateList candidateList = JsonConvert.DeserializeObject<FindPlacesCandidateList>(response_str);
                    if (!candidateList.Status.Equals("OK")) {
                        // If the response status from the API is not OK, then we try to return the most appropriate Error
                        ResponseStatus status = PlacesStatus.processErrorMessage(candidateList.Status, candidateList.Error_message);
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, status);
                    }
                    else if (candidateList.Candidates.Count == 0) {
                        // If the response provides an empty candidate set, then we return the ZERO_RESULTS (204) error
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        // If the response is deserialized as expected and contains one or more candidates, then we
                        // return the list along with an OK status
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(candidateList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    // If the deserialization of the response fails, then we return an error
                    Console.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                // If the response status from the API is not a success, then we return an error using the data returned
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }

        /*
         * Method: FindPlacesWithCircularLocationBias
         * 
         * Description: This method can be used to query the Places API for places with some specific matching 
         *   parameter located within a circular region. The API will prefer results that are within the region
         *   formed by the location point and the radius parameters.
         * 
         * Parameters:
         *   - query (String): A string parameter that will be used to search through the Places API and find 
         *       places that contain matching information. This could be any collection of keywords that can 
         *       appropriately describe the place sought.
         *   - inputType (InputType): THis identifies the type of input provided in the query. This can either be
         *       TEXTQUERY or PHONENUMBER.
         *   - location (Location): The coordinates of the point which will be used as teh center of the circular 
         *       search region by the Places API.
         *   - radius (double): Radius of the circular search region from the location point. This is in meters
         *      and the allowable range is between 0 and 50,000 meters (inclusive).
         *   - fields (List<Fields>): OPTIONAL parameter. This is a list of details you wish to get about the places
         *       that match the query. If the list is empty or null, then the Places API will only return the place_id
         *       by default.
         *   - language_code (String): OPTIONAL parameter indicating the language in which results will be returned.
         *       By default this is set to English. List of supported languages and their codes: 
         *       https://developers.google.com/maps/faq#languagesupport
         *       
         *   - APIKey (String): Implicity required paramter which should be set through the constructor when
         *       creating an object of this class. For more details about the Google API Key please see:
         *       https://developers.google.com/places/web-service/get-api-key
         * 
         * Return: The method returns a tuple of two items. The first is an object of FindPlacesCandidateList
         *   which contains the candidates returned based on the query provided. The second element is a
         *   ResponseStatus object indicating the status of the query along with the appropiate HTTP code. The
         *   tuple wrapped in a Task<> because the method makes Asynchronous HTTP requests to the Places API.
         */
        public async Task<Tuple<FindPlacesCandidateList, ResponseStatus>> FindPlacesWithCircularLocationBias(String query,
            InputType inputType, Location location, double radius, List<Fields> fields, String language_code = "") {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (BasicFunctions.isEmpty(query)) {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.MISSING_QUERY);
            }
            if (location == null) {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.MISSING_LOCATION);
            }
            if (radius <= 0 || radius > 50000) {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.INVALID_RADIUS);
            }

            String processed_query = BasicFunctions.processTextQuery(query);

            // Converting the query into a URL friendly version
            String HTTP_query = $"findplacefromtext/json?input={processed_query}&inputtype={inputType.ToString().ToLower()}" +
                $"&locationbias=circle:{radius}@{location.Lat},{location.Lng}";

            // We append any optional parameters that are set
            if (fields != null && fields.Count != 0) {
                HTTP_query += $"&fields={BasicFunctions.getFieldsListString(fields)}";
            }
            if (!BasicFunctions.isEmpty(language_code)) {
                HTTP_query += $"&language={language_code}";
            }
            HTTP_query += $"&key={APIKey}";

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Making the asynchronous GET request to Places API and colleting the response
            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            Console.WriteLine(response_str);

            // Similar two-step hop as we have seen in the previous function
            if (response.IsSuccessStatusCode) {
                // Similar two-step hop as we have seen in prior functions
                try {
                    FindPlacesCandidateList candidateList = JsonConvert.DeserializeObject<FindPlacesCandidateList>(response_str);
                    if (!candidateList.Status.Equals("OK")) {
                        // If the response status from the API is not OK, then we try to return the most appropriate Error
                        ResponseStatus status = PlacesStatus.processErrorMessage(candidateList.Status, candidateList.Error_message);
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, status);
                    }
                    else if (candidateList.Candidates.Count == 0) {
                        // If the response provides an empty response set, then we return the ZERO_RESULTS (204) error
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(candidateList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    // If the deserialization of the response fails, then we return an error
                    Debug.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                // If the response status from the API is not a success, then we return an error using the data returned
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }

        /*
         * Method: FindPlacesWithRectLocationBias
         * 
         * Description: This method can be used to query the Places API for places with some specific matching 
         *   parameter located within a rectangular region. The API will prefer results that are within the region
         *   formed by SouthWest and NorthEast corner points.
         * 
         * Parameters:
         *   - query (String): A string parameter that will be used to search through the Places API and find 
         *       places that contain matching information. This could be any collection of keywords that can 
         *       appropriately describe the place sought.
         *   - inputType (InputType): THis identifies the type of input provided in the query. This can either be
         *       TEXTQUERY or PHONENUMBER.
         *   - southWestCorner (Location): The coordinates of the SouthWest point  of the search rectangle.
         *   - northEastCorner (Location): The coordinates of the NorthEast point of the search rectangle.
         *   - fields (List<Fields>): OPTIONAL parameter. This is a list of details you wish to get about the places
         *       that match the query. If the list is empty or null, then the Places API will only return the place_id
         *       by default.
         *   - language_code (String): OPTIONAL parameter indicating the language in which results will be returned.
         *       By default this is set to English. List of supported languages and their codes: 
         *       https://developers.google.com/maps/faq#languagesupport
         * 
         *   - APIKey (String): Implicity required paramter which should be set through the constructor when
         *       creating an object of this class. For more details about the Google API Key please see:
         *       https://developers.google.com/places/web-service/get-api-key
         * 
         * Return: The method returns a tuple of two items. The first is an object of FindPlacesCandidateList
         *   which contains the candidates returned based on the query provided. The second element is a
         *   ResponseStatus object indicating the status of the query along with the appropiate HTTP code. The
         *   tuple wrapped in a Task<> because the method makes Asynchronous HTTP requests to the Places API.
         */
        public async Task<Tuple<FindPlacesCandidateList, ResponseStatus>> FindPlacesWithRectLocationBias(String query,
            InputType inputType, Location southWestCorner, Location northEastCorner, List<Fields> fields, 
            String language_code = "") {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (BasicFunctions.isEmpty(query)) {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.MISSING_QUERY);
            }
            if (southWestCorner == null || northEastCorner == null) {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.MISSING_LOCATION);
            }

            String processed_query = BasicFunctions.processTextQuery(query);

            // Converting the query into a URL friendly version
            String HTTP_query = $"findplacefromtext/json?input={processed_query}&inputtype={inputType.ToString().ToLower()}" +
                $"&locationbias=rectangle:{southWestCorner.Lat},{southWestCorner.Lng}|{northEastCorner.Lat},{northEastCorner.Lng}";

            // We append any optional parameters that are set
            if (fields != null && fields.Count != 0) {
                HTTP_query += $"&fields={BasicFunctions.getFieldsListString(fields)}";
            }
            if (!BasicFunctions.isEmpty(language_code)) {
                HTTP_query += $"&language={language_code}";
            }
            HTTP_query += $"&key={APIKey}";

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Making the asynchronous GET request to Places API and colleting the response
            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            
            // Similar two-step hop as we have seen in prior functions
            if (response.IsSuccessStatusCode) {
                try {
                    FindPlacesCandidateList candidateList = JsonConvert.DeserializeObject<FindPlacesCandidateList>(response_str);
                    if (!candidateList.Status.Equals("OK")) {
                        // If the response status from the API is not OK, then we try to return the most appropriate Error
                        ResponseStatus status = PlacesStatus.processErrorMessage(candidateList.Status, candidateList.Error_message);
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, status);
                    }
                    else if (candidateList.Candidates.Count == 0) {
                        // If the response provides an empty response set, then we return the ZERO_RESULTS (204) error
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(candidateList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    // If the deserialization of the response fails, then we return an error
                    Debug.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                // If the response status from the API is not a success, then we return an error using the data returned
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }

        // Nearby Search Requests

        /*
         * Method: GetNearbySearchResultsRankByProminence
         * 
         * Description: This method can be used to find places, ranked by prominence, within a circular region
         *   described by the parameters. The prominence based ranking is determined through a combination of factors 
         *   such as the popularity, frequency of search, and ranking in Google's index.
         * 
         * Parameters:
         *  - location (Location): This is the representation of a point (latitude, longtitude). This along with
         *      the radius will be used to create a circular search region.
         *  - radius (double): Radius of the circular search region from the location point. This is in meters
         *      and the allowable value is between 0 and 50,000 meters (inclusive).
         * 
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *       creating an object of this class. For more details about the Google API Key please see:
         *       https://developers.google.com/places/web-service/get-api-key
         * 
         * Return: The method returns a tuple of two items. The first is an object of NearbySearchResultList which
         *   contains all the places within the given circular search region. The list is sorted based on
         *   descending order of prominence (importance) with the most prominant places at the head of the list.
         *   The second element is a ResponseStatus object indicating the status of the query along with the
         *   appropiate HTTP code. The tuple is wrapped in a Task<> because the method makes Asynchronous HTTP 
         *   requests to the Places API.
         */
        public async Task<Tuple<NearbySearchResultList, ResponseStatus>> GetNearbySearchResultsRankByProminence(Location location,
            double radius) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (location == null) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_LOCATION);
            }
            if (radius < 0 || radius > 50000) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.INVALID_RADIUS);
            }

            // Creating the HTTP query url
            String HTTP_query = $"nearbysearch/json?location={location.Lat},{location.Lng}&radius={radius}&key={APIKey}";

            // Setting up the request header to indicate that the request body will be in json
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Making an asynchronous HTTP GET request to the Places API and collecting the output
            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            
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
                    NearbySearchResultList searchResultList = JsonConvert.DeserializeObject<NearbySearchResultList>(response_str);
                    if (!searchResultList.Status.Equals("OK")) {
                        // If the response status from the API is not OK, then we try to return the most appropriate Error
                        ResponseStatus status = PlacesStatus.processErrorMessage(searchResultList.Status, searchResultList.Error_message);
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, status);
                    }
                    else if (searchResultList.Results.Count == 0) {
                        // If the response provides an empty response set, then we return the ZERO_RESULTS (204) error
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<NearbySearchResultList, ResponseStatus>(searchResultList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    // If the deserialization of the response fails, then we return an error
                    Debug.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                // If the response status from the API is not a success, then we return an error using the data returned
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }

        /*
         * Method: GetNearbySearchResultsRankByProminenceWithOptions
         * 
         * Description: This method can be used to find places, ranked by prominence, within a circular region
         *   described by the parameters. The prominence based ranking is determined through a combination of factors 
         *   such as the popularity, frequency of search, and ranking in Google's index.
         * 
         * Parameters:
         *  - location (Location): This is the representation of a point (latitude, longtitude). This along with
         *      the radius will be used to create a circular search region.
         *  - radius (double): Radius of the circular search region from the location point. This is in meters
         *      and the allowable value is between 0 and 50,000 meters (inclusive).
         *      
         *  - open_now (boolean): This is an OPTIONAL parameter. If set to true, then the query will only return
         *      places in the search region which are open at the time of querying.
         *  - keyword (String): This OPTIONAL parameter will be matched against the content that Google has
         *      indexed for each place. Places within the search region with a match will be returned.
         *  - language_code (String): OPTIONAL parameter indicating the language in which results will be returned.
         *      By default this is set to English. List of supported languages and their codes: 
         *      https://developers.google.com/maps/faq#languagesupport
         *  - min_price and max_price (int): OPTIONAL parameters which indicates the desired price range of the
         *      places that will be returned. The min and max_price values should be between 0 (lowest) and 4 
         *      (highest), inclusive. The exact price indicated by a specific value varies based on region.
         *  - type (NearbySearchTypes): This an OPTIONAL parameter. It is used to restrict the results to a
         *      specific type only.
         * 
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *      creating an object of this class. For more details about the Google API Key please see:
         *      https://developers.google.com/places/web-service/get-api-key
         * 
         * Return: The method returns a tuple of two items. The first is an object of NearbySearchResultList which
         *   contains all the places within the given circular search region. The list is sorted based on
         *   descending order of prominence (importance) with the most prominant places at the head of the list.
         *   The second element is a ResponseStatus object indicating the status of the query along with the
         *   appropiate HTTP code. The tuple is wrapped in a Task<> because the method makes Asynchronous HTTP 
         *   requests to the Places API.
         */
        public async Task<Tuple<NearbySearchResultList, ResponseStatus>> GetNearbySearchResultsRankByProminenceWithOptions(Location location,
            double radius, Boolean open_now = false, String keyword = "", String language_code = "", int min_price = -1,
            int max_price = -1, NearbySearchTypes type = NearbySearchTypes.NO_TYPE) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (location == null) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_LOCATION);
            }
            if (radius < 0 || radius > 50000) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.INVALID_RADIUS);
            }

            // Creating the HTTP query url
            String HTTP_query = $"nearbysearch/json?location={location.Lat},{location.Lng}&radius={radius}";

            // Adding in all the optional parameters that are set
            if (open_now) {
                HTTP_query += $"&opennow={open_now}";
            }
            if (!BasicFunctions.isEmpty(keyword)) {
                HTTP_query += $"&keyword={keyword}";
            }
            if (!BasicFunctions.isEmpty(language_code)) {
                HTTP_query += $"&language={language_code}";
            }
            if (min_price > 0) {
                HTTP_query += $"&minprice={min_price}";
            }
            if (max_price > 0) {
                HTTP_query += $"&maxprice={max_price}";
            }
            if (type != NearbySearchTypes.NO_TYPE) {
                HTTP_query += $"&type={type.ToString().ToLower()}";
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

            // Similar two-step jump as we have seen before
            if (response.IsSuccessStatusCode) {
                try {
                    NearbySearchResultList searchResultList = JsonConvert.DeserializeObject<NearbySearchResultList>(response_str);
                    if (!searchResultList.Status.Equals("OK")) {
                        // If the response status from the API is not OK, then we try to return the most appropriate Error
                        ResponseStatus status = PlacesStatus.processErrorMessage(searchResultList.Status, searchResultList.Error_message);
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, status);
                    }
                    else if (searchResultList.Results.Count == 0) {
                        // If the response provides an empty response set, then we return the ZERO_RESULTS (204) error
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<NearbySearchResultList, ResponseStatus>(searchResultList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    // If the deserialization of the response fails, then we return an error
                    Debug.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                // If the response status from the API is not a success, then we return an error using the data returned
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }

        /*
         * Method: GetNearbySearchResultsRankByDistance
         * 
         * Description: This method can be used to get nearby places, ranked by distance from a given location. It
         *   also takes in either a keyword or a type to narrow down the results. The query returns a list of
         *   places ordered by increasing distance from the provded location. 
         * 
         * Parameters:
         *  - location (Location): This is the representation of a geographic point (latitude, longtitude). 
         * At least one of the following two parameters is required for this query:
         *  - keyword (String): This parameter will be matched against the content that Google has indexed for 
         *      each place. Places within the search region with a match will be returned.
         *  - type (NearbySearchTypes): This parameter is used to restrict the results to a specific type only.
         * 
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *      creating an object of this class. For more details about the Google API Key please see:
         *      https://developers.google.com/places/web-service/get-api-key
         * 
         * Return: The method returns a tuple of two items. The first is an object of NearbySearchResultList which
         *   contains all the places within the given circular search region. The list is sorted based on
         *   descending order of prominence (importance) with the most prominant places at the head of the list.
         *   The second element is a ResponseStatus object indicating the status of the query along with the
         *   appropiate HTTP code. The tuple is wrapped in a Task<> because the method makes Asynchronous HTTP 
         *   requests to the Places API.
         */
        public async Task<Tuple<NearbySearchResultList, ResponseStatus>> GetNearbySearchResultsRankByDistance(Location location,
            String keyword = "", NearbySearchTypes type = NearbySearchTypes.NO_TYPE) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (BasicFunctions.isEmpty(keyword) && type == NearbySearchTypes.NO_TYPE) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.TOO_FEW_PARAMETERS);
            }
            if (location == null) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_LOCATION);
            }

            // Creating the HTTP query url
            String HTTP_query = $"nearbysearch/json?rankby=distance&location={location.Lat},{location.Lng}";

            if (!BasicFunctions.isEmpty(keyword)) {
                HTTP_query += $"&keyword={keyword}";
            }
            if (type != NearbySearchTypes.NO_TYPE) {
                HTTP_query += $"&type={type.ToString().ToLower()}";
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
            
            // Similar two-step jump as we have seen before
            if (response.IsSuccessStatusCode) {
                try {
                    NearbySearchResultList searchResultList = JsonConvert.DeserializeObject<NearbySearchResultList>(response_str);
                    if (!searchResultList.Status.Equals("OK")) {
                        // If the response status from the API is not OK, then we try to return the most appropriate Error
                        ResponseStatus status = PlacesStatus.processErrorMessage(searchResultList.Status, searchResultList.Error_message);
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, status);
                    }
                    else if (searchResultList.Results.Count == 0) {
                        // If the response provides an empty response set, then we return the ZERO_RESULTS (204) error
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<NearbySearchResultList, ResponseStatus>(searchResultList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    // If the deserialization of the response fails, then we return an error
                    Debug.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                // If the response status from the API is not a success, then we return an error using the data returned
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }

        /*
         * Method: GetNearbySearchResultsRankByDistanceWithOptions
         * 
         * Description: This method can be used to get nearby places, ranked by distance from a given location. It
         *   also takes in either a keyword or a type to narrow down the results. To further narrow down results,
         *   optional parameters can be used as well. The query returns a list of places ordered by increasing
         *   distance from the provded location. 
         * 
         * Parameters:
         *  - location (Location): This is the representation of a geographic point (latitude, longtitude).
         * At least one of the following two parameters is required for this query:
         *  - keyword (String): This parameter will be matched against the content that Google has indexed for 
         *      each place. Places within the search region with a match will be returned.
         *  - type (NearbySearchTypes): This parameter is used to restrict the results to a specific type only.
         * 
         *  - open_now (boolean): This is an OPTIONAL parameter. If set to true, then the query will only return
         *      places in the search region which are open at the time of querying.
         *  - keyword (String): This OPTIONAL parameter will be matched against the content that Google has
         *      indexed for each place. Places within the search region with a match will be returned.
         *  - language_code (String): OPTIONAL parameter indicating the language in which results will be returned.
         *      By default this is set to English. List of supported languages and their codes: 
         *      https://developers.google.com/maps/faq#languagesupport
         *  - min_price and max_price (int): OPTIONAL parameters which indicates the desired price range of the
         *      places that will be returned. The min and max_price values should be between 0 (lowest) and 4 
         *      (highest), inclusive. The exact price indicated by a specific value varies based on region.
         *  - type (NearbySearchTypes): This an OPTIONAL parameter. It is used to restrict the results to a
         *      specific type only.
         * 
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *      creating an object of this class. For more details about the Google API Key please see:
         *      https://developers.google.com/places/web-service/get-api-key
         * 
         * Return: The method returns a tuple of two items. The first is an object of NearbySearchResultList which
         *   contains all the places within the given circular search region. The list is sorted based on
         *   descending order of prominence (importance) with the most prominant places at the head of the list.
         *   The second element is a ResponseStatus object indicating the status of the query along with the
         *   appropiate HTTP code. The tuple is wrapped in a Task<> because the method makes Asynchronous HTTP 
         *   requests to the Places API.
         */
        public async Task<Tuple<NearbySearchResultList, ResponseStatus>> GetNearbySearchResultsRankByDistanceWithOptions(Location location,
            Boolean open_now = false, String keyword = "", NearbySearchTypes type = NearbySearchTypes.NO_TYPE,
            String language_code = "", int min_price = -1, int max_price = -1) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (BasicFunctions.isEmpty(keyword) && type == NearbySearchTypes.NO_TYPE) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.TOO_FEW_PARAMETERS);
            }
            if (location == null) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_LOCATION);
            }

            // Creating the HTTP query url
            String HTTP_query = $"nearbysearch/json?rankby=distance&location={location.Lat},{location.Lng}";

            // Adding in all the optional parameters that are set
            if (!BasicFunctions.isEmpty(keyword)) {
                HTTP_query += $"&keyword={keyword}";
            }
            if (type != NearbySearchTypes.NO_TYPE) {
                HTTP_query += $"&type={type.ToString().ToLower()}";
            }
            if (open_now) {
                HTTP_query += $"&opennow={open_now}";
            }
            if (!BasicFunctions.isEmpty(language_code)) {
                HTTP_query += $"&language={language_code}";
            }
            if (min_price > 0) {
                HTTP_query += $"&minprice={min_price}";
            }
            if (max_price > 0) {
                HTTP_query += $"&maxprice={max_price}";
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
            
            // Similar two-step jump as we have seen before
            if (response.IsSuccessStatusCode) {
                try {
                    NearbySearchResultList searchResultList = JsonConvert.DeserializeObject<NearbySearchResultList>(response_str);
                    if (!searchResultList.Status.Equals("OK")) {
                        // If the response status from the API is not OK, then we try to return the most appropriate Error
                        ResponseStatus status = PlacesStatus.processErrorMessage(searchResultList.Status, searchResultList.Error_message);
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, status);
                    }
                    else if (searchResultList.Results.Count == 0) {
                        // If the response provides an empty response set, then we return the ZERO_RESULTS (204) error
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<NearbySearchResultList, ResponseStatus>(searchResultList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    // If the deserialization of the response fails, then we return an error
                    Debug.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                // If the response status from the API is not a success, then we return an error using the data returned
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }

        /*
         * Method: GetAdditionalNearbySearchResults
         * 
         * Description: This method can be used to get an additional set of results for a previously run query. 
         *   The query uses the nextPageToken, a String wil uniquely identifies the next page of results to access
         *   and return the requested results.
         * 
         * Parameters:
         *  - pageToken (String): This is a unique indentifier for the next page of results. It is included in the
         *      response body of the previously run query.
         *      
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *      creating an object of this class. For more details about the Google API Key please see:
         *      https://developers.google.com/places/web-service/get-api-key
         * 
         * Return: The method returns a tuple of two items. The first is an object of NearbySearchResultList which
         *   contains all the places within the given circular search region. The list is sorted based on
         *   descending order of prominence (importance) with the most prominant places at the head of the list.
         *   The second element is a ResponseStatus object indicating the status of the query along with the
         *   appropiate HTTP code. The tuple is wrapped in a Task<> because the method makes Asynchronous HTTP 
         *   requests to the Places API.
         */
        public async Task<Tuple<NearbySearchResultList, ResponseStatus>> GetAdditionalNearbySearchResults(String pageToken) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (BasicFunctions.isEmpty(pageToken)) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_PAGE_TOKEN);
            }

            // Creating the HTTP query url
            String HTTP_query = $"nearbysearch/json?pagetoken={pageToken}&key={APIKey}";

            // Setting up the request header to indicate that the request body will be in json
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Making an asynchronous HTTP GET request to the Places API and collecting the output
            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            Console.WriteLine(response_str);

            // Similar two-step jump as we have seen before
            if (response.IsSuccessStatusCode) {
                try {
                    NearbySearchResultList searchResultList = JsonConvert.DeserializeObject<NearbySearchResultList>(response_str);
                    if (!searchResultList.Status.Equals("OK")) {
                        // If the response status from the API is not OK, then we try to return the most appropriate Error
                        ResponseStatus status = PlacesStatus.processErrorMessage(searchResultList.Status, searchResultList.Error_message);
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, status);
                    }
                    else if (searchResultList.Results.Count == 0) {
                        // If the response provides an empty response set, then we return the ZERO_RESULTS (204) error
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<NearbySearchResultList, ResponseStatus>(searchResultList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    // If the deserialization of the response fails, then we return an error
                    Debug.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                // If the response status from the API is not a success, then we return an error using the data returned
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }

        // Text Search Requests

        /*
         * Method: GetTextSearchResults
         * 
         * Description: This method can be used to get places using a string such as "pizza in New York" or 
         *   "museums in Washington DC". 
         * 
         * Parameters:
         *  - query (String): A string parameter that will be used to search through the Places API and find 
         *      places that contain matching information. This could be any collection of keywords that can 
         *      appropriately describe the place sought.
         *
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *      creating an object of this class. For more details about the Google API Key please see:
         *      https://developers.google.com/places/web-service/get-api-key
         * 
         * Return: The method returns a tuple of two items. The first is an object of NearbySearchResultList which
         *   contains all the places within the given circular search region. The list is sorted based on
         *   descending order of prominence (importance) with the most prominant places at the head of the list.
         *   The second element is a ResponseStatus object indicating the status of the query along with the
         *   appropiate HTTP code. The tuple is wrapped in a Task<> because the method makes Asynchronous HTTP 
         *   requests to the Places API.
         */
        public async Task<Tuple<NearbySearchResultList, ResponseStatus>> GetTextSearchResults(String query) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (BasicFunctions.isEmpty(query)) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_QUERY);
            }

            String HTTP_query = $"textsearch/json?query={query}&key={APIKey}";

            // Setting up the request header to indicate that the request body will be in json
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Making an asynchronous HTTP GET request to the Places API and collecting the output
            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            Console.WriteLine(response_str);

            // Similar two-step jump as we have seen before
            if (response.IsSuccessStatusCode) {
                try {
                    NearbySearchResultList searchResultList = JsonConvert.DeserializeObject<NearbySearchResultList>(response_str);
                    if (!searchResultList.Status.Equals("OK")) {
                        // If the response status from the API is not OK, then we try to return the most appropriate Error
                        ResponseStatus status = PlacesStatus.processErrorMessage(searchResultList.Status, searchResultList.Error_message);
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, status);
                    }
                    else if (searchResultList.Results.Count == 0) {
                        // If the response provides an empty response set, then we return the ZERO_RESULTS (204) error
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<NearbySearchResultList, ResponseStatus>(searchResultList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    // If the deserialization of the response fails, then we return an error
                    Debug.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                // If the response status from the API is not a success, then we return an error using the data returned
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }

        /*
         * Method: GetTextSearchResultsWithOptions
         * 
         * Description: This method can be used to get places using a string such as "pizza in New York" or 
         *   "museums in Washington DC". The optional parameters can be used to narrow down teh results to meet
         *   the desired preferences.
         * 
         * Parameters:
         *  - query (String): A string parameter that will be used to search through the Places API and find 
         *      places that contain matching information. This could be any collection of keywords that can 
         *      appropriately describe the place sought.
         *      
         *  - open_now (boolean): This is an OPTIONAL parameter. If set to true, then the query will only return
         *      places in the search region which are open at the time of querying.
         *  - location (Location): This is an OPTIONAL parameter. It is the representation of a geographic point
         *      (latitude, longtitude). This along with the radius is used to create a circular search region.
         *  - radius (double): This is an OPTIONAL parameter that represents the radius of the circular search 
         *      region from the location point. This is in meters and the allowable range is between 0 and 50,000
         *      meters (inclusive).
         *  - region_code (String): This is an OPTIONAL parameter, that indicates the region code, specified as a
         *      ccTLD format. This is used to influence the query's results but relevant results outside the
         *      region may also be included.
         *  - language_code (String): OPTIONAL parameter indicating the language in which results will be returned.
         *      By default this is set to English. List of supported languages and their codes: 
         *      https://developers.google.com/maps/faq#languagesupport
         *  - min_price and max_price (int): OPTIONAL parameters which indicates the desired price range of the
         *      places that will be returned. The min and max_price values should be between 0 (lowest) and 4 
         *      (highest), inclusive. The exact price indicated by a specific value varies based on region.
         *  - type (NearbySearchTypes): This an OPTIONAL parameter. It is used to restrict the results to a
         *      specific type only.
         *
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *      creating an object of this class. For more details about the Google API Key please see:
         *      https://developers.google.com/places/web-service/get-api-key
         * 
         * Return: The method returns a tuple of two items. The first is an object of NearbySearchResultList which
         *   contains all the places within the given circular search region. The list is sorted based on
         *   descending order of prominence (importance) with the most prominant places at the head of the list.
         *   The second element is a ResponseStatus object indicating the status of the query along with the
         *   appropiate HTTP code. The tuple is wrapped in a Task<> because the method makes Asynchronous HTTP 
         *   requests to the Places API.
         */
        public async Task<Tuple<NearbySearchResultList, ResponseStatus>> GetTextSearchResultsWithOptions(String query,
            Boolean open_now = false, Location location = null, double radius = -1, String region_code = "",
            String language_code = "", NearbySearchTypes type = NearbySearchTypes.NO_TYPE, int min_price = -1, 
            int max_price = -1) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (BasicFunctions.isEmpty(query)) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_QUERY);
            }

            // Creating the query URL
            String HTTP_query = $"textsearch/json?query={query}";

            // Appending any optional parameters that are set
            if (location != null) {
                HTTP_query += $"&location={location.Lat},{location.Lng}";
            }
            if (radius > 0 && radius < 50000) {
                HTTP_query += $"&radius={radius}";
            }
            if (BasicFunctions.isEmpty(region_code)) {
                HTTP_query += $"&region={region_code}";
            }
            if (type != NearbySearchTypes.NO_TYPE) {
                HTTP_query += $"&type={type.ToString().ToLower()}";
            }
            if (open_now) {
                HTTP_query += $"&opennow={open_now}";
            }
            if (!BasicFunctions.isEmpty(language_code)) {
                HTTP_query += $"&language={language_code}";
            }
            if (min_price > 0) {
                HTTP_query += $"&minprice={min_price}";
            }
            if (max_price > 0) {
                HTTP_query += $"&maxprice={max_price}";
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
            Console.WriteLine(response_str);

            // Similar two-step jump as we have seen before
            if (response.IsSuccessStatusCode) {
                try {
                    NearbySearchResultList searchResultList = JsonConvert.DeserializeObject<NearbySearchResultList>(response_str);
                    if (!searchResultList.Status.Equals("OK")) {
                        // If the response status from the API is not OK, then we try to return the most appropriate Error
                        ResponseStatus status = PlacesStatus.processErrorMessage(searchResultList.Status, searchResultList.Error_message);
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, status);
                    }
                    else if (searchResultList.Results.Count == 0) {
                        // If the response provides an empty response set, then we return the ZERO_RESULTS (204) error
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<NearbySearchResultList, ResponseStatus>(searchResultList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    // If the deserialization of the response fails, then we return an error
                    Debug.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                // If the response status from the API is not a success, then we return an error using the data returned
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }

        /*
         * Method: GetAdditionalTextSearchResults
         * 
         * Description: This method can be used to get an additional set of text results for a previously run query. 
         *   The query uses the nextPageToken, a String wil uniquely identifies the next page of results to access
         *   and return the requested results.
         * 
         * Parameters:
         *  - pageToken (String): This is a unique indentifier for the next page of results. It is included in the
         *      response body of the previously run query.
         *      
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *      creating an object of this class. For more details about the Google API Key please see:
         *      https://developers.google.com/places/web-service/get-api-key
         * 
         * Return: The method returns a tuple of two items. The first is an object of NearbySearchResultList which
         *   contains all the places within the given circular search region. The list is sorted based on
         *   descending order of prominence (importance) with the most prominant places at the head of the list.
         *   The second element is a ResponseStatus object indicating the status of the query along with the
         *   appropiate HTTP code. The tuple is wrapped in a Task<> because the method makes Asynchronous HTTP 
         *   requests to the Places API.
         */
        public async Task<Tuple<NearbySearchResultList, ResponseStatus>> GetAdditionalTextSearchResults(String nextPageToken) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (BasicFunctions.isEmpty(nextPageToken)) {
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.MISSING_PAGE_TOKEN);
            }

            // Creating the HTTP query url
            String HTTP_query = $"textsearch/json?pagetoken={nextPageToken}&key={APIKey}";

            // Setting up the request header to indicate that the request body will be in json
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Making an asynchronous HTTP GET request to the Places API and collecting the output
            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            Console.WriteLine(response_str);

            // Similar two-step jump as we have seen before
            if (response.IsSuccessStatusCode) {
                try {
                    NearbySearchResultList searchResultList = JsonConvert.DeserializeObject<NearbySearchResultList>(response_str);
                    if (!searchResultList.Status.Equals("OK")) {
                        // If the response status from the API is not OK, then we try to return the most appropriate Error
                        ResponseStatus status = PlacesStatus.processErrorMessage(searchResultList.Status, searchResultList.Error_message);
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, status);
                    }
                    else if (searchResultList.Results.Count == 0) {
                        // If the response provides an empty response set, then we return the ZERO_RESULTS (204) error
                        return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<NearbySearchResultList, ResponseStatus>(searchResultList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    // If the deserialization of the response fails, then we return an error
                    Debug.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<NearbySearchResultList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                // If the response status from the API is not a success, then we return an error using the data returned
                return new Tuple<NearbySearchResultList, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }
    }
}
