// Copyright (c) Min. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public abstract class MySqlConnection
    {
        public MySqlConnection(SocketConnection connection, CapabilityFlag capability)
        {
            this.connection = connection;
            this.Capability = capability;

            this.connection.OnReceiveData += this.OnDataReceived;
        }

        private SocketConnection connection;

        public CapabilityFlag Capability { get; private set; }

        protected async Task<IProtocol?> GetProtocol(byte[] data)
        {
            var protocolFactories = new List<IProtocolFactory>();

            var protocolTask = new Task<IProtocol?>(() =>
            {
                foreach (var factory in protocolFactories)
                {
                    if (factory.TryCreate(data, out var protocol))
                    {
                        return protocol;
                    }
                }

                return null;
            });

            protocolTask.Start();

            return await protocolTask;
        }

        private async void OnDataReceived(object? sender, byte[] data)
        {
            var protocol = await this.GetProtocol(data);

            if (protocol != null)
            {
                /* Modify protocol if needed*/
                protocol.ToBinary();
            }
        }
    }
}
