using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class VideoIntelligence {

        private static HttpClient httpClient;

        public VideoIntelligence() {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://videointelligence.googleapis.com/");
        }

        /*
         * Method: AnnotateVideoWithLabelDetection
         * 
         * Description: This method can be used to annotate the desired video with labels using the Google Cloud
         *   Video Intelligence API. If you wish to run multiple annotations at once, please use 
         *   AnnotateVideoWithMultipleDetections.
         * 
         * Parameters:
         *  - APIKey (String): This represents the Gooogle Cloud Services API access key. For more details:
         *      https://developers.google.com/places/web-service/get-api-key.
         *  - inputUri (String): This string is the URI location of the video. Currently, only Google Cloud 
         *      Storage URIs are supported. If this is set, then inputContent should be unset
         *  - inputContent (String): Bytes format string for the video data. This shoudl be set only if inputUri 
         *      is not set.
         *  - context (VideoContext): Additional context for the video or parameters specific to Label Annotation
         *  - outputUri (String): Optional Google Cloud Storage URI where the resulting JSON will be stored.
         *  - cloudRegionId (String): Optional Google Cloud region id where the video annotation should take place.
         *      If no region is specified then one will be chosen based on the file's location.
         *      
         * Return: If the query is successful, then the method will return an object of tyep Operation. This can
         *   be used to track the status of the Annotation. The Operation object is wrapped in the Task<> class 
         *   because the HTTP request and the annotation are both performed asynchronously. Returns null if
         *   an error is thrown or if a validation fails. (TODO: Improve to return error codes)
         */
        public async Task<VideoAnnotationResponse> AnnotateVideoWithLabelDetection(String APIKey, String inputUri, String inputContent,
            VideoContext context, String outputUri = "", String cloudRegionId = "") {

            // One and only one of inputUri and inputContent can be set
            if ((BasicFunctions.isEmpty(inputUri) && BasicFunctions.isEmpty(inputContent)) ||
                (!BasicFunctions.isEmpty(inputUri) && !BasicFunctions.isEmpty(inputContent))) {
                return null;
            }

            List<VideoFeature> videoFeatures = new List<VideoFeature>();
            videoFeatures.Add(VideoFeature.LABEL_DETECTION);

            AnnotateVideoRequest annotateVideoRequest = new AnnotateVideoRequest(inputUri, inputContent, videoFeatures, context, outputUri, cloudRegionId);

            String str = JsonConvert.SerializeObject(annotateVideoRequest);
            Console.WriteLine(str);

            // Setting up the header for the request body
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API address to which we will make the HTTP POST query
            String request_query = "v1/videos:annotate?" + $"key={APIKey}";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, annotateVideoRequest);

            Stream stream = response.Content.ReadAsStreamAsync().Result;
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            Console.WriteLine(response_str);
            /* 
             * Similar two-step hop as we have seen before. We try to deserialize the response string, expecting
             * an object of Operation. If the response is not an Operation object, then we will encounter a
             * JSONSerialization error and return null. If it is as we expect, then we just return the 
             * Operation object.
             */
            if (response.IsSuccessStatusCode) {
                Operation operation;
                try {
                    operation = JsonConvert.DeserializeObject<Operation>(response_str);
                } catch (JsonSerializationException e) {
                    Console.WriteLine(e.StackTrace);
                    return null;
                }
                
                String operation_json = await GetVideoAnnotateOperation(APIKey, operation.Name);
                try {
                    VideoAnnotationResponse annotationResponse = JsonConvert.DeserializeObject<VideoAnnotationResponse>(operation_json);

                    while (!annotationResponse.Done) {
                        System.Threading.Thread.Sleep(60000);
                        operation_json = await GetVideoAnnotateOperation(APIKey, operation.Name);
                        annotationResponse = JsonConvert.DeserializeObject<VideoAnnotationResponse>(operation_json);
                    }

                    Console.WriteLine(operation_json);
                    return annotationResponse;
                } catch (JsonSerializationException e) {
                    Console.WriteLine(e.StackTrace);
                    return null;
                }
            }
            else {
                //if the query returns an error code, then we just print what we received and return null
                return null;
            }
            //return null;
        }

        public async Task<string> GetVideoAnnotateOperation(String APIKey, String op_name) {
            if (BasicFunctions.isEmpty(APIKey) || BasicFunctions.isEmpty(op_name)) {
                return null;
            }

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API address to which we will make the HTTP POST query
            String request_query = "v1/operations/" + $"{op_name}?key={APIKey}";
            HttpResponseMessage response = await httpClient.GetAsync(request_query);

            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            Console.WriteLine(response_str);

            return response_str;
        }

        public Operation AnnotateVideoWithShotChangeDetection(String inputUri, String inputContent, VideoContext context,
            String outputUri = "", String cloudRegionId = "") {
            return null;
        }

        public Operation AnnotateVideoWithExplicitContentDetection(String inputUri, String inputContent,
            VideoContext context, String outputUri = "", String cloudRegionId = "") {
            return null;
        }

        public Operation AnnotateVideoWithMultipleDetections(String inputUri, String inputContent, VideoContext context,
            String outputUri = "", String cloudRegionId = "", Boolean labelDetection = false,
            Boolean shotChangeDetection = false, Boolean explicitContentDetection = false) {
            return null;
        }
    }
}
