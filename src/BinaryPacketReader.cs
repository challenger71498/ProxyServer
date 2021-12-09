// using System;
// using System.IO;

// namespace ProxyServer.Protocol
// {
//     public class BinaryPayloadReader
//     {
//         public void Read(Stream stream, Foo foo)
//         {
//             foreach (var dataType in foo.types)
//             {

//             }
//         }

//         private int ReadFixedInteger(Stream stream, int length)
//         {
//             var bytes = new byte[length];

//             stream.Read(bytes);

//             Array.Reverse(bytes);

//             var hex = Convert.ToHexString(bytes);
//             var number = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);

//             return number;
//         }
//     }

//     public class Foo
//     {
//         public IDataType[] types;
//     }
// }

