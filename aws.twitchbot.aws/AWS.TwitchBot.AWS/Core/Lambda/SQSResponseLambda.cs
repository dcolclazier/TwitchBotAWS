using Amazon.Lambda.Core;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;

namespace AWS.TwitchBot.AWS.Core.Lambda
{
    public class SQSResponseLambda
    {

        [LambdaSerializer(typeof(JsonSerializer))]
        public async Task<string> OnAPIRequestAsync(JObject request, ILambdaContext context)
        {
            return await new SQSResponseLambdaProxy().OnApiRequest(request, context);
        }
        internal class SQSResponseLambdaProxy : LoggingResource
        {
            private IStreamEventProcessor _eventProcessor { get; set; }
            public SQSResponseLambdaProxy() : base(nameof(SQSResponseLambda)) { }

            internal async Task<string> OnApiRequest(JObject request, ILambdaContext context)
            {
                var jArray = JArray.Parse(request["Records"].ToString());

                var sqsMessages = jArray.ToObject<List<Message>>();
                _Logger.LogInformation($"Received SQS Messages: {sqsMessages.ToJsonString()}");

                var client = new TwitchClient(new WebSocketClient(new ClientOptions
                {
                    MessagesAllowedInPeriod = 750,
                    ThrottlingPeriod = TimeSpan.FromSeconds(30)
                }));
                client.Initialize(new ConnectionCredentials(TwitchInfos.UserName, TwitchInfos.Token), "Nerdtastic");
                client.Connect();
                foreach (var sqsMessage in sqsMessages)
                {

                    var streamResponse = JObject.Parse(sqsMessage.Body).ToObject<StreamEventResponse>();
                    //log in as our bot
                    //send the message to the user.
                    
                    if (streamResponse.Whisper)
                    {
                        client.SendWhisper(streamResponse.UserName, streamResponse.Response);
                    }
                    else
                    {
                        client.SendMessage(new JoinedChannel(streamResponse.Channel), streamResponse.Response);
                    }
                    
                }
                client.Disconnect();

                return "Jobs done - fix me";
            }
        }

    }
}
