using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class ImageContext {
        private LatLongRect latLongRect;
        private List<String> languageHints;
        private CropHintsParams cropHintParams;
        private WebDetectionParams webDetectionParams;

        [JsonProperty("latLongRect")]
        public LatLongRect LatLongRect {
            get => latLongRect; set => latLongRect = value;
        }

        [JsonProperty("languageHints")]
        public List<String> LanguageHints {
            get => languageHints; set => languageHints = value;
        }

        [JsonProperty("cropHintsParams")]
        public CropHintsParams CropHintsParams {
            get => cropHintParams; set => cropHintParams = value;
        }

        [JsonProperty("webDetectionParams")]
        public WebDetectionParams WebDetectionParams {
            get => webDetectionParams; set => webDetectionParams = value;
        }

        public ImageContext(LatLongRect latLngRect, List<String> langHints, CropHintsParams cropHintsParams, WebDetectionParams webDetectionParams) {
            this.LatLongRect = latLngRect;
            this.LanguageHints = langHints;
            this.CropHintsParams = cropHintsParams;
            this.WebDetectionParams = webDetectionParams;
        }
    }
}
