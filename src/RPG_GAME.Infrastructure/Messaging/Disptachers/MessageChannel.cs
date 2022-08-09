﻿using RPG_GAME.Application.Messaging;
using System.Threading.Channels;

namespace RPG_GAME.Infrastructure.Messaging.Disptachers
{
    internal sealed class MessageChannel : IMessageChannel
    {
        private readonly Channel<IMessage> _messages = Channel.CreateUnbounded<IMessage>();

        public ChannelReader<IMessage> Reader => _messages.Reader;

        public ChannelWriter<IMessage> Writer => _messages.Writer;
    }
}