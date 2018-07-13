using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class Status {
        private double code;
        private String message;
        private List<Object> details;

        [JsonProperty("code")]
        public double Code {
            get => code; set => code = value;
        }

        [JsonProperty("message")]
        public String Message {
            get => message; set => message = value;
        }

        [JsonProperty("details")]
        public List<Object> Details {
            get => details; set => details = value;
        }

        public Status(double code, String message, List<Object> details) {
            this.Code = code;
            this.Message = message;
            this.Details = details;
        }
    }
}
