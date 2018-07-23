using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.Places {
    public class Period {
        private ClosingOpeningTime close;
        private ClosingOpeningTime open;

        [JsonProperty("close")]
        public ClosingOpeningTime Close {
            get => close;
            set => close = value;
        }

        [JsonProperty("open")]
        public ClosingOpeningTime Open {
            get => open;
            set => open = value;
        }

        public Period(ClosingOpeningTime close, ClosingOpeningTime open) {
            Close = close;
            Open = open;
        }
    }
}
