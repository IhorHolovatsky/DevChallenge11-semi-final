# DevChallenge11-semi-final

How To Run:

1. cd to Vagrant folder, run "vagrant up" command, then wait until virtual machine will be initialized
2. Done,WebApi will be available by this url - http://127.0.0.1:1025/

For example,
1) you can find document by identifier: http://127.0.0.1:1025/Document/GetDocumentByIdentifier/?documentIdentifier=479-17-06
2) you can sync documents: http://127.0.0.1:1025/Admin/SyncDocuments?publishStartDate=7/1/2017&publishEndDate=6/1/2017&token=E651A2A0-C5DC-4231-B8E6-B1D4F37CF0FF

 P.S: E651A2A0-C5DC-4231-B8E6-B1D4F37CF0FF - it's admin token

*NOTES*:
 Currently database is located at Amazon. But you can replace connection string with yours.
 Since, I used EF Code first approach, database will be created automatically.

