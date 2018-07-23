using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.Places {
    public class OpeningHours {
        private Boolean openNow;
        private List<Period> periods;
        private List<String> weekdayText;

        [JsonProperty("open_now")]
        public bool OpenNow {
            get => openNow;
            set => openNow = value;
        }

        [JsonProperty("periods")]
        public List<Period> Periods {
            get => periods;
            set => periods = value;
        }

        [JsonProperty("weekday_text")]
        public List<string> WeekdayText {
            get => weekdayText;
            set => weekdayText = value;
        }

        public OpeningHours(bool openNow, List<Period> periods, List<String> weekdayText) {
            OpenNow = openNow;
            Periods = periods;
            WeekdayText = weekdayText;
        }
    }
}
