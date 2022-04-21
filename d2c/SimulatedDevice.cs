using System.Security.Cryptography;
using Microsoft.CSharp.RuntimeBinder;
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// This application uses the Azure IoT Hub device SDK for .NET
// For samples see: https://github.com/Azure/azure-iot-sdk-csharp/tree/master/iothub/device/samples

using System;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace simulatedDevice
{
    class SimulatedDevice
    {
        private static DeviceClient s_deviceClient;

        // The device connection string to authenticate the device with your IoT hub.
        private const string s_connectionString = "HostName=az220-iot-hub.azure-devices.net;DeviceId=iotdev1;SharedAccessKey=Ejd8IWw14VqtiI460q/4ggsfsYC2skJno6Z0D3/96s0=";
        private const string s_serviceConnectionString = "HostName=az220-iot-hub.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=PGm3abo4XnoqOZ4mBAVcjdfjRRRjWofPQgOmvLEx5i8=";

        /*
        // Async method to send simulated telemetry
        private static async void SendDeviceToCloudMessagesAsync()
        {

            while (true)
            {
                var messageString = "{'status':'success','message':'Success retrieving data'}";
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.ContentType="application/json";
                message.ContentEncoding="utf-8";

                // Send the tlemetry message
                await s_deviceClient.SendEventAsync(message).ConfigureAwait(false);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(1000).ConfigureAwait(false);
            }
        }
        private static async void RecieveC2dAsync()
        {
            Console.WriteLine("\nReceiving cloud to device messages from service");
            while(true)
            {
                Message receiveMessage = await s_deviceClient.ReceiveAsync();
                if(receiveMessage == null) continue;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Received message: {0}",
                Encoding.ASCII.GetString(receiveMessage.GetBytes()));
                Console.ResetColor();

                await s_deviceClient.CompleteAsync(receiveMessage);

            }
        }*/
        private async static Task SendCloudToDeviceMessagesAsync()
        {
            ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(s_serviceConnectionString);
            string targetDevice = "iotdev1";
            var commandMessage = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes("This is my C2D message."));
            await serviceClient.SendAsync(targetDevice, commandMessage);
        }

        static async void HandleDesiredPropertiesChange()
        {
            await s_deviceClient.SetDesiredPropertyUpdateCallbackAsync(async (desired, ctx) =>
            {
                Newtonsoft.Json.Linq.JValue fpsJson = desired["FPS"];
                var fps = fpsJson.Value;

                Console.WriteLine("Recieved desired FPS: {0}", fps);
            }, null);
        } 


        private static void Main()
        {
            Console.WriteLine("IoT Hub Quickstarts - Simulated device. Ctrl-C to exit.\n");

            // Connect to the IoT hub using the MQTT protocol
            s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString, Microsoft.Azure.Devices.Client.TransportType.Mqtt);
            //SendDeviceToCloudMessagesAsync();
            //RecieveC2dAsync();
            //SendCloudToDeviceMessagesAsync();
            HandleDesiredPropertiesChange();
            Console.ReadLine();
        }
    }
}
