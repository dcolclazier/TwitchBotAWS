using Amazon.Lambda.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace AWS.TwitchBot.AWS.Core.Lambda
{
    




    public class DockerContainerLogger
    {
        [LambdaSerializer(typeof(JsonSerializer))]
        public async Task<string> OnAPIRequestAsync(JObject request, ILambdaContext context)
        {
            return await new DockerContainerLoggerProxy().OnApiRequest(request, context);
        }
    }

    internal class DockerContainerLoggerProxy : LoggingResource
    {
        public DockerContainerLoggerProxy() : base(nameof(DockerContainerLoggerProxy)) { }
        

        internal async Task<string> OnApiRequest(JObject request, ILambdaContext context)
        {
            var messages = request["Messages"].ToObject<List<DockerMessage>>();

            await Task.Run(() =>
            {
                foreach (var message in messages)
                {
                    switch (message.Trace)
                    {
                        case TraceLevel.Error:
                            _Logger.LogError(message.Message);
                            break;
                        case TraceLevel.Warning:
                            _Logger.LogWarning(message.Message);
                            break;
                        case TraceLevel.Info:
                            _Logger.LogInformation(message.Message);
                            break;
                        case TraceLevel.Verbose:
                            _Logger.LogDebug(message.Message);
                            break;
                    }
                }
                
            });

            return "";

        }
    }
}
