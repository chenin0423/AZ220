﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommandLine;
using Microsoft.Azure.Devices.Client;
using System;
using System.Threading.Tasks;

namespace AZ220Course.IoTHub.UploadFile
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            const string CONNECTION_STRING="HostName=az220-iot-hub.azure-devices.net;DeviceId=iotdev1;SharedAccessKey=Ejd8IWw14VqtiI460q/4ggsfsYC2skJno6Z0D3/96s0=";
            // Parse application parameters
            using var deviceClient = DeviceClient.CreateFromConnectionString(
                CONNECTION_STRING);
            var sample = new FileUploadSample(deviceClient);
            await sample.RunSampleAsync();

            await deviceClient.CloseAsync();

            Console.WriteLine("Done.");
            return 0;
        }
    }
}
