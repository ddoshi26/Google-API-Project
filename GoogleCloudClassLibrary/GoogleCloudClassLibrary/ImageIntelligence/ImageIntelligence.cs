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
        }

        public async Task<List<AnnotateImageResponse>> AnnotateImage(String APIKey, List<AnnotateImageRequests> imageRequests) {
            if (imageRequests == null || imageRequests.Count == 0) {
                return null;
            }

            AnnotateImageRequestList imageRequestList = new AnnotateImageRequestList(imageRequests);
            
            httpClient.BaseAddress = new Uri("https://vision.googleapis.com/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            String request_query = "v1/images:annotate?" + $"key={APIKey}";

            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, imageRequestList);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();

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
