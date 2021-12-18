// Copyright (c) Min. All rights reserved.

using System;
using System.Threading.Tasks;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer
{
    public class PayloadService
    {
        private readonly IProtocolFactory[] protocolFactories;
        private readonly IProtocolProcessor[] protocolProcessors;

        public PayloadService(IProtocolFactory[] protocolFactories, IProtocolProcessor[] protocolProcessors)
        {
            this.protocolFactories = protocolFactories;
            this.protocolProcessors = protocolProcessors;
        }

        public async Task<PayloadInfo?> TryProcess(byte[] payload)
        {
            var protocol = await this.GetProtocol(payload);

            if (protocol == null)
            {
                return null;
            }

            var processed = await this.Process(protocol);

            return processed;
        }

        private async Task<IProtocol?> GetProtocol(byte[] data)
        {
            // Console.WriteLine("Trying to get protocol...");

            // NOTE: Multiprocessing?
            var protocolTask = new Task<IProtocol?>(() =>
            {
                foreach (var factory in this.protocolFactories)
                {
                    if (factory.TryCreate(data, out var protocol))
                    {
                        return protocol;
                    }
                }

                return null;
            });

            protocolTask.Start();

            return await protocolTask;
        }

        private async Task<PayloadInfo?> Process(IProtocol protocol)
        {
            // NOTE: Multiprocessing?
            var processTask = new Task<PayloadInfo?>(() =>
            {
                foreach (var processor in this.protocolProcessors)
                {
                    if (processor.TryProcess(protocol, out var info))
                    {
                        return info;
                    }
                }

                return null;
            });

            processTask.Start();

            return await processTask;
        }
    }
}
