// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace OscCore.LowLevel
{
    public class OscWriter
    {
        private static readonly bool IsLittleEndian;
        private readonly byte[] argumentBuffer;
        private int argumentBufferCount;

        private readonly MemoryStream buffer;
        private int count;
        private BitFlipper32 flipper32;

        private WriterState state;

        static OscWriter()
        {
            IsLittleEndian = BitConverter.IsLittleEndian;
        }

        public OscWriter(MemoryStream buffer)
        {
            flipper32 = new BitFlipper32();

            this.buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            argumentBuffer = new byte[16];
            argumentBufferCount = 0;
            count = 0;
            state = WriterState.NotStarted;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartBundle(string ident, ref OscTimeTag timestamp)
        {
            StartMessage();

            // write the address
            WriteDirect(Encoding.UTF8.GetBytes(ident));

            // write null terminator
            Write((byte) 0);

            WritePadding();

            Write(unchecked((long) timestamp.Value));
            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartMessage()
        {
            state = WriterState.Address;
            count = 0;
            argumentBufferCount = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteAddress(string address)
        {
            CheckWriterState(WriterState.Address);

            // write the address
            WriteDirect(Encoding.UTF8.GetBytes(address));

            // write null terminator
            Write((byte) 0);

            WritePadding();

            // write the comma for the type-tag
            Write((byte) ',');

            Flush();

            state = WriterState.TypeTag;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBlob(byte[] buffer)
        {
            CheckWriterState(WriterState.Arguments);

            // write length 
            Write(buffer.Length);
            Flush();

            // write bytes 
            WriteDirect(buffer);

            WritePadding();
            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBlob(ArraySegment<byte> buffer)
        {
            CheckWriterState(WriterState.Arguments);

            // write length 
            Write(buffer.Count);
            Flush();

            // write bytes 
            WriteDirect(buffer.Array, buffer.Offset, buffer.Count);

            WritePadding();
            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBundleMessageLength(int messageSizeInBytes)
        {
            Write(messageSizeInBytes);
            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteChar(byte value)
        {
            CheckWriterState(WriterState.Arguments);

            Write(value);
            Write((byte) 0);
            Write((byte) 0);
            Write((byte) 0);

            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteColor(ref OscColor value)
        {
            CheckWriterState(WriterState.Arguments);

            int intValue = (value.R << 24) |
                           (value.G << 16) |
                           (value.B << 8) |
                           (value.A << 0);

            Write(intValue);

            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteDouble(double value)
        {
            CheckWriterState(WriterState.Arguments);

            Write(BitConverter.DoubleToInt64Bits(value));

            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFloat(float value)
        {
            CheckWriterState(WriterState.Arguments);

            flipper32.ValueFloat = value;

            Write(flipper32.ValueInt32);

            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteInt(int value)
        {
            CheckWriterState(WriterState.Arguments);

            Write(value);

            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteLong(long value)
        {
            CheckWriterState(WriterState.Arguments);

            Write(value);

            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteMidi(ref OscMidiMessage value)
        {
            CheckWriterState(WriterState.Arguments);

            Write(unchecked((int) value.FullMessage));

            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteString(string value)
        {
            CheckWriterState(WriterState.Arguments);

            // write the address
            WriteDirect(Encoding.UTF8.GetBytes(value));
            // write null terminator
            Write((byte) 0);

            WritePadding();
            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteSymbol(ref OscSymbol value)
        {
            CheckWriterState(WriterState.Arguments);

            // write the address
            WriteDirect(Encoding.UTF8.GetBytes(value.Value));
            // write null terminator
            Write((byte) 0);

            WritePadding();
            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteTimeTag(ref OscTimeTag value)
        {
            CheckWriterState(WriterState.Arguments);

            Write(unchecked((long) value.Value));

            Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteTypeTag(OscToken token)
        {
            CheckWriterState(WriterState.TypeTag);

            FlushConditionally();

            switch (token)
            {
                case OscToken.Char:
                    Write((byte) 'c');
                    break;
                case OscToken.True:
                    Write((byte) 'T');
                    break;
                case OscToken.False:
                    Write((byte) 'F');
                    break;
                case OscToken.String:
                    Write((byte) 's');
                    break;
                case OscToken.Symbol:
                    Write((byte) 'S');
                    break;
                case OscToken.Impulse:
                    Write((byte) 'I');
                    break;
                case OscToken.Null:
                    Write((byte) 'N');
                    break;
                case OscToken.Int:
                    Write((byte) 'i');
                    break;
                case OscToken.Long:
                    Write((byte) 'h');
                    break;
                case OscToken.Float:
                    Write((byte) 'f');
                    break;
                case OscToken.Double:
                    Write((byte) 'd');
                    break;
                case OscToken.TimeTag:
                    Write((byte) 't');
                    break;
                case OscToken.Blob:
                    Write((byte) 'b');
                    break;
                case OscToken.Color:
                    Write((byte) 'r');
                    break;
                case OscToken.Midi:
                    Write((byte) 'm');
                    break;
                case OscToken.ArrayStart:
                    Write((byte) '[');
                    break;
                case OscToken.ArrayEnd:
                    Write((byte) ']');
                    break;
                default:
                    throw new OscException(OscError.UnexpectedToken, $"Unexpected token {token}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteTypeTagEnd()
        {
            CheckWriterState(WriterState.TypeTag);

            // write null terminator
            Write((byte) 0);

            WritePadding();
            Flush();

            state = WriterState.Arguments;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CalculatePadding()
        {
            int nullCount = 4 - count % 4;

            return nullCount < 4 ? nullCount : 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckWriterState(WriterState requiredState)
        {
            if (state != requiredState)
            {
                throw new OscException(OscError.UnexpectedWriterState, $"Unexpected writer state {state}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Flush()
        {
            if (argumentBufferCount == 0)
            {
                return;
            }

            buffer.Write(argumentBuffer, 0, argumentBufferCount);
            argumentBufferCount = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FlushConditionally()
        {
            if (argumentBufferCount + 4 > argumentBuffer.Length)
            {
                Flush();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Write(byte value)
        {
            argumentBuffer[argumentBufferCount++] = value;
            count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Write(int value)
        {
            if (IsLittleEndian)
            {
                uint uValue = unchecked((uint) value);

                for (int i = 0; i < 4; i++)
                {
                    argumentBuffer[argumentBufferCount++] = unchecked((byte) (((uValue & 0xff000000) >> 24) & 0xff));
                    uValue = uValue << 8;
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    argumentBuffer[argumentBufferCount++] = unchecked((byte) (value & 0xff));
                    value = value >> 8;
                    count++;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Write(long value)
        {
            if (IsLittleEndian)
            {
                ulong uValue = unchecked((ulong) value);

                for (int i = 0; i < 8; i++)
                {
                    argumentBuffer[argumentBufferCount++] = unchecked((byte) (((uValue & 0xff00000000000000) >> 56) & 0xff));
                    uValue = uValue << 8;
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    argumentBuffer[argumentBufferCount++] = unchecked((byte) (value & 0xff));
                    value = value >> 8;
                    count++;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteDirect(byte[] bytes)
        {
            buffer.Write(bytes, 0, bytes.Length);
            count += bytes.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteDirect(byte[] bytes, int index, int count)
        {
            buffer.Write(bytes, index, count);
            this.count += count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WritePadding()
        {
            int nullCount = CalculatePadding();

            for (int i = 0; i < nullCount; i++)
            {
                argumentBuffer[argumentBufferCount++] = 0;
                count++;
            }
        }

        private enum WriterState
        {
            NotStarted,
            Address,
            TypeTag,
            Arguments
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct BitFlipper32
        {
            [FieldOffset(0)] public readonly int ValueInt32;

            [FieldOffset(0)] public float ValueFloat;
        }
    }
}