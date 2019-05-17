using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;
using System;


namespace ConsoleCloudFile
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Cloud Files");
            var app = new Program();
            // app.GoRead();
            app.GoWrite();
        }
        readonly string STRAGE_CONNECTION = 
            "";

        void GoRead()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(STRAGE_CONNECTION);
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
            CloudFileShare share = fileClient.GetShareReference("shared");
            if ( share.Exists() )
            {
                CloudFileDirectory rootDir = share.GetRootDirectoryReference();
                CloudFile file = rootDir.GetFileReference("test.txt");
                if (file.Exists())
                {
                    Console.WriteLine(file.DownloadText());
                }
            }
        }

        void GoWrite()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(STRAGE_CONNECTION);
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
            CloudFileShare share = fileClient.GetShareReference("shared");
            if (share.Exists())
            {
                CloudFileDirectory rootDir = share.GetRootDirectoryReference();
                CloudFile file = rootDir.GetFileReference("test-" + DateTime.Now.ToString("yyyyMMdd-hhmm") + ".txt");
                file.UploadText($"Upload sample : {DateTime.Now}");
            }
        }
    }
}
