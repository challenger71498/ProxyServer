// Copyright (c) Min. All rights reserved.

using System.Linq;
using System.Text;
using Min.MySqlProxyServer.Encryption;

namespace Min.MySqlProxyServer
{
    public class AuthService
    {
        public byte[] GetAuthData(HashAlgorithmType type, byte[] password, byte[] nonce)
        {
            using var hash = HashAlgorithmFactory.Create(type);

            var firstHashed = hash.ComputeHash(password);
            var secondHashed = hash.ComputeHash(firstHashed);

            var concat = new byte[secondHashed.Length + nonce.Length];
            nonce.CopyTo(concat, 0);
            secondHashed.CopyTo(concat, nonce.Length);

            var thirdHashed = hash.ComputeHash(concat);

            var result = firstHashed.Select((b, i) => (byte)(b ^ thirdHashed[i])).ToArray();

            return result;
        }
    }
}
