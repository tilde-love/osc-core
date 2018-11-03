// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Runtime.CompilerServices;

namespace OscCore.LowLevel
{
    public struct OscTypeTag
    {
        private readonly string typeTag;

        public OscTypeTag(string typeTag)
        {
            this.typeTag = typeTag;
            Index = 0;
        }

        public OscToken CurrentToken => GetTokenFromTypeTag(Index);

        public int Index { get; private set; }

        public OscToken NextToken()
        {
            return GetTokenFromTypeTag(++Index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetArgumentCount(out OscToken arrayType)
        {
            return GetArrayLength(0, out arrayType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetArrayElementCount(out OscToken arrayType)
        {
            return GetArrayLength(Index + 1, out arrayType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private OscToken GetTokenFromTypeTag(int index)
        {
            if (index == typeTag.Length)
            {
                return OscToken.End;
            }

            if (index < 0 || index > typeTag.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, "Index is not a valid part of the type tag");
            }

            char type = typeTag[index];

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (type)
            {
                case 'b':
                    return OscToken.Blob;
                case 's':
                    return OscToken.String;
                case 'S':
                    return OscToken.Symbol;
                case 'i':
                    return OscToken.Int;
                case 'h':
                    return OscToken.Long;
                case 'f':
                    return OscToken.Float;
                case 'd':
                    return OscToken.Double;
                case 't':
                    return OscToken.TimeTag;
                case 'c':
                    return OscToken.Char;
                case 'r':
                    return OscToken.Color;
                case 'm':
                    return OscToken.Midi;
                case 'T':
                    return OscToken.True;
                case 'F':
                    return OscToken.False;
                case 'N':
                    return OscToken.Null;
                case 'I':
                    return OscToken.Impulse;
                case '[':
                    return OscToken.ArrayStart;
                case ']':
                    return OscToken.ArrayEnd;
                default:
                    // Unknown argument type
                    throw new OscException(OscError.UnknownArguemntType, $@"Unknown OSC type '{type}' on argument '{index}'");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetArrayLength(int index, out OscToken arrayType)
        {
            arrayType = OscToken.None;

            if (index == typeTag.Length)
            {
                return 0;
            }

            if (index < 0 || index > typeTag.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, "Index is not a valid part of the type tag");
            }

            int count = 0;
            int inset = 0;

            while (true)
            {
                OscToken token = GetTokenFromTypeTag(index++);

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (token)
                {
                    case OscToken.None:
                    case OscToken.OscAddress:
                    case OscToken.TypeTag:
                        throw new OscException(OscError.UnexpectedToken, $"Unexpected token {token}");
                    case OscToken.True:
                    case OscToken.False:
                        if (arrayType == OscToken.None)
                        {
                            arrayType = OscToken.Bool;
                        }
                        else if (arrayType != OscToken.Bool)
                        {
                            arrayType = OscToken.MixedTypes;
                        }

                        if (inset == 0)
                        {
                            count++;
                        }

                        break;
                    case OscToken.Null:
                        if (arrayType != OscToken.String &&
                            arrayType != OscToken.Blob)
                        {
                            arrayType = OscToken.MixedTypes;
                        }

                        if (inset == 0)
                        {
                            count++;
                        }

                        break;
                    case OscToken.String:
                    case OscToken.Blob:
                    case OscToken.Char:
                    case OscToken.Symbol:
                    case OscToken.Impulse:
                    case OscToken.Int:
                    case OscToken.Long:
                    case OscToken.Float:
                    case OscToken.Double:
                    case OscToken.TimeTag:
                    case OscToken.Color:
                    case OscToken.Midi:
                        if (arrayType == OscToken.None)
                        {
                            arrayType = token;
                        }
                        else if (arrayType != token)
                        {
                            arrayType = OscToken.MixedTypes;
                        }

                        if (inset == 0)
                        {
                            count++;
                        }

                        break;
                    case OscToken.ArrayStart:
                        if (inset == 0)
                        {
                            count++;
                        }

                        inset++;
                        break;
                    case OscToken.ArrayEnd:
                        inset--;

                        if (inset == -1)
                        {
                            return count;
                        }

                        break;
                    case OscToken.End:
                        return count;
                    case OscToken.MixedTypes:
                    default:
                        throw new OscException(OscError.UnknownArguemntType, $@"Unknown OSC type '{token}' on argument '{index}'");
                }
            }
        }
    }
}