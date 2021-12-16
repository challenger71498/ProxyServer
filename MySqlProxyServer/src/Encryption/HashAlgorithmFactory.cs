using System.Security.Cryptography;

namespace Min.MySqlProxyServer.Encryption
{
    public class HashAlgorithmFactory
    {
        public static HashAlgorithm Create(HashAlgorithmType type)
        {
            return type switch
            {
                HashAlgorithmType.SHA1 => SHA1.Create(),
                HashAlgorithmType.SHA256 => SHA256.Create(),
            };
        }
    }
}