using System;

namespace MySql.ProxyServer.Protocol
{
    public class CapabilityFlagAdapter
    {
        private readonly int lower;
        private readonly int upper;

        public CapabilityFlag Data
        {
            get
            {
                var result = (this.upper << 8) + this.lower;
                return (CapabilityFlag)result;
            }
        }

        public CapabilityFlagAdapter(int lower, int upper)
        {
            this.lower = lower;
            this.upper = upper;
        }
    }
}