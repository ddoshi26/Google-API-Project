# Google-API-Project

The Google API Project is a combination of three projects:-
* GoogleCloudClassLibrary - Class library containing all the functions that can be used to query Google Cloud Places, Video, Image, and Natural Language Intelligence APIs.
* GoogleCloudConsole - Console project that can be used to run functions in the GoogleCloudClassLibrary and observe their performance
* GoogleCloudLibraryTest - Unit test project that contains a suite of tests. Each function in the GoogleCloudClassLibrary has a set of test which verify that it behaves as expected with valid requests and handles invalid requests cleanly.

The goal of the project is to provide a library of functions that can be called on by other C# to access the Google Cloud APIs. The library is designed to be an intermediary in between a calling program and the APIs, and is tasked to take in the request parameters and send back the response. All the implementation and backend details will be handled by the library.

## Setup

To setup and utilize the library please follow the following steps:

* Download Visual Studio: https://visualstudio.microsoft.com/downloads/. Commnity version is perfectly sufficient for this project
* Clone the repo into a directory on your system
* In Visual Studio, import a new project (File -> Open -> Project/Solution). Access the directory where you cloned the repo. Navigate into the GoogleCloudClassLibrary sub-directory and click on the .sln file there.
* Open the imported GoogleCloudClassLibrary project in Visual Studio. Access the NuGet Package Manager (Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution). In the NuGet Package Manager, make sure that the following packages are installed and up-to-date:
   - Newtonsoft.Json: https://www.newtonsoft.com/json
   - NUnit framework
   - NUnit3TestAdapter
   - Microsoft.AspNet.WebApi.Client
   - System.Net.Http
* Repeat the same for the other two projects, namely GoogleCloudConsole, and GoogleCloudLibraryTest.
* Compile both the GoogleCloudClassLibrary and GoogleCloudLibraryTest projects.
* In the GoogleCloudLibraryTest project add a reference to the projects References for the GoogleCloudClassLibrary.dll by following this guide from after step 8: https://www.c-sharpcorner.com/uploadfile/61b832/creating-class-library-in-visual-C-Sharp/.

## Execution and Testing

### Tests
To ensure that the GoogleClassLibrary functions are performing correctly, you can run the set of unit tests. For those who choose to run the tests, please be warned that the tests can take about 12-20 minutes to complete.

To run the tests, you will need to follow these steps:
* Create a new User Environment variable on your system with the name GC_LIBRARY_TEST and the value as <Your repo directory>/Google-API-Project/GoogleCloudLibraryTest/GoogleCloudLibraryTest/test-files.
* Navigate to your directory where the cloned repo exists and then go into GoogleCloudLibraryTest -> GoogleCloudLibraryTest -> test-files. In that folder, open the APIKEY.txt file. In the file, insert a line as following: API_KEY:YOUR_API_KEY
* Also ensure that all the API URLs provided are accurate and represent the desired endpoints.
* Once you have the file ready, go back to Visual Studio and access the Test Explorer  (Ctrl + E, T) and run all tests.
* If some of the tests fail, please rerun the tests. You can either rerun the failling tests or the entire set. If some of the tests fail again (except for those in AnnotateVideoMultipleDetections), then please report the issue on this repo.

### Using the Library

To use the functions in your own project, add the GoogleCloudClassLibrary.dll to your project's References by using this guide from after step 8: https://www.c-sharpcorner.com/uploadfile/61b832/creating-class-library-in-visual-C-Sharp/. Once you have that done, you will just need to import the appropriate class in your file headers, and you can directly call the functions wuth the necessary parameters. For explanation about the parameters for each function, refer to their respective comment blocks in GoogleCloudClassLibrary. Also make sure you have a file with the API URLs and your Google Cloud API key in the same format as shown in the test-files/APIKEY.txt file.
