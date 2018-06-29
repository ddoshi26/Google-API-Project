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
        }

        public async Task<Operation> AnnotateVideoWithLabelDetection(String APIKey, String videoUri, String videoData, VideoContext context,
            String outputUri = "", String cloudRegionId = "") {

            if ((BasicFunctions.isEmpty(videoUri) && BasicFunctions.isEmpty(videoData)) ||
                (!BasicFunctions.isEmpty(videoUri) && !BasicFunctions.isEmpty(videoData))) {
                return null;
            }

            AnnotateVideoRequest annotateVideoRequest = new AnnotateVideoRequest(videoUri, videoData, new VideoFeature[] { VideoFeature.LABEL_DETECTION }, context, outputUri, cloudRegionId);

            httpClient.BaseAddress = new Uri("https://videointelligence.googleapis.com/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            String request_query = "v1/videos:annotate?" + $"key={APIKey}";

            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, annotateVideoRequest);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();

            if (response.IsSuccessStatusCode) {
                Operation operation;
                try {
                    operation = JsonConvert.DeserializeObject<Operation>(response_str);
                } catch (JsonSerializationException e) {
                    Console.WriteLine(e.StackTrace);
                    return null;
                }

                return operation;
            }
            else {
                Console.WriteLine(response_str);
                return null;
            }
        }

        public Operation AnnotateVideoWithShotChangeDetection(String videoUri, String videoData, VideoContext context,
            String outputUri = "", String cloudRegionId = "") {
            return null;
        }

        public Operation AnnotateVideoWithExplicitContentDetection(String videoUri, String videoData,
            VideoContext context, String outputUri = "", String cloudRegionId = "") {
            return null;
        }

        public Operation AnnotateVideoWithMultipleDetections(String videoUri, String videoData, VideoContext context,
            String outputUri = "", String cloudRegionId = "", Boolean labelDetection = false,
            Boolean shotChangeDetection = false, Boolean explicitContentDetection = false) {
            return null;
        }
    }
}
