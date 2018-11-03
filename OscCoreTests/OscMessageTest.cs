// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Net;
using OscCore;
using Xunit;

namespace OscCoreTests
{
    /// <summary>
    ///This is a test class for OscMessageTest and is intended
    ///to contain all OscMessageTest Unit Tests
    ///</summary>
    public class OscMessageTest
    {
        [Fact]
        public void BadlyFormedMessage_Address1()
        {
            try
            {
                byte[] data = UnitTestHelper.BadlyFormedMessage_Address1;

                OscMessage actual = OscMessage.Read(data, 0, data.Length);

                Assert.True(false, "Exception not thrown");
            }
            catch (OscException ex)
            {
                Assert.Equal(ex.OscError, OscError.MissingAddress);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void BadlyFormedMessage_Address2()
        {
            try
            {
                byte[] data = UnitTestHelper.BadlyFormedMessage_Address2;

                OscMessage actual = OscMessage.Read(data, 0, data.Length);

                Assert.True(false, "Exception not thrown");
            }
            catch (OscException ex)
            {
                Assert.Equal(ex.OscError, OscError.MissingAddress);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void BadlyFormedMessage_ErrorParsingBlob()
        {
            try
            {
                byte[] data = UnitTestHelper.BadlyFormedMessage_ErrorParsingBlob;

                OscMessage actual = OscMessage.Read(data, 0, data.Length);

                Assert.True(false, "Exception not thrown");
            }
            catch (OscException ex)
            {
                Assert.Equal(ex.OscError, OscError.ErrorParsingBlob);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void BadlyFormedMessage_ErrorParsingBlob2()
        {
            try
            {
                byte[] data = UnitTestHelper.BadlyFormedMessage_ErrorParsingBlob2;

                OscMessage actual = OscMessage.Read(data, 0, data.Length);

                Assert.True(false, "Exception not thrown");
            }
            catch (OscException ex)
            {
                Assert.Equal(ex.OscError, OscError.ErrorParsingBlob);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void BadlyFormedMessage_ErrorParsingDouble()
        {
            try
            {
                byte[] data = UnitTestHelper.BadlyFormedMessage_ErrorParsingDouble;

                OscMessage actual = OscMessage.Read(data, 0, data.Length);

                Assert.True(false, "Exception not thrown");
            }
            catch (OscException ex)
            {
                Assert.Equal(ex.OscError, OscError.ErrorParsingDouble);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void BadlyFormedMessage_ErrorParsingFloat()
        {
            try
            {
                byte[] data = UnitTestHelper.BadlyFormedMessage_ErrorParsingFloat;

                OscMessage actual = OscMessage.Read(data, 0, data.Length);

                Assert.True(false, "Exception not thrown");
            }
            catch (OscException ex)
            {
                Assert.Equal(ex.OscError, OscError.ErrorParsingSingle);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void BadlyFormedMessage_ErrorParsingInt()
        {
            try
            {
                byte[] data = UnitTestHelper.BadlyFormedMessage_ErrorParsingInt;

                OscMessage actual = OscMessage.Read(data, 0, data.Length);

                Assert.True(false, "Exception not thrown");
            }
            catch (OscException ex)
            {
                Assert.Equal(ex.OscError, OscError.ErrorParsingInt32);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void BadlyFormedMessage_ErrorParsingString()
        {
            try
            {
                byte[] data = UnitTestHelper.BadlyFormedMessage_ErrorParsingString;

                OscMessage actual = OscMessage.Read(data, 0, data.Length);

                Assert.True(false, "Exception not thrown");
            }
            catch (OscException ex)
            {
                Assert.Equal(ex.OscError, OscError.ErrorParsingString);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void BadlyFormedMessage_ErrorParsingString2()
        {
            try
            {
                byte[] data = UnitTestHelper.BadlyFormedMessage_ErrorParsingString2;

                OscMessage actual = OscMessage.Read(data, 0, data.Length);

                Assert.True(false, "Exception not thrown");
            }
            catch (OscException ex)
            {
                Assert.Equal(ex.OscError, OscError.ErrorParsingString);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void BadlyFormedMessage_MissingComma()
        {
            try
            {
                byte[] data = UnitTestHelper.BadlyFormedMessage_MissingComma;

                OscMessage actual = OscMessage.Read(data, 0, data.Length);

                Assert.True(false, "Exception not thrown");
            }
            catch (OscException ex)
            {
                Assert.Equal(ex.OscError, OscError.MissingComma);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void BadlyFormedMessage_MissingTypeTag()
        {
            try
            {
                byte[] data = UnitTestHelper.BadlyFormedMessage_MissingTypeTag;

                OscMessage actual = OscMessage.Read(data, 0, data.Length);

                Assert.True(false, "Exception not thrown");
            }
            catch (OscException ex)
            {
                Assert.Equal(ex.OscError, OscError.MissingTypeTag);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void BadlyFormedMessage_PacketLength()
        {
            try
            {
                byte[] data = UnitTestHelper.BadlyFormedMessage_PacketLength;

                OscMessage actual = OscMessage.Read(data, 0, data.Length);

                Assert.True(false, "Exception not thrown");
            }
            catch (OscException ex)
            {
                Assert.Equal(ex.OscError, OscError.InvalidSegmentLength);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }


        [Fact]
        public void BadlyFormedMessage_UnknownArguemntType()
        {
            try
            {
                byte[] data = UnitTestHelper.BadlyFormedMessage_UnknownArguemntType;

                OscMessage actual = OscMessage.Read(data, 0, data.Length);

                Assert.True(false, "Exception not thrown");
            }
            catch (OscException ex)
            {
                Assert.Equal(ex.OscError, OscError.UnknownArguemntType);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Array_Ints()
//        {
//            OscMessage expected = UnitTestHelper.Message_Array_Ints();
//            byte[] bytes = UnitTestHelper.MessageBody_Array_Ints;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Array_Ints2()
//        {
//            OscMessage expected = UnitTestHelper.Message_Array_Ints2();
//            byte[] bytes = UnitTestHelper.MessageBody_Array_Ints2;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Array_NestedInts()
//        {
//            OscMessage expected = UnitTestHelper.Message_Array_NestedInts();
//            byte[] bytes = UnitTestHelper.MessageBody_Array_NestedInts;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Blob()
//        {
//            OscMessage expected = UnitTestHelper.Message_Blob();
//            byte[] bytes = UnitTestHelper.MessageBody_Blob;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Char()
//        {
//            OscMessage expected = UnitTestHelper.Message_Char();
//            byte[] bytes = UnitTestHelper.MessageBody_Char;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Color_Blue()
//        {
//            OscMessage expected = UnitTestHelper.Message_Color_Blue();
//            byte[] bytes = UnitTestHelper.MessageBody_Color_Blue;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Color_Green()
//        {
//            OscMessage expected = UnitTestHelper.Message_Color_Green();
//            byte[] bytes = UnitTestHelper.MessageBody_Color_Green;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Color_Red()
//        {
//            OscMessage expected = UnitTestHelper.Message_Color_Red();
//            byte[] bytes = UnitTestHelper.MessageBody_Color_Red;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Color_Transparent()
//        {
//            OscMessage expected = UnitTestHelper.Message_Color_Transparent();
//            byte[] bytes = UnitTestHelper.MessageBody_Color_Transparent;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Double()
//        {
//            OscMessage expected = UnitTestHelper.Message_Double();
//            byte[] bytes = UnitTestHelper.MessageBody_Double;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_False()
//        {
//            OscMessage expected = UnitTestHelper.Message_False();
//            byte[] bytes = UnitTestHelper.MessageBody_False;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Float()
//        {
//            OscMessage expected = UnitTestHelper.Message_Float();
//            byte[] bytes = UnitTestHelper.MessageBody_Float;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Float2()
//        {
//            OscMessage expected = UnitTestHelper.Message_Float2();
//            byte[] bytes = UnitTestHelper.MessageBody_Float2;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Float3()
//        {
//            OscMessage expected = UnitTestHelper.Message_Float3();
//            byte[] bytes = UnitTestHelper.MessageBody_Float3;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Infinitum()
//        {
//            OscMessage expected = UnitTestHelper.Message_Infinitum();
//            byte[] bytes = UnitTestHelper.MessageBody_Infinitum;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Int()
//        {
//            OscMessage expected = UnitTestHelper.Message_Int();
//            byte[] bytes = UnitTestHelper.MessageBody_Int;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Long()
//        {
//            OscMessage expected = UnitTestHelper.Message_Long();
//            byte[] bytes = UnitTestHelper.MessageBody_Long;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Midi()
//        {
//            OscMessage expected = UnitTestHelper.Message_Midi();
//            byte[] bytes = UnitTestHelper.MessageBody_Midi;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Nil()
//        {
//            OscMessage expected = UnitTestHelper.Message_Nil();
//            byte[] bytes = UnitTestHelper.MessageBody_Nil;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_String()
//        {
//            OscMessage expected = UnitTestHelper.Message_String();
//            byte[] bytes = UnitTestHelper.MessageBody_String;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_Symbol()
//        {
//            OscMessage expected = UnitTestHelper.Message_Symbol();
//            byte[] bytes = UnitTestHelper.MessageBody_Symbol;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_TimeTag()
//        {
//            OscMessage expected = UnitTestHelper.Message_TimeTag();
//            byte[] bytes = UnitTestHelper.MessageBody_TimeTag;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }
//
//        /// <summary>
//        ///A test for Equals
//        ///</summary>
//        [Fact]
//        public void EqualsTest_True()
//        {
//            OscMessage expected = UnitTestHelper.Message_True();
//            byte[] bytes = UnitTestHelper.MessageBody_True;
//
//            int count = bytes.Length;
//            OscMessage actual;
//
//            actual = OscMessage.Read(bytes, 0, count);
//
//            Assert.True(actual.Equals(expected));
//        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Array_Ints()
        {
            OscMessage target = UnitTestHelper.Message_Array_Ints();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Array_Ints.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Array_Ints2()
        {
            OscMessage target = UnitTestHelper.Message_Array_Ints2();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Array_Ints2.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Array_NestedInts()
        {
            OscMessage target = UnitTestHelper.Message_Array_NestedInts();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Array_NestedInts.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Blob()
        {
            OscMessage target = UnitTestHelper.Message_Blob();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Blob.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Char()
        {
            OscMessage target = UnitTestHelper.Message_Char();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Char.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Color_Blue()
        {
            OscMessage target = UnitTestHelper.Message_Color_Blue();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Color_Blue.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Color_Green()
        {
            OscMessage target = UnitTestHelper.Message_Color_Green();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Color_Green.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Color_Red()
        {
            OscMessage target = UnitTestHelper.Message_Color_Red();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Color_Red.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Color_Transparent()
        {
            OscMessage target = UnitTestHelper.Message_Color_Transparent();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Color_Transparent.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Double()
        {
            OscMessage target = UnitTestHelper.Message_Double();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Double.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_False()
        {
            OscMessage target = UnitTestHelper.Message_False();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_False.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Float()
        {
            OscMessage target = UnitTestHelper.Message_Float();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Float.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Float2()
        {
            OscMessage target = UnitTestHelper.Message_Float2();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Float2.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Float3()
        {
            OscMessage target = UnitTestHelper.Message_Float3();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Float3.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Infinitum()
        {
            OscMessage target = UnitTestHelper.Message_Infinitum();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Infinitum.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Int()
        {
            OscMessage target = UnitTestHelper.Message_Int();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Int.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Long()
        {
            OscMessage target = UnitTestHelper.Message_Long();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Long.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Midi()
        {
            OscMessage target = UnitTestHelper.Message_Midi();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Midi.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Nil()
        {
            OscMessage target = UnitTestHelper.Message_Nil();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Nil.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_String()
        {
            OscMessage target = UnitTestHelper.Message_String();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_String.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_Symbol()
        {
            OscMessage target = UnitTestHelper.Message_Symbol();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_Symbol.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_TimeTag()
        {
            OscMessage target = UnitTestHelper.Message_TimeTag();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_TimeTag.Length);
        }

        /// <summary>
        ///A test for MessageSize
        ///</summary>
        [Fact]
        public void MessageSizeTest_True()
        {
            OscMessage target = UnitTestHelper.Message_True();
            Assert.Equal(target.SizeInBytes, UnitTestHelper.MessageBody_True.Length);
        }

        /// <summary>
        ///A test for OscMessage Constructor
        ///</summary>
        [Fact]
        ////[DeploymentItem("Rug.Osc.dll")]
        public void OscMessageConstructorTest_EmptyArgs()
        {
            string address = "/test";

            OscMessage target = new OscMessage(address);

            UnitTestHelper.AreEqual(target, address, 12);
        }

        /// <summary>
        ///A test for OscMessage Constructor
        ///</summary>
        [Fact]
        //[DeploymentItem("Rug.Osc.dll")]
        public void OscMessageConstructorTest_NoAddress()
        {
            string address = null;

            try
            {
                OscMessage target = new OscMessage(address);

                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.IsAssignableFrom(typeof(ArgumentNullException), ex);
            }
        }

        /// <summary>
        ///A test for OscMessage Constructor
        ///</summary>
        [Fact]
        //[DeploymentItem("Rug.Osc.dll")]
        public void OscMessageConstructorTest_NullArg()
        {
            string address = "/test";

            try
            {
                OscMessage target = new OscMessage(address, new object[] {null});

                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.IsAssignableFrom(typeof(ArgumentException), ex);
            }
        }

        [Fact]
        //[DeploymentItem("Rug.Osc.dll")]
        public void OscMessageConstructorTest_OriginAddressArgs()
        {
            Uri ipEndPoint = new Uri($"udp://{IPAddress.Loopback}:{8001}");

            string address = "/test";
            byte[] value = {4, 2};

            OscMessage target = new OscMessage(ipEndPoint, address, value);

            UnitTestHelper.AreEqual(target, address, 20, value);

            Assert.Equal(ipEndPoint, target.Origin); //, "Supplied IP Endpoint and resolved IP Endpoint do not match."); 
        }

        [Fact]
        //[DeploymentItem("Rug.Osc.dll")]
        public void OscMessageConstructorTest_OriginAddressArgs_InvalidAddress()
        {
            Uri ipEndPoint = new Uri($"udp://{IPAddress.Loopback}:{8001}");

            string address = "test";
            byte[] value = {4, 2};

            try
            {
                OscMessage target = new OscMessage(ipEndPoint, address, value);

                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.IsAssignableFrom(typeof(ArgumentException), ex);
            }
        }

        [Fact]
        //[DeploymentItem("Rug.Osc.dll")]
        public void OscMessageConstructorTest_OriginAddressArgs_NoAddress()
        {
            Uri ipEndPoint = new Uri($"udp://{IPAddress.Loopback}:{8001}");

            string address = null;
            byte[] value = {4, 2};

            try
            {
                OscMessage target = new OscMessage(ipEndPoint, address, value);

                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.IsAssignableFrom(typeof(ArgumentNullException), ex);
            }
        }

        /// <summary>
        ///A test for OscMessage Constructor
        ///</summary>
        [Fact]
        //[DeploymentItem("Rug.Osc.dll")]
        public void OscMessageConstructorTest_SingleArg_Blob()
        {
            string address = "/test";
            byte[] value = {4, 2};

            OscMessage target = new OscMessage(address, value);

            UnitTestHelper.AreEqual(target, address, 20, value);
        }

        /// <summary>
        ///A test for OscMessage Constructor
        ///</summary>
        [Fact]
        ////[DeploymentItem("Rug.Osc.dll")]
        public void OscMessageConstructorTest_SingleArg_Float()
        {
            string address = "/test";
            float value = 42;
            OscMessage target = new OscMessage(address, value);

            UnitTestHelper.AreEqual(target, address, 16, value);
        }

        /// <summary>
        ///A test for OscMessage Constructor
        ///</summary>
        [Fact]
        ////[DeploymentItem("Rug.Osc.dll")]
        public void OscMessageConstructorTest_SingleArg_Int()
        {
            string address = "/test";
            int value = 42;
            OscMessage target = new OscMessage(address, value);

            UnitTestHelper.AreEqual(target, address, 16, value);
        }

        /// <summary>
        ///A test for OscMessage Constructor
        ///</summary>
        [Fact]
        //[DeploymentItem("Rug.Osc.dll")]
        public void OscMessageConstructorTest_SingleArg_String()
        {
            string address = "/test";
            string value = "42";
            OscMessage target = new OscMessage(address, value);

            UnitTestHelper.AreEqual(target, address, 16, value);
        }


        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Array_Ints()
        {
            string str = UnitTestHelper.MessageString_Array_Ints;
            OscMessage expected = UnitTestHelper.Message_Array_Ints();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Array_Ints2()
        {
            string str = UnitTestHelper.MessageString_Array_Ints2;
            OscMessage expected = UnitTestHelper.Message_Array_Ints2();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Array_NestedInts()
        {
            string str = UnitTestHelper.MessageString_Array_NestedInts;
            OscMessage expected = UnitTestHelper.Message_Array_NestedInts();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Blob_Array()
        {
            string str = UnitTestHelper.MessageString_Blob_Array;
            OscMessage expected = UnitTestHelper.Message_Blob();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Blob_Base64()
        {
            string str = UnitTestHelper.MessageString_Blob_Base64;
            OscMessage expected = UnitTestHelper.Message_Blob();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Blob_Hex()
        {
            string str = UnitTestHelper.MessageString_Blob_Hex;
            OscMessage expected = UnitTestHelper.Message_Blob();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Char()
        {
            string str = UnitTestHelper.MessageString_Char;
            OscMessage expected = UnitTestHelper.Message_Char();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Double()
        {
            string str = UnitTestHelper.MessageString_Double;
            OscMessage expected = UnitTestHelper.Message_Double();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_False()
        {
            string str = UnitTestHelper.MessageString_False;
            OscMessage expected = UnitTestHelper.Message_False();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Float()
        {
            string str = UnitTestHelper.MessageString_Float;
            OscMessage expected = UnitTestHelper.Message_Float();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Float_NaN()
        {
            string str = UnitTestHelper.MessageString_Float_NaN;
            OscMessage expected = UnitTestHelper.Message_Float_NaN();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Float_NegativeInfinity()
        {
            string str = UnitTestHelper.MessageString_Float_NegativeInfinity;
            OscMessage expected = UnitTestHelper.Message_Float_NegativeInfinity();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Float_PositiveInfinity()
        {
            string str = UnitTestHelper.MessageString_Float_PositiveInfinity;
            OscMessage expected = UnitTestHelper.Message_Float_PositiveInfinity();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Float2()
        {
            string str = UnitTestHelper.MessageString_Float2;
            OscMessage expected = UnitTestHelper.Message_Float2();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Float3()
        {
            string str = UnitTestHelper.MessageString_Float3;
            OscMessage expected = UnitTestHelper.Message_Float3();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Infinitum()
        {
            string str = UnitTestHelper.MessageString_Infinitum;
            OscMessage expected = UnitTestHelper.Message_Infinitum();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Int()
        {
            string str = UnitTestHelper.MessageString_Int;
            OscMessage expected = UnitTestHelper.Message_Int();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Long()
        {
            string str = UnitTestHelper.MessageString_Long;
            OscMessage expected = UnitTestHelper.Message_Long();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Midi()
        {
            string str = UnitTestHelper.MessageString_Midi;
            OscMessage expected = UnitTestHelper.Message_Midi();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Nil()
        {
            string str = UnitTestHelper.MessageString_Nil;
            OscMessage expected = UnitTestHelper.Message_Nil();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_String()
        {
            string str = UnitTestHelper.MessageString_String;
            OscMessage expected = UnitTestHelper.Message_String();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Symbol()
        {
            string str = UnitTestHelper.MessageString_Symbol;
            OscMessage expected = UnitTestHelper.Message_Symbol();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_Symbol2()
        {
            string str = UnitTestHelper.MessageString_Symbol2;
            OscMessage expected = UnitTestHelper.Message_Symbol();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_True()
        {
            string str = UnitTestHelper.MessageString_True;
            OscMessage expected = UnitTestHelper.Message_True();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void ReadTest_Array_Ints()
        {
            OscMessage expected = UnitTestHelper.Message_Array_Ints();
            byte[] bytes = UnitTestHelper.MessageBody_Array_Ints;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void ReadTest_Array_Ints2()
        {
            OscMessage expected = UnitTestHelper.Message_Array_Ints2();
            byte[] bytes = UnitTestHelper.MessageBody_Array_Ints2;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void ReadTest_Array_NestedInts()
        {
            OscMessage expected = UnitTestHelper.Message_Array_NestedInts();
            byte[] bytes = UnitTestHelper.MessageBody_Array_NestedInts;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Blob()
        {
            OscMessage expected = UnitTestHelper.Message_Blob();
            byte[] bytes = UnitTestHelper.MessageBody_Blob;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Blob2()
        {
            OscMessage expected = UnitTestHelper.Message_Blob2();
            byte[] bytes = UnitTestHelper.MessageBody_Blob2;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Char()
        {
            OscMessage expected = UnitTestHelper.Message_Char();
            byte[] bytes = UnitTestHelper.MessageBody_Char;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Color_Blue()
        {
            OscMessage expected = UnitTestHelper.Message_Color_Blue();
            byte[] bytes = UnitTestHelper.MessageBody_Color_Blue;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Color_Green()
        {
            OscMessage expected = UnitTestHelper.Message_Color_Green();
            byte[] bytes = UnitTestHelper.MessageBody_Color_Green;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Color_Red()
        {
            OscMessage expected = UnitTestHelper.Message_Color_Red();
            byte[] bytes = UnitTestHelper.MessageBody_Color_Red;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Color_Transparent()
        {
            OscMessage expected = UnitTestHelper.Message_Color_Transparent();
            byte[] bytes = UnitTestHelper.MessageBody_Color_Transparent;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Double()
        {
            OscMessage expected = UnitTestHelper.Message_Double();
            byte[] bytes = UnitTestHelper.MessageBody_Double;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_False()
        {
            OscMessage expected = UnitTestHelper.Message_False();
            byte[] bytes = UnitTestHelper.MessageBody_False;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Float()
        {
            OscMessage expected = UnitTestHelper.Message_Float();
            byte[] bytes = UnitTestHelper.MessageBody_Float;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Float2()
        {
            OscMessage expected = UnitTestHelper.Message_Float2();
            byte[] bytes = UnitTestHelper.MessageBody_Float2;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Float3()
        {
            OscMessage expected = UnitTestHelper.Message_Float3();
            byte[] bytes = UnitTestHelper.MessageBody_Float3;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Infinitum()
        {
            OscMessage expected = UnitTestHelper.Message_Infinitum();
            byte[] bytes = UnitTestHelper.MessageBody_Infinitum;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Int()
        {
            OscMessage expected = UnitTestHelper.Message_Int();
            byte[] bytes = UnitTestHelper.MessageBody_Int;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Long()
        {
            OscMessage expected = UnitTestHelper.Message_Long();
            byte[] bytes = UnitTestHelper.MessageBody_Long;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Midi()
        {
            OscMessage expected = UnitTestHelper.Message_Midi();
            byte[] bytes = UnitTestHelper.MessageBody_Midi;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Nil()
        {
            OscMessage expected = UnitTestHelper.Message_Nil();
            byte[] bytes = UnitTestHelper.MessageBody_Nil;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_String()
        {
            OscMessage expected = UnitTestHelper.Message_String();
            byte[] bytes = UnitTestHelper.MessageBody_String;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Symbol()
        {
            OscMessage expected = UnitTestHelper.Message_Symbol();
            byte[] bytes = UnitTestHelper.MessageBody_Symbol;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_TimeTag()
        {
            OscMessage expected = UnitTestHelper.Message_TimeTag();
            byte[] bytes = UnitTestHelper.MessageBody_TimeTag;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_True()
        {
            OscMessage expected = UnitTestHelper.Message_True();
            byte[] bytes = UnitTestHelper.MessageBody_True;

            int count = bytes.Length;
            OscMessage actual;

            actual = OscMessage.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [Fact]
        public void ToStringTest_Float_NaN()
        {
            OscMessage message = UnitTestHelper.Message_Float_NaN();
            string expected = UnitTestHelper.MessageString_Float_NaN;
            string actual;
            actual = message.ToString();
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [Fact]
        public void ToStringTest_Float_NegativeInfinity()
        {
            OscMessage message = UnitTestHelper.Message_Float_NegativeInfinity();
            string expected = UnitTestHelper.MessageString_Float_NegativeInfinity;
            string actual;
            actual = message.ToString();
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [Fact]
        public void ToStringTest_Float_PositiveInfinity()
        {
            OscMessage message = UnitTestHelper.Message_Float_PositiveInfinity();
            string expected = UnitTestHelper.MessageString_Float_PositiveInfinity;
            string actual;
            actual = message.ToString();
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Array_Ints()
        {
            OscMessage target = UnitTestHelper.Message_Array_Ints();
            byte[] expectedData = UnitTestHelper.MessageBody_Array_Ints;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Array_Ints2()
        {
            OscMessage target = UnitTestHelper.Message_Array_Ints2();
            byte[] expectedData = UnitTestHelper.MessageBody_Array_Ints2;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Array_NestedInts()
        {
            OscMessage target = UnitTestHelper.Message_Array_NestedInts();
            byte[] expectedData = UnitTestHelper.MessageBody_Array_NestedInts;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Blob_1()
        {
            OscMessage target = UnitTestHelper.Message_Blob();

            byte[] data = new byte[UnitTestHelper.MessageBody_Blob.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Blob;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Blob_2()
        {
            OscMessage target = UnitTestHelper.Message_Blob();
            byte[] expectedData = UnitTestHelper.MessageBody_Blob;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Char_1()
        {
            OscMessage target = UnitTestHelper.Message_Char();

            byte[] data = new byte[UnitTestHelper.MessageBody_Char.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Char;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Char_2()
        {
            OscMessage target = UnitTestHelper.Message_Char();
            byte[] expectedData = UnitTestHelper.MessageBody_Char;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Color_Blue_1()
        {
            OscMessage target = UnitTestHelper.Message_Color_Blue();

            byte[] data = new byte[UnitTestHelper.MessageBody_Color_Blue.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Color_Blue;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Color_Blue_2()
        {
            OscMessage target = UnitTestHelper.Message_Color_Blue();
            byte[] expectedData = UnitTestHelper.MessageBody_Color_Blue;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Color_Green_1()
        {
            OscMessage target = UnitTestHelper.Message_Color_Green();

            byte[] data = new byte[UnitTestHelper.MessageBody_Color_Green.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Color_Green;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Color_Green_2()
        {
            OscMessage target = UnitTestHelper.Message_Color_Green();
            byte[] expectedData = UnitTestHelper.MessageBody_Color_Green;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Color_Red_1()
        {
            OscMessage target = UnitTestHelper.Message_Color_Red();

            byte[] data = new byte[UnitTestHelper.MessageBody_Color_Red.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Color_Red;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Color_Red_2()
        {
            OscMessage target = UnitTestHelper.Message_Color_Red();
            byte[] expectedData = UnitTestHelper.MessageBody_Color_Red;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Color_Transparent_1()
        {
            OscMessage target = UnitTestHelper.Message_Color_Transparent();

            byte[] data = new byte[UnitTestHelper.MessageBody_Color_Transparent.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Color_Transparent;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Color_Transparent_2()
        {
            OscMessage target = UnitTestHelper.Message_Color_Transparent();
            byte[] expectedData = UnitTestHelper.MessageBody_Color_Transparent;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Double_1()
        {
            OscMessage target = UnitTestHelper.Message_Double();

            byte[] data = new byte[UnitTestHelper.MessageBody_Double.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Double;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Double_2()
        {
            OscMessage target = UnitTestHelper.Message_Double();
            byte[] expectedData = UnitTestHelper.MessageBody_Double;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_False_1()
        {
            OscMessage target = UnitTestHelper.Message_False();

            byte[] data = new byte[UnitTestHelper.MessageBody_False.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_False;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_False_2()
        {
            OscMessage target = UnitTestHelper.Message_False();
            byte[] expectedData = UnitTestHelper.MessageBody_False;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Float_1()
        {
            OscMessage target = UnitTestHelper.Message_Float();

            byte[] data = new byte[UnitTestHelper.MessageBody_Float.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Float;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Float_2()
        {
            OscMessage target = UnitTestHelper.Message_Float();
            byte[] expectedData = UnitTestHelper.MessageBody_Float;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Float2()
        {
            OscMessage target = UnitTestHelper.Message_Float2();

            byte[] data = new byte[UnitTestHelper.MessageBody_Float2.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Float2;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Float2_2()
        {
            OscMessage target = UnitTestHelper.Message_Float2();
            byte[] expectedData = UnitTestHelper.MessageBody_Float2;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Float3()
        {
            OscMessage target = UnitTestHelper.Message_Float3();

            byte[] data = new byte[UnitTestHelper.MessageBody_Float3.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Float3;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Float3_2()
        {
            OscMessage target = UnitTestHelper.Message_Float3();
            byte[] expectedData = UnitTestHelper.MessageBody_Float3;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Infinitum_1()
        {
            OscMessage target = UnitTestHelper.Message_Infinitum();

            byte[] data = new byte[UnitTestHelper.MessageBody_Infinitum.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Infinitum;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Infinitum_2()
        {
            OscMessage target = UnitTestHelper.Message_Infinitum();
            byte[] expectedData = UnitTestHelper.MessageBody_Infinitum;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Int_1()
        {
            OscMessage target = UnitTestHelper.Message_Int();

            byte[] data = new byte[UnitTestHelper.MessageBody_Int.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Int;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Int_2()
        {
            OscMessage target = UnitTestHelper.Message_Int();
            byte[] expectedData = UnitTestHelper.MessageBody_Int;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Long_1()
        {
            OscMessage target = UnitTestHelper.Message_Long();

            byte[] data = new byte[UnitTestHelper.MessageBody_Long.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Long;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Long_2()
        {
            OscMessage target = UnitTestHelper.Message_Long();
            byte[] expectedData = UnitTestHelper.MessageBody_Long;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Midi_1()
        {
            OscMessage target = UnitTestHelper.Message_Midi();

            byte[] data = new byte[UnitTestHelper.MessageBody_Midi.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Midi;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Midi_2()
        {
            OscMessage target = UnitTestHelper.Message_Midi();
            byte[] expectedData = UnitTestHelper.MessageBody_Midi;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Nil_1()
        {
            OscMessage target = UnitTestHelper.Message_Nil();

            byte[] data = new byte[UnitTestHelper.MessageBody_Nil.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Nil;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Nil_2()
        {
            OscMessage target = UnitTestHelper.Message_Nil();
            byte[] expectedData = UnitTestHelper.MessageBody_Nil;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_String_1()
        {
            OscMessage target = UnitTestHelper.Message_String();

            byte[] data = new byte[UnitTestHelper.MessageBody_String.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_String;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_String_2()
        {
            OscMessage target = UnitTestHelper.Message_String();
            byte[] expectedData = UnitTestHelper.MessageBody_String;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Symbol_1()
        {
            OscMessage target = UnitTestHelper.Message_Symbol();

            byte[] data = new byte[UnitTestHelper.MessageBody_Symbol.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_Symbol;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_Symbol_2()
        {
            OscMessage target = UnitTestHelper.Message_Symbol();
            byte[] expectedData = UnitTestHelper.MessageBody_Symbol;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_TimeTag_1()
        {
            OscMessage target = UnitTestHelper.Message_TimeTag();

            byte[] data = new byte[UnitTestHelper.MessageBody_TimeTag.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_TimeTag;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_TimeTag_2()
        {
            OscMessage target = UnitTestHelper.Message_TimeTag();
            byte[] expectedData = UnitTestHelper.MessageBody_TimeTag;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_True_1()
        {
            OscMessage target = UnitTestHelper.Message_True();

            byte[] data = new byte[UnitTestHelper.MessageBody_True.Length];
            byte[] expectedData = UnitTestHelper.MessageBody_True;

            int actual;
            actual = target.Write(data, 0);

            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }


        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void WriteTest_True_2()
        {
            OscMessage target = UnitTestHelper.Message_True();
            byte[] expectedData = UnitTestHelper.MessageBody_True;

            int actual;
            byte[] data;
            data = target.ToByteArray();
            actual = data.Length;


            Assert.Equal(expectedData.Length, actual);
            UnitTestHelper.AreEqual(expectedData, data);
        }
    }
}