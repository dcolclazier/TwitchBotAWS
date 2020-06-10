using AWS.TwitchBot.AWS.Core.Lambda;
using System.Threading.Tasks;
using TwitchLib.Api.ThirdParty.UsernameChange;
using TwitchLib.Client.Models;

namespace AWS.TwitchBot.AWS.Core.Events
{
    public class TicketsCommandEvent : IStreamEvent
    {
        public string Name => nameof(TicketsCommandEvent);

        public ChatMessage Message { get; }

        public static string Command => "!tickets";

        public async Task<StreamEventResponse> ProcessMeAsync()
        {
            //check dynamo for current ticket count for user
            var ticketCount = 7;
            var whisper = true;
            //add a response to the response queue containing answer.
            return new StreamEventResponse
            {
                UserName = Message.Username,
                Channel = Message.Channel,
                Response = $"Hey {Message.Username}, your current ticket balance is {ticketCount}",
                Whisper = whisper,
            };
        }

        public TicketsCommandEvent(ChatMessage message)
        {
            Message = message;
        }
    }
}