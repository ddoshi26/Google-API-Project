using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
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

        [JsonProperty("tag")]
        public PartsOfSpeechTag Tag {
            get => tag; set => tag = value;
        }

        [JsonProperty("aspect")]
        public PartsOfSpeechAspect Aspect {
            get => aspect; set => aspect = value;
        }

        [JsonProperty("case")]
        public PartsOfSpeechCase Case {
            get => p_case; set => p_case = value;
        }

        [JsonProperty("form")]
        public PartsOfSpeechForm Form {
            get => form; set => form = value;
        }

        [JsonProperty("gender")]
        public PartsOfSpeechGender Gender {
            get => gender; set => gender = value;
        }

        [JsonProperty("mood")]
        public PartsOfSpeechMood Mood {
            get => mood; set => mood = value;
        }

        [JsonProperty("number")]
        public PartsOfSpeechNumber Number {
            get => number; set => number = value;
        }

        [JsonProperty("person")]
        public PartsOfSpeechPerson Person {
            get => person; set => person = value;
        }

        [JsonProperty("proper")]
        public PartsOfSpeechProper Proper {
            get => proper; set => proper = value;
        }

        [JsonProperty("reciprocity")]
        public PartsOfSpeechReciprocity Reciprocity {
            get => reciprocity; set => reciprocity = value;
        }

        [JsonProperty("tense")]
        public PartsOfSpeechTense Tense {
            get => tense; set => tense = value;
        }

        [JsonProperty("voice")]
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
}
