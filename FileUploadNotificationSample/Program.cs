using System;
using Microsoft.Azure.Devices;

namespace FileUploadNotificationSample
{
    class Program
    {
        static ServiceClient serviceClient;
        static string connectionString = "HostName=az220-iot-hub.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=PGm3abo4XnoqOZ4mBAVcjdfjRRRjWofPQgOmvLEx5i8=";
        static void Main(string[] args)
        {
            Console.WriteLine("Receive file upload notifications\n");
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            ReceiveFileUploadNotificationAsync();
            Console.WriteLine("Press Enter to exit\n");
            Console.ReadLine();
        }

        private async static void ReceiveFileUploadNotificationAsync()
        {
            var notificationReceiver = serviceClient.GetFileNotificationReceiver();
            Console.WriteLine("\nListening for file upload notification...");
            while (true)
            {
                var fileUploadNotification = await notificationReceiver.ReceiveAsync();
                if (fileUploadNotification == null) continue;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Received file upload notification: {0}",
                  string.Join(", ", fileUploadNotification.BlobName));
                Console.ResetColor();
                await notificationReceiver.CompleteAsync(fileUploadNotification);
            }
        }
    }
}
