using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class NaturalLanguageIntelligence {

        private static HttpClient httpClient;

        public NaturalLanguageIntelligence() {
            httpClient = new HttpClient();
        }

        public AnalyzeEntitiesResponse AnalyzeEntities(Document document, EncodingType encodingType) {
            return null;
        }
        
        public async Task<AnalyzeEntitiesResponse> AnalyzeEntitySentiment(String APIKey, Document document, EncodingType encodingType) {
            if (document == null || BasicFunctions.isEmpty(APIKey)) {
                return null;
            }

            AnalyzeEntitiesRequest entitiesRequest = new AnalyzeEntitiesRequest(document, encodingType);
            if (entitiesRequest == null) {
                return null;
            }

            httpClient.BaseAddress = new Uri("https://language.googleapis.com/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            String request_query = "v1/documents:analyzeEntitySentiment?" + $"key={APIKey}";

            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, entitiesRequest);
            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();

            if (response.IsSuccessStatusCode) {
                AnalyzeEntitiesResponse entitiesResponse;
                try {
                    entitiesResponse = JsonConvert.DeserializeObject<AnalyzeEntitiesResponse>(response_str);
                } catch (JsonSerializationException e) {
                    Console.WriteLine(e.StackTrace);
                    return null;
                }

                return entitiesResponse;
            }
            else {
                Console.WriteLine(response_str);
                return null;
            }
        }

        public AnalyzeSentimentResponse AnalyzeSentiment(Document document, EncodingType encodingType) {
            return null;
        }

        public AnalyzeSyntaxResponse AnalyzeSyntax(Document document, EncodingType encodingType) {
            return null;
        }

        public AnnotateText AnnotateText(Document document, TextFeatures features, EncodingType encodingType) {
            return null;
        }

        public List<ClassificationCategory> ClassifyText(Document document) {
            return null;
        }
    }
}
