using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SpecFlowSampleProject
{
    [Route("[controller]")]
    public class DownloadController: Controller
    {
        
        private string localWorkingDownloadDirectory = "/tmp/specflow/local/download/";
        private string localWorkingResultDirectory = "/tmp/specflow/local/result/";
        
        // get endpoint to download a file
        [HttpGet]
        public string Get()
        {
            return Download();
        }

        private string Download()
        {
            string message = "Starting download!";
            
            SftpManagerDefault sftpManager = new SftpManagerDefault();
            List<string> filesDownloaded = new List<string>();
            try
            {
                filesDownloaded = sftpManager.DownloadFile(localWorkingDownloadDirectory);
            }
            catch (Exception e)
            {
                message = e.StackTrace;
                Console.WriteLine(e);
            }
            
            foreach (var sftpFile in filesDownloaded)
            {
                try
                {
                    var sampleJeList = new Transformer().TransformFile(localWorkingDownloadDirectory + sftpFile);
                    foreach (var sampleJe in sampleJeList)
                    {
                        // just print content on console for now
                        Console.WriteLine("Downloaded Content: " + sftpFile + ": " + sampleJe);
                    }

                    sftpManager.UploadFile(localWorkingDownloadDirectory, sftpFile, "Success");
                    message = "Download is complete!";

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    message = e.Message + "\n" +  e.StackTrace;
                    try
                    {
                        //write error to result folder on sftp server
                        sftpManager.UploadFile( localWorkingDownloadDirectory,sftpFile,"Failure");
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }
            }

            return message;
        }
    }
}