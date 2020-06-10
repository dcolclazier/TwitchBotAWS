using Amazon.Lambda.Core;
using System.Threading.Tasks;

namespace AWS.TwitchBot.AWS.Core.Lambda
{
    public interface ISQSStrategy
    {
        Task ProcessMessage(string message, ILambdaContext context);
    }
}
