using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class VideoIntelligence {

        private static readonly HttpClient client = new HttpClient();

        public Operation AnnotateVideoWithLabelDetection(String videoUri, String videoData, VideoContext context,
            String outputUri = "", String cloudRegionId = "") {

            if ((BasicFunctions.isEmpty(videoUri) && BasicFunctions.isEmpty(videoData)) ||
                (!BasicFunctions.isEmpty(videoUri) && !BasicFunctions.isEmpty(videoData))) {
                return null;
            }

            AnnotateVideoRequest annotateVideoRequest = new AnnotateVideoRequest(videoUri, videoData, new VideoFeature[] { VideoFeature.LABEL_DETECTION }, context, outputUri, cloudRegionId);

            String request_body = JsonConvert.SerializeObject(annotateVideoRequest);

            HttpWebRequest httpRequest = WebRequest.Create("https://videointelligence.googleapis.com/v1/videos:annotate") as HttpWebRequest;
            httpRequest.ContentType = "application/json";
            httpRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream())) {
                streamWriter.Write(request_body);
                streamWriter.Flush();
                streamWriter.Close();
            }

            HttpWebResponse response = httpRequest.GetResponse() as HttpWebResponse;
            Stream response_stream = response.GetResponseStream();

            String result_json = BasicFunctions.processResponseStream(response_stream);
            Operation op_result;

            try {
                op_result = JsonConvert.DeserializeObject<Operation>(result_json);
            } catch (JsonSerializationException e) {
                Console.WriteLine(e.StackTrace);
                return null;
            }

            return op_result;
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
