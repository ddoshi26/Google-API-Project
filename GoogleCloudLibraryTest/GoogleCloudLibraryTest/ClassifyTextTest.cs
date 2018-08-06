using System;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using GoogleCloudClassLibrary.NaturalLanguageIntelligence;
using ResponseStatus = GoogleCloudClassLibrary.ResponseStatus;

namespace GoogleClassLibraryTest {
    [TestFixture]
    public class ClassifyTextTest {
        private static NaturalLanguageIntelligence nlIntelligence;

        private GC.GoogleCloudClassSetup VALID_SETUP;
        private GC.GoogleCloudClassSetup INVALID_SETUP;

        private static readonly String INVALID_DOC_URL = "INVALID_DOC_URL";
        private static readonly String VALID_DOC_URL = "gs://gccl_dd_01/doc1.txt";

        private static String VALID_FILE_LOCATION;
        private static readonly String EMPTY_CONTENT = "";

        private static readonly Document MISSING_DOCUMENT = null;
        
        public ClassifyTextTest() {
            String dir = Environment.GetEnvironmentVariable("GC_LIBRARY_TEST", EnvironmentVariableTarget.User);
            VALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\APIKEY.txt");
            INVALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\INVALID_APIKEY.txt");
            VALID_FILE_LOCATION = dir + "\\SampleDoc.txt";
            nlIntelligence = new NaturalLanguageIntelligence(VALID_SETUP);
        }

        [Test]
        public void ClassifyText_InvalidKey() {
            nlIntelligence.UpdateKey(INVALID_SETUP);

            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);

            Task<Tuple<ClassifyTextResponse, ResponseStatus>> response = nlIntelligence.ClassifyText(document);
            response.Wait();

            nlIntelligence.UpdateKey(VALID_SETUP);

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.INVALID_API_KEY);
        }

        [Test]
        public void ClassifyText_MissingDocument() {
            Task<Tuple<ClassifyTextResponse, ResponseStatus>> response = nlIntelligence.ClassifyText(MISSING_DOCUMENT);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.MISSING_DOCUMENT);
        }

        [Test]
        public void ClassifyText_InvalidDocumentURL() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: INVALID_DOC_URL);

            Task<Tuple<ClassifyTextResponse, ResponseStatus>> response = nlIntelligence.ClassifyText(document);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.INVALID_ARGUMENT);
        }

        [Test]
        public void ClassifyText_EmptyDocumentContent() {
            Document document = new Document(DocumentType.PLAIN_TEXT, content: EMPTY_CONTENT);

            Task<Tuple<ClassifyTextResponse, ResponseStatus>> response = nlIntelligence.ClassifyText(document);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.ZERO_RESULTS);
        }

        [Test]
        public void ClassifyText_ValidQueryWithDocumentURL() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);

            Task<Tuple<ClassifyTextResponse, ResponseStatus>> response = nlIntelligence.ClassifyText(document);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            ClassifyTextResponse textResponse = response.Result.Item1;

            Assert.IsNotNull(textResponse.Categories);
            Assert.GreaterOrEqual(textResponse.Categories.Count, 1);

            foreach (ClassificationCategory classification in textResponse.Categories) {
                Assert.IsNotNull(classification.Name);
                Assert.IsNotEmpty(classification.Name);

                Assert.GreaterOrEqual(classification.Confidence, 0);
                Assert.LessOrEqual(classification.Confidence, 1);
            }
        }

        [Test]
        public void ClassifyText_ValidQueryWithDocumentContent() {
            Document document = new Document(DocumentType.PLAIN_TEXT, content: File.ReadAllText(VALID_FILE_LOCATION));

            Task<Tuple<ClassifyTextResponse, ResponseStatus>> response = nlIntelligence.ClassifyText(document);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            ClassifyTextResponse textResponse = response.Result.Item1;

            Assert.IsNotNull(textResponse.Categories);
            Assert.GreaterOrEqual(textResponse.Categories.Count, 1);

            foreach (ClassificationCategory classification in textResponse.Categories) {
                Assert.IsNotNull(classification.Name);
                Assert.IsNotEmpty(classification.Name);

                Assert.GreaterOrEqual(classification.Confidence, 0);
                Assert.LessOrEqual(classification.Confidence, 1);
            }
        }
    }
}
