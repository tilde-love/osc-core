// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using OscCore.Address;

namespace OscCore.LowLevel
{
    public class OscStringWriter
    {
        private readonly StringBuilder builder = new StringBuilder();
        private readonly IFormatProvider provider;

        public OscStringWriter(IFormatProvider provider = null)
        {
            this.provider = provider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return builder.ToString();
        }

        public void Write(OscAddress address)
        {
            builder.Append(address);
        }

        public void Write(string value)
        {
            if (value == null)
            {
                WriteNull();

                return;
            }

            builder.Append($@"""{OscSerializationUtils.Escape(value)}""");
        }

        public void Write(float value)
        {
            if (float.IsInfinity(value) || float.IsNaN(value))
            {
                builder.Append(value.ToString(provider));
            }
            else
            {
                builder.Append($"{value.ToString(provider)}f");
            }
        }

        public void Write(double value)
        {
            builder.Append($"{value.ToString(provider)}d");
        }

        public void Write(byte value)
        {
            builder.Append($@"'{OscSerializationUtils.Escape(new string((char) value, 1))}'");
        }

        public void Write(int value)
        {
            builder.Append(value.ToString(provider));
        }

        public void Write(long value)
        {
            builder.Append($"{value.ToString(provider)}L");
        }

        public void Write(bool value)
        {
            builder.Append($"{value.ToString()}");
        }

        public void Write(byte[] value)
        {
            builder.Append($"{{ Blob: {OscSerializationUtils.ToStringBlob(value)} }}");
        }

        public void Write(ref OscSymbol value)
        {
            if (value.Value == null)
            {
                WriteNull();

                return;
            }

            builder.Append($@"$""{OscSerializationUtils.Escape(value.Value)}""");
        }

        public void Write(OscSymbol value)
        {
            Write(ref value);
        }

        public void Write(ref OscTimeTag value)
        {
            builder.Append($"{{ Time: {value} }}");
        }

        public void Write(OscTimeTag value)
        {
            Write(ref value);
        }

        public void Write(ref OscMidiMessage value)
        {
            builder.Append($"{{ Midi: {value} }}");
        }

        public void Write(OscMidiMessage value)
        {
            Write(ref value);
        }

        public void Write(ref OscColor value)
        {
            builder.Append($"{{ Color: {value} }}");
        }

        public void Write(OscColor value)
        {
            Write(ref value);
        }

        public void Write(object @object)
        {
            switch (@object)
            {
                case object[] value:
                    WriteToken(OscSerializationToken.ArrayStart);
                    Write(value);
                    WriteToken(OscSerializationToken.ArrayEnd);
                    break;
                case int value:
                    Write(value);
                    break;
                case long value:
                    Write(value);
                    break;
                case float value:
                    Write(value);
                    break;
                case double value:
                    Write(value);
                    break;
                case byte value:
                    Write(value);
                    break;
                case OscColor value:
                    Write(value);
                    break;
                case OscTimeTag value:
                    Write(value);
                    break;
                case OscMidiMessage value:
                    Write(value);
                    break;
                case bool value:
                    Write(value);
                    break;
                case OscNull value:
                    WriteNull();
                    break;
                case OscImpulse value:
                    WriteImpulse();
                    break;
                case string value:
                    Write(value);
                    break;
                case OscSymbol value:
                    Write(value);
                    break;
                case byte[] value:
                    Write(value);
                    break;
                default:
                    throw new Exception($"Unsupported arguemnt type '{@object.GetType()}'");
            }
        }

        public void Write(IEnumerable<object> args)
        {
            bool first = true;

            foreach (object @object in args)
            {
                if (first == false)
                {
                    WriteToken(OscSerializationToken.Separator);
                }
                else
                {
                    first = false;
                }

                Write(@object);
            }
        }

        public void Write<T>(IEnumerable<T> array)
        {
            bool first = true;

            switch (array)
            {
                case IEnumerable<string> value:
                    foreach (string item in value)
                    {
                        if (first == false)
                        {
                            WriteToken(OscSerializationToken.Separator);
                        }

                        Write(item);

                        first = false;
                    }

                    break;
                case IEnumerable<float> value:
                    foreach (float item in value)
                    {
                        if (first == false)
                        {
                            WriteToken(OscSerializationToken.Separator);
                        }

                        Write(item);

                        first = false;
                    }

                    break;
                case IEnumerable<double> value:
                    foreach (double item in value)
                    {
                        if (first == false)
                        {
                            WriteToken(OscSerializationToken.Separator);
                        }

                        Write(item);

                        first = false;
                    }

                    break;
                case IEnumerable<int> value:
                    foreach (int item in value)
                    {
                        if (first == false)
                        {
                            WriteToken(OscSerializationToken.Separator);
                        }

                        Write(item);

                        first = false;
                    }

                    break;
                case IEnumerable<long> value:
                    foreach (long item in value)
                    {
                        if (first == false)
                        {
                            WriteToken(OscSerializationToken.Separator);
                        }

                        Write(item);

                        first = false;
                    }

                    break;
                case IEnumerable<bool> value:
                    foreach (bool item in value)
                    {
                        if (first == false)
                        {
                            WriteToken(OscSerializationToken.Separator);
                        }

                        Write(item);

                        first = false;
                    }

                    break;
                case IEnumerable<byte[]> value:
                    Write(value);
                    break;
                case IEnumerable<OscSymbol> value:
                    foreach (OscSymbol item in value)
                    {
                        if (first == false)
                        {
                            WriteToken(OscSerializationToken.Separator);
                        }

                        Write(item);

                        first = false;
                    }

                    break;
                case IEnumerable<OscTimeTag> value:
                    foreach (OscTimeTag item in value)
                    {
                        if (first == false)
                        {
                            WriteToken(OscSerializationToken.Separator);
                        }

                        Write(item);

                        first = false;
                    }

                    break;
                case IEnumerable<OscMidiMessage> value:
                    foreach (OscMidiMessage item in value)
                    {
                        if (first == false)
                        {
                            WriteToken(OscSerializationToken.Separator);
                        }

                        Write(item);

                        first = false;
                    }

                    break;
                case IEnumerable<OscColor> value:
                    foreach (OscColor item in value)
                    {
                        if (first == false)
                        {
                            WriteToken(OscSerializationToken.Separator);
                        }

                        Write(item);

                        first = false;
                    }

                    break;
                case IEnumerable<object> value:
                    foreach (object item in value)
                    {
                        if (first == false)
                        {
                            WriteToken(OscSerializationToken.Separator);
                        }

                        Write(item);

                        first = false;
                    }

                    break;
                default:
                    throw new Exception();
            }
        }

        public void WriteAddress(string address)
        {
            builder.Append(address);
        }

        public void WriteBundleIdent(OscTimeTag timeTag)
        {
            builder.Append($"#bundle, {timeTag}");
        }

        public void WriteImpulse()
        {
            builder.Append(OscImpulse.Value);
        }

        public void WriteNull()
        {
            builder.Append(OscNull.Value);
        }

        public void WriteToken(OscSerializationToken token)
        {
            switch (token)
            {
                case OscSerializationToken.Separator:
                    builder.Append(", ");
                    break;
                case OscSerializationToken.ArrayStart:
                    builder.Append("[ ");
                    break;
                case OscSerializationToken.ArrayEnd:
                    builder.Append(" ]");
                    break;
                case OscSerializationToken.ObjectStart:
                    builder.Append("{ ");
                    break;
                case OscSerializationToken.ObjectEnd:
                    builder.Append(" }");
                    break;
                case OscSerializationToken.None:
                case OscSerializationToken.Literal:
                case OscSerializationToken.String:
                case OscSerializationToken.Symbol:
                case OscSerializationToken.Char:
                case OscSerializationToken.End:
                default:
                    throw new ArgumentOutOfRangeException(nameof(token), token, null);
            }
        }
    }
}