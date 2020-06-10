using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace AWS.TwitchBot.AWS.Core.Lambda
{
    public interface IStreamEvent
    {
        string Name { get; }
        static string Command { get; }
        ChatMessage Message { get; }
        Task<StreamEventResponse> ProcessMeAsync();
    }
}