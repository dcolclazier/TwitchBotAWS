namespace AWS.TwitchBot.AWS.Core.Lambda
{
    public class StreamEventResponse
    {
        public string UserName { get; set; }
        public string Response { get; set; }
        public string Channel { get; set; }

        public bool Whisper { get; set; } = false;
    }
}