using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class NaturalLanguageIntelligence {
        private static HttpClient httpClient;
        private static String APIKey;

        public NaturalLanguageIntelligence(GoogleCloudClassSetup setup) {
            httpClient = new HttpClient();

            // Since this is the first time we use the httpClient, we need to intialize its base address
            httpClient.BaseAddress = new Uri(setup.getAPIUrl("NATURAL_LANGUAGE_INTELLIGENCE_API_URL").ToString());
            APIKey = setup.getAPIKey();
        }

        public void UpdateURL(GoogleCloudClassSetup setup) {
            httpClient.BaseAddress = new Uri(setup.getAPIUrl("NATURAL_LANGUAGE_INTELLIGENCE_API_URL").ToString());
        }

        public void UpdateKey(GoogleCloudClassSetup setup) {
            APIKey = setup.getAPIKey();
        }

        /*
         * Method: AnalyzeEntities
         * 
         * Description: This method can be used to find entities in a document or text. If you only wish to run
         *   sentiment analysis on each entity, simultaneously, then please use the AnalyzeEntitySentiment()
         *   method.
         * 
         * Parameters:
         *  - document (Document): The document/text from which you want Natural Language API to identify and 
         *      analyze entities.
         *  - encodingType (EncodingType): The encoding type to help the API determine offsets. Acceptable values
         *      are NONE, UTF8, UTF16, UTF32. If NONE is specified, then encoding-specific information is not set.
         *      
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *      creating an object of this class. For more details about the Google API Key please see:
         *      https://developers.google.com/places/web-service/get-api-key
         *      
         * Return: The method returns a tuple of two items. If the query is successful, then the first item will
         *   be an object of type AnalyzeEntitiesResponse, which conatins a list of all the entities identified and
         *   the language of the document. If the query is unsuccessful and an error is returned, then the method
         *   returns null. The second element is a ResponseStatus object indicating the status of the query along
         *   with the appropiate HTTP code. Since the HTTP query is performed asynchronously, the return object is
         *   wrapped in Task<>. 
         */
        public async Task<Tuple<AnalyzeEntitiesResponse, ResponseStatus>> AnalyzeEntities(Document document, EncodingType encodingType) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null, NaturalLanguageStatus.MISSING_API_KEY);
            }
            if (document == null) {
                return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null, NaturalLanguageStatus.MISSING_DOCUMENT);
            }

            AnalyzeEntitiesRequest entitiesRequest = new AnalyzeEntitiesRequest(document, encodingType);
            if (entitiesRequest == null) {
                return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null, NaturalLanguageStatus.INTERNAL_SERVER_ERROR);
            }

            // Preparing the header to send a JSON request body
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API address to which we make the HTTP POST query
            String request_query = "v1/documents:analyzeEntities?" + $"key={APIKey}";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, entitiesRequest);

            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            
            /* 
             * Similar two-step hop as we have seen before. We try to deserialize the response string, expecting
             * an object of AnalyzeEntitiesResponse. If the response is not an AnalyzeEntitiesResponse object, 
             * then we will encounter a JSONSerialization error and return null. If it is as we expect, then we 
             * just return the AnalyzeEntitiesResponse object, so long as it is not empty or null.
             */
            if (response.IsSuccessStatusCode) {
                AnalyzeEntitiesResponse entitiesResponse;

                try {
                    entitiesResponse = JsonConvert.DeserializeObject<AnalyzeEntitiesResponse>(response_str);

                    if (entitiesResponse == null || entitiesResponse.Entities.Count == 0) {
                        return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null, NaturalLanguageStatus.ZERO_RESULTS);
                    }
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null, NaturalLanguageStatus.DESERIALIZATION_ERROR);
                }

                return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(entitiesResponse, NaturalLanguageStatus.OK);
            }
            else {
                // If the query is not successful, then we try to extract details about the error from the response
                AnalyzeEntitiesResponse entitiesResponse;

                try {
                    entitiesResponse = JsonConvert.DeserializeObject<AnalyzeEntitiesResponse>(response_str);
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null, NaturalLanguageStatus.DESERIALIZATION_ERROR);
                }

                // If no error details are available, then we use the information from the response to determine
                // the appropriate error code
                if (entitiesResponse.Error == null) {
                    return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null,
                        NaturalLanguageStatus.processErrorMessage(response.StatusCode.ToString(), response.ReasonPhrase));
                }
                else {
                    // If we do have an Error object, then we use it to identify the appropriate error code and message
                    return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null,
                        NaturalLanguageStatus.processErrorMessage(entitiesResponse.Error.Code.ToString(), entitiesResponse.Error.Message));
                }
            }
        }

        /*
         * Method: AnalyzeEntitySentiment
         * 
         * Description: This method can be used to find entities in a document or text and analyze the sentiment 
         *   associated with each entity. If you only want to identify entities, then please use AnalyzeEntities()
         *   method.
         * 
         * Parameters:
         *  - document (Document): TThe document/text on which you want Natural Language API to perform analysis.
         *  - encodingType (EncodingType): The encoding type to help the API determine offsets. Acceptable values
         *      are NONE, UTF8, UTF16, UTF32. If NONE is specified, then encoding-specific information is not set.
         *      
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *      creating an object of this class. For more details about the Google API Key please see:
         *      https://developers.google.com/places/web-service/get-api-key
         *      
         * Return: The method returns a tuple of two items. If the query is successful, then the first item will
         *   be an object of type AnalyzeEntitiesResponse, which conatins a list of all the entities identified and
         *   the language of the document. If the query is unsuccessful and an error is returned, then the method
         *   returns null. The second element is a ResponseStatus object indicating the status of the query along
         *   with the appropiate HTTP code. Since the HTTP query is performed asynchronously, the return object is
         *   wrapped in Task<>. 
         */
        public async Task<Tuple<AnalyzeEntitiesResponse, ResponseStatus>> AnalyzeEntitySentiment(
            Document document, EncodingType encodingType) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null, NaturalLanguageStatus.MISSING_API_KEY);
            }
            if (document == null) {
                return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null, NaturalLanguageStatus.MISSING_DOCUMENT);
            }

            AnalyzeEntitiesRequest entitiesRequest = new AnalyzeEntitiesRequest(document, encodingType);
            if (entitiesRequest == null) {
                return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null, NaturalLanguageStatus.INTERNAL_SERVER_ERROR);
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

                    if (entitiesResponse == null || entitiesResponse.Entities.Count == 0) {
                        return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null, NaturalLanguageStatus.ZERO_RESULTS);
                    }
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null, NaturalLanguageStatus.DESERIALIZATION_ERROR);
                }

                return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(entitiesResponse, NaturalLanguageStatus.OK);
            }
            else {
                // If the query is not successful, then we try to extract details about the error from the response
                AnalyzeEntitiesResponse entitiesResponse;

                try {
                    entitiesResponse = JsonConvert.DeserializeObject<AnalyzeEntitiesResponse>(response_str);
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null, NaturalLanguageStatus.DESERIALIZATION_ERROR);
                }

                // If no error details are available, then we use the information from the response to determine
                // the appropriate error code
                if (entitiesResponse.Error == null) {
                    return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null,
                        NaturalLanguageStatus.processErrorMessage(response.StatusCode.ToString(), response.ReasonPhrase));
                }
                else {
                    // If we do have an Error object, then we use it to identify the appropriate error code and message
                    return new Tuple<AnalyzeEntitiesResponse, ResponseStatus>(null,
                        NaturalLanguageStatus.processErrorMessage(entitiesResponse.Error.Code.ToString(), entitiesResponse.Error.Message));
                }
            }
        }

        /*
         * Method: AnalyzeSentiment
         * 
         * Description: This method can be used to analyze the sentiment of a document as a whole as well as on a
         *   sentence by sentence basis. If you only want to identify entities, then please use AnalyzeEntities()
         *   method.
         * 
         * Parameters:
         *  - document (Document): The document/text on which you want Natural Language API to perform analysis.
         *  - encodingType (EncodingType): The encoding type to help the API determine offsets. Acceptable values
         *      are NONE, UTF8, UTF16, UTF32. If NONE is specified, then encoding-specific information is not set.
         *      
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *      creating an object of this class. For more details about the Google API Key please see:
         *      https://developers.google.com/places/web-service/get-api-key
         *      
         * Return: The method returns a tuple of two items. If the query is successful, then the first item will
         *   be an object of type AnalyzeSentimentResponse. If the query is unsuccessful and an error is returned,
         *   then the method returns null. The second element is a ResponseStatus object indicating the status of
         *   the query along with the appropiate HTTP code. Since the HTTP query is performed asynchronously, the
         *   return object is wrapped in Task<>. 
         */
        public async Task<Tuple<AnalyzeSentimentResponse, ResponseStatus>> AnalyzeSentiment(Document document,
            EncodingType encodingType) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<AnalyzeSentimentResponse, ResponseStatus>(null, NaturalLanguageStatus.MISSING_API_KEY);
            }
            if (document == null) {
                return new Tuple<AnalyzeSentimentResponse, ResponseStatus>(null, NaturalLanguageStatus.MISSING_DOCUMENT);
            }

            AnalyzeEntitiesRequest entitiesRequest = new AnalyzeEntitiesRequest(document, encodingType);
            if (entitiesRequest == null) {
                return new Tuple<AnalyzeSentimentResponse, ResponseStatus>(null, NaturalLanguageStatus.INTERNAL_SERVER_ERROR);
            }

            // Preparing the header to send a JSON request body
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API address to which we make the HTTP POST query
            String request_query = "v1/documents:analyzeSentiment?" + $"key={APIKey}";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, entitiesRequest);

            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            
            // Similar two-step hop as we have seen in prior methods
            if (response.IsSuccessStatusCode) {
                AnalyzeSentimentResponse sentimentResponse;

                try {
                    sentimentResponse = JsonConvert.DeserializeObject<AnalyzeSentimentResponse>(response_str);

                    if (sentimentResponse == null || sentimentResponse.Sentences.Count == 0) {
                        return new Tuple<AnalyzeSentimentResponse, ResponseStatus>(null, NaturalLanguageStatus.ZERO_RESULTS);
                    }
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<AnalyzeSentimentResponse, ResponseStatus>(null, NaturalLanguageStatus.DESERIALIZATION_ERROR);
                }

                return new Tuple<AnalyzeSentimentResponse, ResponseStatus>(sentimentResponse, NaturalLanguageStatus.OK);
            }
            else {
                // If the query is not successful, then we try to extract details about the error from the response
                AnalyzeSentimentResponse sentimentResponse;

                try {
                    sentimentResponse = JsonConvert.DeserializeObject<AnalyzeSentimentResponse>(response_str);
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<AnalyzeSentimentResponse, ResponseStatus>(null, NaturalLanguageStatus.DESERIALIZATION_ERROR);
                }

                // If no error details are available, then we use the information from the response to determine
                // the appropriate error code
                if (sentimentResponse.Error == null) {
                    return new Tuple<AnalyzeSentimentResponse, ResponseStatus>(null,
                        NaturalLanguageStatus.processErrorMessage(response.StatusCode.ToString(), response.ReasonPhrase));
                }
                else {
                    // If we do have an Error object, then we use it to identify the appropriate error code and message
                    return new Tuple<AnalyzeSentimentResponse, ResponseStatus>(null,
                        NaturalLanguageStatus.processErrorMessage(sentimentResponse.Error.Code.ToString(), sentimentResponse.Error.Message));
                }
            }
        }

        /*
         * Method: AnalyzeSyntax
         * 
         * Description: This method can be used to analyze the syntax of the text from a document and provide
         *   sentence boundaries and tokenization along with properties about each element.
         * 
         * Parameters:
         *  - document (Document): The document/text on which you want Natural Language API to perform analysis.
         *  - encodingType (EncodingType): The encoding type to help the API determine offsets. Acceptable values
         *      are NONE, UTF8, UTF16, UTF32. If NONE is specified, then encoding-specific information is not set.
         *      
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *      creating an object of this class. For more details about the Google API Key please see:
         *      https://developers.google.com/places/web-service/get-api-key
         *      
         * Return: The method returns a tuple of two items. If the query is successful, then the first item will
         *   be an object of type AnalyzeSyntaxResponse. If the query is unsuccessful and an error is returned,
         *   then the method returns null. The second element is a ResponseStatus object indicating the status of
         *   the query along with the appropiate HTTP code. Since the HTTP query is performed asynchronously, the
         *   return object is wrapped in Task<>. 
         */
        public async Task<Tuple<AnalyzeSyntaxResponse, ResponseStatus>> AnalyzeSyntax(Document document,
            EncodingType encodingType) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<AnalyzeSyntaxResponse, ResponseStatus>(null, NaturalLanguageStatus.MISSING_API_KEY);
            }
            if (document == null) {
                return new Tuple<AnalyzeSyntaxResponse, ResponseStatus>(null, NaturalLanguageStatus.MISSING_DOCUMENT);
            }

            AnalyzeEntitiesRequest entitiesRequest = new AnalyzeEntitiesRequest(document, encodingType);
            if (entitiesRequest == null) {
                return new Tuple<AnalyzeSyntaxResponse, ResponseStatus>(null, NaturalLanguageStatus.INTERNAL_SERVER_ERROR);
            }

            // Preparing the header to send a JSON request body
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API address to which we make the HTTP POST query
            String request_query = "v1/documents:analyzeSyntax?" + $"key={APIKey}";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, entitiesRequest);

            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            
            // Similar two-step hop as we have seen in prior methods.
            if (response.IsSuccessStatusCode) {
                AnalyzeSyntaxResponse syntaxResponse;

                try {
                    syntaxResponse = JsonConvert.DeserializeObject<AnalyzeSyntaxResponse>(response_str);

                    if (syntaxResponse == null || (syntaxResponse.Sentences.Count == 0 && syntaxResponse.Tokens.Count == 0)) {
                        return new Tuple<AnalyzeSyntaxResponse, ResponseStatus>(null, NaturalLanguageStatus.ZERO_RESULTS);
                    }
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<AnalyzeSyntaxResponse, ResponseStatus>(null, NaturalLanguageStatus.DESERIALIZATION_ERROR);
                }

                return new Tuple<AnalyzeSyntaxResponse, ResponseStatus>(syntaxResponse, NaturalLanguageStatus.OK);
            }
            else {
                // If the query is not successful, then we try to extract details about the error from the response
                AnalyzeSyntaxResponse syntaxResponse;

                try {
                    syntaxResponse = JsonConvert.DeserializeObject<AnalyzeSyntaxResponse>(response_str);
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<AnalyzeSyntaxResponse, ResponseStatus>(null, NaturalLanguageStatus.DESERIALIZATION_ERROR);
                }

                /* 
                 * If no error details are available, then we use the information from the response to determine
                 * the appropriate error code.
                 * If we do have an Error object, then we use it to identify the appropriate error code and message
                 */  
                if (syntaxResponse.Error == null) {
                    return new Tuple<AnalyzeSyntaxResponse, ResponseStatus>(null,
                        NaturalLanguageStatus.processErrorMessage(response.StatusCode.ToString(), response.ReasonPhrase));
                }
                else {
                    return new Tuple<AnalyzeSyntaxResponse, ResponseStatus>(null,
                        NaturalLanguageStatus.processErrorMessage(syntaxResponse.Error.Code.ToString(), syntaxResponse.Error.Message));
                }
            }
        }

        /*
         * Method: AnnotateText
         * 
         * Description: This method combines all the features of analyzeEntities, analyzeSentiment, and
         *   analyzeSytax into one implementation. All the fields returned by those individual methods will be
         *   combined into an AnnotateTextResponse object and returned.
         * 
         * Parameters:
         *  - document (Document): The document/text on which you want Natural Language API to perform analysis.
         *  - encodingType (EncodingType): The encoding type to help the API determine offsets. Acceptable values
         *      are NONE, UTF8, UTF16, UTF32. If NONE is specified, then encoding-specific information is not set.
         *      
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *      creating an object of this class. For more details about the Google API Key please see:
         *      https://developers.google.com/places/web-service/get-api-key
         *      
         * Return: The method returns a tuple of two items. If the query is successful, then the first item will
         *   be an object of type AnnotateTextResponse. If the query is unsuccessful and an error is returned,
         *   then the method returns null. The second element is a ResponseStatus object indicating the status of
         *   the query along with the appropiate HTTP code. Since the HTTP query is performed asynchronously, the
         *   return object is wrapped in Task<>. 
         */
        public async Task<Tuple<AnnotateTextResponse, ResponseStatus>> AnnotateText(Document document, 
            TextFeatures features, EncodingType encodingType) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<AnnotateTextResponse, ResponseStatus>(null, NaturalLanguageStatus.MISSING_API_KEY);
            }
            if (features == null) {
                return new Tuple<AnnotateTextResponse, ResponseStatus>(null, NaturalLanguageStatus.MISSING_FEATURES);
            }
            if (document == null) {
                return new Tuple<AnnotateTextResponse, ResponseStatus>(null, NaturalLanguageStatus.MISSING_DOCUMENT);
            }

            AnnotateTextRequest textRequest = new AnnotateTextRequest(document, features, encodingType);
            if (textRequest == null) {
                return new Tuple<AnnotateTextResponse, ResponseStatus>(null, NaturalLanguageStatus.INTERNAL_SERVER_ERROR);
            }

            // Preparing the header to send a JSON request body
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API address to which we make the HTTP POST query
            String request_query = "v1/documents:annotateText?" + $"key={APIKey}";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, textRequest);

            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            
            // Similar two-step hop as we have seen in prior methods.
            if (response.IsSuccessStatusCode) {
                AnnotateTextResponse annotateResponse;

                try {
                    annotateResponse = JsonConvert.DeserializeObject<AnnotateTextResponse>(response_str);

                    if (annotateResponse == null || (annotateResponse.Sentences.Count == 0 && 
                        annotateResponse.Categories.Count == 0 && annotateResponse.Tokens.Count == 0 &&
                        annotateResponse.Entities.Count == 0)) {
                        return new Tuple<AnnotateTextResponse, ResponseStatus>(null, NaturalLanguageStatus.ZERO_RESULTS);
                    }
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<AnnotateTextResponse, ResponseStatus>(null, NaturalLanguageStatus.DESERIALIZATION_ERROR);
                }

                return new Tuple<AnnotateTextResponse, ResponseStatus>(annotateResponse, NaturalLanguageStatus.OK);
            }
            else {
                // If the query is not successful, then we try to extract details about the error from the response
                AnnotateTextResponse annotateResponse;

                try {
                    annotateResponse = JsonConvert.DeserializeObject<AnnotateTextResponse>(response_str);
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<AnnotateTextResponse, ResponseStatus>(null, NaturalLanguageStatus.DESERIALIZATION_ERROR);
                }

                /* 
                 * If no error details are available, then we use the information from the response to determine
                 * the appropriate error code.
                 * If we do have an Error object, then we use it to identify the appropriate error code and message
                 */
                if (annotateResponse.Error == null) {
                    return new Tuple<AnnotateTextResponse, ResponseStatus>(null,
                        NaturalLanguageStatus.processErrorMessage(response.StatusCode.ToString(), response.ReasonPhrase));
                }
                else {
                    return new Tuple<AnnotateTextResponse, ResponseStatus>(null,
                        NaturalLanguageStatus.processErrorMessage(annotateResponse.Error.Code.ToString(), annotateResponse.Error.Message));
                }
            }
        }

        /*
         * Method: ClassifyText
         * 
         * Description: This method can be used to classify the document into categories. Each category is
         *   identified by a name and associated with a confidence number indicating how confident the API is
         *   about the classification.
         *   
         * Parameters:
         *  - document (Document): The document/text on which you want Natural Language API to perform analysis.
         *      
         *  - APIKey (String): Implicity required paramter which should be set through the constructor when
         *      creating an object of this class. For more details about the Google API Key please see:
         *      https://developers.google.com/places/web-service/get-api-key
         *      
         * Return: The method returns a tuple of two items. If the query is successful, then the first item will
         *   be an object of type ClassifyTextResponse. If the query is unsuccessful and an error is returned,
         *   then the method returns null. The second element is a ResponseStatus object indicating the status of
         *   the query along with the appropiate HTTP code. Since the HTTP query is performed asynchronously, the
         *   return object is wrapped in Task<>. 
         */
        public async Task<Tuple<ClassifyTextResponse, ResponseStatus>> ClassifyText(Document document) {
            if (BasicFunctions.isEmpty(APIKey)) {
                return new Tuple<ClassifyTextResponse, ResponseStatus>(null, NaturalLanguageStatus.MISSING_API_KEY);
            }
            if (document == null) {
                return new Tuple<ClassifyTextResponse, ResponseStatus>(null, NaturalLanguageStatus.MISSING_DOCUMENT);
            }

            ClassifyTextRequest classifyTextRequest = new ClassifyTextRequest(document);
            if (classifyTextRequest == null) {
                return new Tuple<ClassifyTextResponse, ResponseStatus>(null, NaturalLanguageStatus.INTERNAL_SERVER_ERROR);
            }

            // Preparing the header to send a JSON request body
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API address to which we make the HTTP POST query
            String request_query = "v1/documents:classifyText?" + $"key={APIKey}";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(request_query, classifyTextRequest);

            Stream stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            String response_str = streamReader.ReadToEnd();
            
            // Similar two-step hop as we have seen in prior methods
            if (response.IsSuccessStatusCode) {
                ClassifyTextResponse classifyTextResponse;

                try {
                    classifyTextResponse = JsonConvert.DeserializeObject<ClassifyTextResponse>(response_str);

                    if (classifyTextResponse == null || classifyTextResponse.Categories.Count == 0) {
                        return new Tuple<ClassifyTextResponse, ResponseStatus>(null, NaturalLanguageStatus.ZERO_RESULTS);
                    }
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<ClassifyTextResponse, ResponseStatus>(null, NaturalLanguageStatus.DESERIALIZATION_ERROR);
                }

                return new Tuple<ClassifyTextResponse, ResponseStatus>(classifyTextResponse, NaturalLanguageStatus.OK);
            }
            else {
                // If the query is not successful, then we try to extract details about the error from the response
                ClassifyTextResponse classifyTextResponse;

                try {
                    classifyTextResponse = JsonConvert.DeserializeObject<ClassifyTextResponse>(response_str);
                } catch (JsonSerializationException e) {
                    Debug.WriteLine(e.StackTrace);
                    return new Tuple<ClassifyTextResponse, ResponseStatus>(null, NaturalLanguageStatus.DESERIALIZATION_ERROR);
                }

                /* 
                 * If no error details are available, then we use the information from the response to determine
                 * the appropriate error code.
                 * If we do have an Error object, then we use it to identify the appropriate error code and message
                 */
                if (classifyTextResponse.Error == null) {
                    return new Tuple<ClassifyTextResponse, ResponseStatus>(null,
                        NaturalLanguageStatus.processErrorMessage(response.StatusCode.ToString(), response.ReasonPhrase));
                }
                else {
                    return new Tuple<ClassifyTextResponse, ResponseStatus>(null,
                        NaturalLanguageStatus.processErrorMessage(classifyTextResponse.Error.Code.ToString(), classifyTextResponse.Error.Message));
                }
            }
        }
    }
}
