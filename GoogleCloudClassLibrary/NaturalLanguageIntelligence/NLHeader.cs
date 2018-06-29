using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public enum DocumentType {
        HTML, PLAIN_TEXT
    }

    public enum EncodingType {
        NONE, UTF8, UTF16, UTF32
    }

    public class Document {
        private DocumentType type;
        private String language;
        private String content;
        private String googleCloudUri = "";

        public DocumentType Type {
            get => type; set => type = value;
        }
        public String Language {
            get => language; set => language = value;
        }
        public String Content {
            get => content; set => content = value;
        }
        public String GoogleCloudUri {
            get => googleCloudUri; set => googleCloudUri = value;
        }

        public Document(DocumentType type, String language, String content) {
            this.Type = type;
            this.Language = language;
            this.Content = content;
        }
    }

    public class AnalyzeEntitiesRequest {
        private Document document;
        private EncodingType encodingType;

        public Document Document { get => document; set => document = value; }
        public EncodingType EncodingType { get => encodingType; set => encodingType = value; }

        public AnalyzeEntitiesRequest(Document document, EncodingType encodingType) {
            Document = document;
            EncodingType = encodingType;
        }
    }

    public class TextFeatures {
        private Boolean extractSyntax;
        private Boolean extractEntities;
        private Boolean extractDocumentSentiment;
        private Boolean extractEntitySentiment;
        private Boolean classifyText;

        public bool ExtractSyntax {
            get => extractSyntax; set => extractSyntax = value;
        }
        public bool ExtractEntities {
            get => extractEntities; set => extractEntities = value;
        }
        public bool ExtractDocumentSentiment {
            get => extractDocumentSentiment; set => extractDocumentSentiment = value;
        }
        public bool ExtractEntitySentiment {
            get => extractEntitySentiment; set => extractEntitySentiment = value;
        }
        public bool ClassifyText {
            get => classifyText; set => classifyText = value;
        }

        public TextFeatures(bool extractSyntax, bool extractEntities, bool extractDocumentSentiment,
            bool extractEntitySentiment, bool classifyText) {

            this.ExtractSyntax = extractSyntax;
            this.ExtractEntities = extractEntities;
            this.ExtractDocumentSentiment = extractDocumentSentiment;
            this.ExtractEntitySentiment = extractEntitySentiment;
            this.ClassifyText = classifyText;
        }
    }

    public class Sentiment {
        private double magnitude;
        private double score;

        public double Magnitude {
            get => magnitude; set => magnitude = value;
        }
        public double Score {
            get => score; set => score = value;
        }

        public Sentiment(double magnitude, double score) {
            this.Magnitude = magnitude;
            this.Score = score;
        }
    }

    public class TextSpan {
        String content;
        int beginOffset;

        public String Content {
            get => content; set => content = value;
        }
        public int BeginOffset {
            get => beginOffset; set => beginOffset = value;
        }

        public TextSpan(String content, int beginOffset) {
            this.Content = content;
            this.BeginOffset = beginOffset;
        }
    }

    public enum EntityResponseType {
        UNKNOWN, PERSON, LOCATION, ORGANIZATION, EVENT, WORK_OF_ART, CONSUMER_GOOD, OTHER
    }

    public enum EntityMentionType {
        TYPE_UNKNOWN, PROPER, COMMON
    }

    public class EntityMention {
        private TextSpan text;
        private EntityMentionType type;
        private Sentiment sentiment;

        public TextSpan Text {
            get => text; set => text = value;
        }
        public EntityMentionType Type {
            get => type; set => type = value;
        }
        public Sentiment Sentiment {
            get => sentiment; set => sentiment = value;
        }

        public EntityMention(TextSpan text, EntityMentionType type, Sentiment sentiment) {
            this.Text = text;
            this.Type = type;
            this.Sentiment = sentiment;
        }
    }

    public class Entity {
        private String name;
        private EntityResponseType type;
        private Dictionary<String, String> metadata;
        private double salience;
        private List<EntityMention> mentions;
        private Sentiment sentiment;

        public String Name {
            get => name; set => name = value;
        }
        public EntityResponseType Type {
            get => type; set => type = value;
        }
        public Dictionary<String, String> Metadata {
            get => metadata; set => metadata = value;
        }
        public double Salience {
            get => salience; set => salience = value;
        }
        public List<EntityMention> Mentions {
            get => mentions; set => mentions = value;
        }
        public Sentiment Sentiment {
            get => sentiment; set => sentiment = value;
        }

        public Entity(String name, EntityResponseType type, Dictionary<String, String> metadata, double salience,
            List<EntityMention> mentions, Sentiment sentiment) {
            this.Name = name;
            this.Type = type;
            this.Metadata = metadata;
            this.Salience = salience;
            this.Mentions = mentions;
            this.Sentiment = sentiment;
        }
    }

    public class AnalyzeEntitiesResponse {
        private List<Entity> entities;
        private String language;

        public List<Entity> Entities {
            get => entities; set => entities = value;
        }
        public String Language {
            get => language; set => language = value;
        }

        public AnalyzeEntitiesResponse(List<Entity> entities, String language) {
            this.Entities = entities;
            this.Language = language;
        }
    }

    public class Sentence {
        private TextSpan text;
        private Sentiment sentiment;

        public TextSpan Text {
            get => text; set => text = value;
        }
        public Sentiment Sentiment {
            get => sentiment; set => sentiment = value;
        }

        public Sentence(TextSpan text, Sentiment sentiment) {
            this.Text = text;
            this.Sentiment = sentiment;
        }
    }

    public class AnalyzeSentimentResponse {
        private Sentiment sentiment;
        private String language;
        private List<Sentence> sentences;

        public Sentiment Sentiment {
            get => sentiment; set => sentiment = value;
        }
        public String Language {
            get => language; set => language = value;
        }
        public List<Sentence> Sentences {
            get => sentences; set => sentences = value;
        }

        public AnalyzeSentimentResponse(Sentiment sentiment, String language, List<Sentence> sentences) {
            this.Sentiment = sentiment;
            this.Language = language;
            this.Sentences = sentences;
        }
    }

    public enum DependencyEdgeLabel {
        UNKNOWN, ABBREV, ACOMP, ADVCL, ADVMOD, AMOD, APPOS, ATTR, AUX, AUXPASS, CC, CCOMP, CONJ, CSUBJ, CSUBJPASS,
        DEP, DET, DISCOURSE, DOBJ, EXPL, GOESWITH, IOBJ, MARK, MWE, MWV, NEG, NN, NPADVMOD, NSUBJ, NSUBPJPASS,
        NUM, NUMBER, P, PARATAXIS, PARTMOD, PCOMP, POBJ, POSS, POSTNEG, PRECOMP, PRECONJ, PREDET, PREF, PREP,
        PRONL, PRT, PS, QUANTMOD, RCMOD, RCMODREL, RDROP, REF, REMNANT, REPARANDUM, ROOT, SNUM, SUFF, TMOD, TOPIC,
        VMOD, VOCATIVE, XCOMP, SUFFIX, TITLE, ADVPHMOD, AUXCAUS, AUXVV, DTMOD, FOREIGN, KW, LIST, NOMC, NOMCSUBJ,
        NOMCSUBJPASS, NUMC, COP, DISLOCATES, ASP, GMOD, GOBJ, INFMOD, MES, NCOMP
    }

    public enum PartsOfSpeechTag {
        UNKNOWN, ADJ, ADP, ADV, CONJ, DET, NOUN, NUM, PRON, PRT, PUNCT, VERB, X, AFFIX
    }

    public enum PartsOfSpeechAspect {
        ASPECT_UNKNOWN, PERFECTIVE, IMPERFECTIVE, PROGRESSIVE
    }

    public enum PartsOfSpeechCase {
        CASE_UNKNOWN, ACCUSATIVE, ADVERBIAL, COMPLEMENTIVE, DATIVE, GENITIVE, INSTRUMENTAL, LOCATIVE, NOMINATIVE,
        OBLIQUE, PARTITIVE, PREPOSITIONAL, REFLEXIVE_CASE, RELATIVE_CASE, VOCATIVE
    }

    public enum PartsOfSpeechForm {
        FORM_UNKNOWN, ADNOMIAL, AUXILIARY, COMPLEMENTIZER, FINAL_ENDING, GERUND, REALIS, IRREALIS, SHORT, LONG, ORDER, SPECIFIC
    }

    public enum PartsOfSpeechGender {
        GENDER_UNKOWN, FEMININE, MASCULINE, NEUTER
    }

    public enum PartsOfSpeechMood {
        MOOD_UNKNOWN, CONDITIONAL_MOOD, IMPERATIVE, INDICATIVE, INTERROGATIVE, JUSSIVE, SUBJUNCTIVE
    }

    public enum PartsOfSpeechNumber {
        NUMBER_UNKNOWN, SINGULAR, PLURAL, DUAL
    }

    public enum PartsOfSpeechPerson {
        PERSON_UNKNOWN, FIRST, SECOND, THIRD, REFLEXIVE_PERSON
    }

    public enum PartsOfSpeechProper {
        PROPER_UNKNOWN, PROPER, NOT_PROPER
    }

    public enum PartsOfSpeechReciprocity {
        RECIPROCITY_UNKNOWN, RECIPROCAL, NON_RECIPROCAL
    }

    public enum PartsOfSpeechTense {
        TENSE_UNKNOWN, CONDITIONAL_TENSE, FUTURE, PAST, PRESENT, IMPERFECT, PLUPERFECT
    }

    public enum PartsOfSpeechVoice {
        VOICE_UNKNOWN, ACTIVE, CAUSATIVE, PASSIVE
    }

    public class PartsOfSpeech {
        PartsOfSpeechTag tag;
        PartsOfSpeechAspect aspect;
        PartsOfSpeechCase p_case;
        PartsOfSpeechForm form;
        PartsOfSpeechGender gender;
        PartsOfSpeechMood mood;
        PartsOfSpeechNumber number;
        PartsOfSpeechPerson person;
        PartsOfSpeechProper proper;
        PartsOfSpeechReciprocity reciprocity;
        PartsOfSpeechTense tense;
        PartsOfSpeechVoice voice;

        public PartsOfSpeechTag Tag {
            get => tag; set => tag = value;
        }
        public PartsOfSpeechAspect Aspect {
            get => aspect; set => aspect = value;
        }
        public PartsOfSpeechCase Case {
            get => p_case; set => p_case = value;
        }
        public PartsOfSpeechForm Form {
            get => form; set => form = value;
        }
        public PartsOfSpeechGender Gender {
            get => gender; set => gender = value;
        }
        public PartsOfSpeechMood Mood {
            get => mood; set => mood = value;
        }
        public PartsOfSpeechNumber Number {
            get => number; set => number = value;
        }
        public PartsOfSpeechPerson Person {
            get => person; set => person = value;
        }
        public PartsOfSpeechProper Proper {
            get => proper; set => proper = value;
        }
        public PartsOfSpeechReciprocity Reciprocity {
            get => reciprocity; set => reciprocity = value;
        }
        public PartsOfSpeechTense Tense {
            get => tense; set => tense = value;
        }
        public PartsOfSpeechVoice Voice {
            get => voice; set => voice = value;
        }

        public PartsOfSpeech(PartsOfSpeechTag tag, PartsOfSpeechAspect aspect, PartsOfSpeechCase p_case,
            PartsOfSpeechForm form, PartsOfSpeechGender gender, PartsOfSpeechMood mood, PartsOfSpeechNumber number,
            PartsOfSpeechPerson person, PartsOfSpeechProper proper, PartsOfSpeechReciprocity reciprocity,
            PartsOfSpeechTense tense, PartsOfSpeechVoice voice) {
            this.Tag = tag;
            this.Aspect = aspect;
            this.Case = p_case;
            this.Form = form;
            this.Gender = gender;
            this.Mood = mood;
            this.Number = number;
            this.Person = person;
            this.Proper = proper;
            this.Reciprocity = reciprocity;
            this.Tense = tense;
            this.Voice = voice;
        }
    }

    public class Token {
        private TextSpan text;
        private PartsOfSpeech partsOfSpeech;
        private DependencyEdgeLabel dependencyEdge;
        private String lemma;

        public TextSpan Text {
            get => text; set => text = value;
        }
        public PartsOfSpeech PartsOfSpeech {
            get => partsOfSpeech; set => partsOfSpeech = value;
        }
        public DependencyEdgeLabel DependencyEdge {
            get => dependencyEdge; set => dependencyEdge = value;
        }
        public String Lemma {
            get => lemma; set => lemma = value;
        }

        public Token(TextSpan text, PartsOfSpeech partsOfSpeech, DependencyEdgeLabel dependencyEdge, String lemma) {
            this.Text = text;
            this.PartsOfSpeech = partsOfSpeech;
            this.DependencyEdge = dependencyEdge;
            this.Lemma = lemma;
        }
    }

    public class AnalyzeSyntaxResponse {
        private List<Sentence> sentences;
        private List<Token> tokens;
        private String language;

        public List<Sentence> Sentences {
            get => sentences; set => sentences = value;
        }
        public List<Token> Tokens {
            get => tokens; set => tokens = value;
        }
        public String Language {
            get => language; set => language = value;
        }

        public AnalyzeSyntaxResponse(List<Sentence> sentences, List<Token> tokens, String language) {
            this.Sentences = sentences;
            this.Tokens = tokens;
            this.Language = language;
        }
    }

    public class ClassificationCategory {
        private String name;
        private double confidence;

        public String Name {
            get => name; set => name = value;
        }
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        public ClassificationCategory(String name, double confidence) {
            this.Name = name;
            this.Confidence = confidence;
        }
    }

    public class AnnotateText {
        private List<Sentence> sentences;
        private List<Token> tokens;
        private List<Entity> entities;
        private Sentiment documentSentiment;
        private String language;
        private List<ClassificationCategory> categories;

        public List<Sentence> Sentences {
            get => sentences; set => sentences = value;
        }
        public List<Token> Tokens {
            get => tokens; set => tokens = value;
        }
        public List<Entity> Entities {
            get => entities; set => entities = value;
        }
        public Sentiment DocumentSentiment {
            get => documentSentiment; set => documentSentiment = value;
        }
        public String Language {
            get => language; set => language = value;
        }
        public List<ClassificationCategory> Categories {
            get => categories; set => categories = value;
        }

        public AnnotateText(List<Sentence> sentences, List<Token> tokens, List<Entity> entities, Sentiment sentiment,
            String language, List<ClassificationCategory> classificationCategories) {
            this.Sentences = sentences;
            this.Tokens = tokens;
            this.Entities = entities;
            this.DocumentSentiment = sentiment;
            this.Language = language;
            this.Categories = classificationCategories;
        }
    }
}
