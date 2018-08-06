using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class ImageIntelligence {
        private static HttpClient httpClient;
        private static String APIKey;

        public ImageIntelligence(GoogleCloudClassSetup setup) {
            httpClient = new HttpClient();

            // Since this is the first time we use the httpClient, we need to intialize its base address
            httpClient.BaseAddress = new Uri(setup.getAPIUrl("IMAGE_INTELLIGENCE_API_URL").ToString());
            APIKey = setup.getAPIKey();
        }

        public void UpdateURL(GoogleCloudClassSetup setup) {
            httpClient.BaseAddress = new Uri(setup.getAPIUrl("IMAGE_INTELLIGENCE_API_URL").ToString());
        }

        public void UpdateKey(GoogleCloudClassSetup setup) {
            APIKey = setup.getAPIKey();
        }

        /*
         * Method: AnnotateImages
         * 
         * Description: This method can be used to query the Google Cloud Image Intelligence API with a batch of 
         *   images and run image detection and annotation on these images.
         * 
         * Parameters:
         *  - imageRequests (AnnotateImageRequestList): List of all the individual AnnotateImageRequests that will
         *      be sent to the Image Intelligence API. 
         *      
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *      creating an object of this class. For more details about the Google API Key please see:
         *      https://developers.google.com/places/web-service/get-api-key
         * 
         *  
         * Return: If the query is successful, then the method will return a tuple of two items. The first is an
         *   AnnotateImageResponseList object with image identification and annotation corresponding to each image
         *   request from the request list. The second element is a ResponseStatus object indicating the status of
         *   the query along with the appropiate HTTP code. Since the HTTP query and the indentification are both
         *   performed asynchronously, the return object is wrapped in Task<>.
         */
        public async Task<Tuple<AnnotateImageResponseList, ResponseStatus>> AnnotateImages(AnnotateImageRequestList imageRequests, String input = "") {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<AnnotateImageResponseList, ResponseStatus>(null, ImageAnnotationStatus.MISSING_API_KEY);
            }
            if (imageRequests == null || imageRequests.Requests.Count == 0) {
                return new Tuple<AnnotateImageResponseList, ResponseStatus>(null, ImageAnnotationStatus.MISSING_REQUEST_LIST);
            }

            // Preparing the header to accept the JSON request body
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // The API address to which we will make the HTTP POST query
            String request_query = "v1/images:annotate?" + $"key={APIKey}";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, imageRequests);
    
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            
            /* 
             * Similar two-step hop as we have seen before. We try to deserialize the response string, expecting
             * an object of AnnotateImageResponseList If the response is not an AnnotateImageResponseList object, 
             * then we will encounter a JSONSerialization error and return null. If it is as we expect, then we 
             * just return the AnnotateImageResponseList object.
             */
            if (response.IsSuccessStatusCode) {
                AnnotateImageResponseList imageResponseList;

                try {
                    imageResponseList = JsonConvert.DeserializeObject<AnnotateImageResponseList>(response_str);

                    if (imageResponseList == null || imageResponseList.Responses.Count == 0) {
                        return new Tuple<AnnotateImageResponseList, ResponseStatus>(null, ImageAnnotationStatus.ZERO_RESULTS);
                    }
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<AnnotateImageResponseList, ResponseStatus>(null, ImageAnnotationStatus.DESERIALIZATION_ERROR);
                }

                return new Tuple<AnnotateImageResponseList, ResponseStatus>(imageResponseList, ImageAnnotationStatus.OK);
            }
            else {
                // If the query is not successful, then we try to extract details about the error from the response
                AnnotateImageResponseList annotateResponse;

                try {
                    annotateResponse = JsonConvert.DeserializeObject<AnnotateImageResponseList>(response_str);

                    if (annotateResponse == null) {
                        return new Tuple<AnnotateImageResponseList, ResponseStatus>(null, ImageAnnotationStatus.INTERNAL_SERVER_ERROR);
                    }
                    if (annotateResponse.Error == null) {
                        return new Tuple<AnnotateImageResponseList, ResponseStatus>(null, ImageAnnotationStatus.ProcessErrorMessage(response.StatusCode.ToString(), response.ReasonPhrase));
                    }
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<AnnotateImageResponseList, ResponseStatus>(null, ImageAnnotationStatus.DESERIALIZATION_ERROR);
                }

                ResponseStatus status = ImageAnnotationStatus.ProcessErrorMessage(annotateResponse.Error.Code.ToString(), annotateResponse.Error.Message);
                return new Tuple<AnnotateImageResponseList, ResponseStatus>(null, status);
            }
        }
    }
}
