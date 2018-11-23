// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace OscCore.LowLevel
{
    public class OscReader
    {
        private static readonly bool IsLittleEndian;
        private readonly ArraySegment<byte> buffer;
        private OscToken currentToken;
        private BitFlipper32 flipper32;
        private int maxPosition;

        public int Position { get; set; }

        static OscReader()
        {
            IsLittleEndian = BitConverter.IsLittleEndian;
        }

        public OscReader(ArraySegment<byte> buffer)
        {
            if (buffer.Count % 4 != 0)
            {
                throw new OscException(OscError.InvalidSegmentLength, "The packet length is not the correct size");
            }

            this.buffer = buffer;
            currentToken = OscToken.OscAddress;
            flipper32 = new BitFlipper32();
            Position = 0;
            maxPosition = buffer.Count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BeginBundle(int count)
        {
            maxPosition = Position + count;
            currentToken = OscToken.OscAddress;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BeginMessage(int count)
        {
            maxPosition = Position + count;
            currentToken = OscToken.OscAddress;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EndArray(ref OscTypeTag typeTag)
        {
            CheckToken(OscToken.ArrayEnd);

            currentToken = typeTag.NextToken();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetArgumentCount(ref OscTypeTag typeTag, out OscToken arrayType)
        {
            return typeTag.GetArgumentCount(out arrayType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetBlobArgumentSize(int position)
        {
            int result = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer.Array, buffer.Offset + position));

            return 4 + result + CalculatePadding(4 + position + result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetStringArgumentSize(int position)
        {
            int stringStart = position;
            bool failed = true;

            // scan forward and look for the end of the string 
            while (position < maxPosition)
            {
                if (buffer.Array[buffer.Offset + position++] != 0)
                {
                    continue;
                }

                failed = false;

                break;
            }

            if (failed)
            {
                throw new OscException(OscError.ErrorParsingString, @"Terminator could not be found while parsing getting string length");
            }

            return position - stringStart + CalculatePadding(position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte PeekByte()
        {
            return buffer.Array[buffer.Offset + Position];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscToken PeekToken()
        {
            return currentToken;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadAddress()
        {
            CheckToken(OscToken.OscAddress);

            int start = Position;
            bool failed = true;

            // scan forward and look for the end of the address string 
            while (Position < maxPosition)
            {
                if (buffer.Array[buffer.Offset + Position++] != 0)
                {
                    continue;
                }

                failed = false;

                break;
            }

            if (failed)
            {
                // this shouldn't happen and means we're decoding rubbish
                throw new OscException(OscError.MissingAddress, "Address terminator could not be found");
            }

            // check for an empty string
            if (Position - start - 1 == 0)
            {
                throw new OscException(OscError.MissingAddress, "Address was empty");
            }

            // read the string 
            string result = Encoding.UTF8.GetString(buffer.Array, buffer.Offset + start, Position - start - 1);

            // Advance to the typetag
            if (SkipPadding() == false)
            {
                throw new OscException(OscError.InvalidSegmentLength, "Unexpected end of message");
            }

            currentToken = Position == buffer.Count ? OscToken.End : OscToken.TypeTag;

            return result;
        }

        /// <summary>
        ///     This method is not the preferred way to parse arguments because any argument will be boxed into a object.
        ///     Instead you should use the correct value type reader for each argument.
        /// </summary>
        /// <returns>the argument value boxed as an object.</returns>
        /// <exception cref="OscException">If the current token is not an argument token.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object ReadArgument(ref OscTypeTag typeTag)
        {
            switch (currentToken)
            {
                case OscToken.Char:
                    return ReadChar(ref typeTag);
                case OscToken.Bool:
                case OscToken.True:
                case OscToken.False:
                    return ReadBool(ref typeTag);
                case OscToken.String:
                    return ReadString(ref typeTag);
                case OscToken.Symbol:
                    return ReadSymbol(ref typeTag);
                case OscToken.Impulse:
                    return ReadImpulse(ref typeTag);
                case OscToken.Null:
                    return ReadNull(ref typeTag);
                case OscToken.Int:
                    return ReadInt(ref typeTag);
                case OscToken.Long:
                    return ReadLong(ref typeTag);
                case OscToken.Float:
                    return ReadFloat(ref typeTag);
                case OscToken.Double:
                    return ReadDouble(ref typeTag);
                case OscToken.TimeTag:
                    return ReadTimeTag(ref typeTag);
                case OscToken.Blob:
                    return ReadBlob(ref typeTag)
                        .ToArray();
                case OscToken.Color:
                    return ReadColor(ref typeTag);
                case OscToken.Midi:
                    return ReadMidi(ref typeTag);
                default:
                    throw new OscException(OscError.UnexpectedToken, $"Expected aregument token got {currentToken}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArraySegment<byte> ReadBlob(ref OscTypeTag typeTag)
        {
            CheckToken(OscToken.Blob);

            CheckForPacketEnd(OscError.ErrorParsingBlob, 4);

            uint length = unchecked((uint) ReadIntDirect()); // unchecked ((uint) IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer.Array, buffer.Offset + Position)));


            // this shouldn't happen and means we're decoding rubbish
            if (length > 0 && Position + length > maxPosition)
            {
                throw new OscException(OscError.ErrorParsingBlob, $"Unexpected end of message while parsing argument '{typeTag.Index}'");
            }

            ArraySegment<byte> segment = new ArraySegment<byte>(buffer.Array, buffer.Offset + Position, (int) length);

            Position += (int) length;

            // Advance pass the padding
            if (SkipPadding() == false)
            {
                throw new OscException(OscError.ErrorParsingBlob, $"Unexpected end of message while parsing argument '{typeTag.Index}'");
            }

            currentToken = typeTag.NextToken();

            return segment;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBool(ref OscTypeTag typeTag)
        {
            CheckToken(OscToken.Bool);

            bool result = currentToken == OscToken.True;

            currentToken = typeTag.NextToken();

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadBundleMessageLength(int start, int count)
        {
            CheckToken(OscToken.BundleMessageLength);

            maxPosition = start + count;

            CheckForPacketEnd(OscError.ErrorParsingInt32, 4);

            int result = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer.Array, buffer.Offset + Position));

            Position += 4;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscTimeTag ReadBundleTimeTag()
        {
            CheckToken(OscToken.TypeTag);

            CheckForPacketEnd(OscError.ErrorParsingOscTimeTag, 8);

            ulong result = unchecked((ulong) IPAddress.NetworkToHostOrder(BitConverter.ToInt64(buffer.Array, buffer.Offset + Position)));

            Position += 8;
            currentToken = OscToken.BundleMessageLength;

            return new OscTimeTag(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadChar(ref OscTypeTag typeTag)
        {
            CheckToken(OscToken.Char);

            CheckForPacketEnd(OscError.ErrorParsingChar, 4);

            byte result = buffer.Array[buffer.Offset + Position];

            Position += 4;
            currentToken = typeTag.NextToken();

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscColor ReadColor(ref OscTypeTag typeTag)
        {
            CheckToken(OscToken.Color);

            CheckForPacketEnd(OscError.ErrorParsingColor, 4);

            uint value = unchecked((uint) IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer.Array, buffer.Offset + Position)));

            byte a, r, g, b;

            r = (byte) ((value & 0xFF000000) >> 24);
            g = (byte) ((value & 0x00FF0000) >> 16);
            b = (byte) ((value & 0x0000FF00) >> 8);
            a = (byte) (value & 0x000000FF);

            Position += 4;
            currentToken = typeTag.NextToken();

            return OscColor.FromArgb(a, r, g, b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArraySegment<byte> ReadDirectBlob()
        {
            CheckForPacketEnd(OscError.ErrorParsingBlob, 4);

            uint length = unchecked((uint) ReadIntDirect());

            // this shouldn't happen and means we're decoding rubbish
            if (length > 0 && Position + length > maxPosition)
            {
                throw new OscException(OscError.ErrorParsingBlob, "Unexpected end of message while parsing blob");
            }

            return new ArraySegment<byte>(buffer.Array, buffer.Offset + Position, (int) length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadDirectChar()
        {
            CheckForPacketEnd(OscError.ErrorParsingChar, 4);

            return buffer.Array[buffer.Offset + Position];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscColor ReadDirectColor()
        {
            CheckForPacketEnd(OscError.ErrorParsingColor, 4);

            uint value = unchecked((uint) IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer.Array, buffer.Offset + Position)));

            byte a, r, g, b;

            r = (byte) ((value & 0xFF000000) >> 24);
            g = (byte) ((value & 0x00FF0000) >> 16);
            b = (byte) ((value & 0x0000FF00) >> 8);
            a = (byte) (value & 0x000000FF);

            return OscColor.FromArgb(a, r, g, b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDirectDouble()
        {
            CheckForPacketEnd(OscError.ErrorParsingDouble, 8);

            long value = IPAddress.NetworkToHostOrder(BitConverter.ToInt64(buffer.Array, buffer.Offset + Position));

            return BitConverter.Int64BitsToDouble(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadDirectFloat()
        {
            CheckForPacketEnd(OscError.ErrorParsingSingle, 4);

            int value = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer.Array, buffer.Offset + Position));

            flipper32.ValueInt32 = value;

            return flipper32.ValueFloat;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadDirectInt()
        {
            CheckForPacketEnd(OscError.ErrorParsingChar, 4);

            return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer.Array, buffer.Offset + Position));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadDirectLong()
        {
            CheckForPacketEnd(OscError.ErrorParsingInt64, 8);

            return IPAddress.NetworkToHostOrder(BitConverter.ToInt64(buffer.Array, buffer.Offset + Position));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscMidiMessage ReadDirectMidi()
        {
            CheckForPacketEnd(OscError.ErrorParsingInt32, 4);

            return new OscMidiMessage(unchecked((uint) IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer.Array, buffer.Offset + Position))));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadDirectString()
        {
            CheckForPacketEnd(OscError.ErrorParsingString, 4);

            int stringStart = Position;
            bool failed = true;

            // scan forward and look for the end of the string 
            while (Position < maxPosition)
            {
                if (buffer.Array[buffer.Offset + Position++] != 0)
                {
                    continue;
                }

                failed = false;


                break;
            }

            if (failed)
            {
                throw new OscException(OscError.ErrorParsingString, @"Terminator could not be found while parsing string");
            }

            return Encoding.UTF8.GetString(buffer.Array, buffer.Offset + stringStart, Position - stringStart - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscSymbol ReadDirectSymbol()
        {
            return new OscSymbol(ReadDirectString());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscTimeTag ReadDirectTimeTag()
        {
            CheckForPacketEnd(OscError.ErrorParsingOscTimeTag, 8);

            ulong result = unchecked((ulong) IPAddress.NetworkToHostOrder(BitConverter.ToInt64(buffer.Array, buffer.Offset + Position)));

            return new OscTimeTag(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDouble(ref OscTypeTag typeTag)
        {
            CheckToken(OscToken.Double);

            CheckForPacketEnd(OscError.ErrorParsingDouble, 8);

            long value = IPAddress.NetworkToHostOrder(BitConverter.ToInt64(buffer.Array, buffer.Offset + Position));

            double result = BitConverter.Int64BitsToDouble(value);

            Position += 8;
            currentToken = typeTag.NextToken();

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloat(ref OscTypeTag typeTag)
        {
            CheckToken(OscToken.Float);

            CheckForPacketEnd(OscError.ErrorParsingSingle, 4);

            int value = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer.Array, buffer.Offset + Position));

            flipper32.ValueInt32 = value;

            float result = flipper32.ValueFloat;

            Position += 4;
            currentToken = typeTag.NextToken();

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscImpulse ReadImpulse(ref OscTypeTag typeTag)
        {
            CheckToken(OscToken.Impulse);

            currentToken = typeTag.NextToken();

            return OscImpulse.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt(ref OscTypeTag typeTag)
        {
            CheckToken(OscToken.Int);

            CheckForPacketEnd(OscError.ErrorParsingInt32, 4);

            int result = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer.Array, buffer.Offset + Position));

            Position += 4;
            currentToken = typeTag.NextToken();

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadLong(ref OscTypeTag typeTag)
        {
            CheckToken(OscToken.Long);

            CheckForPacketEnd(OscError.ErrorParsingInt64, 8);

            long result = IPAddress.NetworkToHostOrder(BitConverter.ToInt64(buffer.Array, buffer.Offset + Position));

            Position += 8;
            currentToken = typeTag.NextToken();

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscMidiMessage ReadMidi(ref OscTypeTag typeTag)
        {
            CheckToken(OscToken.Midi);

            CheckForPacketEnd(OscError.ErrorParsingInt32, 4);

            OscMidiMessage result = new OscMidiMessage(unchecked((uint) IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer.Array, buffer.Offset + Position))));

            Position += 4;
            currentToken = typeTag.NextToken();

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object ReadNull(ref OscTypeTag typeTag)
        {
            CheckToken(OscToken.Null);

            currentToken = typeTag.NextToken();

            return OscNull.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadString(ref OscTypeTag typeTag)
        {
            if (currentToken == OscToken.Null)
            {
                return null;
            }

            CheckToken(OscToken.String);

            CheckForPacketEnd(OscError.ErrorParsingString, 4);

            return ReadStringInner(ref typeTag, OscError.ErrorParsingString);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscSymbol ReadSymbol(ref OscTypeTag typeTag)
        {
            CheckToken(OscToken.Symbol);

            CheckForPacketEnd(OscError.ErrorParsingSymbol, 4);

            return new OscSymbol(ReadStringInner(ref typeTag, OscError.ErrorParsingSymbol));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscTimeTag ReadTimeTag(ref OscTypeTag typeTag)
        {
            CheckToken(OscToken.TimeTag);

            CheckForPacketEnd(OscError.ErrorParsingOscTimeTag, 8);

            ulong result = unchecked((ulong) IPAddress.NetworkToHostOrder(BitConverter.ToInt64(buffer.Array, buffer.Offset + Position)));

            Position += 8;
            currentToken = typeTag.NextToken();

            return new OscTimeTag(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscTypeTag ReadTypeTag()
        {
            CheckToken(OscToken.TypeTag);

            // check that the next char is a comma                
            if ((char) buffer.Array[buffer.Offset + Position++] != ',')
            {
                throw new OscException(OscError.MissingComma, "No comma found");
            }

            // mark the start of the type tag
            int start = Position;
            int count = 0;
            int inset = 0;
            bool failed = true;

            // scan forward and look for the end of the typetag string 
            while (Position < maxPosition)
            {
                char @char = (char) buffer.Array[buffer.Offset + Position++];

                if (@char == 0)
                {
                    failed = false;
                    break;
                }

                if (inset == 0)
                {
                    count++;
                }

                if (@char == '[')
                {
                    inset++;
                }
                else if (@char == ']')
                {
                    inset--;
                }

                if (inset < 0)
                {
                    throw new OscException(OscError.MalformedTypeTag, "Malformed type tag");
                }
            }

            if (failed)
            {
                // this shouldn't happen and means we're decoding rubbish
                throw new OscException(OscError.MissingTypeTag, "Type tag terminator could not be found");
            }

            // read the string 
            string result = Encoding.UTF8.GetString(buffer.Array, buffer.Offset + start, Position - start - 1);

            // Advance to the arguments
            if (SkipPadding() == false)
            {
                throw new OscException(OscError.InvalidSegmentLength, "Unexpected end of message");
            }

            OscTypeTag typeTag = new OscTypeTag(result);

            currentToken = typeTag.CurrentToken;

            return typeTag;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int StartArray(ref OscTypeTag typeTag, out OscToken arrayType)
        {
            CheckToken(OscToken.ArrayStart);

            int length = typeTag.GetArrayElementCount(out arrayType);

            currentToken = typeTag.NextToken();

            return length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CalculatePadding(int position)
        {
            int nullCount = 4 - position % 4;

            return nullCount < 4 ? nullCount : 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckForPacketEnd(OscError error, int count)
        {
            if (Position + count > maxPosition)
            {
                throw new OscException(error, "Unexpected end of message");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[Conditional("MOOP")] 
        private void CheckToken(OscToken expected)
        {
            if (currentToken == expected)
            {
                return;
            }

            if (expected == OscToken.Bool)
            {
                if (currentToken == OscToken.True || currentToken == OscToken.False)
                {
                    return;
                }
            }

            if (expected == OscToken.BundleMessageLength && currentToken == OscToken.End)
            {
                return;
            }

            throw new OscException(OscError.UnexpectedToken, $"Unexpected token {currentToken} expected {expected}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ReadIntDirect()
        {
            int value = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer.Array, buffer.Offset + Position));

            Position += 4;

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string ReadStringInner(ref OscTypeTag typeTag, OscError error)
        {
            int stringStart = Position;
            bool failed = true;

            // scan forward and look for the end of the string 
            while (Position < maxPosition)
            {
                if (buffer.Array[buffer.Offset + Position++] != 0)
                {
                    continue;
                }

                failed = false;


                break;
            }

            if (failed)
            {
                throw new OscException(error, $@"Terminator could not be found while parsing argument '{typeTag.Index}'");
            }

            string result = Encoding.UTF8.GetString(buffer.Array, buffer.Offset + stringStart, Position - stringStart - 1);

            // Advance pass the padding
            if (SkipPadding() == false)
            {
                throw new OscException(error, $"Unexpected end of message while parsing argument '{typeTag.Index}'");
            }

            currentToken = typeTag.NextToken();

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool SkipPadding()
        {
            if (Position % 4 == 0)
            {
                return true;
            }

            int newPosition = Position + (4 - Position % 4);

            // this shouldn't happen and means we're decoding rubbish
            if (newPosition > maxPosition)
            {
                return false;
            }

            Position = newPosition;

            return true;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct BitFlipper32
        {
            [FieldOffset(0)] public int ValueInt32;

            [FieldOffset(0)] public readonly float ValueFloat;
        }
    }
}