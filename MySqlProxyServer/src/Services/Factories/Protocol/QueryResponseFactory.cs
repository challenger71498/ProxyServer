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

            this.ReadColumnCount(columnCountPayload, queryResponse);

            var columns = new List<ColumnData>();

            for (var i = 1; i < queryResponse.ColumnCount + 1; ++i)
            {
                var column = this.ReadColumnData(payloadData.Payloads.ElementAt(i));
                columns.Add(column);
            }

            var rows = new List<byte[]?[]>();

            foreach (var payload in payloadData.Payloads.ToArray()[(queryResponse.ColumnCount + 1)..^1])
            {
                using var stream = new MemoryStream(payload);
                using var reader = new BinaryReader(stream);

                var flag = reader.ReadByte();
                stream.Seek(-1, SeekOrigin.Current);

                if (flag == 0xfe)
                {
                    break;
                }

                var row = new List<byte[]?>();

                for (var j = 0; j < queryResponse.ColumnCount; ++j)
                {
                    var nullCheck = reader.ReadByte();

                    if (nullCheck == 0xfb)
                    {
                        row.Add(null);
                        continue;
                    }

                    stream.Seek(-1, SeekOrigin.Current);

                    var element = reader.ReadLengthEncodedString();
                    row.Add(element);
                }

                rows.Add(row.ToArray());
            }

            var lastPayload = payloadData.Payloads.Last();
            var okPacket = this.ReadOKPacket(lastPayload, state);
            queryResponse.OKPacket = okPacket;

            queryResponse.Columns = columns;
            queryResponse.Rows = rows;

            return queryResponse;
        }

        private void ReadColumnCount(byte[] payload, QueryResponse response)
        {
            using var stream = new MemoryStream(payload);
            using var reader = new BinaryReader(stream);

            response.ColumnCount = reader.ReadLengthEncodedInt();
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
            stream.Seek(-1, SeekOrigin.Current);

            if (nullCheck == 0xfb)
            {
                return null;
            }

            return reader.ReadLengthEncodedString();
        }

        private OKPacket ReadOKPacket(byte[] payload, ProxyState state)
        {
            var okPacketFactory = new OKPacketFactory();
            var dummyPayloadData = new PayloadData(0, new[] { payload });

            if (!okPacketFactory.TryCreate(dummyPayloadData, out var okPacket, state))
            {
                throw new Exception("Failed to create ok packet!");
            }

            return (OKPacket)okPacket;
        }
    }
}