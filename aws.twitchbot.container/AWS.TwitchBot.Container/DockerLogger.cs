using Amazon.Lambda;
using Amazon.Lambda.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AWS.TwitchBot.Container
{
    public class DockerLogger
    {
        private string _loggerNamer;
        private long ticksSinceLastSend;
        private readonly List<DockerMessage> _messages = new List<DockerMessage>();

        public DockerLogger(string loggerName)
        {
            _loggerNamer = loggerName;
        }


        public void LogDebug(string message) => Invoke(message, TraceLevel.Info);
        public void LogInformation(string message) => Invoke(message, TraceLevel.Info);
        public void LogError(string message) => Invoke(message, TraceLevel.Error);
        public void LogWarning(string message) => Invoke(message, TraceLevel.Warning);


        private void Invoke(string message, TraceLevel trace)
        {
            try
            {
                _messages.Add(new DockerMessage
                {
                    Message = $"{_loggerNamer} ({DateTime.Now.ToLocalTime()}): {message}",
                    Trace = trace
                });

                if (TimeSpan.FromTicks(DateTime.Now.Ticks - ticksSinceLastSend).TotalSeconds >= 3)
                {
                    Flush();
                    ticksSinceLastSend = DateTime.Now.Ticks;
                }
            }
            catch(Exception ex)
            {
                File.AppendAllText(@"\usr\local\share\InvokeErr.txt", ex.ToString() + System.Environment.NewLine);
            }
        }

        public void Flush()
        {
            try
            {
                using (var client = new AmazonLambdaClient(Amazon.RegionEndpoint.USEast1))
                {
                    client.InvokeAsync(new InvokeRequest
                    {
                        FunctionName = "DockerContainerLogger",
                        InvocationType = "Event",
                        Payload = new
                        {
                            Messages = _messages
                        }.ToJsonString()
                    });
                    _messages.Clear();
                };
            }
            catch (Exception)
            {
                //ignored
            }
        }
    }
}
