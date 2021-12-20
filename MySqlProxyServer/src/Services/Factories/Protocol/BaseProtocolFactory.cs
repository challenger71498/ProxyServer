// Copyright (c) Min. All rights reserved.

using System;
using System.IO;
using Min.MySqlProxyServer.Protocol;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public abstract class BaseProtocolFactory : IProtocolFactory
    {
        public bool TryCreate(IPayloadData payloadData, out IProtocol protocol, ProxyState state)
        {
            try
            {
                protocol = Read(payloadData, state);
                return true;
            }
            catch (Exception e)
            {
                // Console.WriteLine($"Error! {e.GetType()} {e.Message}");
                // Console.WriteLine(e.StackTrace);

                // TODO: Handle exception properly.
                protocol = null;
                return false;
            }
        }

        protected abstract IProtocol Read(IPayloadData payloadData, ProxyState state);
    }
}