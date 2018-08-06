using System;

namespace GoogleCloudClassLibrary {
    public class ResponseStatus {
        private int code;
        private String description;

        public int Code {
            get => code;
            set => code = value;
        }

        public String Description {
            get => description;
            set => description = value;
        }

        public ResponseStatus(int code, String description) {
            this.Code = code;
            this.Description = description;
        }
    }
}
