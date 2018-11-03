// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

// TODO: Flagged for possible deletion, I have never seen anybody use this class
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using OscCore.LowLevel;

namespace OscCore
{
/* 
	Note Off 	 0x8# 	 note number 	 velocity 
	Note On 	 0x9# 	 note number 	 velocity 
	Poly Pressure 	 0xa# 	 note number 	 value 
	Control Change 	 0xb# 	 controller number 	 value 
	Program Change 	 0xc# 	 program number 	
	Channel Pressure 	 0xd# 	 value 	
	Pitch Bend 	 0xe# 	 0 	 bend amount 
	System Exclusive 	 0xf0 	 (sysex message) 	 0xf7 
	Time Code 	 0xf1 	 data 	
	Song Position 	 0xf2 	 0 	 position 
	Song Select 	 0xf3 	 song number 	
	Tune Request 	 0xf6 		
	Clock Tick 	 0xf8 		
	Start 	 0xfa 		
	Continue 	 0xfb 		
	Stop 	 0xfc 		
	Active Sense 	 0xfe 		
	System Reset 	 0xff
	*/

    public enum OscMidiMessageType : byte
    {
        NoteOff = 0x80,
        NoteOn = 0x90,
        PolyPressure = 0xA0,
        ControlChange = 0xB0,
        ProgramChange = 0xC0,
        ChannelPressure = 0xD0,
        PitchBend = 0xE0,
        SystemExclusive = 0xF0
    }

    public enum OscMidiSystemMessageType : byte
    {
        SystemExclusive = 0x00,
        TimeCode = 0x01,
        SongPosition = 0x02,
        SongSelect = 0x03,
        TuneRequest = 0x06,
        ClockTick = 0x08,
        Start = 0x0A,
        Continue = 0x0B,
        Stop = 0x0C,
        ActiveSense = 0x0E,
        SystemReset = 0x0F
    }


    public enum OscMidiNote : byte
    {
        C0 = 0,
        CS0,
        D0,
        DS0,
        E0,
        F0,
        FS0,
        G0,
        GS0,
        A0,
        AS0,
        B0,

        C1,
        CS1,
        D1,
        DS1,
        E1,
        F1,
        FS1,
        G1,
        GS1,
        A1,
        AS1,
        B1,

        C2,
        CS2,
        D2,
        DS2,
        E2,
        F2,
        FS2,
        G2,
        GS2,
        A2,
        AS2,
        B2,

        C3,
        CS3,
        D3,
        DS3,
        E3,
        F3,
        FS3,
        G3,
        GS3,
        A3,
        AS3,
        B3,

        C4,
        CS4,
        D4,
        DS4,
        E4,
        F4,
        FS4,
        G4,
        GS4,
        A4,
        AS4,
        B4,

        C5,
        CS5,
        D5,
        DS5,
        E5,
        F5,
        FS5,
        G5,
        GS5,
        A5,
        AS5,
        B5,

        C6,
        CS6,
        D6,
        DS6,
        E6,
        F6,
        FS6,
        G6,
        GS6,
        A6,
        AS6,
        B6,

        C7,
        CS7,
        D7,
        DS7,
        E7,
        F7,
        FS7,
        G7,
        GS7,
        A7,
        AS7,
        B7,

        C8,
        CS8,
        D8,
        DS8,
        E8,
        F8,
        FS8,
        G8,
        GS8,
        A8,
        AS8,
        B8,

        C9,
        CS9,
        D9,
        DS9,
        E9,
        F9,
        FS9,
        G9,
        GS9,
        A9,
        AS9,
        B9,

        C10,
        CS10,
        D10,
        DS10,
        E10,
        F10,
        FS10,
        G10
    }

    /// <summary>
    ///     Midi Message
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct OscMidiMessage
    {
        #region Fields

        [FieldOffset(0)] public uint FullMessage;

        [FieldOffset(3)] public byte PortID;

        [FieldOffset(2)] public byte StatusByte;

        [FieldOffset(1)] public byte Data1;

        [FieldOffset(1)] public OscMidiNote Note;

        [FieldOffset(0)] public byte Data2;

        #endregion

        #region Properties

        /// <summary>
        ///     The midi message type
        /// </summary>
        public OscMidiMessageType MessageType => (OscMidiMessageType) (StatusByte & 0xF0);

        /// <summary>
        ///     The system message type, only valid when MessageType is SystemExclusive
        /// </summary>
        public OscMidiSystemMessageType SystemMessageType => (OscMidiSystemMessageType) (StatusByte & 0x0F);

        /// <summary>
        ///     The channel, only valid when MessageType is not SystemExclusive
        /// </summary>
        public int Channel => StatusByte & 0x0F;

        /// <summary>
        ///     14 bit data value, for pitch bend messages
        /// </summary>
        public ushort Data14BitValue => (ushort) ((Data1 & 0x7F) | ((Data2 & 0x7F) << 7));

        #endregion

        #region Constructor

        /// <summary>
        ///     Parse a midi message from a single 4 byte integer
        /// </summary>
        /// <param name="value">4 byte integer portID | (type | channel) | data1 | data2</param>
        public OscMidiMessage(uint value)
        {
            PortID = 0;
            StatusByte = 0;
            Data1 = 0;
            Note = OscMidiNote.C0;
            Data2 = 0;

            FullMessage = value;
        }

        /// <summary>
        ///     Create midi message
        /// </summary>
        /// <param name="portID">port id</param>
        /// <param name="statusByte">status byte</param>
        /// <param name="data1">data 1</param>
        /// <param name="data2">data 2</param>
        public OscMidiMessage(
            byte portID,
            byte statusByte,
            byte data1,
            byte data2)
        {
            FullMessage = 0;
            Note = OscMidiNote.C0;

            PortID = portID;
            StatusByte = statusByte;
            Data1 = (byte) (data1 & 0x7F);
            Data2 = (byte) (data2 & 0x7F);
        }

        /// <summary>
        ///     Create midi message
        /// </summary>
        /// <param name="portID">the id of the destination port</param>
        /// <param name="type">the type of message</param>
        /// <param name="channel">the channel</param>
        /// <param name="data1">data argument 1</param>
        /// <param name="data2">data argument 2</param>
        public OscMidiMessage(
            byte portID,
            OscMidiMessageType type,
            byte channel,
            byte data1,
            byte data2)
        {
            if (channel >= 16)
            {
                throw new ArgumentOutOfRangeException("channel");
            }

            FullMessage = 0;
            Note = OscMidiNote.C0;

            PortID = portID;
            StatusByte = (byte) ((int) type | channel);
            Data1 = (byte) (data1 & 0x7F);
            Data2 = (byte) (data2 & 0x7F);
        }

        /// <summary>
        ///     Create midi message
        /// </summary>
        /// <param name="portID">port id</param>
        /// <param name="type">midi message type</param>
        /// <param name="channel">midi channel</param>
        /// <param name="data1">data 1</param>
        public OscMidiMessage(
            byte portID,
            OscMidiMessageType type,
            byte channel,
            byte data1)
            : this(portID, type, channel, data1, 0)
        {
        }

        /// <summary>
        ///     Create midi message
        /// </summary>
        /// <param name="portID">port id</param>
        /// <param name="type">midi message type</param>
        /// <param name="channel">midi channel</param>
        /// <param name="value">14 bit data value</param>
        public OscMidiMessage(
            byte portID,
            OscMidiMessageType type,
            byte channel,
            ushort value)
            : this(portID, type, channel, (byte) (value & 0x7F), (byte) ((value & 0x3F80) >> 7))
        {
        }

        /// <summary>
        ///     Create midi message
        /// </summary>
        /// <param name="portID">port id</param>
        /// <param name="type">midi system message type</param>
        /// <param name="value">14 bit data value</param>
        public OscMidiMessage(byte portID, OscMidiSystemMessageType type, ushort value)
            : this(portID, OscMidiMessageType.SystemExclusive, (byte) type, (byte) (value & 0x7F), (byte) ((value & 0x3F80) >> 7))
        {
        }

        /// <summary>
        ///     Create midi message
        /// </summary>
        /// <param name="portID">port id</param>
        /// <param name="type">midi system message type</param>
        /// <param name="data1">data 1</param>
        public OscMidiMessage(byte portID, OscMidiSystemMessageType type, byte data1)
            : this(portID, OscMidiMessageType.SystemExclusive, (byte) type, data1, 0)
        {
        }

        /// <summary>
        ///     Create midi message
        /// </summary>
        /// <param name="portID">port id</param>
        /// <param name="type">midi system message type</param>
        /// <param name="data1">data 1</param>
        /// <param name="data2">data 2</param>
        public OscMidiMessage(
            byte portID,
            OscMidiSystemMessageType type,
            byte data1,
            byte data2)
            : this(portID, OscMidiMessageType.SystemExclusive, (byte) type, data1, data2)
        {
        }

        #endregion

        #region Standard Overrides

        public override bool Equals(object obj)
        {
            if (obj is uint)
            {
                return FullMessage.Equals((uint) obj);
            }

            if (obj is OscMidiMessage)
            {
                return FullMessage.Equals(((OscMidiMessage) obj).FullMessage);
            }

            return FullMessage.Equals(obj);
        }

        public override int GetHashCode()
        {
            return FullMessage.GetHashCode();
        }

        #endregion

        #region To String

        public override string ToString()
        {
            return ToString(CultureInfo.InvariantCulture);
        }

        public string ToString(IFormatProvider provider)
        {
            if (MessageType != OscMidiMessageType.SystemExclusive)
            {
                return string.Format(
                    "{0}, {1}, {2}, {3}, {4}",
                    PortID.ToString(provider),
                    MessageType.ToString(),
                    Channel.ToString(provider),
                    Data1.ToString(provider),
                    Data2.ToString(provider)
                );
            }

            return string.Format(
                "{0}, {1}, {2}, {3}",
                PortID.ToString(provider),
                SystemMessageType.ToString(),
                Data1.ToString(provider),
                Data2.ToString(provider)
            );
        }

        #endregion

        #region Parse

        public static OscMidiMessage Parse(ref OscStringReader reader, IFormatProvider provider)
        {
//            string[] pieces = new string[4];
//
//            OscSerializationToken token = OscSerializationToken.None; 
//            
//            for (int i = 0; i < 4; i++)
//            {
//                token = reader.ReadNextToken(out string value);
//                pieces[i] = value; 
//                token = reader.ReadNextToken(out string _);
//            }
//
//            if (token != OscSerializationToken.ObjectEnd)
//            {
//                throw new Exception($"Invalid color");
//            }
//
//            byte a, r, g, b;
//            
//            r = byte.Parse(pieces[0].Trim(), System.Globalization.NumberStyles.None, provider);
//            g = byte.Parse(pieces[1].Trim(), System.Globalization.NumberStyles.None, provider);
//            b = byte.Parse(pieces[2].Trim(), System.Globalization.NumberStyles.None, provider);
//            a = byte.Parse(pieces[3].Trim(), System.Globalization.NumberStyles.None, provider);
//
//            return OscColor.FromArgb(a, r, g, b);
//            
//            return new OscMidiMessage(0, OscMidiMessageType.ControlChange, 0, 0); 

            List<string> parts = new List<string>();

            OscSerializationToken token = OscSerializationToken.None;

            do
            {
                token = reader.ReadNextToken(out string value);
                parts.Add(value);
                token = reader.ReadNextToken(out string _);
            }
            while (token != OscSerializationToken.ObjectEnd);

            if (parts.Count < 4)
            {
                throw new Exception($"Not a midi message '{parts.Count}'");
            }

            int index = 0;
            byte portID = byte.Parse(
                parts[index++]
                    .Trim(),
                provider
            );

            byte statusByte;
            OscMidiMessageType messageType;

            if (byte.TryParse(
                    parts[index]
                        .Trim(),
                    NumberStyles.Integer,
                    provider,
                    out statusByte
                ) == false)
            {
                OscMidiSystemMessageType systemMessage;

                if (Enum.TryParse(
                    parts[index]
                        .Trim(),
                    true,
                    out systemMessage
                ))
                {
                    messageType = OscMidiMessageType.SystemExclusive;
                    statusByte = (byte) ((int) messageType | (int) systemMessage);
                    index++;
                }
                else if (Enum.TryParse(
                    parts[index]
                        .Trim(),
                    true,
                    out messageType
                ))
                {
                    index++;
                    byte channel = byte.Parse(
                        parts[index++]
                            .Trim(),
                        NumberStyles.Integer,
                        provider
                    );

                    if (channel > 15)
                    {
                        throw new ArgumentOutOfRangeException(nameof(channel));
                    }

                    statusByte = (byte) ((int) messageType | channel);

                    if (parts.Count < 5)
                    {
                        throw new Exception($"Not a midi message '{parts.Count}'");
                    }
                }
                else
                {
                    throw new Exception($"Not a midi message '{parts.Count}'");
                }
            }

            byte data1 = byte.Parse(
                parts[index++]
                    .Trim(),
                NumberStyles.Integer,
                provider
            );

            if (data1 > 0x7F)
            {
                throw new ArgumentOutOfRangeException(nameof(data1));
            }

            byte data2 = byte.Parse(
                parts[index++]
                    .Trim(),
                NumberStyles.Integer,
                provider
            );

            if (data2 > 0x7F)
            {
                throw new ArgumentOutOfRangeException(nameof(data2));
            }

            if (index != parts.Count)
            {
                throw new Exception($"Not a midi message '{parts.Count}'");
            }

            return new OscMidiMessage(portID, statusByte, data1, data2);
        }

        public static OscMidiMessage Parse(string str, IFormatProvider provider)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new Exception($"Not a midi message '{str}'");
            }

            string[] parts = str.Split(',');

            if (parts.Length < 4)
            {
                throw new Exception($"Not a midi message '{str}'");
            }

            int index = 0;
            byte portID = byte.Parse(
                parts[index++]
                    .Trim(),
                provider
            );

            byte statusByte;
            OscMidiMessageType messageType;

            if (byte.TryParse(
                    parts[index]
                        .Trim(),
                    NumberStyles.Integer,
                    provider,
                    out statusByte
                ) == false)
            {
                OscMidiSystemMessageType systemMessage;

                if (Enum.TryParse(
                    parts[index]
                        .Trim(),
                    true,
                    out systemMessage
                ))
                {
                    messageType = OscMidiMessageType.SystemExclusive;
                    statusByte = (byte) ((int) messageType | (int) systemMessage);
                    index++;
                }
                else if (Enum.TryParse(
                    parts[index]
                        .Trim(),
                    true,
                    out messageType
                ))
                {
                    index++;
                    byte channel = byte.Parse(
                        parts[index++]
                            .Trim(),
                        NumberStyles.Integer,
                        provider
                    );

                    if (channel > 15)
                    {
                        throw new ArgumentOutOfRangeException(nameof(channel));
                    }

                    statusByte = (byte) ((int) messageType | channel);

                    if (parts.Length < 5)
                    {
                        throw new Exception($"Not a midi message '{str}'");
                    }
                }
                else
                {
                    throw new Exception($"Not a midi message '{str}'");
                }
            }

            byte data1 = byte.Parse(
                parts[index++]
                    .Trim(),
                NumberStyles.Integer,
                provider
            );

            if (data1 > 0x7F)
            {
                throw new ArgumentOutOfRangeException(nameof(data1));
            }

            byte data2 = byte.Parse(
                parts[index++]
                    .Trim(),
                NumberStyles.Integer,
                provider
            );

            if (data2 > 0x7F)
            {
                throw new ArgumentOutOfRangeException(nameof(data2));
            }

            if (index != parts.Length)
            {
                throw new Exception($"Not a midi message '{str}'");
            }

            return new OscMidiMessage(portID, statusByte, data1, data2);
        }

        public static OscMidiMessage Parse(string str)
        {
            return Parse(str, CultureInfo.InvariantCulture);
        }

        public static bool TryParse(string str, IFormatProvider provider, out OscMidiMessage message)
        {
            try
            {
                message = Parse(str, provider);

                return true;
            }
            catch
            {
                message = default(OscMidiMessage);

                return false;
            }
        }

        public static bool TryParse(string str, out OscMidiMessage message)
        {
            try
            {
                message = Parse(str, CultureInfo.InvariantCulture);

                return true;
            }
            catch
            {
                message = default(OscMidiMessage);

                return false;
            }
        }

        #endregion
    }
}