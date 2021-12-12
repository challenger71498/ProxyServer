using System;

namespace Min.MySqlProxyServer.Protocol
{
    public struct CapabilityFlagAdapter
    {
        public int lower;
        public int upper;

        public CapabilityFlag Value
        {
            get
            {
                var result = (this.upper << 8) + this.lower;
                return (CapabilityFlag)result;
            }
        }
    }
}
