using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class Operation {
        private String name;
        private Object metadata;
        private Boolean operationDone;
        private Status status;
        private Object response;

        [JsonProperty("name")]
        public String Name {
            get => name; set => name = value;
        }

        [JsonProperty("metadata")]
        public Object Metadata {
            get => metadata; set => metadata = value;
        }

        [JsonProperty("done")]
        public Boolean OperationDone {
            get => operationDone; set => operationDone = value;
        }

        [JsonProperty("status")]
        public Status Status {
            get => status; set => status = value;
        }

        [JsonProperty("response")]
        public Object Response {
            get => response; set => response = value;
        }

        public Operation(String name, Object metadata, Boolean operationDone, Status status, Object response) {
            this.Name = name;
            this.Metadata = metadata;
            this.OperationDone = operationDone;
            this.Status = status;
            this.Response = response;
        }
    }
}
