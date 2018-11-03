// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using OscCore.Address;

namespace OscCore.LowLevel
{
    public struct OscStringReader
    {
        private readonly string original;
        private int position;
        private readonly int maxPosition;

        public OscStringReader(string value)
        {
            original = value;
            position = 0;
            maxPosition = value.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadAddress(bool validate)
        {
            OscSerializationToken token = ReadNextToken(out string value);

            if (token != OscSerializationToken.Literal)
            {
                throw new OscException(OscError.ErrorParsingOscAdress, $"Unexpected serialization token {token}");
            }

            string address = value.Trim();

            if (validate != true)
            {
                return address;
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new Exception("Address was empty");
            }

            if (OscAddress.IsValidAddressPattern(address) == false)
            {
                throw new Exception("Invalid address");
            }

            return address;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadSeparator()
        {
            OscSerializationToken token = ReadNextToken(out string _);

            if (token != OscSerializationToken.Separator)
            {
                throw new OscException(OscError.ErrorParsingOscAdress, $"Unexpected serialization token {token}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscSerializationToken ReadSeparatorOrEnd()
        {
            OscSerializationToken token = ReadNextToken(out string _);

            if (token != OscSerializationToken.Separator &&
                token != OscSerializationToken.ArrayEnd &&
                token != OscSerializationToken.ObjectEnd &&
                token != OscSerializationToken.End)
            {
                throw new OscException(OscError.ErrorParsingOscAdress, $"Unexpected serialization token {token}");
            }

            return token;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object[] ReadArguments(IFormatProvider provider, OscSerializationToken endToken)
        {
            List<object> arguments = new List<object>();

            OscSerializationToken token;

            do
            {
                token = ReadNextToken(out string value);

                if (token == endToken)
                {
                    break;
                }

                switch (token)
                {
                    case OscSerializationToken.Literal:
                        arguments.Add(ParseLiteral(value, provider));
                        break;
                    case OscSerializationToken.String:
                        arguments.Add(ParseString(value, provider));
                        break;
                    case OscSerializationToken.Symbol:
                        arguments.Add(ParseSymbol(value, provider));
                        break;
                    case OscSerializationToken.Char:
                        arguments.Add(ParseChar(value, provider));
                        break;
                    case OscSerializationToken.Separator:
                        break;
                    case OscSerializationToken.ArrayStart:
                        arguments.Add(ReadArray(provider));
                        break;
                    case OscSerializationToken.ObjectStart:
                        arguments.Add(ParseObject(value, provider));
                        break;
                    case OscSerializationToken.End:
                        break;
                    case OscSerializationToken.None:
                    case OscSerializationToken.ObjectEnd:
                    case OscSerializationToken.ArrayEnd:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            while (token != endToken && token != OscSerializationToken.End);

            if (token != endToken)
            {
                throw new OscException(OscError.UnexpectedToken, $"Unexpected token {token}");
            }

            return arguments.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private object[] ReadArray(IFormatProvider provider)
        {
            OscSerializationToken endToken = OscSerializationToken.ArrayEnd;

            List<object> arguments = new List<object>();

            OscSerializationToken token;

            do
            {
                token = ReadNextToken(out string value);

                switch (token)
                {
                    case OscSerializationToken.Literal:
                        arguments.Add(ParseLiteral(value, provider));
                        break;
                    case OscSerializationToken.String:
                        arguments.Add(ParseString(value, provider));
                        break;
                    case OscSerializationToken.Symbol:
                        arguments.Add(ParseSymbol(value, provider));
                        break;
                    case OscSerializationToken.Char:
                        arguments.Add(ParseChar(value, provider));
                        break;
                    case OscSerializationToken.Separator:
                        break;
                    case OscSerializationToken.ArrayStart:
                        arguments.Add(ReadArray(provider));
                        break;
                    case OscSerializationToken.ArrayEnd:
                        break;
                    case OscSerializationToken.ObjectStart:
                        arguments.Add(ParseObject(value, provider));
                        break;
                    case OscSerializationToken.None:
                    case OscSerializationToken.End:
                    case OscSerializationToken.ObjectEnd:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            while (token != endToken && token != OscSerializationToken.End);

            if (token != endToken)
            {
                throw new OscException(OscError.UnexpectedToken, $"Unexpected token {token}");
            }

            return arguments.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private object ParseSymbol(string value, IFormatProvider provider)
        {
            return new OscSymbol(OscSerializationUtils.Unescape(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private object ParseObject(string value, IFormatProvider provider)
        {
            string name = ReadObjectNameToken();

            if (name.Length == 0)
            {
                throw new Exception(@"Malformed object missing type name");
            }

            string nameLower = name.ToLowerInvariant();

            switch (nameLower)
            {
                case "midi":
                case "m":
                    return OscMidiMessage.Parse(ref this, provider);
                case "time":
                case "t":
                    return OscTimeTag.Parse(ref this, provider);
                case "color":
                case "c":
                    return OscColor.Parse(ref this, provider);
                case "blob":
                case "b":
                case "data":
                case "d":
                    return ParseBlob(provider);
                default:
                    throw new Exception($@"Unknown object type '{name}'");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte[] ParseBlob(IFormatProvider provider)
        {
            OscSerializationToken token = ReadNextToken(out string value);

            if (token == OscSerializationToken.ObjectEnd)
            {
                return new byte[0];
            }

            if (token == OscSerializationToken.Literal)
            {
                string trimmed = value.Trim();

                if (trimmed.StartsWith("64x"))
                {
                    byte[] bytes = Convert.FromBase64String(trimmed.Substring(3));

                    if (ReadNextToken(out string _) != OscSerializationToken.ObjectEnd)
                    {
                        throw new Exception("Expected end of object");
                    }

                    return bytes;
                }

                if (trimmed.StartsWith("0x"))
                {
                    trimmed = trimmed.Substring(2);

                    if (trimmed.Length % 2 != 0)
                    {
                        // this is an error
                        throw new Exception("Invalid blob string length");
                    }

                    int length = trimmed.Length / 2;

                    byte[] bytes = new byte[length];

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        bytes[i] = byte.Parse(trimmed.Substring(i * 2, 2), NumberStyles.HexNumber, provider);
                    }

                    if (ReadNextToken(out string _) != OscSerializationToken.ObjectEnd)
                    {
                        throw new Exception("Expected end of object");
                    }

                    return bytes;
                }
            }

            using (MemoryStream stream = new MemoryStream())
            {
                while (token != OscSerializationToken.ObjectEnd)
                {
                    stream.WriteByte(byte.Parse(value.Trim(), NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, provider));

                    token = ReadNextToken(out value);

                    if (token == OscSerializationToken.Separator)
                    {
                        token = ReadNextToken(out value);
                    }
                }

                return stream.ToArray();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private object ParseChar(string value, IFormatProvider provider)
        {
            string unescapeString = OscSerializationUtils.Unescape(value);

            if (unescapeString.Length > 1)
            {
                throw new Exception();
            }

            char c = unescapeString.Trim()[0];

            return (byte) c;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private object ParseString(string value, IFormatProvider provider)
        {
            return OscSerializationUtils.Unescape(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private object ParseLiteral(string value, IFormatProvider provider)
        {
            long valueInt64;
            float valueFloat;
            double valueDouble;

            string argString = value.Trim();

            if (argString.Length == 0)
            {
                throw new Exception("Argument is empty");
            }

            // try to parse a hex value
            if (argString.Length > 2 && argString.StartsWith("0x"))
            {
                string hexString = argString.Substring(2);

                // parse a int32
                if (hexString.Length <= 8)
                {
                    if (uint.TryParse(hexString, NumberStyles.HexNumber, provider, out uint valueUInt32))
                    {
                        return unchecked((int) valueUInt32);
                    }
                }
                // parse a int64
                else
                {
                    if (ulong.TryParse(hexString, NumberStyles.HexNumber, provider, out ulong valueUInt64))
                    {
                        return unchecked((long) valueUInt64);
                    }
                }
            }

            // parse int64
            if (argString.EndsWith("L"))
            {
                if (long.TryParse(argString.Substring(0, argString.Length - 1), NumberStyles.Integer, provider, out valueInt64))
                {
                    return valueInt64;
                }
            }

            // parse int32
            if (int.TryParse(argString, NumberStyles.Integer, provider, out int valueInt32))
            {
                return valueInt32;
            }

            // parse int64
            if (long.TryParse(argString, NumberStyles.Integer, provider, out valueInt64))
            {
                return valueInt64;
            }

            // parse double
            if (argString.EndsWith("d"))
            {
                if (double.TryParse(argString.Substring(0, argString.Length - 1), NumberStyles.Float, provider, out valueDouble))
                {
                    return valueDouble;
                }
            }

            // parse float
            if (argString.EndsWith("f"))
            {
                if (float.TryParse(argString.Substring(0, argString.Length - 1), NumberStyles.Float, provider, out valueFloat))
                {
                    return valueFloat;
                }
            }

            if (argString.Equals(float.PositiveInfinity.ToString(provider)))
            {
                return float.PositiveInfinity;
            }

            if (argString.Equals(float.NegativeInfinity.ToString(provider)))
            {
                return float.NegativeInfinity;
            }

            if (argString.Equals(float.NaN.ToString(provider)))
            {
                return float.NaN;
            }

            // parse float 
            if (float.TryParse(argString, NumberStyles.Float, provider, out valueFloat))
            {
                return valueFloat;
            }

            // parse double
            if (double.TryParse(argString, NumberStyles.Float, provider, out valueDouble))
            {
                return valueDouble;
            }

            // parse bool
            if (bool.TryParse(argString, out bool valueBool))
            {
                return valueBool;
            }

            // parse null 
            if (OscNull.IsNull(argString))
            {
                return OscNull.Value;
            }

            // parse impulse/bang
            if (OscImpulse.IsImpulse(argString))
            {
                return OscImpulse.Value;
            }

            // if all else fails then its a symbol i guess (?!?) 
            return new OscSymbol(argString);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SkipWhiteSpace()
        {
            for (; position < maxPosition; position++)
            {
                char @char = original[position];

                switch (@char)
                {
                    case ' ':
                    case '\n':
                    case '\r':
                    case '\t':
                        continue;
                    default:
                        return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OscSerializationToken ReadNextToken(out string value)
        {
            SkipWhiteSpace();

            value = null;

            if (position >= maxPosition)
            {
                return OscSerializationToken.End;
            }

            char @char = original[position];

            switch (@char)
            {
                case '$':
                    position++;

                    if (original[position] == '"')
                    {
                        position++;
                        value = ReadStringToken();
                        return OscSerializationToken.Symbol;
                    }
                    else
                    {
                        position--;
                        value = ReadLiteralToken();
                        return OscSerializationToken.Literal;
                    }

                case '"':
                    position++;
                    value = ReadStringToken();
                    return OscSerializationToken.String;

                case '\'':
                    position++;
                    value = ReadCharToken();
                    return OscSerializationToken.Char;

                case ',':
                    position++;
                    return OscSerializationToken.Separator;

                case '[':
                    position++;
                    return OscSerializationToken.ArrayStart;

                case ']':
                    position++;
                    return OscSerializationToken.ArrayEnd;

                case '{':
                    position++;
                    return OscSerializationToken.ObjectStart;

                case '}':
                    position++;
                    return OscSerializationToken.ObjectEnd;

                default:
                    value = ReadLiteralToken();
                    return OscSerializationToken.Literal;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string ReadLiteralToken()
        {
            int start = position;

            int index = original.IndexOfAny(LiteralTokenControlChars, position);

            if (index < 0 || index > maxPosition)
            {
                index = maxPosition;
            }

            position = index;

            return original.Substring(start, position - start);
        }

        private static readonly char[] ObjectNameControlChars = {',', ':', ']', '}'};
        private static readonly char[] LiteralTokenControlChars = {',', ']', '}'};

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string ReadObjectNameToken()
        {
            int start = position;

            bool valid = true;

            int index = original.IndexOfAny(ObjectNameControlChars, position);

            if (index < 0 || index > maxPosition)
            {
                index = maxPosition;
            }
            else
            {
                valid = original[index] == ':';
            }

            string value = original.Substring(start, index - start)
                .Trim();

            if (valid == false)
            {
                throw new OscException(OscError.InvalidObjectName, $"Invalid object name {value}");
            }

            position = index + 1;

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string ReadCharToken()
        {
            bool escaped = false;

            int start = position;
            int end = start;

            for (; position < maxPosition; position++)
            {
                char @char = original[position];

                end = position;

                if (escaped)
                {
                    continue;
                }

                if (@char == '\\')
                {
                    escaped = true;

                    continue;
                }

                if (@char == '\'')
                {
                    position++;

                    break;
                }
            }

            return original.Substring(start, end - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string ReadStringToken()
        {
            bool escaped = false;

            int start = position;
            int end = start;

            for (; position < maxPosition; position++)
            {
                char @char = original[position];

                end = position;

                if (escaped)
                {
                    escaped = false;

                    continue;
                }

                if (@char == '\\')
                {
                    escaped = true;

                    continue;
                }

                if (@char == '"')
                {
                    position++;

                    break;
                }
            }

            return original.Substring(start, end - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char PeekChar()
        {
            int startPosition = position;

            SkipWhiteSpace();

            char value = original[position];

            position = startPosition;

            return value;
        }
    }
}