using System;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;
using GC = GoogleCloudClassLibrary;
using GoogleCloudClassLibrary.NaturalLanguageIntelligence;
using ResponseStatus = GoogleCloudClassLibrary.ResponseStatus;

namespace GoogleClassLibraryTest {
    [TestFixture]
    public class AnnotateTextTest {
        private static NaturalLanguageIntelligence nlIntelligence;

        private GC.GoogleCloudClassSetup VALID_SETUP;
        private GC.GoogleCloudClassSetup INVALID_SETUP;

        private static readonly String VALID_DOC_URL = "gs://gccl_dd_01/doc1.txt";
        private static readonly String INVALID_DOC_URL = "INVALID_DOC_URL";

        private static String VALID_FILE_LOCATION;
        private static readonly String EMPTY_CONTENT = "";

        private static readonly Document MISSING_DOCUMENT = null;
        private static readonly TextFeatures MISSING_FEATURES = null;

        public AnnotateTextTest() {
            String dir = Environment.GetEnvironmentVariable("GC_LIBRARY_TEST", EnvironmentVariableTarget.User);
            VALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\APIKEY.txt");
            INVALID_SETUP = new GC.GoogleCloudClassSetup(dir + "\\INVALID_APIKEY.txt");
            VALID_FILE_LOCATION = dir + "\\SampleDoc.txt";
            nlIntelligence = new NaturalLanguageIntelligence(VALID_SETUP);
        }

        [Test]
        public void AnnotateText_InvalidKey() {
            nlIntelligence.UpdateKey(INVALID_SETUP);

            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);
            TextFeatures features = new TextFeatures(extractSyntax: true);

            Task<Tuple<AnnotateTextResponse, ResponseStatus>> response = nlIntelligence.AnnotateText(document, features, EncodingType.UTF8);
            response.Wait();

            nlIntelligence.UpdateKey(VALID_SETUP);

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.INVALID_API_KEY);
        }

        [Test]
        public void AnnotateText_MissingDocument() {
            TextFeatures features = new TextFeatures(extractSyntax: true);

            Task<Tuple<AnnotateTextResponse, ResponseStatus>> response = nlIntelligence.AnnotateText(MISSING_DOCUMENT, features, EncodingType.UTF8);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.MISSING_DOCUMENT);
        }

        [Test]
        public void AnnotateText_MissingTextFeatures() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);

            Task<Tuple<AnnotateTextResponse, ResponseStatus>> response = nlIntelligence.AnnotateText(document, MISSING_FEATURES, EncodingType.UTF8);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.MISSING_FEATURES);
        }

        [Test]
        public void AnnotateText_InvalidDocumentURL() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: INVALID_DOC_URL);
            TextFeatures features = new TextFeatures(extractSyntax: true);

            Task<Tuple<AnnotateTextResponse, ResponseStatus>> response = nlIntelligence.AnnotateText(document, features, EncodingType.UTF8);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.INVALID_ARGUMENT);
        }

        [Test]
        public void AnnotateText_EmptyDocumentContent() {
            Document document = new Document(DocumentType.PLAIN_TEXT, content: EMPTY_CONTENT);
            TextFeatures features = new TextFeatures(extractSyntax: true);

            Task<Tuple<AnnotateTextResponse, ResponseStatus>> response = nlIntelligence.AnnotateText(document, features, EncodingType.UTF8);
            response.Wait();

            Assert.IsNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.ZERO_RESULTS);
        }

        [Test]
        public void AnnotateText_ValidQueryWithExtractSyntax() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);
            TextFeatures features = new TextFeatures(extractSyntax: true);

            Task<Tuple<AnnotateTextResponse, ResponseStatus>> response = nlIntelligence.AnnotateText(document, features, EncodingType.UTF8);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            AnnotateTextResponse annotateResponse = response.Result.Item1;

            Assert.IsNotNull(annotateResponse.Sentences);
            Assert.GreaterOrEqual(annotateResponse.Sentences.Count, 1);

            Assert.IsNotNull(annotateResponse.Language);
            Assert.IsNotEmpty(annotateResponse.Language);

            foreach (Sentence sentence in annotateResponse.Sentences) {
                Assert.IsNull(sentence.Sentiment);

                Assert.IsNotNull(sentence.Text);
                Assert.IsNotNull(sentence.Text.Content);
                Assert.GreaterOrEqual(sentence.Text.BeginOffset, 0);
            }

            Assert.IsNotNull(annotateResponse.Tokens);
            Assert.GreaterOrEqual(annotateResponse.Tokens.Count, 1);

            foreach (Token token in annotateResponse.Tokens) {
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
        public void AnnotateText_ValidQueryWithExtractEntities() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);
            TextFeatures features = new TextFeatures(extractEntities: true);

            Task<Tuple<AnnotateTextResponse, ResponseStatus>> response = nlIntelligence.AnnotateText(document, features, EncodingType.UTF8);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            AnnotateTextResponse annotateResponse = response.Result.Item1;

            Assert.IsNotNull(annotateResponse.Language);
            Assert.IsNotEmpty(annotateResponse.Language);

            foreach (Entity entity in annotateResponse.Entities) {
                Assert.IsNotNull(entity.Name);
                Assert.IsNotEmpty(entity.Name);

                Assert.IsNotNull(entity.Mentions);
                Assert.GreaterOrEqual(entity.Mentions.Count, 1);

                Assert.GreaterOrEqual(entity.Salience, 0);
                Assert.LessOrEqual(entity.Salience, 1);

                Assert.IsNull(entity.Sentiment);

                Assert.IsNotNull(entity.Type);
                Assert.AreNotEqual(entity.Type, EntityResponseType.UNKNOWN);
            }
        }

        [Test]
        public void AnnotateText_ValidQueryWithExtractEntitySentiment() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);
            TextFeatures features = new TextFeatures(extractEntitySentiment: true);

            Task<Tuple<AnnotateTextResponse, ResponseStatus>> response = nlIntelligence.AnnotateText(document, features, EncodingType.UTF8);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            AnnotateTextResponse annotateResponse = response.Result.Item1;

            Assert.IsNotNull(annotateResponse.Language);
            Assert.IsNotEmpty(annotateResponse.Language);

            foreach (Entity entity in annotateResponse.Entities) {
                Assert.IsNotNull(entity.Name);
                Assert.IsNotEmpty(entity.Name);

                Assert.IsNotNull(entity.Mentions);
                Assert.GreaterOrEqual(entity.Mentions.Count, 1);

                Assert.GreaterOrEqual(entity.Salience, 0);
                Assert.LessOrEqual(entity.Salience, 1);

                Assert.IsNotNull(entity.Sentiment);
                Assert.GreaterOrEqual(entity.Sentiment.Magnitude, 0);
                Assert.GreaterOrEqual(entity.Sentiment.Score, -1);
                Assert.LessOrEqual(entity.Sentiment.Score, 1);

                Assert.IsNotNull(entity.Type);
                Assert.AreNotEqual(entity.Type, EntityResponseType.UNKNOWN);
            }
        }

        [Test]
        public void AnnotateText_ValidQueryWithExtractDocumentSentiment() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);
            TextFeatures features = new TextFeatures(extractDocumentSentiment: true);

            Task<Tuple<AnnotateTextResponse, ResponseStatus>> response = nlIntelligence.AnnotateText(document, features, EncodingType.UTF8);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            AnnotateTextResponse annotateResponse = response.Result.Item1;

            Assert.IsNotNull(annotateResponse.Sentences);
            Assert.GreaterOrEqual(annotateResponse.Sentences.Count, 1);

            Assert.IsNotNull(annotateResponse.Language);
            Assert.IsNotEmpty(annotateResponse.Language);

            Assert.IsNotNull(annotateResponse.DocumentSentiment);
            Assert.GreaterOrEqual(annotateResponse.DocumentSentiment.Magnitude, 0);
            Assert.GreaterOrEqual(annotateResponse.DocumentSentiment.Score, -1);
            Assert.LessOrEqual(annotateResponse.DocumentSentiment.Score, 1);

            foreach (Sentence sentence in annotateResponse.Sentences) {
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
        public void AnnotateText_ValidQueryWithClassifyText() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);
            TextFeatures features = new TextFeatures(classifyText: true);

            Task<Tuple<AnnotateTextResponse, ResponseStatus>> response = nlIntelligence.AnnotateText(document, features, EncodingType.UTF8);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            AnnotateTextResponse annotateResponse = response.Result.Item1;

            Assert.IsNotNull(annotateResponse.Language);
            Assert.IsNotEmpty(annotateResponse.Language);
            
            foreach (ClassificationCategory classification in annotateResponse.Categories) {
                Assert.IsNotNull(classification);

                Assert.IsNotNull(classification.Name);
                Assert.IsNotEmpty(classification.Name);

                Assert.GreaterOrEqual(classification.Confidence, 0);
            }
        }

        [Test]
        public void AnnotateText_ValidQueryDocumentURLWithAllOptions() {
            Document document = new Document(DocumentType.PLAIN_TEXT, googleCloudUri: VALID_DOC_URL);
            TextFeatures features = new TextFeatures(extractEntities: true, extractEntitySentiment: true,
                extractDocumentSentiment: true, extractSyntax: true, classifyText: true);

            Task<Tuple<AnnotateTextResponse, ResponseStatus>> response = nlIntelligence.AnnotateText(document, features, EncodingType.UTF8);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            AnnotateTextResponse annotateResponse = response.Result.Item1;

            // Verifying Language
            Assert.IsNotNull(annotateResponse.Language);
            Assert.IsNotEmpty(annotateResponse.Language);

            // Verifying DocumentSentiment from the response
            Assert.IsNotNull(annotateResponse.DocumentSentiment);
            Assert.GreaterOrEqual(annotateResponse.DocumentSentiment.Magnitude, 0);
            Assert.GreaterOrEqual(annotateResponse.DocumentSentiment.Score, -1);
            Assert.LessOrEqual(annotateResponse.DocumentSentiment.Score, 1);

            // Verifying Sentences from the response
            Assert.IsNotNull(annotateResponse.Sentences);
            Assert.GreaterOrEqual(annotateResponse.Sentences.Count, 1);

            foreach (Sentence sentence in annotateResponse.Sentences) {
                Assert.IsNotNull(sentence.Sentiment);

                Assert.GreaterOrEqual(sentence.Sentiment.Magnitude, 0);
                Assert.GreaterOrEqual(sentence.Sentiment.Score, -1);
                Assert.LessOrEqual(sentence.Sentiment.Score, 1);

                Assert.IsNotNull(sentence.Text);
                Assert.IsNotNull(sentence.Text.Content);
                Assert.GreaterOrEqual(sentence.Text.BeginOffset, 0);
            }

            // Verifying Categories from the response
            Assert.IsNotNull(annotateResponse.Categories);
            Assert.GreaterOrEqual(annotateResponse.Categories.Count, 1);

            foreach (ClassificationCategory classification in annotateResponse.Categories) {
                Assert.IsNotNull(classification);

                Assert.IsNotNull(classification.Name);
                Assert.IsNotEmpty(classification.Name);

                Assert.GreaterOrEqual(classification.Confidence, 0);
            }

            // Verifying Entities from the response
            Assert.IsNotNull(annotateResponse.Entities);
            Assert.GreaterOrEqual(annotateResponse.Entities.Count, 1);

            foreach (Entity entity in annotateResponse.Entities) {
                Assert.IsNotNull(entity.Name);
                Assert.IsNotEmpty(entity.Name);

                Assert.IsNotNull(entity.Mentions);
                Assert.GreaterOrEqual(entity.Mentions.Count, 1);

                Assert.GreaterOrEqual(entity.Salience, 0);
                Assert.LessOrEqual(entity.Salience, 1);

                Assert.IsNotNull(entity.Sentiment);
                Assert.GreaterOrEqual(entity.Sentiment.Magnitude, 0);
                Assert.GreaterOrEqual(entity.Sentiment.Score, -1);
                Assert.LessOrEqual(entity.Sentiment.Score, 1);

                Assert.IsNotNull(entity.Type);
                Assert.AreNotEqual(entity.Type, EntityResponseType.UNKNOWN);
            }

            // Verifying Tokens from the response
            Assert.IsNotNull(annotateResponse.Tokens);
            Assert.GreaterOrEqual(annotateResponse.Tokens.Count, 1);

            foreach (Token token in annotateResponse.Tokens) {
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
        public void AnnotateText_ValidQueryDocumentContentWithAllOptions() {
            Document document = new Document(DocumentType.PLAIN_TEXT, content: File.ReadAllText(VALID_FILE_LOCATION));

            TextFeatures features = new TextFeatures(extractEntities: true, extractEntitySentiment: true,
                extractDocumentSentiment: true, extractSyntax: true, classifyText: true);

            Task<Tuple<AnnotateTextResponse, ResponseStatus>> response = nlIntelligence.AnnotateText(document, features, EncodingType.UTF8);
            response.Wait();

            Assert.IsNotNull(response.Result.Item1);
            Assert.AreEqual(response.Result.Item2, NaturalLanguageStatus.OK);

            AnnotateTextResponse annotateResponse = response.Result.Item1;

            // Verifying Language
            Assert.IsNotNull(annotateResponse.Language);
            Assert.IsNotEmpty(annotateResponse.Language);

            // Verifying DocumentSentiment from the response
            Assert.IsNotNull(annotateResponse.DocumentSentiment);
            Assert.GreaterOrEqual(annotateResponse.DocumentSentiment.Magnitude, 0);
            Assert.GreaterOrEqual(annotateResponse.DocumentSentiment.Score, -1);
            Assert.LessOrEqual(annotateResponse.DocumentSentiment.Score, 1);

            // Verifying Sentences from the response
            Assert.IsNotNull(annotateResponse.Sentences);
            Assert.GreaterOrEqual(annotateResponse.Sentences.Count, 1);

            foreach (Sentence sentence in annotateResponse.Sentences) {
                Assert.IsNotNull(sentence.Sentiment);

                Assert.GreaterOrEqual(sentence.Sentiment.Magnitude, 0);
                Assert.GreaterOrEqual(sentence.Sentiment.Score, -1);
                Assert.LessOrEqual(sentence.Sentiment.Score, 1);

                Assert.IsNotNull(sentence.Text);
                Assert.IsNotNull(sentence.Text.Content);
                Assert.GreaterOrEqual(sentence.Text.BeginOffset, 0);
            }

            // Verifying Categories from the response
            Assert.IsNotNull(annotateResponse.Categories);
            Assert.GreaterOrEqual(annotateResponse.Categories.Count, 1);

            foreach (ClassificationCategory classification in annotateResponse.Categories) {
                Assert.IsNotNull(classification);

                Assert.IsNotNull(classification.Name);
                Assert.IsNotEmpty(classification.Name);

                Assert.GreaterOrEqual(classification.Confidence, 0);
            }

            // Verifying Entities from the response
            Assert.IsNotNull(annotateResponse.Entities);
            Assert.GreaterOrEqual(annotateResponse.Entities.Count, 1);

            foreach (Entity entity in annotateResponse.Entities) {
                Assert.IsNotNull(entity.Name);
                Assert.IsNotEmpty(entity.Name);

                Assert.IsNotNull(entity.Mentions);
                Assert.GreaterOrEqual(entity.Mentions.Count, 1);

                Assert.GreaterOrEqual(entity.Salience, 0);
                Assert.LessOrEqual(entity.Salience, 1);

                Assert.IsNotNull(entity.Sentiment);
                Assert.GreaterOrEqual(entity.Sentiment.Magnitude, 0);
                Assert.GreaterOrEqual(entity.Sentiment.Score, -1);
                Assert.LessOrEqual(entity.Sentiment.Score, 1);

                Assert.IsNotNull(entity.Type);
                Assert.AreNotEqual(entity.Type, EntityResponseType.UNKNOWN);
            }

            // Verifying Tokens from the response
            Assert.IsNotNull(annotateResponse.Tokens);
            Assert.GreaterOrEqual(annotateResponse.Tokens.Count, 1);

            foreach (Token token in annotateResponse.Tokens) {
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
