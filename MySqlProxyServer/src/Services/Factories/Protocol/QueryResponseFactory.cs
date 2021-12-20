using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Min.MySqlProxyServer.Protocol;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class QueryResponseFactory : BaseProtocolFactory
    {
        protected override IProtocol Read(IPayloadData payloadData, ProxyState state)
        {
            var queryResponse = new QueryResponse();

            var columnCountPayload = payloadData.Payloads.First();

            using var stream = new MemoryStream(columnCountPayload);
            using var reader = new BinaryReader(stream);

            queryResponse.ColumnCount = reader.ReadLengthEncodedInt();

            var columns = new List<ColumnData>();

            for (var i = 1; i < queryResponse.ColumnCount + 1; ++i)
            {
                var column = this.ReadColumnData(payloadData.Payloads.ElementAt(i));
                columns.Add(column);
            }

            var rows = new List<byte[]?[]>();

            for (var i = queryResponse.ColumnCount + 1; i < payloadData.Payloads.Count() - 1; i += queryResponse.ColumnCount)
            {
                var row = new List<byte[]?>();

                for (var j = 0; j < queryResponse.ColumnCount; ++j)
                {
                    var element = this.ReadRowData(payloadData.Payloads.ElementAt(i));
                    row.Add(element);
                }

                rows.Add(row.ToArray());
            }

            var okPacketFactory = new OKPacketFactory();

            var lastPayload = payloadData.Payloads.Last();
            var dummyPayloadData = new PayloadData(0, new[] { lastPayload });

            if (!okPacketFactory.TryCreate(dummyPayloadData, out var okPacket, state))
            {
                throw new Exception("Failed to create ok packet!");
            }

            queryResponse.OKPacket = (OKPacket)okPacket;

            queryResponse.Columns = columns;
            queryResponse.Rows = rows;

            if (stream.Position != stream.Length)
            {
                throw new Exception($"Reader {this.GetType()} did not reach EOF. {stream.Position} {stream.Length}");
            }

            return queryResponse;
        }

        private ColumnData ReadColumnData(byte[] payload)
        {
            var column = new ColumnData();

            using var stream = new MemoryStream(payload);
            using var reader = new BinaryReader(stream);

            column.Catalog = reader.ReadLengthEncodedString();
            column.Schema = reader.ReadLengthEncodedString();
            column.Table = reader.ReadLengthEncodedString();
            column.OrgTable = reader.ReadLengthEncodedString();
            column.Name = reader.ReadLengthEncodedString();
            column.OrgName = reader.ReadLengthEncodedString();

            reader.ReadLengthEncodedInt(); // 0x0c

            column.CharacterSet = reader.ReadFixedInt(2);
            column.ColumnLength = reader.ReadFixedInt(4);
            column.Type = reader.ReadFixedInt(1);
            column.Flags = reader.ReadFixedInt(2);
            column.Decimals = reader.ReadFixedInt(1);

            reader.ReadFixedInt(2); // filler

            return column;
        }

        private byte[]? ReadRowData(byte[] payload)
        {
            using var stream = new MemoryStream(payload);
            using var reader = new BinaryReader(stream);

            var nullCheck = reader.ReadFixedInt(1);

            if (nullCheck == 0xfb)
            {
                return null;
            }

            stream.Seek(-1, SeekOrigin.Current);

            return reader.ReadLengthEncodedString();
        }
    }
}