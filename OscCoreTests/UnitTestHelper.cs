// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Text;
using OscCore;
using OscCore.Address;
using Xunit;

namespace OscCoreTests
{
	static class UnitTestHelper
	{

		public static string RebuildOscAddress(OscAddress address)
		{
			StringBuilder sb = new StringBuilder();

			foreach (OscAddressPart part in address)
			{
				sb.Append(part.Interpreted);
			}

			return sb.ToString();
		}

		#region Message Test Data

		#region Message Single Arg (Int)

		internal static byte[] MessageBody_Int = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'i', 0, 0, 
				// value
				0, 0, 0, 0x2A
			};

		internal static OscMessage Message_Int()
		{
			return new OscMessage("/test", 42);
		}

		internal static string MessageString_Int = "/test, 42";

		#endregion

		#region Message Single Arg (Long)

		internal static byte[] MessageBody_Long = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'h', 0, 0, 
				// value
				0xA1, 0xC2, 0xE3, 0xF4, 0xA5, 0xC6, 0xE7, 0xF8 
			};

		internal static OscMessage Message_Long()
		{
			return new OscMessage("/test", unchecked((long)0xA1C2E3F4A5C6E7F8));
		}

		internal static string MessageString_Long = "/test, 0xA1C2E3F4A5C6E7F8";

		#endregion

		#region Message Single Arg (Float)

		internal static byte[] MessageBody_Float = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'f', 0, 0, 
				// value				
				0x41, 0xCA, 00, 00
			};

		internal static OscMessage Message_Float()
		{
			return new OscMessage("/test", 25.25f);
		}

		internal static string MessageString_Float = "/test, 25.25";

        internal static OscMessage Message_Float_PositiveInfinity()
        {
            return new OscMessage("/test", float.PositiveInfinity);
        }

        internal static string MessageString_Float_PositiveInfinity = "/test, Infinity";


        internal static OscMessage Message_Float_NegativeInfinity()
        {
            return new OscMessage("/test", float.NegativeInfinity);
        }

        internal static string MessageString_Float_NegativeInfinity = "/test, -Infinity";


        internal static OscMessage Message_Float_NaN()
        {
            return new OscMessage("/test", float.NaN);
        }

        internal static string MessageString_Float_NaN = "/test, NaN";

        #endregion

        #region Message Single Arg (Double)

        internal static byte[] MessageBody_Double = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'d', 0, 0, 
				// value				
				0x40, 0x28, 0xd8, 0xc7, 0xe2, 0x82, 0x40, 0xb8 
			};

		internal static OscMessage Message_Double()
		{
			return new OscMessage("/test", 12.4234);
		}

		internal static string MessageString_Double = "/test, 12.4234d";

		#endregion

		#region Message Single Arg (TimeTag)

		internal static byte[] MessageBody_TimeTag = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'t', 0, 0, 
				// value				
				0x13, 0xC1, 0xDA, 0x49, 0xE6, 0xB5, 0x0B, 0x0F
			};

		internal static OscMessage Message_TimeTag()
		{
			return new OscMessage("/test", new OscTimeTag(0x13C1DA49E6B50B0F));
		}

		#endregion

		#region Message Single Arg (Char)

		internal static byte[] MessageBody_Char = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'c', 0, 0, 
				// value				
				(byte)'p', 0x00, 0x00, 0x00
			};

		internal static OscMessage Message_Char()
		{
			return new OscMessage("/test", (byte)'p');
		}

		internal static string MessageString_Char = "/test, 'p'";

		#endregion

		#region Message Single Arg (Color)

		internal static byte[] MessageBody_Color_Red = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'r', 0, 0, 
				// value				
				0xFF, 0x00, 0x00, 0xFF
			};

		internal static OscMessage Message_Color_Red()
		{
			return new OscMessage("/test", OscColor.FromArgb(255, 255, 0, 0));
		}

		internal static byte[] MessageBody_Color_Green = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'r', 0, 0, 
				// value				
				0x00, 0xFF, 0x00, 0xFF
			};

		internal static OscMessage Message_Color_Green()
		{
			return new OscMessage("/test", OscColor.FromArgb(255, 0, 255, 0));
		}

		internal static byte[] MessageBody_Color_Blue = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'r', 0, 0, 
				// value				
				0x00, 0x00, 0xFF, 0xFF
			};

		internal static OscMessage Message_Color_Blue()
		{
			return new OscMessage("/test", OscColor.FromArgb(255, 0, 0, 255));
		}

		internal static byte[] MessageBody_Color_Transparent = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'r', 0, 0, 
				// value				
				0x00, 0x00,0x00, 0x00
			};

		internal static OscMessage Message_Color_Transparent()
		{
			return new OscMessage("/test", OscColor.FromArgb(0, 0, 0, 0));
		}

		#endregion

		#region Message Single Arg (Midi)

		internal static byte[] MessageBody_Midi = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'m', 0, 0, 
				// value				
				0x03, 0xF3, 0x56, 0x26
			};

		internal static OscMessage Message_Midi()
		{
			return new OscMessage("/test", new OscMidiMessage(3, OscMidiSystemMessageType.SongSelect, 0x1356));
		}

		internal static string MessageString_Midi = "/test, { Midi: 3, SongSelect, 86, 38 }";

		#endregion

		#region Message Single Arg (True)

		internal static byte[] MessageBody_True = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'T', 0, 0, 				
			};

		internal static OscMessage Message_True()
		{
			return new OscMessage("/test", true);
		}

		internal static string MessageString_True = "/test, true";

		#endregion

		#region Message Single Arg (False)

		internal static byte[] MessageBody_False = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'F', 0, 0, 				
			};

		internal static OscMessage Message_False()
		{
			return new OscMessage("/test", false);
		}

		internal static string MessageString_False = "/test, false";

		#endregion

		#region Message Single Arg (Nil)

		internal static byte[] MessageBody_Nil = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'N', 0, 0, 				
			};

		internal static OscMessage Message_Nil()
		{
			return new OscMessage("/test", OscNull.Value);
		}

		internal static string MessageString_Nil = "/test, nil";

		#endregion

		#region Message Single Arg (Infinitum)

		internal static byte[] MessageBody_Infinitum = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'I', 0, 0, 				
			};

		internal static OscMessage Message_Infinitum()
		{
			return new OscMessage("/test", OscImpulse.Value);
		}

		internal static string MessageString_Infinitum = "/test, inf";

		#endregion

		#region Message Double Arg (Float)

		internal static byte[] MessageBody_Float2 = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'f', (byte)'f', 0, 
				// value 1				
				0x41, 0xCA, 00, 00,
				// value 2				
				0x41, 0xCA, 00, 00
			};

		internal static OscMessage Message_Float2()
		{
			return new OscMessage("/test", 25.25f, 25.25f);
		}

		internal static string MessageString_Float2 = "/test, 25.25f, 25.25f";

		#endregion

		#region Message Tripple Arg (Float)

		internal static byte[] MessageBody_Float3 = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'f', (byte)'f', (byte)'f', 0, 0, 0, 0,
				// value 1				
				0x41, 0xCA, 00, 00,
				// value 2				
				0x41, 0xCA, 00, 00,
				// value 3				
				0x41, 0xCA, 00, 00
			};

		internal static OscMessage Message_Float3()
		{
			return new OscMessage("/test", 25.25f, 25.25f, 25.25f);
		}

		internal static string MessageString_Float3 = "/test, 25.25f, 25.25f, 25.25f";

		#endregion

		#region Message Single Arg (String)

		internal static byte[] MessageBody_String = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'s', 0, 0, 
				// value				
				(byte)'h', (byte)'e', (byte)'l', (byte)'l', (byte)'o', (byte)'!', 0, 0
			};

		internal static OscMessage Message_String()
		{
			return new OscMessage(null, "/test", "hello!");
		}

		internal static string MessageString_String = "/test, \"hello!\"";

		
		internal static OscMessage Message_NestedQuoteString()
		{
			return new OscMessage(null, "/test", @"Lets say ""hello!""");
		}

		internal static string MessageString_NestedQuoteString = @"/test, ""Lets say \""hello!\""""";
		
		#endregion

		#region Message Single Arg (Symbol)

		internal static byte[] MessageBody_Symbol = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'S', 0, 0, 
				// value				
				(byte)'h', (byte)'e', (byte)'l', (byte)'l', (byte)'o', (byte)'!', 0, 0
			};

		internal static OscMessage Message_Symbol()
		{
			return new OscMessage("/test", new OscSymbol("hello!"));
		}

		internal static string MessageString_Symbol = "/test, hello!";
		internal static string MessageString_Symbol2 = @"/test, $""hello!""";

		#endregion

		#region Message Single Arg (Blob)

		internal static byte[] MessageBody_Blob = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'b', 0, 0, 
				// length 
				0, 0, 0, 3,
				// value				
				3, 2, 1, 0
			};

		internal static OscMessage Message_Blob()
		{
			return new OscMessage("/test", new byte[] { 3, 2, 1 });
		}

		internal static string MessageString_Blob_Array = "/test, { blob: 3, 2, 1 }";
		internal static string MessageString_Blob_Hex = "/test, { blob: 0x030201 }";
		internal static string MessageString_Blob_Base64 = "/test, { blob: 64xAwIB }";

		internal static byte[] MessageBody_Blob2 = new byte[] 
			{ 
				0x2f, 0x73, 0x65, 0x72, 0x69, 0x61, 0x6c, 0x2f, 0x31, 0x00, 0x00, 0x00,
				0x2c, 0x62, 0x00, 0x00, 
				0x00, 0x00, 0x00, 0x20,
				0x53, 0x00, 0x00, 0x00, 0x01, 0x00, 0x07, 0xff, 0xb4, 0x00, 0x18, 0x04, 
				0x08, 0xf4, 0x46, 0xfb, 0xa8, 0xf5, 0x51, 0xb8, 0xf7, 0x51, 0x1f, 0xa5, 
				0x00, 0x6e, 0xfe, 0x90, 0xe9, 0x44, 0xb8, 0xfe
			};

		internal static OscMessage Message_Blob2()
		{
			return new OscMessage("/serial/1", new byte[] 
			{ 0x53, 0x00, 0x00, 0x00, 0x01, 0x00, 0x07, 0xff, 0xb4, 0x00, 0x18, 0x04, 
				0x08, 0xf4, 0x46, 0xfb, 0xa8, 0xf5, 0x51, 0xb8, 0xf7, 0x51, 0x1f, 0xa5, 
				0x00, 0x6e, 0xfe, 0x90, 0xe9, 0x44, 0xb8, 0xfe 
			});
		}


		#endregion

		#region Message Array Arg (Ints)

		internal static byte[] MessageBody_Array_Ints = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'i', (byte)'[', (byte)'i', (byte)'i', (byte)'i', (byte)']', 0,
				// value
				0x1A, 0x2A, 0x3A, 0x4A,

				// value array 
				0x1A, 0x2A, 0x3A, 0x4A,
				0x5A, 0x6A, 0x7A, 0x8A,
				0x9A, 0xAA, 0xBA, 0xCA,
			};

		internal static OscMessage Message_Array_Ints()
		{
			return new OscMessage("/test", unchecked((int)0x1A2A3A4A), 
				new object[] 
				{ 
					unchecked((int)0x1A2A3A4A),
					unchecked((int)0x5A6A7A8A),
					unchecked((int)0x9AAABACA),
				});
		}

		internal static string MessageString_Array_Ints = "/test, 0x1A2A3A4A, [0x1A2A3A4A, 0x5A6A7A8A, 0x9AAABACA]";

		internal static byte[] MessageBody_Array_Ints2 = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'[', (byte)'i', (byte)'i', (byte)'i', (byte)']', (byte)'i', 0,

				// value array 
				0x1A, 0x2A, 0x3A, 0x4A,
				0x5A, 0x6A, 0x7A, 0x8A,
				0x9A, 0xAA, 0xBA, 0xCA,

				// value
				0x1A, 0x2A, 0x3A, 0x4A,
			};

		internal static OscMessage Message_Array_Ints2()
		{
			return new OscMessage("/test", 
				new object[] 
				{ 
					unchecked((int)0x1A2A3A4A),
					unchecked((int)0x5A6A7A8A),
					unchecked((int)0x9AAABACA),
				},
				unchecked((int)0x1A2A3A4A));
		}

		internal static string MessageString_Array_Ints2 = "/test, [0x1A2A3A4A, 0x5A6A7A8A, 0x9AAABACA], 0x1A2A3A4A";

		#endregion

		#region Message Nested Array Arg (Ints)

		internal static byte[] MessageBody_Array_NestedInts = new byte[] 
			{ 
				// Address 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0, 
				// Typetag
				(byte)',', (byte)'i', (byte)'[', (byte)'i', (byte)'i', (byte)'[', (byte)'i', (byte)'i', (byte)'i', (byte)']', (byte)'i', (byte)']', 0, 0, 0, 0,
				// value
				0x1A, 0x2A, 0x3A, 0x4A,

				// value array 
				0x1A, 0x2A, 0x3A, 0x4A,
				0x5A, 0x6A, 0x7A, 0x8A,

				// nested value array 
				0x1A, 0x2A, 0x3A, 0x4A,
				0x5A, 0x6A, 0x7A, 0x8A,
				0x9A, 0xAA, 0xBA, 0xCA,

				0x9A, 0xAA, 0xBA, 0xCA,
			};

		internal static OscMessage Message_Array_NestedInts()
		{
			return new OscMessage("/test", unchecked((int)0x1A2A3A4A),
				new object[] 
				{ 
					unchecked((int)0x1A2A3A4A),
					unchecked((int)0x5A6A7A8A),
					
					new object[] 
					{ 
						unchecked((int)0x1A2A3A4A),
						unchecked((int)0x5A6A7A8A),
						unchecked((int)0x9AAABACA),
					},

					unchecked((int)0x9AAABACA),
					
				});
		}

		internal static string MessageString_Array_NestedInts = "/test, 0x1A2A3A4A, [0x1A2A3A4A, 0x5A6A7A8A, [0x1A2A3A4A, 0x5A6A7A8A, 0x9AAABACA], 0x9AAABACA]";

		#endregion

		#region Badly Formed Messages

		internal static byte[] BadlyFormedMessage_PacketLength = new byte[] 
			{ 
				0
			};

		internal static byte[] BadlyFormedMessage_Address1 = new byte[] 
			{ 
				0, 0, 0, 0
			};

		internal static byte[] BadlyFormedMessage_Address2 = new byte[] 
			{ 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s'
			};

		internal static byte[] BadlyFormedMessage_MissingComma = new byte[] 
			{ 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0,

				0, 0, 0, 0
			};

		internal static byte[] BadlyFormedMessage_MissingTypeTag = new byte[] 
			{ 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0,

				(byte)',', (byte)'i', (byte)'i', (byte)'i'
			};

		internal static byte[] BadlyFormedMessage_MissingArgs = new byte[] 
			{ 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0,

				(byte)',', (byte)'i', 0, 0
			};

		internal static byte[] BadlyFormedMessage_UnknownArguemntType = new byte[] 
			{ 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0,

				(byte)',', (byte)'g', 0, 0,

				0, 0, 0, 0x2A
			};

		internal static byte[] BadlyFormedMessage_ErrorParsingBlob = new byte[] 
			{ 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0,

				(byte)',', (byte)'b', 0, 0,

				0, 0, 0, 3
			};

		internal static byte[] BadlyFormedMessage_ErrorParsingBlob2 = new byte[] 
			{ 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0,

				(byte)',', (byte)'i', (byte)'b', 0,

				0, 0, 0, 0x2A
			};

		internal static byte[] BadlyFormedMessage_ErrorParsingString = new byte[] 
			{ 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0,

				(byte)',', (byte)'s', 0, 0,

				(byte)'t', (byte)'e', (byte)'s', (byte)'t',
			};

		internal static byte[] BadlyFormedMessage_ErrorParsingString2 = new byte[] 
			{ 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0,

				(byte)',', (byte)'i', (byte)'s', 0,

				0, 0, 0, 0x2A
			};

		internal static byte[] BadlyFormedMessage_ErrorParsingInt = new byte[] 
			{ 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0,

				(byte)',', (byte)'i', (byte)'i', 0,

				0, 0, 0, 0x2A
			};

		internal static byte[] BadlyFormedMessage_ErrorParsingFloat = new byte[] 
			{ 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0,

				(byte)',', (byte)'i', (byte)'f', 0,

				0, 0, 0, 0x2A
			};

		internal static byte[] BadlyFormedMessage_ErrorParsingDouble = new byte[] 
			{ 
				(byte)'/', (byte)'t', (byte)'e', (byte)'s', (byte)'t', 0, 0, 0,

				(byte)',', (byte)'d', 0, 0,

				0, 0, 0, 0x2A
			};

		#endregion

		#endregion	
	
		#region Bundle 

		internal static OscBundle Bundle_Int()
		{	
			return new OscBundle(OscTimeTag.BaseDate.Add(new TimeSpan(0, 0, 0, 34, 3532)), new OscMessage("/test", 1, 2, 3));
		}

		internal static string BundleString_Int = "#bundle, 00:00:34.3532, { /test, 1, 2, 3 }";
		
		internal static byte[] DoubleNestedBundleBody = new byte[] 
			{ 
				35,	 98,  117, 110, 100, 108, 101, 0,	// #bundle\0
				0,	 0,	  0,   0,   0,   0,   0,   0,	// time-tag (0)

				0,   0,   0,   32,						// length (32)
				35,  98,  117, 110,	100, 108, 101, 0,	// #bundle\0
				0,	 0,	  0,   0,   0,   0,   0,   0,	// time-tag (0)

				0,   0,   0,   12,						// length (12)
				47,  97,  97,  0,						// /aa\0
				44,  105, 0,   0,						// ,i\0\0
				255, 255, 255, 255,						// int32 (-1)

				0,   0,   0,   32,						// length (32)
				35,	 98,  117, 110, 100, 108, 101, 0,	// #bundle\0
				0,	 0,	  0,   0,   0,   0,   0,   0,	// time-tag (0)

				0,   0,   0,   12,						// length (12)
				47,  97,  97,  0,						// /aa\0
				44,  105, 0,   0,						// ,i\0\0
				255, 255, 255, 255,						// int32 (-1)
			};

			internal static byte[] DoubleNestedBundleBody_Hex = new byte[] 
			{ 
				0x23, 0x62, 0x75, 0x6e, 0x64, 0x6c, 0x65, 0x00,	// #bundle\0
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,	// time-tag (0)

				0x00, 0x00, 0x00, 0x40,							// length (32)
				0x23, 0x62, 0x75, 0x6e, 0x64, 0x6c, 0x65, 0x00,	// #bundle\0
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,	// time-tag (0)

				0x00, 0x00, 0x00, 0x0c,							// length (12)
				0x2f, 0x61, 0x61, 0x00,							// /aa\0
				0x2c, 0x69, 0x00, 0x00, 						// ,i\0\0
				0xff, 0xff, 0xff, 0xff,							// int32 (-1)

				0x00, 0x00, 0x00, 0x40,							// length (32)
				0x23, 0x62, 0x75, 0x6e, 0x64, 0x6c, 0x65, 0x00, // #bundle\0
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,	// time-tag (0)

				0x00, 0x00, 0x00, 0x0c,							// length (12)
				0x2f, 0x61, 0x61, 0x00,							// /aa\0
				0x2c, 0x69, 0x00, 0x00,							// ,i\0\0
				0xff, 0xff, 0xff, 0xff,							// int32 (-1)
			};

		internal static OscBundle DoubleNestedBundle()
		{
			return new OscBundle(new OscTimeTag(0),
				new OscBundle(new OscTimeTag(0), new OscMessage("/aa", -1)),
				new OscBundle(new OscTimeTag(0), new OscMessage("/aa", -1))
			);
		}

		internal static string DoubleNestedBundleString = "#bundle, 01-01-1900 00:00:00.0000Z, { #bundle, 01-01-1900 00:00:00.0000Z, { /aa, -1 } }, { #bundle, 01-01-1900 00:00:00.0000Z, { /aa, -1 } }";

		
		internal static OscBundle Bundle_MultiLineString()
		{
			StringBuilder sb = new StringBuilder();
			
			sb.AppendLine("THIS TEST IS ON");
			sb.AppendLine("SEPERATE LINES");

			return new OscBundle(OscTimeTag.BaseDate.Add(new TimeSpan(0, 0, 0, 34, 3532)), new OscMessage(null, "/test", sb.ToString()));
		}

		internal static string BundleString_MultiLineString = "#bundle, 01-01-1900 00:00:37.5320Z, { /test, \"THIS TEST IS ON\r\nSEPERATE LINES\r\n\" }";

		internal static string[] Bundles_Good = new string[] { 
			@"#bundle,00:00:34.3532Z,{/test,""\""moop\""""},{/test,""{the doop}""}",
			"#bundle, 00:00:34.3532Z, { /test, 1, 2, 3 }", 
			"#bundle, 00:00:34.3532Z, { /test, 1, 2, 3 }, { /test, 1, 2, 3 }",
			"#bundle,00:00:34.3532Z,{/test,1,2,3},{/test,1,2,3}",
			"#bundle, 00:00:34.3532Z, { /test, \"THIS TEST IS ON\nSEPERATE LINES\" }",
			"#bundle, 00:00:34.3532Z,\r\n{ /test, 1, 2, 3 },\r\n{ /test, 1, 2, 3 }",
			"#bundle, 00:00:34.3532Z, { \r\n/test, 1, 2, 3\r\n}",
			"#bundle, 00:00:34.3532Z, { \r\n/test,\r\n1,\r\n2,\r\n3\r\n}",
			"#bundle, 01-01-1900 00:00:00.0000Z, { #bundle, 01-01-1900 00:01:33.4140Z, }",
			"#bundle, 01-01-1900 00:00:00.0000Z, { #bundle, 01-01-1900 00:01:33.4140Z }",
		};

		//Fixed Issue #3,


		internal static string[] Bundles_Bad = new string[] { 
			", 00:00:34.3532Z, { /test, 1, 2, 3 }", 
			"#bundle 00:00:34.3532Z, { /test, 1, 2, 3 }, { /test, 1, 2, 3 }",
			"#bundle, 00:00:34.3532Z { /test, 1, 2, 3 }, { /test, 1, 2, 3 }",
			"#bundle, 00:00:34.3532Z,  /test, 1, 2, 3 }, { /test, 1, 2, 3 }",
			"#bundle, 00:00:34.3532Z, { /test, 1, 2, 3 , { /test, 1, 2, 3 }",
			"#bundle, 00:00:34.3532Z, { /test, 1, 2, 3 },  /test, 1, 2, 3 }",
			"#bundle, 00:00:34.3532Z, { /test, \"THIS TEST IS ON\r\nSEPERATE LINES",
			"#bundle, 00:00:34.3532Z, { /test, \"THIS TEST IS ON\r\nSEPERATE LINES\" ",
			"#bundle, 00:00:34.3532Z, { /\r\ntest, 1, 2, 3}",
			"#bundle, 01-01-1900 00:00:00.0000Z { #bundle, 01-01-1900 00:01:33.4140Z, }",
			"#bundle, 01-01-1900 00:00:00.0000Z { #bundle, 01-01-1900 00:01:33.4140Z }",
		};

		#endregion 

		#region Are Equal

		internal static void AreEqual(OscBundle expected, OscBundle actual)
		{
			Assert.Equal(expected.Timestamp, actual.Timestamp); // , "Message timestamps do not match");

			AreEqual(expected.ToArray(), actual.ToArray());
		}

		internal static void AreEqual(OscPacket[] expected, OscPacket[] actual)
		{
			Assert.Equal(expected.Length, actual.Length); // , "Number of arguments do not match");

			for (int i = 0; i < actual.Length; i++)
			{								
				AreEqual(expected[i], actual[i]);
			}
		}

		internal static void AreEqual(OscPacket expected, OscPacket actual)
		{
			Assert.Equal(expected.GetType(), actual.GetType()); // , "Packets are not of the same type");

			if (expected is OscMessage)
			{
				AreEqual(expected as OscMessage, actual as OscMessage);
			}
			else if (expected is OscBundle)
			{
				AreEqual(expected as OscBundle, actual as OscBundle);
			}
			else
			{
				Assert.False(true, "Unexpected packet type"); 
			}
		}

		internal static void AreEqual(OscMessage[] expected, OscMessage[] actual)
		{
			Assert.Equal(expected.Length, actual.Length); // , "Number of arguments do not match");

			for (int i = 0; i < actual.Length; i++)
			{
				AreEqual(expected[i], actual[i]); 
			}
		}

		internal static void AreEqual(OscMessage expected, OscMessage actual)
		{
			Assert.Equal(expected.Address, actual.Address); // , "Message addresses do not match");

			AreEqual(expected.ToArray(), actual.ToArray());
		}

		internal static void AreEqual(object[] expected, object[] actual)
		{
			Assert.Equal(expected.Length, actual.Length); // , "Number of arguments do not match");

			for (int i = 0; i < actual.Length; i++)
			{
				Assert.Equal(expected[i].GetType(), actual[i].GetType()); // , "Argument types at index {0} do not match", i);

				if (expected[i] is object[])
				{
					object[] expectedArg = (object[])expected[i];
					object[] actualArg = (object[])actual[i];

					AreEqual(expectedArg, actualArg);
				}
				else if (expected[i] is byte[])
				{
					byte[] expectedArg = (byte[])expected[i];
					byte[] actualArg = (byte[])actual[i];

					AreEqual(expectedArg, actualArg);
				}
				else if (expected[i] is OscColor)
				{
					OscColor expectedArg = (OscColor)expected[i];
					OscColor actualArg = (OscColor)actual[i];

					Assert.Equal(expectedArg.R, actualArg.R); // , "Color arguments at index {0} Red componets do not match", i);
					Assert.Equal(expectedArg.G, actualArg.G); // , "Color arguments at index {0} Green componets do not match", i);
					Assert.Equal(expectedArg.B, actualArg.B); // , "Color arguments at index {0} Blue componets do not match", i);
					Assert.Equal(expectedArg.A, actualArg.A); // , "Color arguments at index {0} Alpha componets do not match", i);
				}
				else
				{
					Assert.Equal(expected[i], actual[i]); // , "Arguments at index {0} do not match", i);
				}
			}
		}

		internal static void AreEqual(byte[] expected, byte[] actual)
		{
			Assert.Equal(expected.Length, actual.Length); // , "Array lengths do not match");

			for (int i = 0; i < expected.Length; i++)
			{
				Assert.True(expected[i] == actual[i], $"Bytes at index {i} do not match");
			}
		}

		internal static void AreEqual(byte[] expected, byte[] actual, long actualLength)
		{
			Assert.Equal(expected.Length, actualLength); // , "Array lengths do not match");

			for (int i = 0; i < expected.Length; i++)
			{
				Assert.True(expected[i].Equals(actual[i]), $"Bytes at index {i} do not match");
			}
		}

		internal static void AreEqual(OscMessage target, string address, int sizeInBytes, params object[] values)
		{
			Assert.Equal(target.Address, address); // , "Addresses do not match");
			Assert.NotNull(target.ToArray()); // , "Arguments are null");

			if (values.Length == 0)
			{
				Assert.True(target.IsEmpty, "Arguments are not empty");
			}
			else
			{
				Assert.False(target.IsEmpty, "Arguments are empty");

				Assert.Equal(target.Count, values.Length); // , "Does not have {0} argument", values.Length);

				for (int i = 0; i < values.Length; i++)
				{
					Assert.True(target[i].Equals(values[i]), $"Argument at index {i} value does not match");
				}
			}

			Assert.Equal(target.SizeInBytes, sizeInBytes); // , "Message size is not correct");
		}

		#endregion

		#region Address Test Data

		internal static string[] Good_AddressPatterns = new string[] 
		{
			"/container_A", 
			"/container_A/method_A", 
			"/0/1/2/3/4", 
			"/container_A/[0-9]", 
			"/container_A/[!0-9]", 
			"/container_A/[abc]", 
			"/container_A/[!abc]", 
			"/container_A/*g", 
			"/container_A/?tr?ng", 
			"/container_A/str?*", 
			"/container_A/str*?", 
			"/container_A/str**", 
			"/container_A/f*ing", 
			"/container_A/f?ing", 
			"/container_A/f?*s", 
			"/container_A/{method_A,method_B}", 
			"/container_A/method_{A,B}", 
			"/container_A/[method]_[A-Z]", 
			"/container_A/[!string]_[0-9]", 
			"/container_A/{method,container}_[A-Z]", 
			"//{method,container}_[A-Z]", 	
			"/container_[Z-A]", 
		};

		internal static string[] Good_AddressPatternMatches = new string[] 
		{
			"/container_A", 
			"/container_A/method_A", 
			"/0/1/2/3/4", 
			"/container_A/3", 
			"/container_A/A", 
			"/container_A/ab", 
			"/container_A/string", 
			"/container_A/string", 
			"/container_A/string", 
			"/container_A/string", 
			"/container_A/string", 
			"/container_A/string", 
			"/container_A/falsethinging", 
			"/container_A/fking", 
			"/container_A/fals", 
			"/container_A/method_B", 
			"/container_A/method_B", 
			"/container_A/method_B", 
			"/container_A/me_hod_3", 
			"/container_A/method_B", 
			"/container_A/container_B/container_C/method_B", 
			"/container_B", 
		};

		internal static string[] Bad_AddressPatterns = new string[] 
		{
			"/", 
			"/0//1", 
			"/0/1/", 
			"/ /1/", 
			"///1/2", 
			"/container A/1/", 
			"/container_A/[0-9]]", 
			"/container_A/[[!0-9]", 
			"/container_A/{{method_A,method_B}", 
			"/container_A/{method_A,method_B}}", 
		};

		#endregion
	}
}