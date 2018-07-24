using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.Places {
    public class PlacesPhotos {
        private static HttpClient httpClient;
        private static String APIKey;

        public PlacesPhotos(GoogleCloudClassSetup setup) {
            httpClient = new HttpClient();

            // Since this is the first time we use the httpClient, we need to intialize its base address
            httpClient.BaseAddress = new Uri(setup.getAPIUrl("PLACES_API_URL").ToString());
            APIKey = setup.getAPIKey();
        }

        public async Task<Tuple<String, ResponseStatus>> GetPlacesPhotos(String photoReference, 
            String fileDestination, int maxHeight = 0, int maxWidth = 0) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<String, ResponseStatus>(null, PlacesStatus.MISSING_API_KEY);
            }
            if (BasicFunctions.isEmpty(photoReference)) {
                return new Tuple<String, ResponseStatus>(null, PlacesStatus.MISSING_PHOTO_REFERENCE);
            }
            if (maxHeight <= 0 && maxWidth <= 0) {
                return new Tuple<string, ResponseStatus>(null, PlacesStatus.MISSING_HEIGHT_WIDTH);
            }
            if (BasicFunctions.isEmpty(fileDestination)) {
                return new Tuple<string, ResponseStatus>(null, PlacesStatus.MISSING_FILE_DESTINATION);
            }

            // Creating the HTTP query url
            String HTTP_query = $"photo?photoreference={photoReference}";

            if (maxHeight > 0) {
                HTTP_query += $"&maxheight={maxHeight}";
            }
            if (maxWidth > 0) {
                HTTP_query += $"&maxwidth={maxWidth}";
            }
            HTTP_query += $"&key={APIKey}";

            // Setting up the request header to indicate that the request body will be in json
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Making an asynchronous HTTP GET request to the Places API and collecting the output
            HttpResponseMessage response = await httpClient.GetAsync(HTTP_query);
            Stream stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode) {

                // The following block of code is borrowed from https://stackoverflow.com/a/2368180

                using (BinaryReader reader = new BinaryReader(stream)) {
                    Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                    try {
                        using (FileStream lxFS = new FileStream(fileDestination, FileMode.Create)) {
                            lxFS.Write(lnByte, 0, lnByte.Length);

                            // End of borrowed code
                        }
                    } catch (ArgumentException e) {
                        Debug.WriteLine(e.StackTrace);
                        return new Tuple<String, ResponseStatus>(null, PlacesStatus.INVALID_FILE_LOCATION);
                    } catch (IOException e) {
                        Debug.WriteLine(e.StackTrace);

                        if (e.Message.ToLower().Contains("could not find a part of the path")) {
                            return new Tuple<string, ResponseStatus>(null, PlacesStatus.INVALID_FILE_LOCATION);
                        }
                        return new Tuple<String, ResponseStatus>(null, PlacesStatus.INTERNAL_SERVER_ERROR);
                    }
                }
            }
            else {
                return new Tuple<string, ResponseStatus>(null, PlacesStatus.processErrorMessage(response.StatusCode.ToString(), response.ReasonPhrase));
            }

            return new Tuple<string, ResponseStatus>(fileDestination, PlacesStatus.OK);
        }
    }
}
