using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;

namespace AWS.TwitchBot.Container
{
    class Program
    {
        private readonly static DockerLogger Logger = new DockerLogger(nameof(TwitchService));
        private static TwitchClient client;
        static async Task Main(string[] args)
        {
            //log into twitch, stay logged in
            //process queue messages while logged in
            //service = new TwitchService();
            client = new TwitchClient(new WebSocketClient(new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            }));
            client.OnConnected += OnConnected;
            client.OnMessageReceived += OnMessageReceived;
            client.OnWhisperReceived += OnWhisperReceived;
            client.OnError += OnError;
            client.OnConnectionError += OnConnectionError;
            client.Initialize(new ConnectionCredentials(TwitchInfo.UserName, TwitchInfo.Token), "Nerdtastic");

            client.Connect();

            Thread.Sleep(-1);
            


        }

        private static void OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            Logger.LogInformation("Got a whisper!");
        }

        private static void OnConnected(object sender, OnConnectedArgs e)
        {
            Logger.LogInformation($"Connected");
        }

        public static void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var message = e.ChatMessage.Message;
#if DEBUG
            if (message.StartsWith('%'))
            {
                Task.Run(async () =>
                {
                    try
                    {
                        using (var sqsClient = new AmazonSQSClient(Amazon.RegionEndpoint.USEast1))
                        {
                            var queueResponse = await sqsClient.GetQueueUrlAsync(new GetQueueUrlRequest
                            {
                                QueueName = "ReqeustQueue",
                            });
                            var response = await sqsClient.SendMessageAsync(new SendMessageRequest
                            {
                                QueueUrl = queueResponse.QueueUrl,
                                MessageBody = e.ToJsonString(),
                                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                                {
                                    {"TestAttribute", new MessageAttributeValue {DataType = "String", StringValue = "TestAttributeValue"} }
                                }
                            });
                            Logger.LogInformation("Sent sqs message");
                        }
                    }
                    catch (Exception ex)
                    {

                        Logger.LogError($"Error: {ex.ToJsonString()}");
                    }
                });
            }
#else
            throw new NotImplementedException("Not ready for release")


#endif
        }


        private static void OnPermissionError(object sender, EventArgs e)
        {
            Logger.LogError($"TwitchClient permission error: {e.ToJsonString()}");
        }

        private static void OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Logger.LogError($"TwitchClient connection error: {e.ToJsonString()}");
        }

        private static void OnError(object sender, OnErrorEventArgs e)
        {
            Logger.LogError($"TwitchClient error: {e.ToJsonString()}");
        }

        private static void OnReconnected(object sender, OnReconnectedEventArgs e)
        {
            Logger.LogInformation($"Service just reconnected: {e.ToJsonString()}");
        }

        private static void OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            Logger.LogInformation($"Service just disconnected: {e.ToJsonString()}");
            Connect();
        }

        public static void Connect()
        {
            Logger.LogInformation("Service connecting...");
            client.Connect();
            Logger.LogInformation("Connected.");
            client.JoinChannel("Nerdtastic");
#if DEBUG

            Logger.LogInformation($"Joined channels: {client.JoinedChannels.Count}");
            foreach (var channel in client.JoinedChannels)
            {
                client.SendMessage(channel, "I'm alive!");
            }

#endif
        }
        public static void Disconnect()
        {
            Logger.LogInformation("Service disconnecting");
            client.Disconnect();
        }

    }
}
