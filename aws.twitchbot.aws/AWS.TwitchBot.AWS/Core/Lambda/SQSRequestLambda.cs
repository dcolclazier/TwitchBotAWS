using Amazon.Lambda.Core;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AWS.TwitchBot.AWS.Core.Lambda
{
    public class SQSRequestLambda 
    {

        [LambdaSerializer(typeof(JsonSerializer))]
        public async Task<string> OnAPIRequestAsync(JObject request, ILambdaContext context)
        {
            return await new SQSRequestLambdaProxy().OnApiRequest(request, context);
        }
        internal class SQSRequestLambdaProxy : LoggingResource
        {
            private IStreamEventProcessor _eventProcessor { get; set; }
            public SQSRequestLambdaProxy() : base(nameof(SQSRequestLambda)) { }

            internal async Task<string> OnApiRequest(JObject request, ILambdaContext context)
            {
                var jArray = JArray.Parse(request["Records"].ToString());

                var sqsMessages = jArray.ToObject<List<Message>>();
                _Logger.LogInformation($"Received SQS Messages: {sqsMessages.ToJsonString()}");

                foreach (var sqsMessage in sqsMessages)
                {
                    var attributes = sqsMessage.MessageAttributes;
                    var eventName = attributes["EventType"]?.StringValue;

                    var eventType = Assembly.GetExecutingAssembly().GetType("AWS.TwitchBot.AWS.Core.Events." + eventName);
                    if (eventType == null)
                    {
                        _Logger.LogError($"Couldn't process SQS Event {eventType}.... type unknown");
                        continue;
                    }

                    var streamEvent = (IStreamEvent)JObject.Parse(sqsMessage.Body).ToObject(eventType);

                    if (streamEvent == null)
                    {
                        _Logger.LogError($"Couldn't process SQS Event {eventType}.... deserialization failed");
                        continue;
                    }

                    try
                    {
                        #region Process message based on event type
                        var result = await streamEvent.ProcessMeAsync();

                        //stick response on the response queue

                        #endregion
                    }
                    catch (Exception ex)
                    {

                    }



                }
                return "Jobs done";
            }
        }

    }
}
