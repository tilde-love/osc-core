// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OscCore.LowLevel;

namespace OscCore
{
    public sealed class OscBundleRaw : IEnumerable<OscMessageRaw>
    {
        private readonly OscMessageRaw[] messages;

        public OscMessageRaw this[int index] => messages[index];

        public int Count => messages.Length;

        public Uri Origin { get; }

        public OscTimeTag Timestamp { get; }

        public OscBundleRaw(ArraySegment<byte> buffer, Uri origin = null)
        {
            Origin = origin;

            OscReader reader = new OscReader(buffer);

            List<OscMessageRaw> messages = new List<OscMessageRaw>();

            ReadMessages(buffer, reader, buffer.Count, messages, out OscTimeTag timestamp);

            Timestamp = timestamp;

            this.messages = messages.ToArray();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return messages.GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<OscMessageRaw> GetEnumerator()
        {
            return (messages as IEnumerable<OscMessageRaw>).GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReadMessages(
            ArraySegment<byte> buffer,
            OscReader reader,
            int count,
            List<OscMessageRaw> messages,
            out OscTimeTag timestamp)
        {
            int start = reader.Position;
            int bundleEnd = start + count;

            reader.BeginBundle(count);

            string ident = reader.ReadAddress();

            if (OscBundle.BundleIdent.Equals(ident, StringComparison.Ordinal) == false)
            {
                // this is an error
                throw new OscException(OscError.InvalidBundleIdent, $"Invalid bundle ident '{ident}'");
            }

            timestamp = reader.ReadBundleTimeTag();

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

                if (reader.PeekByte() == (byte) '#')
                {
                    ReadMessages(buffer, reader, messageLength, messages, out OscTimeTag _);
                }
                else
                {
                    messages.Add(new OscMessageRaw(new ArraySegment<byte>(buffer.Array, buffer.Offset + reader.Position, messageLength), Origin, timestamp));

                    reader.Position += messageLength;
                }
            }
        }
    }
}