// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using OscCore.LowLevel;

namespace OscCore
{
    /// <summary>
    ///     Base class for all osc packets
    /// </summary>
    public abstract class OscPacket
    {
        /// <summary>
        ///     The packet origin
        /// </summary>
        public Uri Origin { get; protected set; }

        /// <summary>
        ///     The size of the packet in bytes
        /// </summary>
        public abstract int SizeInBytes { get; }

        internal OscPacket()
        {
        }

        public static OscPacket Parse(string str, IFormatProvider provider = null)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException(nameof(str));
            }

            OscStringReader reader = new OscStringReader(str);

            return Parse(ref reader, provider, OscSerializationToken.End);
        }

        public static OscPacket Parse(ref OscStringReader reader, IFormatProvider provider = null, OscSerializationToken endToken = OscSerializationToken.End)
        {
            if (reader.PeekChar() == '#')
            {
                return OscBundle.Parse(ref reader, provider, endToken);
            }

            return OscMessage.Parse(ref reader, provider, endToken);
        }

        /// <summary>
        ///     Read the osc packet from a byte array
        /// </summary>
        /// <param name="bytes">array to read from</param>
        /// <param name="index">the offset within the array where reading should begin</param>
        /// <param name="count">the number of bytes in the packet</param>
        /// <param name="origin">the origin that is the origin of this packet</param>
        /// <param name="timeTag">the time tag asociated with the parent</param>
        /// <returns>the packet</returns>
        public static OscPacket Read(
            byte[] bytes,
            int index,
            int count,
            Uri origin = null,
            OscTimeTag? timeTag = null)
        {
            //if (OscBundle.IsBundle(bytes, index, count) == true)
            if (bytes[index] == (byte) '#')
            {
                return OscBundle.Read(bytes, index, count, origin);
            }

            return OscMessage.Read(bytes, index, count, origin, timeTag);
        }

        public static OscPacket Read(
            OscReader reader,
            int count,
            Uri origin = null,
            OscTimeTag? timeTag = null)
        {
            if (reader.PeekByte() == (byte) '#')
            {
                return OscBundle.Read(reader, count, origin);
            }

            return OscMessage.Read(reader, count, origin, timeTag);
        }

        /// <summary>
        ///     Get an array of bytes containing the entire packet
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToByteArray();

        public static bool TryParse(string str, out OscPacket packet)
        {
            try
            {
                packet = Parse(str);

                return true;
            }
            catch
            {
                packet = default(OscPacket);

                return false;
            }
        }

        public static bool TryParse(string str, IFormatProvider provider, out OscPacket packet)
        {
            try
            {
                packet = Parse(str, provider);

                return true;
            }
            catch
            {
                packet = default(OscPacket);

                return false;
            }
        }

        /// <summary>
        ///     Send the packet into a byte array
        /// </summary>
        /// <param name="data">the destination for the packet</param>
        /// <param name="index">the offset within the array where writing should begin</param>
        /// <returns>the length of the packet in bytes</returns>
        public abstract int Write(byte[] data, int index);

        public abstract void Write(OscWriter writer);

        public abstract void WriteToString(OscStringWriter writer);
    }
}