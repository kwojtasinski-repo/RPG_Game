using RPG_GAME.Application.Messaging;
using System.Threading.Channels;

namespace RPG_GAME.Infrastructure.Messaging.Disptachers
{
    internal interface IMessageChannel
    {
        ChannelReader<IMessage> Reader { get; }
        ChannelWriter<IMessage> Writer { get; }
    }
}
