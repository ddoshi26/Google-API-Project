using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class VideoIntelligence {
        private static HttpClient httpClient;
        private static String APIKey;
        private static readonly int SLEEP_TIME = 10000;

        public VideoIntelligence(GoogleCloudClassSetup setup) {
            httpClient = new HttpClient();

            // Since this is the first time we use the httpClient, we need to intialize its base address
            httpClient.BaseAddress = new Uri(setup.getAPIUrl("VIDEO_INTELLIGENCE_API_URL").ToString());
            APIKey = setup.getAPIKey();
        }

        public void UpdateURL(GoogleCloudClassSetup setup) {
            httpClient.BaseAddress = new Uri(setup.getAPIUrl("VIDEO_INTELLIGENCE_API_URL").ToString());
        }

        public void UpdateKey(GoogleCloudClassSetup setup) {
            APIKey = setup.getAPIKey();
        }

        // Function to get an Operation result
        public async Task<string> GetVideoAnnotateOperation(String op_name) {
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

            return response_str;
        }

        /*
         * Method: AnnotateVideoWithLabelDetection
         * 
         * Description: This method can be used to annotate the desired video with labels using the Google Cloud
         *   Video Intelligence API. If you wish to run multiple annotations at once, please use the 
         *   AnnotateVideoWithMultipleDetections() method.
         * 
         * Parameters:
         * Only one of the following two parameters should be provided. If none or both are provided, then an
         * error will be returned:
         *  - inputUri (String): This string is the URI location of the video. Currently, only Google Cloud 
         *      Storage URIs are supported. If this is set, then inputContent should not be set.
         *  - inputContent (String): Bytes format string for the video data. This should be set only if inputUri 
         *      is not set.
         *      
         *  - context (VideoContext): Additional context for the video or parameters specific to Label Annotation.
         *      This is an OPTIONAL parameter.
         *  - outputUri (String): OPTIONAL Google Cloud Storage URI where the resulting JSON will be stored.
         *  - cloudRegionId (String): OPTIONAL Google Cloud region id where the video annotation should take place.
         *      If no region is specified then one will be chosen based on the file's location.
         *      
         * Return: The method returns a tuple of two items. If the query is successful, then the first item will
         *   be an object of type VideoAnnotationResponse. If the query is unsuccessful and an error is returned,
         *   then the method returns null. The second element is a ResponseStatus object indicating the status of
         *   the query along with the appropiate HTTP code. Since the HTTP query is performed asynchronously, the
         *   return object is wrapped in Task<>.
         */
        public async Task<Tuple<VideoAnnotationResponse, ResponseStatus>> AnnotateVideoWithLabelDetection(String inputUri = "",
            String inputContent = "", VideoContext context = null, String outputUri = "", String cloudRegionId = "") {

            // One and only one of inputUri and inputContent can be set
            if ((BasicFunctions.isEmpty(inputUri) && BasicFunctions.isEmpty(inputContent))) {
                return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.TOO_FEW_PARAMETERS);                
            }
            if (!BasicFunctions.isEmpty(inputUri) && !BasicFunctions.isEmpty(inputContent)) {
                return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.TOO_MANY_PARAMETERS);
            }

            // Create the features list and the annotation request object
            List<String> videoFeatures = new List<String> {
                VideoFeature.LABEL_DETECTION.ToString()
            };

            AnnotateVideoRequest annotateVideoRequest = new AnnotateVideoRequest(inputUri, inputContent, videoFeatures, context, outputUri, cloudRegionId);

            // Setting up the header for the request body
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API address to which we will make the HTTP POST query
            String request_query = "v1/videos:annotate?" + $"key={APIKey}";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, annotateVideoRequest);

            Stream stream = response.Content.ReadAsStreamAsync().Result;
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            
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
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.DESERIALIZATION_ERROR);
                }
                
                // We get the operation details for the annotation
                String operation_json = await GetVideoAnnotateOperation(operation.Name);
                if (BasicFunctions.isEmpty(operation_json)) {
                    return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.INTERNAL_SERVER_ERROR);
                }

                try {
                    VideoAnnotationResponse annotationResponse = JsonConvert.DeserializeObject<VideoAnnotationResponse>(operation_json);
                    
                    // We run a loop that sleeps for 10 seconds and then rechecks to see if the annotation is complete
                    while (!annotationResponse.Done) {
                        System.Threading.Thread.Sleep(SLEEP_TIME);

                        operation_json = await GetVideoAnnotateOperation(operation.Name);
                        if (BasicFunctions.isEmpty(operation_json)) {
                            return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.INTERNAL_SERVER_ERROR);
                        }

                        annotationResponse = JsonConvert.DeserializeObject<VideoAnnotationResponse>(operation_json);
                    }

                    return new Tuple<VideoAnnotationResponse, ResponseStatus>(annotationResponse, VideoAnnotationStatus.OK);
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }

        /*
         * Method: AnnotateVideoWithShotChangeDetection
         * 
         * Description: This method can be used to shot changes in the video using the Google Cloud
         *   Video Intelligence API. If you wish to run multiple annotations at once, please use the 
         *   AnnotateVideoWithMultipleDetections() method.
         * 
         * Parameters:
         * Only one of the following two parameters should be provided. If none or both are provided, then an
         * error will be returned:
         *  - inputUri (String): This string is the URI location of the video. Currently, only Google Cloud 
         *      Storage URIs are supported. If this is set, then inputContent should not be set.
         *  - inputContent (String): Bytes format string for the video data. This should be set only if inputUri 
         *      is not set.
         *      
         *  - context (VideoContext): Additional context for the video or parameters specific to Label Annotation.
         *      This is an OPTIONAL parameter.
         *  - outputUri (String): OPTIONAL Google Cloud Storage URI where the resulting JSON will be stored.
         *  - cloudRegionId (String): OPTIONAL Google Cloud region id where the video annotation should take place.
         *      If no region is specified then one will be chosen based on the file's location.
         *      
         * Return: The method returns a tuple of two items. If the query is successful, then the first item will
         *   be an object of type VideoAnnotationResponse. If the query is unsuccessful and an error is returned,
         *   then the method returns null. The second element is a ResponseStatus object indicating the status of
         *   the query along with the appropiate HTTP code. Since the HTTP query is performed asynchronously, the
         *   return object is wrapped in Task<>.
         */
        public async Task<Tuple<VideoAnnotationResponse, ResponseStatus>> AnnotateVideoWithShotChangeDetection (
            String inputUri = "", String inputContent = "", VideoContext context = null, String outputUri = "",
            String cloudRegionId = "") {

            // One and only one of inputUri and inputContent can be set
            if ((BasicFunctions.isEmpty(inputUri) && BasicFunctions.isEmpty(inputContent))) {
                return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.TOO_FEW_PARAMETERS);
            }
            if (!BasicFunctions.isEmpty(inputUri) && !BasicFunctions.isEmpty(inputContent)) {
                return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.TOO_MANY_PARAMETERS);
            }

            List<String> videoFeatures = new List<String> {
                VideoFeature.SHOT_CHANGE_DETECTION.ToString()
            };

            AnnotateVideoRequest annotateVideoRequest = new AnnotateVideoRequest(inputUri, inputContent, videoFeatures, context, outputUri, cloudRegionId);

            // Setting up the header for the request body
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API address to which we will make the HTTP POST query
            String request_query = "v1/videos:annotate?" + $"key={APIKey}";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, annotateVideoRequest);

            Stream stream = response.Content.ReadAsStreamAsync().Result;
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();

            // Similar two-step hop as we have seen in prior methods
            if (response.IsSuccessStatusCode) {
                Operation operation;
                try {
                    operation = JsonConvert.DeserializeObject<Operation>(response_str);
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.DESERIALIZATION_ERROR);
                }

                // We get the operation details for the annotation
                String operation_json = await GetVideoAnnotateOperation(operation.Name);
                if (BasicFunctions.isEmpty(operation_json)) {
                    return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.INTERNAL_SERVER_ERROR);
                }

                try {
                    VideoAnnotationResponse annotationResponse = JsonConvert.DeserializeObject<VideoAnnotationResponse>(operation_json);

                    // We run a loop that sleeps for 10 seconds and then rechecks to see if the annotation is complete
                    while (!annotationResponse.Done) {
                        System.Threading.Thread.Sleep(10000);
                        operation_json = await GetVideoAnnotateOperation(operation.Name);
                        if (BasicFunctions.isEmpty(operation_json)) {
                            return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.INTERNAL_SERVER_ERROR);
                        }

                        annotationResponse = JsonConvert.DeserializeObject<VideoAnnotationResponse>(operation_json);
                    }

                    return new Tuple<VideoAnnotationResponse, ResponseStatus>(annotationResponse, VideoAnnotationStatus.OK);
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }

        /*
         * Method: AnnotateVideoWithExplicitContentDetection
         * 
         * Description: This method can be used to detect adult content in a video using the Google Cloud
         *   Video Intelligence API. If you wish to run multiple annotations at once, please use the 
         *   AnnotateVideoWithMultipleDetections() method.
         * 
         * Parameters:
         * Only one of the following two parameters should be provided. If none or both are provided, then an
         * error will be returned:
         *  - inputUri (String): This string is the URI location of the video. Currently, only Google Cloud 
         *      Storage URIs are supported. If this is set, then inputContent should not be set.
         *  - inputContent (String): Bytes format string for the video data. This should be set only if inputUri 
         *      is not set.
         *      
         *  - context (VideoContext): Additional context for the video or parameters specific to Label Annotation.
         *      This is an OPTIONAL parameter.
         *  - outputUri (String): OPTIONAL Google Cloud Storage URI where the resulting JSON will be stored.
         *  - cloudRegionId (String): OPTIONAL Google Cloud region id where the video annotation should take place.
         *      If no region is specified then one will be chosen based on the file's location.
         *      
         * Return: The method returns a tuple of two items. If the query is successful, then the first item will
         *   be an object of type VideoAnnotationResponse. If the query is unsuccessful and an error is returned,
         *   then the method returns null. The second element is a ResponseStatus object indicating the status of
         *   the query along with the appropiate HTTP code. Since the HTTP query is performed asynchronously, the
         *   return object is wrapped in Task<>.
         */
        public async Task<Tuple<VideoAnnotationResponse, ResponseStatus>> AnnotateVideoWithExplicitContentDetection(
            String inputUri = "", String inputContent = "", VideoContext context = null, String outputUri = "",
            String cloudRegionId = "") {

            // One and only one of inputUri and inputContent can be set
            if ((BasicFunctions.isEmpty(inputUri) && BasicFunctions.isEmpty(inputContent))) {
                return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.TOO_FEW_PARAMETERS);
            }
            if (!BasicFunctions.isEmpty(inputUri) && !BasicFunctions.isEmpty(inputContent)) {
                return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.TOO_MANY_PARAMETERS);
            }

            List<String> videoFeatures = new List<String> {
                VideoFeature.EXPLICIT_CONTENT_DETECTION.ToString()
            };

            AnnotateVideoRequest annotateVideoRequest = new AnnotateVideoRequest(inputUri, inputContent, videoFeatures, context, outputUri, cloudRegionId);

            // Setting up the header for the request body
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API address to which we will make the HTTP POST query
            String request_query = "v1/videos:annotate?" + $"key={APIKey}";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, annotateVideoRequest);

            Stream stream = response.Content.ReadAsStreamAsync().Result;
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();

            // Similar two-step hop as we have seen in prior functions
            if (response.IsSuccessStatusCode) {
                Operation operation;
                try {
                    operation = JsonConvert.DeserializeObject<Operation>(response_str);
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.DESERIALIZATION_ERROR);
                }

                // We get the operation details for the annotation
                String operation_json = await GetVideoAnnotateOperation(operation.Name);
                try {
                    VideoAnnotationResponse annotationResponse = JsonConvert.DeserializeObject<VideoAnnotationResponse>(operation_json);

                    // We run a loop that sleeps for 10 seconds and then rechecks to see if the annotation is complete
                    while (!annotationResponse.Done) {
                        System.Threading.Thread.Sleep(10000);
                        operation_json = await GetVideoAnnotateOperation(operation.Name);
                        annotationResponse = JsonConvert.DeserializeObject<VideoAnnotationResponse>(operation_json);
                    }

                    return new Tuple<VideoAnnotationResponse, ResponseStatus>(annotationResponse, VideoAnnotationStatus.OK);
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }


        /*
         * Method: AnnotateVideoWithMultipleDetections
         * 
         * Description: This method can be used to detect adult content in a video using the Google Cloud
         *   Video Intelligence API. If you wish to run multiple annotations at once, please use the 
         *   AnnotateVideoWithMultipleDetections() method.
         * 
         * Parameters:
         * Only one of the following two parameters should be provided. If none or both are provided, then an
         * error will be returned:
         *  - inputUri (String): This string is the URI location of the video. Currently, only Google Cloud 
         *      Storage URIs are supported. If this is set, then inputContent should not be set.
         *  - inputContent (String): Bytes format string for the video data. This should be set only if inputUri 
         *      is not set.
         *      
         * At least one of the following three parameters must be set to true:
         *  - labelDetection (Boolean): Set to true if you want to run label detection on the video
         *  - shotChangeDetection (Boolean): Set to true if you want to run shot change detection on the video
         *  - explicitContentDetection (Boolean): Set to true if you want to run explicit content detection on the
         *      video
         *      
         *  - context (VideoContext): Additional context for the video or parameters specific to Label Annotation.
         *      This is an OPTIONAL parameter.
         *  - outputUri (String): OPTIONAL Google Cloud Storage URI where the resulting JSON will be stored.
         *  - cloudRegionId (String): OPTIONAL Google Cloud region id where the video annotation should take place.
         *      If no region is specified then one will be chosen based on the file's location.
         *      
         * Return: The method returns a tuple of two items. If the query is successful, then the first item will
         *   be an object of type VideoAnnotationResponse. If the query is unsuccessful and an error is returned,
         *   then the method returns null. The second element is a ResponseStatus object indicating the status of
         *   the query along with the appropiate HTTP code. Since the HTTP query is performed asynchronously, the
         *   return object is wrapped in Task<>.
         */
        public async Task<Tuple<VideoAnnotationResponse, ResponseStatus>> AnnotateVideoWithMultipleDetections(
            String inputUri = null, String inputContent = null, VideoContext context = null,
            String outputUri = "", String cloudRegionId = "", Boolean labelDetection = false,
            Boolean shotChangeDetection = false, Boolean explicitContentDetection = false) {

            // One and only one of inputUri and inputContent can be set
            if ((BasicFunctions.isEmpty(inputUri) && BasicFunctions.isEmpty(inputContent))) {
                return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.TOO_FEW_PARAMETERS);
            }
            if (!BasicFunctions.isEmpty(inputUri) && !BasicFunctions.isEmpty(inputContent)) {
                return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.TOO_MANY_PARAMETERS);
            }

            if (shotChangeDetection == false && explicitContentDetection == false && labelDetection == false) {
                return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.MISSING_ANNOTATION_FIELD);
            }

            List<String> videoFeatures = new List<String>();

            if (labelDetection) {
                videoFeatures.Add(VideoFeature.LABEL_DETECTION.ToString());
            }
            if (shotChangeDetection) {
                videoFeatures.Add(VideoFeature.SHOT_CHANGE_DETECTION.ToString());
            }
            if (explicitContentDetection) {
                videoFeatures.Add(VideoFeature.EXPLICIT_CONTENT_DETECTION.ToString());
            }

            AnnotateVideoRequest annotateVideoRequest = new AnnotateVideoRequest(inputUri, inputContent, videoFeatures, context, outputUri, cloudRegionId);
            
            // Setting up the header for the request body
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API address to which we will make the HTTP POST query
            String request_query = "v1/videos:annotate?" + $"key={APIKey}";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, annotateVideoRequest);

            Stream stream = response.Content.ReadAsStreamAsync().Result;
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();

            // Similar two-step hop as we have seen in prior functions
            if (response.IsSuccessStatusCode) {
                Operation operation;
                try {
                    operation = JsonConvert.DeserializeObject<Operation>(response_str);
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.DESERIALIZATION_ERROR);
                }

                int ctr = 1;
                
                // We get the operation details for the annotation
                String operation_json = await GetVideoAnnotateOperation(operation.Name);
                try {
                    VideoAnnotationResponse annotationResponse = JsonConvert.DeserializeObject<VideoAnnotationResponse>(operation_json);

                    // We run a loop that sleeps for 10 seconds and then rechecks to see if the annotation is complete.
                    // For every iteration of the loop, we increase 
                    while (!annotationResponse.Done) { 
                        System.Threading.Thread.Sleep(10000*ctr);
                        operation_json = await GetVideoAnnotateOperation(operation.Name);
                        annotationResponse = JsonConvert.DeserializeObject<VideoAnnotationResponse>(operation_json);
                        ctr++;
                    }

                    return new Tuple<VideoAnnotationResponse, ResponseStatus>(annotationResponse, VideoAnnotationStatus.OK);
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, VideoAnnotationStatus.DESERIALIZATION_ERROR);
                }
            }
            else {
                return new Tuple<VideoAnnotationResponse, ResponseStatus>(null, new ResponseStatus((int) response.StatusCode, response.ReasonPhrase));
            }
        }
    }
}
