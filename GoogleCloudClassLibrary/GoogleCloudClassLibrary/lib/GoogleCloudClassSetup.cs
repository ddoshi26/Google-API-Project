using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary {
    public class GoogleCloudClassSetup {
        private String configFileLocation;
        
        public string ConfigFileLocation {
            get => configFileLocation;
            set => configFileLocation = value;
        }

        public GoogleCloudClassSetup(String configFileLocation) {
            ConfigFileLocation = configFileLocation;
        }

        public String getAPIKey() {
            String fileText = System.IO.File.ReadAllText(ConfigFileLocation);

            String[] textArray = fileText.Split("\r\n".ToCharArray());

            for (int i = 0; i < textArray.Length; i++) {
                if (textArray[i].StartsWith("API_KEY")) {
                    String key = textArray[i].Substring(textArray[i].IndexOf(':') + 1);
                    return key;
                }
            }

            throw new MissingFieldException("API_URL field not found. Expected format: \"API_URL:<URL>\"");
        }

        public String getAPIUrl() {
            String fileText = System.IO.File.ReadAllText(ConfigFileLocation);

            String[] textArray = fileText.Split("\r\n".ToCharArray());

            for (int i = 0; i < textArray.Length; i++) {
                if (textArray[i].StartsWith("API_URL")) {
                    String URL = textArray[i].Substring(textArray[i].IndexOf(':') + 1);
                    return URL;
                }
            }

            throw new MissingFieldException("API_URL field not found. Expected format: \"API_URL:<URL>\"");
        }

        public String getAPIUrl(String URL_Tag) {
            String fileText = System.IO.File.ReadAllText(ConfigFileLocation);

            String[] textArray = fileText.Split("\r\n".ToCharArray());
            
            for (int i = 0; i < textArray.Length; i++) {
                if (textArray[i].StartsWith(URL_Tag)) {
                    String URL = textArray[i].Substring(textArray[i].IndexOf(':') + 1);
                    return URL;
                }
            }

            throw new MissingFieldException(URL_Tag + " field not found. Expected format: \"API_URL:<URL>\"");
        }
    }
}
