using ProxyServer.Protocol.DataType;

namespace ProxyServer.Protocol
{
    public class Handshake : IProtocol
    {
        public IDataType[] Datas => throw new System.NotImplementedException();

        public int protocolVersion;
        public string serverVersion;
        public int connectionId;
        public AuthPluginData authPluginData;
        public CapabilityFlag capabilityFlag;
    }

    public struct AuthPluginData
    {
        public string part1;
        public string part2;
    }

    public class Protocol
    {
        public void Read(byte[] binary, IDataType[] datas)
        {
            foreach (var data in datas)
            {
                if (data.GetType() == typeof(FixedIntegerType))
                {
                    ((FixedIntegerType)data).Read(binary);
                }
            }
        }
    }

    public interface IProtocol
    {
        IDataType[] CurrentDataTypes { get; }


    }
}

