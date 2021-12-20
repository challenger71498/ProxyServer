using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer
{
    public class PayloadService : IPayloadService
    {
        public IObservable<IData> ToPacket(IObservable<IData> dataStream)
        {
            return dataStream.Select((data) =>
            {
                if (data is not IPayloadData payloadData)
                {
                    return data;
                }

                // Console.WriteLine(Convert.ToHexString(payloadData.Payloads.ElementAt(0)));

                var packetData = this.GetPackets(payloadData);

                // Console.WriteLine(Convert.ToHexString(packetData.Packets.ElementAt(0).Payload));

                return packetData;
            });
        }

        public IObservable<IData> FromPacket(IObservable<IData> dataStream)
        {
            return dataStream
                .GroupBy(data => data is PacketData)
                .SelectMany(data =>
                {
                    if (!data.Key)
                    {
                        return (IObservable<IData>)data;
                    }

                    var packetStream = data
                        .Select(data => (IPacketData)data);

                    return packetStream.Select(this.GetPayload);
                    // .Do(payload => Console.WriteLine($"{Convert.ToHexString(payload.Payload)}"))
                    // .Do(payload => Console.WriteLine($"{System.Text.Encoding.ASCII.GetString(payload.Payload)}"));
                    // TODO: Fix buffering.
                    // .Buffer(packetStream.Where(_ => false))
                    // .Do(packets =>
                    // {
                    //     foreach (var packet in packets)
                    //     {
                    //         Console.WriteLine($"GOT: {Convert.ToHexString(packet.Payload)}");
                    //     }
                    // })
                    // .SelectMany(a => a);;
                });
        }

        private IPacketData GetPackets(IPayloadData payloadData)
        {
            var packets = new List<IPacket>();
            var id = payloadData.InitialSequenceId;

            foreach (var payload in payloadData.Payloads)
            {
                var buffer = payload;

                while (buffer.Length >= 2E24 - 1)
                {
                    var length = (1024 * 1024 * 16) - 1;

                    var split = payload[..length];
                    buffer = payload[(length + 1)..];

                    var packet = new Packet(split.Length, id, split);

                    id += 1;
                    packets.Add(packet);
                }

                var last = new Packet(buffer.Length, id, buffer);
                packets.Add(last);
                id += 1;
            }

            return new PacketData(packets);
        }

        private bool IsPacketNotEOF(IPacket packet)
        {
            Console.Write("On IsPacketNotEOF: ");
            // Console.WriteLine(Convert.ToHexString(packet.Payload));

            return packet.PayloadLength != 0xffffff;
        }

        private IPayloadData GetPayload(IPacketData packetData)
        {
            var payloads = new List<byte[]>();
            var buffer = new List<byte>();
            int id = -1;

            foreach (var packet in packetData.Packets)
            {
                if (id == -1)
                {
                    id = packet.SequenceId;
                }

                buffer.AddRange(packet.Payload);

                if (packet.PayloadLength != 0xffffff)
                {
                    payloads.Add(buffer.ToArray());
                    buffer.Clear();
                }
            }

            var payloadData = new PayloadData(id, payloads);
            return payloadData;
        }
    }
}

// var payloadData = dataStream
//     .Where(data => data.GetType() == typeof(IPayloadData))
//     .Select(data => (IPayloadData)data)
//     .Select<IPayloadData, IData>(payloadData =>
//     {
//         foreach (var factory in this.factories)
//         {
//             if (factory.TryCreate(payloadData.Payload, out var protocol))
//             {
//                 return protocol;
//             }
//         }

//         return payloadData;
//     });

// var failedData = payloadData
//     .Where(payload => payload.GetType() == typeof(IPayloadData))
//     .Let(this.packetService.ToRawData);

// return Observable.Merge(rawStream, payloadData);

// return dataStream.Let<IData, IData>(data =>
// {
//     // Filter only IPayloadData.
//     var rawData = data.Where(data => data.GetType() != typeof(IPayloadData));

//     var payloadData = data.Select(data => (IPayloadData)data);



//     // If failed, rollback to raw binary data.
//     var packets = this.GetPackets(payloadData.InitialSequenceId, payloadData.Payload);
//     var binaries = packets.Select(packet => ?);
// });