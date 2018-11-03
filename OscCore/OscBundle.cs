// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using OscCore.LowLevel;

namespace OscCore
{
    /// <summary>
    ///     Bundle of osc messages
    /// </summary>
    public sealed class OscBundle : OscPacket, IEnumerable<OscPacket>, IEnumerable<OscMessage>
    {
        public const string BundleIdent = "#bundle";

        private const int BundleHeaderSizeInBytes = 16;

        private OscPacket[] packets;

        /// <summary>
        ///     Access bundle messages by index
        /// </summary>
        /// <param name="index">the index of the message</param>
        /// <returns>message at the supplied index</returns>
        public OscPacket this[int index] => packets[index];

        /// <summary>
        ///     The number of messages in the bundle
        /// </summary>
        public int Count => packets.Length;

        /// <summary>
        ///     The size of the packet in bytes
        /// </summary>
        public override int SizeInBytes => BundleHeaderSizeInBytes + (this as IEnumerable<OscPacket>).Sum(message => 4 + message.SizeInBytes);

        /// <summary>
        ///     Osc timestamp associated with this bundle
        /// </summary>
        public OscTimeTag Timestamp { get; private set; }

        /// <summary>
        ///     Create a bundle of messages
        /// </summary>
        /// <param name="timestamp">timestamp</param>
        /// <param name="messages">messages to bundle</param>
        public OscBundle(OscTimeTag timestamp, params OscPacket[] messages)
        {
            Origin = null;

            Timestamp = timestamp;
            packets = messages;
        }

        /// <summary>
        ///     Create a bundle of messages
        /// </summary>
        /// <param name="timestamp">timestamp</param>
        /// <param name="messages">messages to bundle</param>
        public OscBundle(DateTime timestamp, params OscPacket[] messages)
        {
            Origin = null;

            Timestamp = OscTimeTag.FromDataTime(timestamp);
            packets = messages;
        }

        /// <summary>
        ///     Create a bundle of messages
        /// </summary>
        /// <param name="origin">the origin of the osc bundle</param>
        /// <param name="timestamp">timestamp</param>
        /// <param name="messages">messages to bundle</param>
        public OscBundle(Uri origin, OscTimeTag timestamp, params OscPacket[] messages)
        {
            Origin = origin;

            Timestamp = timestamp;
            packets = messages;
        }

        /// <summary>
        ///     Create a bundle of messages
        /// </summary>
        /// <param name="origin">the origin of the osc bundle</param>
        /// <param name="timestamp">timestamp</param>
        /// <param name="messages">messages to bundle</param>
        public OscBundle(Uri origin, DateTime timestamp, params OscPacket[] messages)
        {
            Origin = origin;

            Timestamp = OscTimeTag.FromDataTime(timestamp);
            packets = messages;
        }

        private OscBundle()
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (packets as IEnumerable).GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator<OscMessage> IEnumerable<OscMessage>.GetEnumerator()
        {
            return Messages();
        }

        /// <summary>
        ///     Enumerate all the osc packets contained in this bundle
        /// </summary>
        /// <returns>A IEnumerator of osc packets</returns>
        public IEnumerator<OscPacket> GetEnumerator()
        {
            return (packets as IEnumerable<OscPacket>).GetEnumerator();
        }

        /// <summary>
        ///     Does the array contain a bundle packet?
        /// </summary>
        /// <param name="bytes">the array that contains a packet</param>
        /// <param name="index">the offset within the array where the packet starts</param>
        /// <param name="count">the number of bytes in the packet</param>
        /// <returns>true if the packet contains a valid bundle header</returns>
        public static bool IsBundle(byte[] bytes, int index, int count)
        {
            if (count < BundleHeaderSizeInBytes)
            {
                return false;
            }

            string ident = Encoding.UTF8.GetString(bytes, index, BundleIdent.Length);

            return BundleIdent.Equals(ident, StringComparison.Ordinal);
        }

        public IEnumerator<OscMessage> Messages()
        {
            foreach (OscPacket packet in packets)
            {
                switch (packet)
                {
                    case OscMessage message:
                        yield return message;
                        break;
                    case OscBundle oscBundle:
                        using (IEnumerator<OscMessage> messages = oscBundle.Messages())
                        {
                            while (messages.MoveNext())
                            {
                                yield return messages.Current;
                            }
                        }

                        break;
                    default:
                        throw new Exception();
                }
            }
        }

        /// <summary>
        ///     parse a bundle from a string using a supplied format provider
        /// </summary>
        /// <param name="str">a string containing a bundle</param>
        /// <param name="provider">the format provider to use</param>
        /// <returns>the parsed bundle</returns>
        public new static OscBundle Parse(string str, IFormatProvider provider = null)
        {
            if (provider == null)
            {
                provider = CultureInfo.InvariantCulture;
            }

            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException(nameof(str));
            }

            OscStringReader reader = new OscStringReader(str);

            return Parse(ref reader, provider, OscSerializationToken.End);
        }

        public new static OscBundle Parse(ref OscStringReader reader, IFormatProvider provider = null, OscSerializationToken endToken = OscSerializationToken.End)
        {
            if (provider == null)
            {
                provider = CultureInfo.InvariantCulture;
            }

            string ident = reader.ReadAddress(false)
                .Trim();

            if (BundleIdent.Equals(ident, StringComparison.Ordinal) == false)
            {
                throw new Exception($"Invalid bundle ident '{ident}'");
            }

            reader.ReadSeparator();

            if (reader.ReadNextToken(out string timeStampStr) != OscSerializationToken.Literal)
            {
                throw new Exception("Invalid bundle timestamp");
            }

            OscTimeTag timeStamp = OscTimeTag.Parse(timeStampStr.Trim(), provider);

            if (reader.ReadSeparatorOrEnd() == endToken)
            {
                return new OscBundle(timeStamp);
            }

            List<OscPacket> packets = new List<OscPacket>();

            OscSerializationToken token = OscSerializationToken.None;

            token = reader.ReadNextToken(out string _);

            while (token != endToken && token != OscSerializationToken.End)
            {
                if (token != OscSerializationToken.ObjectStart)
                {
                    throw new Exception("Invalid bundle token");
                }

                packets.Add(OscPacket.Parse(ref reader, provider, OscSerializationToken.ObjectEnd));

                token = reader.ReadNextToken(out string _);

                if (token == OscSerializationToken.Separator)
                {
                    token = reader.ReadNextToken(out string _);
                }
            }

            if (token != endToken)
            {
                throw new OscException(OscError.UnexpectedToken, $"Unexpected token {token}");
            }

            return new OscBundle(timeStamp, packets.ToArray());
        }

        /// <summary>
        ///     Read a OscBundle from a array of bytes
        /// </summary>
        /// <param name="bytes">the array that contains the bundle</param>
        /// <param name="index">the offset within the array where reading should begin</param>
        /// <param name="count">the number of bytes in the bundle</param>
        /// <param name="origin">the origin that is the origin of this bundle</param>
        /// <returns>the bundle</returns>
        public static OscBundle Read(
            byte[] bytes,
            int index,
            int count,
            Uri origin = null)
        {
            ArraySegment<byte> arraySegment = new ArraySegment<byte>(bytes, index, count);

            OscReader reader = new OscReader(arraySegment);

            return Read(reader, count, origin);
        }

        public static OscBundle Read(OscReader reader, int count, Uri origin = null)
        {
            OscBundle bundle = new OscBundle
            {
                Origin = origin
            };

            int start = reader.Position;
            int bundleEnd = start + count;

            reader.BeginBundle(count);

            string ident = reader.ReadAddress();

            if (BundleIdent.Equals(ident, StringComparison.Ordinal) == false)
            {
                // this is an error
                throw new OscException(OscError.InvalidBundleIdent, $"Invalid bundle ident '{ident}'");
            }

            bundle.Timestamp = reader.ReadBundleTimeTag();

            List<OscPacket> messages = new List<OscPacket>();

            while (reader.Position < bundleEnd)
            {
                if (reader.Position + 4 > bundleEnd)
                {
                    // this is an error
                    throw new OscException(OscError.InvalidBundleMessageHeader, "Invalid bundle message header");
                }

                int messageLength = reader.ReadBundleMessageLength(start, count);

                if (reader.Position + messageLength > bundleEnd ||
                    messageLength < 0 ||
                    messageLength % 4 != 0)
                {
                    // this is an error
                    throw new OscException(OscError.InvalidBundleMessageLength, "Invalid bundle message length");
                }

                messages.Add(Read(reader, messageLength, origin, bundle.Timestamp));
            }

            bundle.packets = messages.ToArray();

            return bundle;
        }

        public OscPacket[] ToArray()
        {
            return packets;
        }

        /// <summary>
        ///     Creates a byte array that contains the osc message
        /// </summary>
        /// <returns></returns>
        public override byte[] ToByteArray()
        {
            byte[] data = new byte[SizeInBytes];

            Write(data, 0);

            return data;
        }

        public override string ToString()
        {
            OscStringWriter writer = new OscStringWriter();

            WriteToString(writer);

            return writer.ToString();
        }

        /// <summary>
        ///     Try to parse a bundle from a string using the InvariantCulture
        /// </summary>
        /// <param name="str">the bundle as a string</param>
        /// <param name="bundle">the parsed bundle</param>
        /// <returns>true if the bundle could be parsed else false</returns>
        public static bool TryParse(string str, out OscBundle bundle)
        {
            try
            {
                bundle = Parse(str, CultureInfo.InvariantCulture);

                return true;
            }
            catch
            {
                bundle = null;

                return false;
            }
        }

        /// <summary>
        ///     Try to parse a bundle from a string using a supplied format provider
        /// </summary>
        /// <param name="str">the bundle as a string</param>
        /// <param name="provider">the format provider to use</param>
        /// <param name="bundle">the parsed bundle</param>
        /// <returns>true if the bundle could be parsed else false</returns>
        public static bool TryParse(string str, IFormatProvider provider, out OscBundle bundle)
        {
            try
            {
                bundle = Parse(str, provider);

                return true;
            }
            catch
            {
                bundle = null;

                return false;
            }
        }

        /// <summary>
        ///     Send the bundle into a byte array
        /// </summary>
        /// <param name="data">an array ouf bytes to write the bundle into</param>
        /// <param name="index">the index within the array where writing should begin</param>
        /// <returns>the number of bytes in the message</returns>
        public override int Write(byte[] data, int index)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                OscWriter writer = new OscWriter(stream);

                stream.Position = index;

                Write(writer);

                return (int) stream.Position - index;
            }
        }

        public override void Write(OscWriter writer)
        {
            OscTimeTag timestamp = Timestamp;

            writer.StartBundle(BundleIdent, ref timestamp);

            foreach (OscPacket message in this)
            {
                writer.WriteBundleMessageLength(message.SizeInBytes);

                message.Write(writer);
            }
        }

        public override void WriteToString(OscStringWriter writer)
        {
            writer.WriteBundleIdent(Timestamp);

            foreach (OscPacket packet in this)
            {
                writer.WriteToken(OscSerializationToken.Separator);
                writer.WriteToken(OscSerializationToken.ObjectStart);
                packet.WriteToString(writer);
                writer.WriteToken(OscSerializationToken.ObjectEnd);
            }
        }
    }
}