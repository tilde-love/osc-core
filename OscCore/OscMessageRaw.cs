// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OscCore.LowLevel;

namespace OscCore
{
    public struct OscArgument
    {
        public int Position;

        public bool IsArray;

        public OscToken Type;

        public OscArgument[] Array;
    }

    public class OscMessageRaw : IOscMessage, IEnumerable<OscArgument>
    {
        private readonly OscArgument[] arguments;

        private readonly OscReader reader;

        public OscArgument this[int index] => arguments[index];

        public OscMessageRaw(ArraySegment<byte> buffer, Uri origin = null, OscTimeTag? timestamp = null)
        {
            Origin = origin;
            Timestamp = timestamp;

            reader = new OscReader(buffer);

            reader.BeginMessage(buffer.Count);

            Address = reader.ReadAddress();

            if (reader.PeekToken() == OscToken.End)
            {
                arguments = new OscArgument[0];

                return;
            }

            OscTypeTag typeTag = reader.ReadTypeTag();

            arguments = new OscArgument[reader.GetArgumentCount(ref typeTag, out OscToken argumentsType)];

            int argumentsStart = reader.Position;
            int position = argumentsStart;

            OscToken token = OscToken.None;

            for (int i = 0; i < arguments.Length; i++)
            {
                token = typeTag.CurrentToken;

                if (token == OscToken.ArrayStart)
                {
                    arguments[i] = GetArrayArgument(ref typeTag, ref position);
                }
                else
                {
                    arguments[i] = new OscArgument
                    {
                        Position = position,
                        Type = token
                    };

                    switch (token)
                    {
                        case OscToken.Bool:
                        case OscToken.True:
                        case OscToken.False:
                        case OscToken.Impulse:
                        case OscToken.Null:
                            break;

                        case OscToken.Char:
                        case OscToken.Int:
                        case OscToken.Float:
                        case OscToken.Color:
                        case OscToken.Midi:
                            position += 4;
                            break;

                        case OscToken.Long:
                        case OscToken.Double:
                        case OscToken.TimeTag:
                            position += 8;
                            break;

                        case OscToken.String:
                        case OscToken.Symbol:
                            position += reader.GetStringArgumentSize(position);
                            break;

                        case OscToken.Blob:
                            position += reader.GetBlobArgumentSize(position);
                            break;

                        case OscToken.None:
                        case OscToken.OscAddress:
                        case OscToken.TypeTag:
                        case OscToken.ArrayStart:
                        case OscToken.ArrayEnd:
                        case OscToken.MixedTypes:
                        case OscToken.End:
                        case OscToken.BundleMessageLength:
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                token = typeTag.NextToken();
            }
        }

        public string Address { get; }

        public int Count => arguments.Length;

        public Uri Origin { get; }

        public OscTimeTag? Timestamp { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArraySegment<byte> ReadBlob(ref OscArgument argument)
        {
            CheckArgument(ref argument, OscToken.Blob);

            reader.Position = argument.Position;

            return reader.ReadDirectBlob();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBool(ref OscArgument argument)
        {
            CheckArgument(ref argument, OscToken.Bool);

            return argument.Type == OscToken.True;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadChar(ref OscArgument argument)
        {
            CheckArgument(ref argument, OscToken.Char);

            reader.Position = argument.Position;

            return reader.ReadDirectChar();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscColor ReadColor(ref OscArgument argument)
        {
            CheckArgument(ref argument, OscToken.Color);

            reader.Position = argument.Position;

            return reader.ReadDirectColor();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDouble(ref OscArgument argument)
        {
            CheckArgument(ref argument, OscToken.Double);

            reader.Position = argument.Position;

            return reader.ReadDirectDouble();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloat(ref OscArgument argument)
        {
            CheckArgument(ref argument, OscToken.Float);

            reader.Position = argument.Position;

            return reader.ReadDirectFloat();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscImpulse ReadImpulse(ref OscArgument argument)
        {
            CheckArgument(ref argument, OscToken.Impulse);

            return OscImpulse.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt(ref OscArgument argument)
        {
            CheckArgument(ref argument, OscToken.Int);

            reader.Position = argument.Position;

            return reader.ReadDirectInt();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadLong(ref OscArgument argument)
        {
            CheckArgument(ref argument, OscToken.Long);

            reader.Position = argument.Position;

            return reader.ReadDirectLong();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscMidiMessage ReadMidi(ref OscArgument argument)
        {
            CheckArgument(ref argument, OscToken.Midi);

            reader.Position = argument.Position;

            return reader.ReadDirectMidi();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadString(ref OscArgument argument)
        {
            if (argument.IsArray == false &&
                argument.Type == OscToken.Null)
            {
                return null;
            }

            CheckArgument(ref argument, OscToken.String);

            reader.Position = argument.Position;

            return reader.ReadDirectString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscSymbol ReadSymbol(ref OscArgument argument)
        {
            if (argument.IsArray == false &&
                argument.Type == OscToken.Null)
            {
                return new OscSymbol(null);
            }

            CheckArgument(ref argument, OscToken.Symbol);

            reader.Position = argument.Position;

            return reader.ReadDirectSymbol();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscTimeTag ReadTimeTag(ref OscArgument argument)
        {
            CheckArgument(ref argument, OscToken.TimeTag);

            reader.Position = argument.Position;

            return reader.ReadDirectTimeTag();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckArgument(ref OscArgument argument, OscToken expectedType)
        {
            if (argument.IsArray)
            {
                throw new OscException(OscError.UnexpectedToken, "Unexpected array");
            }

            if (expectedType == OscToken.Bool)
            {
                if (argument.Type != OscToken.True &&
                    argument.Type != OscToken.False)
                {
                    throw new OscException(OscError.UnexpectedToken, $"Unexpected token {argument.Type}");
                }

                return;
            }

            if (argument.Type != expectedType)
            {
                throw new OscException(OscError.UnexpectedToken, $"Unexpected token {argument.Type}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private OscArgument GetArrayArgument(ref OscTypeTag typeTag, ref int position)
        {
            int arrayLength = reader.StartArray(ref typeTag, out OscToken arrayType);

            OscArgument[] arguments = new OscArgument[arrayLength];

            OscToken token = OscToken.None;

            for (int i = 0; i < arguments.Length; i++)
            {
                token = typeTag.CurrentToken;

                if (token == OscToken.ArrayStart)
                {
                    arguments[i] = GetArrayArgument(ref typeTag, ref position);
                }
                else
                {
                    arguments[i] = new OscArgument
                    {
                        Position = position,
                        Type = token
                    };

                    switch (token)
                    {
                        case OscToken.Bool:
                        case OscToken.True:
                        case OscToken.False:
                        case OscToken.Impulse:
                        case OscToken.Null:
                            break;

                        case OscToken.Char:
                        case OscToken.Int:
                        case OscToken.Float:
                        case OscToken.Color:
                        case OscToken.Midi:
                            position += 4;
                            break;

                        case OscToken.Long:
                        case OscToken.Double:
                        case OscToken.TimeTag:
                            position += 8;
                            break;

                        case OscToken.String:
                        case OscToken.Symbol:
                            position += reader.GetStringArgumentSize(position);
                            break;

                        case OscToken.Blob:
                            position += reader.GetBlobArgumentSize(position);
                            break;

                        case OscToken.None:
                        case OscToken.OscAddress:
                        case OscToken.TypeTag:
                        case OscToken.ArrayStart:
                        case OscToken.ArrayEnd:
                        case OscToken.MixedTypes:
                        case OscToken.End:
                        case OscToken.BundleMessageLength:
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                token = typeTag.NextToken();
            }

            return new OscArgument
            {
                Position = position,
                Type = arrayType,
                IsArray = true,
                Array = arguments
            };
        }

        /// <inheritdoc />
        public IEnumerator<OscArgument> GetEnumerator()
        {
            return (arguments as IEnumerable<OscArgument>).GetEnumerator(); 
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}