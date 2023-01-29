using System;
using System.Collections.Generic;
using System.IO;
using Renci.SshNet;

namespace SpecFlowSampleProject
{
    public class SftpManagerDefault : SftpManager
    {
        string sftpServer = "localhost";
        string username = "specflow";
        string password = "pass";
        int port = 2222;
        
        private string remoteUploadDirectory = "/upload/";
        private string remoteResultDirectory = "/result/";
        public List<string> DownloadFile(string localWorkingDownloadDirectory)
        {
            List<string> downloadedFiles = new List<string>();
            using (var sftp = new SftpClient(sftpServer, port, username, password)) {
                sftp.Connect();
                var files = sftp.ListDirectory("/upload");
                foreach (var sftpFile in files) 
                {
                    if (!sftpFile.IsDirectory)
                    {
                        string fileToDownload = sftpFile.Name;
                        Console.WriteLine("Downloading file: " + fileToDownload + " to working dir " +
                                          localWorkingDownloadDirectory);
                        using (var fileStream = File.OpenWrite( localWorkingDownloadDirectory + fileToDownload))
                        {
                            sftp.DownloadFile(remoteUploadDirectory + fileToDownload, fileStream);
                        }
                        downloadedFiles.Add(fileToDownload);
                        //Delete after download
                        sftp.DeleteFile(remoteUploadDirectory + fileToDownload);
                    }
                }
                
                sftp.Disconnect();
            }

            return downloadedFiles;
        }

        public void UploadFile(string localWorkingDirectory, string filename, string content)
        {
            // upload file to sftp server
            using (var sftp = new SftpClient(sftpServer, port, username, password))
            {
                sftp.Connect();
                Console.WriteLine("Staging Location for content: " + localWorkingDirectory + filename);
                Console.WriteLine("Staging Content: "  + content + " to " + remoteResultDirectory + "ack_" + filename + "");
                File.WriteAllText(localWorkingDirectory + filename, content);
                string resultFilename =  "ack_" + filename;
                using (var fileStream = File.OpenRead( localWorkingDirectory + filename))
                {
                    sftp.UploadFile(fileStream, remoteResultDirectory + resultFilename);
                }
                sftp.Disconnect();
            }
        }
    }
}