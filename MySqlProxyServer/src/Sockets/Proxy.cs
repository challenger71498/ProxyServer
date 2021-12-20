using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public struct ProxyState
    {
        public CapabilityFlag? Capability { get; set; }
    }

    public class Proxy
    {
        private readonly IConnectionDelegator client;
        private readonly IConnectionDelegator server;
        private readonly ProtocolService protocolService;
        private readonly AuthService authService;
        private readonly LoggerService loggerService;

        private readonly Subject<IData> toServerDataSubject = new();
        private readonly Subject<IData> toClientDataSubject = new();

        private ProxyState state;
        private IData? lastData = null;
        private int? lastPayloadSequenceId = null;

        public Proxy(
            IConnectionDelegator client,
            IConnectionDelegator server,
            ProtocolService protocolService,
            AuthService authService,
            LoggerService loggerService)
        {
            this.client = client;
            this.server = server;

            this.authService = authService;
            this.protocolService = protocolService;
            this.loggerService = loggerService;

            var clientDataStream = this.client.WhenDataCreated
                .Select(this.OnDataCreated)
                .Select(this.OnClientProtocolCreated)
                .Where(data => data != null)
                .Repeat();

            var serverDataStream = this.server.WhenDataCreated
                .Select(this.OnDataCreated)
                .Select(this.OnServerProtocolCreated)
                .Where(data => data != null)
                .Repeat();

            this.server.SetDataReceiveStream(this.toServerDataSubject.Select(this.OnProtocolReceived));
            this.client.SetDataReceiveStream(this.toClientDataSubject.Select(this.OnProtocolReceived));

            clientDataStream.Subscribe(data => this.toServerDataSubject.OnNext(data));
            serverDataStream.Subscribe(data => this.toClientDataSubject.OnNext(data));
        }

        private IData OnDataCreated(IData data)
        {
            if (data is not PayloadData payload)
            {
                return data;
            }

            this.lastPayloadSequenceId = payload.InitialSequenceId;

            // Console.WriteLine($"{data.GetType()} is getting available factories from {this.lastData?.GetType()}");
            var factories = this.protocolService.GetAvailableFactories(this.lastData);

            foreach (var factory in factories)
            {
                if (factory.TryCreate(payload, out var protocol, this.state))
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

            var payload = protocol.ToPayloads();

            return new PayloadData(this.lastPayloadSequenceId ?? 0, payload);
        }

        private IData? OnClientProtocolCreated(IData data)
        {
            if (data is QueryResponse queryResponse)
            {
                var len = queryResponse.ColumnCount;
                var random = new Random();

                var mask = random.Next(0, len);

                queryResponse.Columns.ElementAt(mask).Type = 0xfe;

                foreach (var row in queryResponse.Rows)
                {
                    row[mask] = Encoding.ASCII.GetBytes("***");
                }
            }

            this.lastData = data;
            return data;
        }

        private IData? OnServerProtocolCreated(IData data)
        {
            if (data is HandshakeResponse handshakeResponse)
            {
                this.state.Capability = handshakeResponse.Capability;

                var root = Encoding.ASCII.GetBytes("root");
                handshakeResponse.Username = root;

                this.lastData = data;
                return handshakeResponse;
            }

            if (data is AuthSwitchResponse authSwitchResponse)
            {
                if (this.lastData is not AuthSwitchRequest lastAuthRequest)
                {
                    throw new Exception("Last data should be auth request.");
                }

                var nonce = lastAuthRequest.AuthPluginData[..^1];
                var foobar = Encoding.ASCII.GetBytes("foobar");

                var authData = this.authService.GetAuthData(Encryption.HashAlgorithmType.SHA1, foobar, nonce);

                authSwitchResponse.AuthPluginResponse = authData;

                this.lastData = data;
                return authSwitchResponse;
            }

            if (data is QueryCommand queryCommand)
            {
                var queryString = Encoding.ASCII.GetString(queryCommand.Query);

                this.loggerService.Log(queryString);

                if (queryString.Contains("CHEQUER"))
                {
                    Console.WriteLine("Nope.");

                    var error = new ErrorProtocol()
                    {
                        ErrorCode = 0,
                        ErrorMessage = Encoding.ASCII.GetBytes("No permission to access the CHEQUER"),
                    };

                    this.lastPayloadSequenceId++;
                    this.toServerDataSubject.OnNext(error);
                    return null;
                }

                this.lastData = data;
                return data;
            }

            this.lastData = data;
            return data;
        }
    }
}
