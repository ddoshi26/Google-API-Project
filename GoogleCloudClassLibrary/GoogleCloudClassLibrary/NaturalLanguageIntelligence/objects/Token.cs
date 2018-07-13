﻿using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class Token {
        private TextSpan text;
        private PartsOfSpeech partOfSpeech;
        private DependencyEdgeLabel dependencyEdge;
        private String lemma;

        [JsonProperty("text")]
        public TextSpan Text {
            get => text; set => text = value;
        }

        [JsonProperty("partOfSpeech")]
        public PartsOfSpeech PartOfSpeech {
            get => partOfSpeech; set => partOfSpeech = value;
        }

        [JsonProperty("dependencyEdge")]
        public DependencyEdgeLabel DependencyEdge {
            get => dependencyEdge; set => dependencyEdge = value;
        }

        [JsonProperty("lemma")]
        public String Lemma {
            get => lemma; set => lemma = value;
        }

        public Token(TextSpan text, PartsOfSpeech partOfSpeech, DependencyEdgeLabel dependencyEdge, String lemma) {
            this.Text = text;
            this.PartOfSpeech = partOfSpeech;
            this.DependencyEdge = dependencyEdge;
            this.Lemma = lemma;
        }
    }
}
