// Copyright (c) Min. All rights reserved.

using System.Linq;
using System.Text;
using Min.MySqlProxyServer.Encryption;

namespace Min.MySqlProxyServer
{
    public class AuthService
    {
        public byte[] GetAuthData(HashAlgorithmType type, string password, string nonce)
        {
            var passwordBinary = Encoding.ASCII.GetBytes(password);
            var nonceBinary = Encoding.ASCII.GetBytes(nonce);

            using var hash = HashAlgorithmFactory.Create(type);

            var firstHashed = hash.ComputeHash(passwordBinary);
            var secondHashed = hash.ComputeHash(firstHashed);

            var concat = new byte[secondHashed.Length + nonce.Length];
            nonceBinary.CopyTo(concat, 0);
            secondHashed.CopyTo(concat, nonce.Length);

            var thirdHashed = hash.ComputeHash(concat);

            var result = firstHashed.Select((b, i) => (byte)(b ^ thirdHashed[i])).ToArray();

            return result;
        }
    }
}
