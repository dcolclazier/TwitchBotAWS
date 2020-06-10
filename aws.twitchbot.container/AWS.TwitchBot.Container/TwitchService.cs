using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;

namespace AWS.TwitchBot.Container
{
    /// <summary>
    /// Twitch Service that wraps TwitchClient
    /// </summary>
    public class TwitchService
    {
        private readonly TwitchClient _client;

        private readonly DockerLogger _logger = new DockerLogger(nameof(TwitchService));
        public TwitchClient Client => _client;
        public TwitchService(int throttlingPeriodInSeconds = 30, int messagesPerPeriod = 750)
        {
            _client = new TwitchClient(new WebSocketClient(new ClientOptions
            {
                MessagesAllowedInPeriod = messagesPerPeriod,
                ThrottlingPeriod = TimeSpan.FromSeconds(throttlingPeriodInSeconds)
            }));

            _client.OnDisconnected += OnDisconnected;
            _client.OnReconnected += OnReconnected;

            _client.OnError += OnError;
            _client.OnConnected += OnConnected;
            _client.OnConnectionError += OnConnectionError;
            _client.OnNoPermissionError += OnPermissionError;
            _client.OnMessageReceived += OnMessageReceived;
            _client.OnWhisperReceived += OnWhisperReceived;

                
            _client.Initialize(new ConnectionCredentials(TwitchInfo.UserName, TwitchInfo.Token), TwitchInfo.ChannelName);
            Connect();
        }

        private void OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            _logger.LogInformation("Got a whisper!");
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            _logger.LogInformation($"Connected");
        }

        public void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var message = e.ChatMessage.Message;
#if DEBUG
            if (message.StartsWith('%'))
            {
                Task.Run(async () =>
                {
                    _client.SendMessage(e.ChatMessage.Channel, "Hey, got your message");
                    using var sqsClient = new AmazonSQSClient();
                    var queueResponse = await sqsClient.GetQueueUrlAsync(new GetQueueUrlRequest
                    {
                        QueueName = "ReqeustQueue",
                    });
                    await sqsClient.SendMessageAsync(new SendMessageRequest
                    {
                        QueueUrl = queueResponse.QueueUrl,
                        MessageBody = e.ToJsonString(),
                        MessageAttributes = new Dictionary<string, MessageAttributeValue>
                        {
                            {"EventType", new MessageAttributeValue {DataType = "String", StringValue = "TicketsCommand"} }
                        }
                    });
                });
                
            }
#else
            throw new NotImplementedException("Not ready for release")
#endif
        }
        private void OnPermissionError(object sender, EventArgs e)
        {
            _logger.LogError($"TwitchClient permission error: {e.ToJsonString()}");
        }

        private void OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            _logger.LogError($"TwitchClient connection error: {e.ToJsonString()}");
        }

        private void OnError(object sender, OnErrorEventArgs e)
        {
            _logger.LogError($"TwitchClient error: {e.ToJsonString()}");
        }

        private void OnReconnected(object sender, OnReconnectedEventArgs e)
        {
            _logger.LogInformation($"Service just reconnected: {e.ToJsonString()}");
        }

        private void OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            _logger.LogInformation($"Service just disconnected: {e.ToJsonString()}");
            Connect();
        }

        public void Connect()
        {
            _logger.LogInformation("Service connecting...");
            _client.Connect();
            _logger.LogInformation("Connected.");
            _client.JoinChannel("Nerdtastic");
#if DEBUG   

            _logger.LogInformation($"Joined channels: {_client.JoinedChannels.Count}");
            foreach(var channel in _client.JoinedChannels)
            {
                _client.SendMessage(channel, "I'm alive!");
            }
            
#endif
        }
        public void Disconnect()
        {
            _logger.LogInformation("Service disconnecting");
            _client.Disconnect();
        }
    }
}
