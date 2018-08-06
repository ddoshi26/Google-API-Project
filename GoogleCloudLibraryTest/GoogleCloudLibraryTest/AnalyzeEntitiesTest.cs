using System;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using GoogleCloudClassLibrary.NaturalLanguageIntelligence;
using ResponseStatus = GoogleCloudClassLibrary.ResponseStatus;

namespace GoogleClassLibraryTest {

    [TestFixture]
    public class AnalyzeEntitiesTest {
        private static NaturalLanguageIntelligence nlIntelligence;

        private GC.GoogleCloudClassSetup VALID_SETUP;
        private GC.GoogleCloudClassSetup INVALID_SETUP;

        private static readonly String INVALID_DOC_URL = "INVALID_DOC_URL";
        private static readonly String VALID_DOC_URL = "gs://gccl_dd_01/doc1.txt";

        private static String VALID_FILE_LOCATION;
        private static readonly String EMPTY_CONTENT = "";

        private static readonly Document MISSING_DOCUMENT = null; 

        public AnalyzeEntitiesTest() {
            String dir = Environment.GetEnvironmentVariable("GC_LIBRARY_TEST", EnvironmentVariableTarget.User);
            VALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\APIKEY.txt");
            INVALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\INVALID_APIKEY.txt");
            VALID_FILE_LOCATION = dir + "\\SampleDoc.txt";

            nlIntelligence = new NaturalLanguageIntelligence(VALID_SETUP);
        }

        [Test]
        public void AnalyzeEntities_InvalidKey() {
            nlIntelligence.UpdateKey(INVALID_SETUP);

            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);

            Task<Tuple<AnalyzeEntitiesResponse, ResponseStatus>> response = nlIntelligence.AnalyzeEntities(document, EncodingType.UTF8);
            response.Wait();

            nlIntelligence.UpdateKey(VALID_SETUP);

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.INVALID_API_KEY);
        }

        [Test]
        public void AnalyzeEntities_MissingDocument() {
            Task<Tuple<AnalyzeEntitiesResponse, ResponseStatus>> response = nlIntelligence.AnalyzeEntities(MISSING_DOCUMENT, EncodingType.UTF8);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.MISSING_DOCUMENT);
        }

        [Test]
        public void AnalyzeEntities_InvalidDocumentURL() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: INVALID_DOC_URL);

            Task<Tuple<AnalyzeEntitiesResponse, ResponseStatus>> response = nlIntelligence.AnalyzeEntities(document, EncodingType.UTF8);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.INVALID_ARGUMENT);
        }

        [Test]
        public void AnalyzeEntities_EmptyDocumentContent() {
            Document document = new Document(DocumentType.PLAIN_TEXT, content: EMPTY_CONTENT);

            Task<Tuple<AnalyzeEntitiesResponse, ResponseStatus>> response = nlIntelligence.AnalyzeEntities(document, EncodingType.UTF8);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.ZERO_RESULTS);
        }

        [Test]
        public void AnalyzeEntities_ValidQueryWithDocumentURL() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);

            Task<Tuple<AnalyzeEntitiesResponse, ResponseStatus>> response = nlIntelligence.AnalyzeEntities(document, EncodingType.UTF8);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            AnalyzeEntitiesResponse entitiesResponse = response.Result.Item1;

            Assert.IsNotNull(entitiesResponse.Entities);
            Assert.GreaterOrEqual(entitiesResponse.Entities.Count, 1);

            foreach (Entity entity in entitiesResponse.Entities) {
                Assert.IsNotNull(entity.Name);
                Assert.IsNotEmpty(entity.Name);

                Assert.IsNotNull(entity.Mentions);
                Assert.GreaterOrEqual(entity.Mentions.Count, 1);

                Assert.GreaterOrEqual(entity.Salience, 0);
                Assert.LessOrEqual(entity.Salience, 1);

                Assert.IsNotNull(entity.Type);
                Assert.AreNotEqual(entity.Type, EntityResponseType.UNKNOWN);
            }
        }

        [Test]
        public void AnalyzeEntities_ValidQueryWithDocumentContent() {
            Document document = new Document(DocumentType.PLAIN_TEXT, content: File.ReadAllText(VALID_FILE_LOCATION));

            Task<Tuple<AnalyzeEntitiesResponse, ResponseStatus>> response = nlIntelligence.AnalyzeEntities(document, EncodingType.UTF8);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            AnalyzeEntitiesResponse entitiesResponse = response.Result.Item1;

            Assert.IsNotNull(entitiesResponse.Entities);
            Assert.GreaterOrEqual(entitiesResponse.Entities.Count, 1);

            foreach (Entity entity in entitiesResponse.Entities) {
                Assert.IsNotNull(entity.Name);
                Assert.IsNotEmpty(entity.Name);

                Assert.IsNotNull(entity.Mentions);
                Assert.GreaterOrEqual(entity.Mentions.Count, 1);

                Assert.GreaterOrEqual(entity.Salience, 0);
                Assert.LessOrEqual(entity.Salience, 1);

                Assert.IsNotNull(entity.Type);
                Assert.AreNotEqual(entity.Type, EntityResponseType.UNKNOWN);
            }
        }
    }
}
