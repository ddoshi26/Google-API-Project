using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    class NaturalLanguageIntelligence {

        //private static readonly HttpClient httpClient = new HttpClient();

        public AnalyzeEntitiesResponse AnalyzeEntities(Document document, EncodingType encodingType) {
            return null;
        }
        
        public AnalyzeEntitiesResponse AnalyzeEntitySentiment(Document document, EncodingType encodingType) {
            if (document == null) {
                return null;
            }

            AnalyzeEntitiesRequest entitiesRequest = new AnalyzeEntitiesRequest(document, encodingType);
            String request_body = JsonConvert.SerializeObject(entitiesRequest);

            HttpWebRequest httpRequest = WebRequest.Create("https://language.googleapis.com/v1/documents:analyzeEntitySentiment") as HttpWebRequest;
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
            AnalyzeEntitiesResponse entitySentimentResponse;

            try {
                entitySentimentResponse = JsonConvert.DeserializeObject<AnalyzeEntitiesResponse>(result_json);
            } catch (JsonSerializationException e) {
                Console.WriteLine(e.StackTrace);
                return null;
            }

            return entitySentimentResponse;
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
