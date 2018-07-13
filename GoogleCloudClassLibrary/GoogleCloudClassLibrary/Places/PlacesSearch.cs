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

        public PlacesSearch() {
            httpClient = new HttpClient();

            // Since this is the first time we use the httpClient, we need to intialize its base address
            httpClient.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/place/");
        }

        // Find Places

        /*
         * Method: FindPlacesUsingTextQuery
         * 
         * Description: This method can be used to query the Places API for places with some specific matching 
         *   paramter. For phone numbers, please use the FindPlacesUsingPhoneNumber() method.
         * 
         * Parameters:
         *   - APIKey (String): String representing the Gooogle Cloud Services API access key. For more details:
         *       https://developers.google.com/places/web-service/get-api-key
         *   - query (String): A string parameter that will be used to search through the Places API and find 
         *       places that contain matching information. This could be any collection of keywords that can 
         *       appropriately describe the information sought. 
         * 
         * Return: The method returns a list of all the candidates that match the query provided. The list is 
         *   wrapped in a Task<> because the method makes Asynchronous HTTP requests to the Places API.
         */
        public async Task<Tuple<FindPlacesCandidateList, ResponseStatus>> FindPlacesUsingTextQuery(String APIKey, String query) {
            if (BasicFunctions.isEmpty(APIKey) || BasicFunctions.isEmpty(query)) {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
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
            Console.WriteLine(response_str);

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
                        ResponseStatus status = PlacesStatus.processErrorMessage(candidateList);
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, status);
                    }
                    else if (candidateList.Candidates.Count == 0) {
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(candidateList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    Console.WriteLine("Exception: " + e.StackTrace);
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
         *   - APIKey (String): String representing the Gooogle Cloud Services API access key. For more details:
         *       https://developers.google.com/places/web-service/get-api-key
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
         * Return: The method returns a list of all the candidates that match the query provided. The list is 
         *   wrapped in a Task<> because the method makes Asynchronous HTTP requests to the Places API.
         */
        public async Task<Tuple<FindPlacesCandidateList, ResponseStatus>> FindPlaceWithPointLocationBias(String APIKey, String query,
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
                // Similar two-step hop as we have seen in the previous function
                try {
                    FindPlacesCandidateList candidateList = JsonConvert.DeserializeObject<FindPlacesCandidateList>(response_str);
                    if (!candidateList.Status.Equals("OK")) {
                        ResponseStatus status = PlacesStatus.processErrorMessage(candidateList);
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, status);
                    }
                    else if (candidateList.Candidates.Count == 0) {
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(candidateList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    Console.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
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
         *   - APIKey (String): String representing the Gooogle Cloud Services API access key. For more details:
         *       https://developers.google.com/places/web-service/get-api-key
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
         * Return: The method returns a list of all the candidates that match the query provided. The list is 
         *   wrapped in a Task<> because the method makes Asynchronous HTTP requests to the Places API.
         */
        public async Task<Tuple<FindPlacesCandidateList, ResponseStatus>> FindPlacesWithCircularLocationBias(String APIKey, String query,
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
                // Similar two-step hop as we have seen in the previous function
                try {
                    FindPlacesCandidateList candidateList = JsonConvert.DeserializeObject<FindPlacesCandidateList>(response_str);
                    if (!candidateList.Status.Equals("OK")) {
                        ResponseStatus status = PlacesStatus.processErrorMessage(candidateList);
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, status);
                    }
                    else if (candidateList.Candidates.Count == 0) {
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(candidateList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    Console.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
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
         *   - APIKey (String): String representing the Gooogle Cloud Services API access key. For more details:
         *       https://developers.google.com/places/web-service/get-api-key
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
         * Return: The method returns a list of all the candidates that match the query provided. The list is 
         *   wrapped in a Task<> because the method makes Asynchronous HTTP requests to the Places API.
         */
        public async Task<Tuple<FindPlacesCandidateList, ResponseStatus>> FindPlacesWithRectLocationBias(String APIKey, 
            String query, InputType inputType, Location southWestCorner, Location northEastCorner, 
            List<Fields> fields, String language_code = "") {
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
                // Similar two-step hop as we have seen in the previous function
                try {
                    FindPlacesCandidateList candidateList = JsonConvert.DeserializeObject<FindPlacesCandidateList>(response_str);
                    if (!candidateList.Status.Equals("OK")) {
                        ResponseStatus status = PlacesStatus.processErrorMessage(candidateList);
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, status);
                    }
                    else if (candidateList.Candidates.Count == 0) {
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.ZERO_RESULTS);
                    }
                    else {
                        return new Tuple<FindPlacesCandidateList, ResponseStatus>(candidateList, PlacesStatus.OK);
                    }
                } catch (JsonSerializationException e) {
                    Console.WriteLine("Exception: " + e.StackTrace);
                    return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, PlacesStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                return new Tuple<FindPlacesCandidateList, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }

        // Nearby Search Requests
        // TODO: Create Custom error messages for validation errors

        /*
         * Method: GetNearbySearchResultsRankByProminence
         * 
         * Description: This method can be used to find places, ranked by prominence, within a circular region
         *   described by the parameters. The prominence based ranking is determined through a combination of factors 
         *   such as the popularity, frequency of search, and ranking in Google's index.
         * 
         * Parameters:
         *  - APIKey (String): String representing the Gooogle Cloud Services API access key. For more details:
         *      https://developers.google.com/places/web-service/get-api-key
         *  - location (Location): This is the representation of a point (latitude, longtitude). This along with
         *      the radius will be used to create a circular search region.
         *  - radius (double): Radius of the circular search region from the location point. This is in meters
         *      and the allowable value is between 0 and 50,000 meters (inclusive).
         * 
         * Return: The method returns a list of all the places within the given circular search region. The 
         *   list is sorted based on descending order of prominence (importance) with the most prominant 
         *   places at the head of the list. The list is wrapped in a Task<> because the method makes 
         *   Asynchronous HTTP requests to the Places API.
         */
        public async Task<NearbySearchResultList> GetNearbySearchResultsRankByProminence(String APIKey, Location location,
            double radius) {
            if (BasicFunctions.isEmpty(APIKey) || location == null)
                return null;

            if (radius < 0 || radius > 50000)
                return null;

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
            try {
                NearbySearchResultList searchResultList = JsonConvert.DeserializeObject<NearbySearchResultList>(response_str);
                if (searchResultList.Results.Count == 0) {
                    return null;
                }
                else {
                    return searchResultList;
                }
            } catch (JsonSerializationException e) {
                Console.WriteLine("Exception: " + e.StackTrace);
                return null;
            }
        }

        /*
         * 
         */ 
        public async Task<NearbySearchResultList> GetNearbySearchResultsRankByProminenceWithOptions(String APIKey,
            Location location, double radius, Boolean open_now = false, String keyword = "", String language_code = "", 
            int min_price = -1, int max_price = -1, String type = "") {
            if (BasicFunctions.isEmpty(APIKey) || location == null)
                return null;

            if (radius < 0 || radius > 50000)
                return null;

            // Creating the HTTP query url
            String HTTP_query = $"nearbysearch/json?location={location.Lat},{location.Lng}&radius={radius}";

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
            if (!BasicFunctions.isEmpty(type)) {
                HTTP_query += $"&type={type}";
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
                    if (searchResultList == null || searchResultList.Results.Count == 0) {
                        return null;
                    }
                    else {
                        return searchResultList;
                    }
                } catch (JsonSerializationException e) {
                    Console.WriteLine("Exception: " + e.StackTrace);
                    return null;
                }
            }

            return null;
        }

        public async Task<NearbySearchResultList> GetNearbySearchResultsRankByDistance(String APIKey, Location location,
            String keyword = "", String type = "") {
            if (BasicFunctions.isEmpty(APIKey) || location == null)
                return null;
            if (BasicFunctions.isEmpty(keyword) && BasicFunctions.isEmpty(type)) {
                return null;
            }

            // Creating the HTTP query url
            String HTTP_query = $"nearbysearch/json?rankby=distance&location={location.Lat},{location.Lng}";

            if (!BasicFunctions.isEmpty(keyword)) {
                HTTP_query += $"&keyword={keyword}";
            }
            if (!BasicFunctions.isEmpty(type)) {
                HTTP_query += $"&type={type}";
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
                    if (searchResultList == null || searchResultList.Results.Count == 0) {
                        return null;
                    }
                    else {
                        return searchResultList;
                    }
                } catch (JsonSerializationException e) {
                    Console.WriteLine("Exception: " + e.StackTrace);
                    return null;
                }
            }

            return null;
        }

        public async Task<NearbySearchResultList> GetNearbySearchResultsRankByDistanceWithOptions(String APIKey,
            Location location, Boolean open_now, String keyword = "", String type = "",
            String language_code = "", int min_price = -1, int max_price = -1) {
            if (BasicFunctions.isEmpty(APIKey) || location == null)
                return null;
            if (BasicFunctions.isEmpty(keyword) && BasicFunctions.isEmpty(type)) {
                return null;
            }

            // Creating the HTTP query url
            String HTTP_query = $"nearbysearch/json?rankby=distance&location={location.Lat},{location.Lng}";

            if (!BasicFunctions.isEmpty(keyword)) {
                HTTP_query += $"&keyword={keyword}";
            }
            if (!BasicFunctions.isEmpty(type)) {
                HTTP_query += $"&type={type}";
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
                    if (searchResultList == null || searchResultList.Results.Count == 0) {
                        return null;
                    }
                    else {
                        return searchResultList;
                    }
                } catch (JsonSerializationException e) {
                    Console.WriteLine("Exception: " + e.StackTrace);
                    return null;
                }
            }

            return null;
        }

        public async Task<NearbySearchResultList> GetAdditionalNearbySearchResults(String APIKey, String pageToken) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return null;
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
                    if (searchResultList == null || searchResultList.Results.Count == 0) {
                        return null;
                    }
                    else {
                        return searchResultList;
                    }
                } catch (JsonSerializationException e) {
                    Console.WriteLine("Exception: " + e.StackTrace);
                    return null;
                }
            }

            return null;
        }

        // Text Search Requests

        public List<NearbySearchResult> GetTextSearchResults(String APIKey, String query) {
            return null;
        }

        public List<NearbySearchResult> GetTextSearchResultsWithOptions(String APIKey, String query, Location location,
            Boolean open_now, double radius = -1, String region_code = "", String language_code = "en", String type = "",
            int min_price = -1, int max_price = -1) {
            return null;
        }

        public List<NearbySearchResult> GetAdditionalTextSearchResults(String APIKey) {
            return null;
        }
    }
}
