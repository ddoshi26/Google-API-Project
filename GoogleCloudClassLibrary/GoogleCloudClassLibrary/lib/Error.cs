using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary {
    public class Error {
        private int code;
        private String message;
        private String status;
        private List<ErrorDetail> details;

        [JsonProperty("code")]
        public int Code {
            get => code;
            set => code = value;
        }

        [JsonProperty("message")]
        public string Message {
            get => message;
            set => message = value;
        }

        [JsonProperty("status")]
        public string Status {
            get => status;
            set => status = value;
        }

        [JsonProperty("details")]
        public List<ErrorDetail> Details {
            get => details;
            set => details = value;
        }

        public Error(int code, string message, string status, List<ErrorDetail> details) {
            Code = code;
            Message = message;
            Status = status;
            Details = details;
        }
    }
}
