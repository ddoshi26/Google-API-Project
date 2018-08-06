using System;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using GoogleCloudClassLibrary.NaturalLanguageIntelligence;
using ResponseStatus = GoogleCloudClassLibrary.ResponseStatus;

namespace GoogleClassLibraryTest {
    [TestFixture]
    public class AnalyzeSyntaxTest {
        private static NaturalLanguageIntelligence nlIntelligence;

        private GC.GoogleCloudClassSetup VALID_SETUP;
        private GC.GoogleCloudClassSetup INVALID_SETUP;

        private static readonly String VALID_DOC_URL = "gs://gccl_dd_01/doc1.txt";
        private static readonly String INVALID_DOC_URL = "INVALID_DOC_URL";

        private static String VALID_FILE_LOCATION;
        private static readonly String EMPTY_CONTENT = "";

        private static readonly Document MISSING_DOCUMENT = null;

        public AnalyzeSyntaxTest() {
            String dir = Environment.GetEnvironmentVariable("GC_LIBRARY_TEST", EnvironmentVariableTarget.User);
            VALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\APIKEY.txt");
            INVALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\INVALID_APIKEY.txt");
            VALID_FILE_LOCATION = dir + "\\SampleDoc.txt";

            nlIntelligence = new NaturalLanguageIntelligence(VALID_SETUP);
        }

        [Test]
        public void AnalyzeSyntax_InvalidKey() {
            nlIntelligence.UpdateKey(INVALID_SETUP);

            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);

            Task<Tuple<AnalyzeSyntaxResponse, ResponseStatus>> response = nlIntelligence.AnalyzeSyntax(document, EncodingType.UTF8);
            response.Wait();

            nlIntelligence.UpdateKey(VALID_SETUP);

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.INVALID_API_KEY);
        }

        [Test]
        public void AnalyzeSyntax_MissingDocument() {
            Task<Tuple<AnalyzeSyntaxResponse, ResponseStatus>> response = nlIntelligence.AnalyzeSyntax(MISSING_DOCUMENT, EncodingType.UTF8);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.MISSING_DOCUMENT);
        }

        [Test]
        public void AnalyzeSyntax_InvalidDocumentURL() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: INVALID_DOC_URL);

            Task<Tuple<AnalyzeSyntaxResponse, ResponseStatus>> response = nlIntelligence.AnalyzeSyntax(document, EncodingType.UTF8);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.INVALID_ARGUMENT);
        }

        [Test]
        public void AnalyzeSyntax_EmptyDocumentContent() {
            Document document = new Document(DocumentType.PLAIN_TEXT, content: EMPTY_CONTENT);

            Task<Tuple<AnalyzeSyntaxResponse, ResponseStatus>> response = nlIntelligence.AnalyzeSyntax(document, EncodingType.UTF8);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.ZERO_RESULTS);
        }

        [Test]
        public void AnalyzeSyntax_ValidQueryWithDocumentURL() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);

            Task<Tuple<AnalyzeSyntaxResponse, ResponseStatus>> response = nlIntelligence.AnalyzeSyntax(document, EncodingType.UTF8);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            AnalyzeSyntaxResponse syntaxResponse = response.Result.Item1;

            Assert.IsNotNull(syntaxResponse.Sentences);
            Assert.GreaterOrEqual(syntaxResponse.Sentences.Count, 1);

            Assert.IsNotNull(syntaxResponse.Language);
            Assert.IsNotEmpty(syntaxResponse.Language);

            foreach (Sentence sentence in syntaxResponse.Sentences) {
                Assert.IsNull(sentence.Sentiment);

                Assert.IsNotNull(sentence.Text);
                Assert.IsNotNull(sentence.Text.Content);
                Assert.GreaterOrEqual(sentence.Text.BeginOffset, 0);
            }

            Assert.IsNotNull(syntaxResponse.Tokens);

            foreach (Token token in syntaxResponse.Tokens) {
                Assert.IsNotNull(token);

                Assert.IsNotNull(token.Text);
                Assert.IsNotNull(token.Text.Content);
                Assert.GreaterOrEqual(token.Text.BeginOffset, 0);

                Assert.IsNotNull(token.PartOfSpeech);

                Assert.IsNotNull(token.DependencyEdge);
                Assert.AreNotEqual(token.DependencyEdge.Label, DependencyEdgeLabel.UNKNOWN);

                Assert.IsNotNull(token.Lemma);
                Assert.IsNotEmpty(token.Lemma);
            }
        }

        [Test]
        public void AnalyzeSyntax_ValidQueryWithDocumentContent() {
            Document document = new Document(DocumentType.PLAIN_TEXT, content: File.ReadAllText(VALID_FILE_LOCATION));

            Task<Tuple<AnalyzeSyntaxResponse, ResponseStatus>> response = nlIntelligence.AnalyzeSyntax(document, EncodingType.UTF8);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            AnalyzeSyntaxResponse syntaxResponse = response.Result.Item1;

            Assert.IsNotNull(syntaxResponse.Sentences);
            Assert.GreaterOrEqual(syntaxResponse.Sentences.Count, 1);

            Assert.IsNotNull(syntaxResponse.Language);
            Assert.IsNotEmpty(syntaxResponse.Language);

            foreach (Sentence sentence in syntaxResponse.Sentences) {
                Assert.IsNull(sentence.Sentiment);

                Assert.IsNotNull(sentence.Text);
                Assert.IsNotNull(sentence.Text.Content);
                Assert.GreaterOrEqual(sentence.Text.BeginOffset, 0);
            }

            Assert.IsNotNull(syntaxResponse.Tokens);

            foreach (Token token in syntaxResponse.Tokens) {
                Assert.IsNotNull(token);

                Assert.IsNotNull(token.Text);
                Assert.IsNotNull(token.Text.Content);
                Assert.GreaterOrEqual(token.Text.BeginOffset, 0);

                Assert.IsNotNull(token.PartOfSpeech);

                Assert.IsNotNull(token.DependencyEdge);
                Assert.AreNotEqual(token.DependencyEdge.Label, DependencyEdgeLabel.UNKNOWN);

                Assert.IsNotNull(token.Lemma);
                Assert.IsNotEmpty(token.Lemma);
            }
        }
    }
}
