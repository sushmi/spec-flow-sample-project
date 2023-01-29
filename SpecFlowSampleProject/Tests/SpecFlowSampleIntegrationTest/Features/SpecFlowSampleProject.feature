Feature: SpecFlowSample Project 
Simple project reads file from SFTP transforms and writes Acknowledgement file to SFTP

@mytag
Scenario: Download & Transform File from SFTP and Upload Acknowledgement
	Given <sftpFile> available on SFTP
	When <sftpFile> gets downloaded and transformed to SampleJE
	Then Acknowledge is as expected <acknowledgeFile> for given <sftpFile>
	Examples: 
	  | sftpFile    | acknowledgeFile    | Remark                  |
	  | 01/test.csv    | 01/result.txt         | Success on Valid file   |
	  | 02/test.csv | 02/result.txt | Failure on Invalid file |