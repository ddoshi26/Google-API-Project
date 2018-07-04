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
            httpClient.BaseAddress = new Uri("https://language.googleapis.com/");
        }

        public AnalyzeEntitiesResponse AnalyzeEntities(Document document, EncodingType encodingType) {
            return null;
        }

        /*
         * Method: AnalyzeEntitySentiment
         * 
         * Description: This method can be used to find entities in a document or text and analyze the sentiment 
         *   associated with each entity. If you only want to idnetifiy entities, then please use AnalyzeEntities()
         *   method.
         * 
         * Parameters:
         *  - APIKey (String): This represents the Gooogle Cloud Services API access key. For more details:
         *      https://developers.google.com/places/web-service/get-api-key.
         *  - document (Document): The document/text in which you want Natural Language API to identify and 
         *      analyze entity sentiments
         *  - encodingType (EncodingType): The encoding type to help the API determine offsets. Acceptable values
         *      are NONE, UTF8, UTF16, UTF32. If NONE is specified, then encoding-specific information is set to -1.
         *      
         * Return: If the query is successful, then the method will return an object of type 
         *   AnalyzeEntitiesResponse, which conatins an array of all the identified antities and the language of the
         *   document. Since the HTTP query and the indentification are both performed asynchronously, the return 
         *   object is wrapped in Task<>. If the query is unsuccessful and an error is returned, then the method 
         *   returns null.
         */
        public async Task<AnalyzeEntitiesResponse> AnalyzeEntitySentiment(String APIKey, Document document, EncodingType encodingType) {
            if (document == null || BasicFunctions.isEmpty(APIKey)) {
                return null;
            }

            AnalyzeEntitiesRequest entitiesRequest = new AnalyzeEntitiesRequest(document, encodingType);
            if (entitiesRequest == null) {
                return null;
            }

            // Preparing the header to send a JSON request body
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API address to which we make the HTTP POST query
            String request_query = "v1/documents:analyzeEntitySentiment?" + $"key={APIKey}";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, entitiesRequest);

            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            Console.WriteLine(response_str);
            /* 
             * Similar two-step hop as we have seen before. We try to deserialize the response string, expecting
             * an object of AnalyzeEntitiesResponse. If the response is not an AnalyzeEntitiesResponse object, 
             * then we will encounter a JSONSerialization error and return null. If it is as we expect, then we 
             * just return the AnalyzeEntitiesResponse object.
             */
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
                return null;
            }
        }

        public AnalyzeSentimentResponse AnalyzeSentiment(Document document, EncodingType encodingType) {
            return null;
        }

        public AnalyzeSyntaxResponse AnalyzeSyntax(Document document, EncodingType encodingType) {
            return null;
        }

        public AnnotateTextResponse AnnotateText(Document document, TextFeatures features, EncodingType encodingType) {
            return null;
        }

        public ClassifyTextResponse ClassifyText(Document document) {
            return null;
        }
    }
}
