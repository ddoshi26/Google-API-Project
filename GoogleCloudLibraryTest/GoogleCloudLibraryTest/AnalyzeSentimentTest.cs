using System;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using GoogleCloudClassLibrary.NaturalLanguageIntelligence;
using ResponseStatus = GoogleCloudClassLibrary.ResponseStatus;

namespace GoogleClassLibraryTest {
    [TestFixture]
    public class AnalyzeSentimentTest {
        private static NaturalLanguageIntelligence nlIntelligence;

        private GC.GoogleCloudClassSetup VALID_SETUP;
        private GC.GoogleCloudClassSetup INVALID_SETUP;

        private static readonly String INVALID_DOC_URL = "INVALID_DOC_URL";
        private static readonly String VALID_DOC_URL = "gs://gccl_dd_01/doc1.txt";

        private static String VALID_FILE_LOCATION;
        private static readonly String EMPTY_CONTENT = "";

        private static readonly Document MISSING_DOCUMENT = null;

        public AnalyzeSentimentTest() {
            String dir = Environment.GetEnvironmentVariable("GC_LIBRARY_TEST", EnvironmentVariableTarget.User);
            VALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\APIKEY.txt");
            INVALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\INVALID_APIKEY.txt");
            VALID_FILE_LOCATION = dir + "\\SampleDoc.txt";

            nlIntelligence = new NaturalLanguageIntelligence(VALID_SETUP);
        }

        [Test]
        public void AnalyzeSentiment_InvalidKey() {
            nlIntelligence.UpdateKey(INVALID_SETUP);

            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);

            Task<Tuple<AnalyzeSentimentResponse, ResponseStatus>> response = nlIntelligence.AnalyzeSentiment(document, EncodingType.UTF8);
            response.Wait();

            nlIntelligence.UpdateKey(VALID_SETUP);

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.INVALID_API_KEY);
        }

        [Test]
        public void AnalyzeSentiment_MissingDocument() {
            Task<Tuple<AnalyzeSentimentResponse, ResponseStatus>> response = nlIntelligence.AnalyzeSentiment(MISSING_DOCUMENT, EncodingType.UTF8);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.MISSING_DOCUMENT);
        }

        [Test]
        public void AnalyzeSentiment_InvalidDocumentURL() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: INVALID_DOC_URL);

            Task<Tuple<AnalyzeSentimentResponse, ResponseStatus>> response = nlIntelligence.AnalyzeSentiment(document, EncodingType.UTF8);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.INVALID_ARGUMENT);
        }

        [Test]
        public void AnalyzeSentiment_EmptyDocumentContent() {
            Document document = new Document(DocumentType.PLAIN_TEXT, content: EMPTY_CONTENT);

            Task<Tuple<AnalyzeSentimentResponse, ResponseStatus>> response = nlIntelligence.AnalyzeSentiment(document, EncodingType.UTF8);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.ZERO_RESULTS);
        }

        [Test]
        public void AnalyzeSentiment_ValidQueryWithDocumentURL() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);

            Task<Tuple<AnalyzeSentimentResponse, ResponseStatus>> response = nlIntelligence.AnalyzeSentiment(document, EncodingType.UTF8);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            AnalyzeSentimentResponse sentimentResponse = response.Result.Item1;

            Assert.IsNotNull(sentimentResponse.Sentences);
            Assert.GreaterOrEqual(sentimentResponse.Sentences.Count, 1);

            Assert.IsNotNull(sentimentResponse.Language);
            Assert.IsNotEmpty(sentimentResponse.Language);

            Assert.IsNotNull(sentimentResponse.Sentiment);
            Assert.GreaterOrEqual(sentimentResponse.Sentiment.Magnitude, 0);
            Assert.GreaterOrEqual(sentimentResponse.Sentiment.Score, -1);
            Assert.LessOrEqual(sentimentResponse.Sentiment.Score, 1);

            foreach (Sentence sentence in sentimentResponse.Sentences) {
                Assert.IsNotNull(sentence.Sentiment);

                Assert.GreaterOrEqual(sentence.Sentiment.Magnitude, 0);
                Assert.GreaterOrEqual(sentence.Sentiment.Score, -1);
                Assert.LessOrEqual(sentence.Sentiment.Score, 1);

                Assert.IsNotNull(sentence.Text);
                Assert.IsNotNull(sentence.Text.Content);
                Assert.GreaterOrEqual(sentence.Text.BeginOffset, 0);
            }
        }

        [Test]
        public void AnalyzeSentiment_ValidQueryWithDocumentContent() {
            Document document = new Document(DocumentType.PLAIN_TEXT, content: File.ReadAllText(VALID_FILE_LOCATION));

            Task<Tuple<AnalyzeSentimentResponse, ResponseStatus>> response = nlIntelligence.AnalyzeSentiment(document, EncodingType.UTF8);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            AnalyzeSentimentResponse sentimentResponse = response.Result.Item1;

            Assert.IsNotNull(sentimentResponse.Sentences);
            Assert.GreaterOrEqual(sentimentResponse.Sentences.Count, 1);

            Assert.IsNotNull(sentimentResponse.Language);
            Assert.IsNotEmpty(sentimentResponse.Language);

            Assert.IsNotNull(sentimentResponse.Sentiment);
            Assert.GreaterOrEqual(sentimentResponse.Sentiment.Magnitude, 0);
            Assert.GreaterOrEqual(sentimentResponse.Sentiment.Score, -1);
            Assert.LessOrEqual(sentimentResponse.Sentiment.Score, 1);

            foreach (Sentence sentence in sentimentResponse.Sentences) {
                Assert.IsNotNull(sentence.Sentiment);

                Assert.GreaterOrEqual(sentence.Sentiment.Magnitude, 0);
                Assert.GreaterOrEqual(sentence.Sentiment.Score, -1);
                Assert.LessOrEqual(sentence.Sentiment.Score, 1);

                Assert.IsNotNull(sentence.Text);
                Assert.IsNotNull(sentence.Text.Content);
                Assert.GreaterOrEqual(sentence.Text.BeginOffset, 0);
            }
        }
    }
}
