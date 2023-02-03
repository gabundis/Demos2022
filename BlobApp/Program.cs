using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=gs204storageaccount;AccountKey=TK9Rr2rrI5GUeeKXX/B8oMV/QtQVCI3aCpV2WogNnszbffvk6P4PO0+9fIgBec7vzXTKOUYREVCp+AStXFeQDg==;EndpointSuffix=core.windows.net";
string containerName = "scripts";
string blobName = "script.sql";
string filePath = "C:\\Projects\\Demos2022\\tmp\\" + blobName;

//await SetBlobMetaData();
//await GetMetaData();
await AcquireLease();

async Task AcquireLease()
{
    BlobClient blobClient = new(connectionString, containerName, blobName);

    BlobLeaseClient blobLeaseClient = blobClient.GetBlobLeaseClient();
    TimeSpan leaseTime = new TimeSpan(0, 0, 1, 00);

    Response<BlobLease> response = await blobLeaseClient.AcquireAsync(leaseTime);

    Console.WriteLine($"Lease id is {response.Value.LeaseId}");
}

//async Task SetBlobMetaData()
//{
//    BlobClient blobClient = new(connectionString, containerName, blobName);

//    IDictionary<string, string> metaData = new Dictionary<string, string>();
//    metaData.Add("Deparment", "Logistics");
//    metaData.Add("Application", "Appa");

//    await blobClient.SetMetadataAsync(metaData);

//    Console.WriteLine("Metadata added");
//}

//async Task GetMetaData()
//{
//    BlobClient blobClient = new(connectionString, containerName, blobName);
//    BlobProperties blobProperties = await blobClient.GetPropertiesAsync();

//    foreach(var metaData in blobProperties.Metadata)
//    {
//        Console.WriteLine($"The key is '{metaData.Key}'");
//        Console.WriteLine($"The value is '{metaData.Value}'");
//    }
//}

// Connecting to the azure storage account
//BlobServiceClient blobServiceClient = new(connectionString);
//BlobContainerClient blobContainerClient = new(connectionString, containerName);

// Creating a container with an access type
//blobServiceClient.CreateBlobContainerAsync(containerName, PublicAccessType.Blob).Wait();

// Getting the BlobClient
//BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
//BlobClient blobClient = new(connectionString, containerName, blobName);

// Uploading a blob via BlobContainerClient
//blobClient.UploadAsync(filePath, true).Wait();

// Listing blobs within a container
//await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
//{
//    Console.WriteLine($"The blob name is '{blobItem.Name}'");
//    Console.WriteLine($"The blob size is '{blobItem.Properties.ContentLength}' bytes");
//}

// Downloading a blob
//await blobClient.DownloadToAsync(filePath);

//Console.WriteLine($"Container '{containerName}' created");
//Console.WriteLine($"Upload file '{blobName}' completed");
//Console.WriteLine($"The file '{blobName}' was downloaded");