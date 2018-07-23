using System;
using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.Places {
    public class ClosingOpeningTime {
        private int day;
        private String time;

        [JsonProperty("day")]
        public int Day {
            get => day;
            set => day = value;
        }

        [JsonProperty("time")]
        public string Time {
            get => time;
            set => time = value;
        }

        public ClosingOpeningTime(int day, string time) {
            Day = day;
            Time = time;
        }
    }
}
