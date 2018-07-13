using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.Places {
    public class OpeningHours {
        private Boolean openNow;

        [JsonProperty("open_now")]
        public bool OpenNow {
            get => openNow;
            set => openNow = value;
        }

        public OpeningHours(bool openNow) {
            OpenNow = openNow;
        }
    }
}
