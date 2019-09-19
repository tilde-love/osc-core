// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using OscCore.Address;
using OscCore.LowLevel;

namespace OscCore
{
    /// <summary>
    ///     Any osc message
    /// </summary>
    public sealed class OscMessage : OscPacket, IEnumerable<object>, IOscMessage
    {
        private object[] arguments;

        /// <summary>
        ///     Access message arguments by index
        /// </summary>
        /// <param name="index">the index of the message</param>
        /// <returns>message at the supplied index</returns>
        public object this[int index] => arguments[index];

        /// <summary>
        ///     IS the argument list empty
        /// </summary>
        public bool IsEmpty => arguments.Length == 0;

        /// <summary>
        ///     The size of the message in bytes
        /// </summary>
        public override int SizeInBytes
        {
            get
            {
                int size = 0;

                // should never happen 
                if (Address == string.Empty)
                {
                    return size;
                }

                // address + terminator 
                size += Address.Length + 1;

                // padding 
                int nullCount = 4 - size % 4;

                if (nullCount < 4)
                {
                    size += nullCount;
                }

                if (arguments.Length == 0)
                {
                    // return the size plus the comma and padding
                    return size + 4;
                }

                // comma 
                size++;

                size += OscUtils.SizeOfObjectArray_TypeTag(arguments);

                // terminator
                size++;

                // padding
                nullCount = 4 - size % 4;

                if (nullCount < 4)
                {
                    size += nullCount;
                }

                size += OscUtils.SizeOfObjectArray(arguments);

                return size;
            }
        }

        /// <summary>
        ///     Construct a osc message
        /// </summary>
        /// <param name="address">An osc address that is the destination for this message</param>
        /// <param name="args">
        ///     Object array of OSC argument values. The type tag string will be created automatically according to
        ///     each argument type
        /// </param>
        /// <example>OscMessage message = new OscMessage("/test/test", 1, 2, 3);</example>
        public OscMessage(string address, params object[] args)
        {
            Origin = null;

            Address = address;
            arguments = args;

            if (string.IsNullOrWhiteSpace(Address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (OscAddress.IsValidAddressPattern(address) == false)
            {
                throw new ArgumentException($"The address '{address}' is not a valid osc address", nameof(address));
            }

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            CheckArguments(arguments);
        }

        /// <summary>
        ///     Construct a OSC message
        /// </summary>
        /// <param name="origin">the origin of the OSC message</param>
        /// <param name="address">An OSC address that is the destination for this message</param>
        /// <param name="args">
        ///     Object array of OSC argument values. The type tag string will be created automatically according to
        ///     each argument type
        /// </param>
        /// <example>OscMessage message = new OscMessage("/test/test", 1, 2, 3);</example>
        public OscMessage(Uri origin, string address, params object[] args)
        {
            Origin = origin;
            Address = address;
            arguments = args;

            if (string.IsNullOrWhiteSpace(Address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (OscAddress.IsValidAddressPattern(address) == false)
            {
                throw new ArgumentException($"The address '{address}' is not a valid osc address", nameof(address));
            }

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            CheckArguments(arguments);
        }

        private OscMessage()
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return arguments.GetEnumerator();
        }

        public IEnumerator<object> GetEnumerator()
        {
            return (arguments as IEnumerable<object>).GetEnumerator();
        }

        /// <summary>
        ///     The address of the message
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        ///     Number of arguments in the message
        /// </summary>
        public int Count => arguments.Length;

        /// <summary>
        ///     Optional time tag, will be non-null if this message was extracted from a bundle
        /// </summary>
        public OscTimeTag? Timestamp { get; set; }

        public OscMessage Clone()
        {
            string address = Address;
            object[] args = arguments.Clone() as object[];

            OscMessage message = new OscMessage(address, args)
            {
                Origin = Origin,
                Timestamp = Timestamp
            };

            return message;
        }

        /// <summary>
        ///     parse a message from a string using a supplied format provider
        /// </summary>
        /// <param name="str">a string containing a message</param>
        /// <param name="provider">the format provider to use</param>
        /// <returns>the parsed message</returns>
        public new static OscMessage Parse(string str, IFormatProvider provider = null)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException("str");
            }

            if (provider == null)
            {
                provider = CultureInfo.InvariantCulture;
            }

            OscStringReader reader = new OscStringReader(str);

            return Parse(ref reader, provider, OscSerializationToken.End);
        }

        public new static OscMessage Parse(ref OscStringReader reader, IFormatProvider provider = null, OscSerializationToken endToken = OscSerializationToken.End)
        {
            string address = reader.ReadAddress(true);

            // parse arguments
            List<object> arguments = new List<object>(reader.ReadArguments(provider, endToken));

            return new OscMessage(address, arguments.ToArray());
        }

        /// <summary>
        ///     Read a OscMessage from a array of bytes
        /// </summary>
        /// <param name="bytes">the array that contains the message</param>
        /// <param name="index">the offset within the array where reading should begin</param>
        /// <param name="count">the number of bytes in the message</param>
        /// <param name="origin">the origin of the packet</param>
        /// <param name="timeTag">time-tag of parent bundle</param>
        /// <exception cref="OscException"></exception>
        /// <returns>the parsed OSC message or an empty message if their was an error while parsing</returns>
        public new static OscMessage Read(
            byte[] bytes,
            int index,
            int count,
            Uri origin = null,
            OscTimeTag? timeTag = null)
        {
            ArraySegment<byte> arraySegment = new ArraySegment<byte>(bytes, index, count);

            OscReader reader = new OscReader(arraySegment);

            return Read(reader, count, origin, timeTag);
        }

        public new static OscMessage Read(
            OscReader reader,
            int count,
            Uri origin = null,
            OscTimeTag? timeTag = null)
        {
            reader.BeginMessage(count);

            OscMessage msg = new OscMessage
            {
                Origin = origin,
                Timestamp = timeTag
            };

            msg.Address = reader.ReadAddress();

            if (reader.PeekToken() == OscToken.End)
            {
                msg.arguments = new object[0];

                return msg;
            }

            OscTypeTag typeTag = reader.ReadTypeTag();

            msg.arguments = new object[reader.GetArgumentCount(ref typeTag, out OscToken argumentsType)];

            ReadArguments(ref reader, ref typeTag, msg.arguments);

            return msg;
        }

        /// <summary>
        ///     Get the arguments as an array
        /// </summary>
        /// <returns>arguments array</returns>
        public object[] ToArray()
        {
            return arguments;
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
        ///     Try to parse a message from a string using the InvariantCulture
        /// </summary>
        /// <param name="str">the message as a string</param>
        /// <param name="message">the parsed message</param>
        /// <returns>true if the message could be parsed else false</returns>
        public static bool TryParse(string str, out OscMessage message)
        {
            try
            {
                message = Parse(str, CultureInfo.InvariantCulture);

                return true;
            }
            catch
            {
                message = null;

                return false;
            }
        }

        /// <summary>
        ///     Try to parse a message from a string using a supplied format provider
        /// </summary>
        /// <param name="str">the message as a string</param>
        /// <param name="provider">the format provider to use</param>
        /// <param name="message">the parsed message</param>
        /// <returns>true if the message could be parsed else false</returns>
        public static bool TryParse(string str, IFormatProvider provider, out OscMessage message)
        {
            try
            {
                message = Parse(str, provider);

                return true;
            }
            catch
            {
                message = null;

                return false;
            }
        }

        /// <summary>
        ///     Write the message body into a byte array
        /// </summary>
        /// <param name="data">an array of bytes to write the message body into</param>
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
            writer.StartMessage();

            // is the a address string empty? 
            if (string.IsNullOrWhiteSpace(Address))
            {
                throw new Exception("Address string may not be null or empty");
            }

            writer.WriteAddress(Address);

            // iterate through arguments and write their types
            WriteTypeTag(ref writer, arguments);

            writer.WriteTypeTagEnd();

            WriteArguments(ref writer, arguments);
        }

        public override void WriteToString(OscStringWriter writer)
        {
            writer.WriteAddress(Address);

            if (IsEmpty)
            {
                return;
            }

            writer.WriteToken(OscSerializationToken.Separator);

            writer.Write(arguments);
        }

        private void CheckArguments(object[] args)
        {
            foreach (object obj in args)
            {
                if (obj == null)
                {
                    throw new ArgumentNullException(nameof(args));
                }

                if (obj is object[])
                {
                    CheckArguments(obj as object[]);
                }
                else if (
                    !(obj is int) &&
                    !(obj is long) &&
                    !(obj is float) &&
                    !(obj is double) &&
                    !(obj is string) &&
                    !(obj is bool) &&
                    !(obj is OscNull) &&
                    !(obj is OscColor) &&
                    !(obj is OscSymbol) &&
                    !(obj is OscTimeTag) &&
                    !(obj is OscMidiMessage) &&
                    !(obj is OscImpulse) &&
                    !(obj is byte) &&
                    !(obj is byte[]))
                {
                    throw new ArgumentException("Argument is of an invalid type.", nameof(args));
                }
            }
        }

        private static void ReadArguments(ref OscReader reader, ref OscTypeTag typeTag, object[] arguments)
        {
            int index = 0;

            while (reader.PeekToken() != OscToken.End)
            {
                OscToken next = reader.PeekToken();

                switch (next)
                {
                    case OscToken.ArrayStart:
                        int arrayLength = reader.StartArray(ref typeTag, out OscToken arrayType);

                        object[] array = new object[arrayLength];

                        arguments[index++] = array;

                        ReadArguments(ref reader, ref typeTag, array);
                        break;
                    case OscToken.ArrayEnd:
                        reader.EndArray(ref typeTag);
                        return;
                    default:
                        // all the values will be boxed anyway so we might as well use the boxing argument reader
                        arguments[index++] = reader.ReadArgument(ref typeTag);
                        break;
                }
            }
        }

        private static void WriteArguments(ref OscWriter writer, object[] args)
        {
            foreach (object obj in args)
            {
                switch (obj)
                {
                    case object[] value:
                        WriteArguments(ref writer, value);
                        break;
                    case int value:
                        writer.WriteInt(value);
                        break;
                    case long value:
                        writer.WriteLong(value);
                        break;
                    case float value:
                        writer.WriteFloat(value);
                        break;
                    case double value:
                        writer.WriteDouble(value);
                        break;
                    case byte value:
                        writer.WriteChar(value);
                        break;
                    case OscColor value:
                        writer.WriteColor(ref value);
                        break;
                    case OscTimeTag value:
                        writer.WriteTimeTag(ref value);
                        break;
                    case OscMidiMessage value:
                        writer.WriteMidi(ref value);
                        break;
                    case bool _:
                    case OscNull _:
                    case OscImpulse value:
                        break;
                    case string value:
                        writer.WriteString(value);
                        break;
                    case OscSymbol value:
                        writer.WriteSymbol(ref value);
                        break;
                    case byte[] value:
                        writer.WriteBlob(value);
                        break;
                    default:
                        throw new Exception($"Unsupported arguemnt type '{obj.GetType()}'");
                }
            }
        }

        private static void WriteTypeTag(ref OscWriter writer, object[] args)
        {
            foreach (object obj in args)
            {
                switch (obj)
                {
                    case object[] value:
                        writer.WriteTypeTag(OscToken.ArrayStart);
                        WriteTypeTag(ref writer, value);
                        writer.WriteTypeTag(OscToken.ArrayEnd);
                        break;
                    case int _:
                        writer.WriteTypeTag(OscToken.Int);
                        break;
                    case long _:
                        writer.WriteTypeTag(OscToken.Long);
                        break;
                    case float _:
                        writer.WriteTypeTag(OscToken.Float);
                        break;
                    case double _:
                        writer.WriteTypeTag(OscToken.Double);
                        break;
                    case byte _:
                        writer.WriteTypeTag(OscToken.Char);
                        break;
                    case OscColor _:
                        writer.WriteTypeTag(OscToken.Color);
                        break;
                    case OscTimeTag _:
                        writer.WriteTypeTag(OscToken.TimeTag);
                        break;
                    case OscMidiMessage _:
                        writer.WriteTypeTag(OscToken.Midi);
                        break;
                    case bool value:
                        writer.WriteTypeTag(value ? OscToken.True : OscToken.False);
                        break;
                    case OscNull _:
                        writer.WriteTypeTag(OscToken.Null);
                        break;
                    case OscImpulse _:
                        writer.WriteTypeTag(OscToken.Impulse);
                        break;
                    case string _:
                        writer.WriteTypeTag(OscToken.String);
                        break;
                    case OscSymbol _:
                        writer.WriteTypeTag(OscToken.Symbol);
                        break;
                    case byte[] _:
                        writer.WriteTypeTag(OscToken.Blob);
                        break;
                    default:
                        throw new Exception($"Unsupported arguemnt type '{obj.GetType()}'");
                }
            }
        }
    }
}