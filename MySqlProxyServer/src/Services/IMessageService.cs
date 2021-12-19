// Copyright (c) Min. All rights reserved.

using System;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Protocol;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public interface IMessageService
    {
        IObservable<IData> ToProtocol(IObservable<ISocketControllerMessage> messageStream);

        IObservable<ISocketControllerMessage> FromProtocol(IObservable<IData> dataStream);
    }

    public class MessageService : IMessageService
    {
        public IObservable<IData> ToProtocol(IObservable<ISocketControllerMessage> messageStream)
        {
            return messageStream.Select<ISocketControllerMessage, IData>(message =>
                {
                    if (message.Type == SocketControllerMessageType.RAW)
                    {
                        var rawMessage = (RawDataMessage)message;

                        return new BinaryData(rawMessage.Raw);
                    }

                    return this.OnMessageReceived(message);
                });
        }

        public IObservable<ISocketControllerMessage> FromProtocol(IObservable<IData> dataStream)
        {
            return dataStream.Select(data =>
            {
                // Create a raw data message, if data is raw.
                if (data is IBinaryData rawData)
                {
                    var rawMessage = new RawDataMessage(rawData.Raw);

                    return rawMessage;
                }

                if (data is not IProtocol protocol)
                {
                    throw new Exception($"This should not be happened. Data format \"{data.GetType()}\" is not equivalant to IProtocol.");
                }

                // Create a message from the protocol.
                return this.OnProtocolReceived(protocol);
            });
        }

        protected virtual IProtocol OnMessageReceived(ISocketControllerMessage message)
        {
            return null;
        }

        protected virtual ISocketControllerMessage OnProtocolReceived(IProtocol protocol)
        {
            return null;
        }
    }
}