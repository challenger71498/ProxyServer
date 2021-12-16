// Copyright (c) Min. All rights reserved.

using System.Reactive;
using System.Text;
using Unity;

namespace Min.MySqlProxyServer.Sockets
{
    public class FakeClient
    {
        [Dependency]
        private readonly ISocketConnection connection;

        [Dependency]
        private AuthModule authModule;

        private bool isSSL;

        private FakeClient()
        {
            this.connection = connection;

            var observer = Observer.Create<byte[]>(this.OnDataReceived);

            this.connection.WhenDataReceived.Subscribe(observer);
        }

        private void OnDataReceived(byte[] received)
        {
            var data = this.isSSL ? this.DecryptSSL(received) : received;
        }

        private byte[] DecryptSSL(byte[] data)
        {
            var output = new byte[data.Length];

            /* TODO: Decrypt SSL */

            return output;
        }

        private byte[] EncryptPassword(string password, byte[] nonce)
        {
            var passwordBinary = Encoding.ASCII.GetBytes(password);

            var authData = this.authModule.GetAuthData(passwordBinary, nonce);

            return authData;
        }
    }
}
