using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class Proxy
    {
        private readonly IConnectionDelegator client;
        private readonly IConnectionDelegator server;
        private readonly ProtocolService protocolService;
        private readonly AuthService authService;

        private IData lastData;
        private int lastPayloadSequenceId;

        public Proxy(
            IConnectionDelegator client,
            IConnectionDelegator server,
            ProtocolService protocolService,
            AuthService authService)
        {
            this.client = client;
            this.server = server;

            this.authService = authService;
            this.protocolService = protocolService;

            var clientProtocolStream = this.client.WhenDataCreated.Select(this.OnDataCreated);
            var serverProtocolStream = this.server.WhenDataCreated.Select(this.OnDataCreated);

            var clientDataStream = clientProtocolStream.Select(this.OnClientProtocolCreated);
            var serverDataStream = serverProtocolStream.Select(this.OnServerProtocolCreated);

            this.server.SetDataReceiveStream(clientDataStream.Select(this.OnProtocolReceived));
            this.client.SetDataReceiveStream(serverDataStream.Select(this.OnProtocolReceived));
        }

        private IData OnDataCreated(IData data)
        {
            if (data is not PayloadData payload)
            {
                return data;
            }

            this.lastPayloadSequenceId = payload.InitialSequenceId;

            var factories = this.protocolService.GetAvailableFactories(this.lastData);

            foreach (var factory in factories)
            {
                if (factory.TryCreate(payload.Payload, out var protocol))
                {
                    return protocol;
                }
            }

            return data;
        }

        private IData OnProtocolReceived(IData data)
        {
            if (data is not IWritableProtocol protocol)
            {
                return data;
            }

            var payload = protocol.ToPayload();

            Console.WriteLine(Convert.ToHexString(payload));

            return new PayloadData(this.lastPayloadSequenceId, payload);
        }

        private IData OnClientProtocolCreated(IData data)
        {
            this.lastData = data;
            return data;
        }

        private IData OnServerProtocolCreated(IData data)
        {
            if (data is HandshakeResponse handshakeResponse)
            {
                var root = Encoding.ASCII.GetBytes("root");
                handshakeResponse.Username = root;

                return handshakeResponse;
            }

            if (data is AuthSwitchResponse authSwitchResponse)
            {
                if (this.lastData is not AuthSwitchRequest lastAuthRequest)
                {
                    throw new Exception("Last data should be auth request.");
                }

                var nonce = lastAuthRequest.AuthPluginData[..^1];

                Console.WriteLine($"NONCE:{Convert.ToHexString(nonce)}");

                Console.WriteLine($"FROM: {Convert.ToHexString(authSwitchResponse.AuthPluginResponse)}");

                var foobar = Encoding.ASCII.GetBytes("foobar");

                var authData = this.authService.GetAuthData(Encryption.HashAlgorithmType.SHA1, foobar, nonce);

                Console.WriteLine($"TO  : {Convert.ToHexString(authData)}");

                authSwitchResponse.AuthPluginResponse = authData;
                return authSwitchResponse;
            }

            this.lastData = data;
            return data;
        }



        private void OnClientDisconnected(object? sender, EventArgs e)
        {
            Console.WriteLine("Client has been disconnected!");
            // this.server.Disconnect(); TODO: Handle client disconnection.
        }

        private void OnServerDisconnected(object? sender, EventArgs e)
        {
            Console.WriteLine("Server has been disconnected!");
        }
    }

    public class ProtocolService
    {
        private IEnumerable<IProtocolFactory> defaultFactories;

        public ProtocolService(IEnumerable<IProtocolFactory> defaultFactories)
        {
            this.defaultFactories = defaultFactories;
        }

        public IEnumerable<IProtocolFactory> GetAvailableFactories(IData? data)
        {
            if (data == null)
            {
                return this.defaultFactories;
            }

            if (data is not IProtocol protocol)
            {
                return this.defaultFactories;
            }

            var availableFactories = protocol.NextAvailableProtocolFactories;

            return availableFactories ?? this.defaultFactories;
        }
    }
}
