using System;

namespace GoogleCloudClassLibrary {
    public class ConnectionSetup {
        private String apiKey;
        private Boolean hasSetKey;

        public string ApiKey { get => apiKey; private set => apiKey = value; }
        public bool HasSetKey { get => hasSetKey; private set => hasSetKey = value; }

        /*
         * Arguments: None
         * 
         * Description: Default Constructor that initializes the ApiKey to an empty string and sets the HasSetKey
         *   flag to false.
         */ 
        public ConnectionSetup() {
            ApiKey = "";
            HasSetKey = false;
        }

        /*
        * Arguments:
        *  - apiKey - String representing the Google Cloud API key for the user.
        *  
        * Description: Constructor that initializes the ApiKey with the apiKey argument value and sets the HasSetKey flag to 
        *   true.
        */
        public ConnectionSetup(String apiKey) {
            ApiKey = apiKey;

            if (!HasSetKey) {
                HasSetKey = true;
            }
        }

        /*
         * Arguments:
         *  - apiKey - String representing the Google Cloud API key for the user. 
         *  
         * Return Value: Returns a boolean
         *  - true: If a key does not exists and the argument passed in to the function is set as the new ApiKey
         *  - false: If a key already exists.
         *  
         * Description: Method to set the Google Cloud ApiKey. This method only sets the argument passed in as the
         *   key if the ApiKey does not already exist.
         */
        public Boolean setApiKeyNoReplace(String apiKey) {
            if (!HasSetKey) {
                ApiKey = apiKey;
                HasSetKey = true;

                return true;
            }

            return false;
        }

        /*
         * Arguments:
         *  - apiKey - String representing the Google Cloud API key for the user. 
         *  
         * Return Value: Void.
         *  
         * Description: Method to set the Google Cloud ApiKey. This method sets the argument passed in to the
         *   function as the key, irrespective of whether a key already exists. If an ApiKey value exists,
         *   then this method will overwrite that.
         */
        public void setApiKeyReplace(String apiKey) {
            ApiKey = apiKey;

            if (!HasSetKey)
            {
                HasSetKey = true;
            }
        }
    }
}
