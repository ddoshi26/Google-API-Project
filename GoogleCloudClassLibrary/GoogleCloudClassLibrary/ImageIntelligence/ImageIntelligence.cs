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

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class ImageIntelligence {

        private static HttpClient httpClient;

        public ImageIntelligence() {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://vision.googleapis.com/");
        }

        /*
         * Method: AnnotateImage
         * 
         * Description: This method can be used to query the Google Cloud Image Intelligence API with a batch of 
         *   images and run image detection and annotation on these images.
         * 
         * Parameters:
         *  - APIKey (String): This represents the Gooogle Cloud Services API access key. For more details:
         *      https://developers.google.com/places/web-service/get-api-key.
         *  - imageRequests (AnnotateImageRequestList): List of all the individual AnnotateImageRequests that will
         *      be sent to the Image Intelligence API. 
         *  
         * Return: If the query is successful, then the method will return a list of AnnotateImageResponse objects
         *   with image identification and annotation for each individual image request. Since the HTTP query 
         *   and the indentification are both performed asynchronously, the return object is wrapped in Task<>. If
         *   the query is unsuccessful and an error is returned, then the method returns null.
         */
        public async Task<List<AnnotateImageResponse>> AnnotateImage(String APIKey, AnnotateImageRequestList imageRequests) {
            if (imageRequests == null || imageRequests.Requests.Count == 0) {
                return null;
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
                } catch (JsonSerializationException e) {
                    Console.WriteLine(e.StackTrace);
                    return null;
                }

                return imageResponseList.Responses;
            }
            else {
                Console.WriteLine(response_str);
                return null;
            }
        }
    }
}
