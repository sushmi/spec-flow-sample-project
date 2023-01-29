using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Renci.SshNet;
using TechTalk.SpecFlow;

namespace SpecFlowSampleIntegrationTest.Steps
{

    [Binding]
    public sealed class SpecFlowSampleProjectStepDefinitions
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;
        string sftpFilename = "";
        string testResourcePath = Path.Combine(Path.GetFullPath("../../../"), "TestResources");
        string integrationTestFilePath = Path.Combine("/tmp/specflow/itest" );

        private static HttpClient httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:5001"),
        };

        public SpecFlowSampleProjectStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            if (Directory.Exists(integrationTestFilePath))
            {
                Directory.Delete(integrationTestFilePath, true);
            }
            Directory.CreateDirectory(integrationTestFilePath);
        }
        
        static async Task TriggerDownload()
        {
            using (HttpResponseMessage response = await httpClient.GetAsync("download"))
            {
                Console.WriteLine(response.EnsureSuccessStatusCode());
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"{responseString}\n");
            }
        }

        [Given("(.*) available on SFTP")]
        public void GivenFileAvailableOnSFTP(string filename)
        {
            string fileToUpload = filename.Split("/").Last();
            string filePath = Path.Combine(testResourcePath, filename);
            using (var sftp = new SftpClient("localhost", 2222, "specflow", "pass"))
            {
                sftp.Connect();
                using (var fileStream = File.OpenRead(filePath))
                {
                    sftp.UploadFile(fileStream, "/upload/" + fileToUpload);
                    Console.WriteLine("Uploaded file: " + fileToUpload + " to sftp server");
                }
                sftp.Disconnect();
            }
        }

        [When("(.*) gets downloaded and transformed to SampleJE")]
        public void WhenFileGetsDownloadedAndTransformedToSampleJe(string filename)
        {
            sftpFilename = filename;
            var task = Task.Run(() => TriggerDownload()); //trigger download
            task.Wait();
            Console.WriteLine("Download triggered and completed.");
        }

        [Then("Acknowledge is as expected (.*) for given (.*)")]
        public void ThenAcknowledgementUploaded(string expectedAckFileContent, string uploadedFileName)
        {
            string[] actualFileDetails = uploadedFileName.Split("/");
            string actualAckFilename = actualFileDetails.Last();
            
            Directory.CreateDirectory(integrationTestFilePath + "/" + actualFileDetails.First());
            string actualAckTempLocation = integrationTestFilePath + "/ack_" + actualAckFilename;
            using (var sftp = new SftpClient("localhost", 2222, "specflow", "pass"))
            {
                sftp.Connect();
                using (var fileStream = File.OpenWrite(actualAckTempLocation))
                {
                    sftp.DownloadFile("/result/ack_" + actualAckFilename, fileStream);
                }

                sftp.Disconnect();
            }

            string actualFileText = File.ReadAllText(actualAckTempLocation);
            string expectedFileText = File.ReadAllText(testResourcePath + "/" + expectedAckFileContent);

            Assert.True(actualFileText == expectedFileText);
        }
    }
}