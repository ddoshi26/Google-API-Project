using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    class ImageIntelligence {

        //private static readonly HttpClient client = new HttpClient();

        public List<AnnotateImageResponse> AnnotateImage(AnnotateImageRequests[] imageRequests) {
            if (imageRequests == null || imageRequests.Length == 0) {
                return null;
            }

            List<AnnotateImageRequests> imageRequestList = imageRequests.ToList();

            String request_body = JsonConvert.SerializeObject(imageRequestList);

            // Make Httprequest
            HttpWebRequest httpRequest = WebRequest.Create("https://vision.googleapis.com/v1/images:annotate") as HttpWebRequest;
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
            AnnotateImageResponseList responseList;

            try {
                responseList = JsonConvert.DeserializeObject<AnnotateImageResponseList>(result_json);
            } catch (JsonSerializationException e) {
                Console.WriteLine(e.StackTrace);
                return null;
            }

            return responseList.Responses;
        }
    }
}
