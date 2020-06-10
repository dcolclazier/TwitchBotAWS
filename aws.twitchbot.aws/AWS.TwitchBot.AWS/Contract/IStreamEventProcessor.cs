using System.Threading.Tasks;

namespace AWS.TwitchBot.AWS.Core.Lambda
{
    internal interface IStreamEventProcessor
    {
        Task ProcessAsync(IStreamEvent eventToProcess);
    }
}