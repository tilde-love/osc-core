// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using OscCore;
using Xunit;

namespace OscCoreTests
{
    /// <summary>
    /// This is a test class for OscBundleTest and is intended
    /// to contain all OscBundleTest Unit Tests
    /// </summary>
    public class OscBundleTest
    {
        [Fact]
        public void Nested_ParseTest()
        {
            string str = UnitTestHelper.DoubleNestedBundleString;
            OscBundle expected = UnitTestHelper.DoubleNestedBundle();
            OscBundle actual;
            actual = OscBundle.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void Nested_ReadTest()
        {
            OscBundle expected = UnitTestHelper.DoubleNestedBundle();
            byte[] bytes = UnitTestHelper.DoubleNestedBundleBody;

            int count = bytes.Length;
            OscBundle actual;

            actual = OscBundle.Read(bytes, 0, count);

            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToByteArray
        ///</summary>
        [Fact]
        public void Nested_ToArrayTest()
        {
            OscBundle target = UnitTestHelper.DoubleNestedBundle();
            byte[] expected = UnitTestHelper.DoubleNestedBundleBody;
            byte[] actual;
            actual = target.ToByteArray();

            Assert.Equal(expected.Length, actual.Length);
            UnitTestHelper.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [Fact]
        public void Nested_ToStringTest()
        {
            OscBundle target = UnitTestHelper.DoubleNestedBundle();
            string expected = UnitTestHelper.DoubleNestedBundleString;
            string actual;
            actual = target.ToString();
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Fact]
        public void Nested_WriteTest()
        {
            OscBundle target = UnitTestHelper.DoubleNestedBundle();
            byte[] data = new byte[UnitTestHelper.DoubleNestedBundleBody.Length];
            int index = 0;
            int expected = UnitTestHelper.DoubleNestedBundleBody.Length;
            int actual;
            actual = target.Write(data, index);

            Assert.Equal(expected, actual);
            UnitTestHelper.AreEqual(data, UnitTestHelper.DoubleNestedBundleBody);
        }

        /// <summary>
        ///A test for OscBundle Constructor
        ///</summary>
        [Fact]
        public void OscBundleConstructorTest()
        {
            OscTimeTag timestamp = new OscTimeTag(14236589681638796952);
            OscMessage[] messages = {UnitTestHelper.Message_Array_Ints(), UnitTestHelper.Message_Array_Ints()};
            OscBundle target = new OscBundle(timestamp, messages);

            Assert.Equal(timestamp, target.Timestamp);
            UnitTestHelper.AreEqual(messages, target.ToArray());
        }

        [Fact]
        public void OscBundleManyMessagesTest_1()
        {
            OscBundle target = OscBundle.Parse("#bundle, 0, { /ping }, { /moop }, { /ping }, { /ping }, { /ping }");
            OscBundle expected = new OscBundle(new OscTimeTag(0),
                new OscMessage("/ping"), new OscMessage("/moop"), new OscMessage("/ping"), new OscMessage("/ping"), new OscMessage("/ping"));

            UnitTestHelper.AreEqual(target, expected);
        }

        [Fact]
        public void OscBundleManyMessagesTest_2()
        {
            OscBundle target = OscBundle.Parse("#bundle, 0, { /ping }, { /moop }, { /ping }, { /ping }, { /ping }");
            OscBundle expected = new OscBundle(new OscTimeTag(0),
                new OscMessage("/ping"), new OscMessage("/moop"), new OscMessage("/ping"), new OscMessage("/ping"), new OscMessage("/ping"));

            byte[] targetBytes = target.ToByteArray();

            OscBundle actual = OscBundle.Read(targetBytes, 0, targetBytes.Length);

            UnitTestHelper.AreEqual(actual, expected);
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest()
        {
            OscTimeTag timestamp = new OscTimeTag(14236589681638796952);
            OscMessage[] messages = {UnitTestHelper.Message_Array_Ints(), UnitTestHelper.Message_Array_Ints()};
            OscBundle expected = new OscBundle(timestamp, messages);

            byte[] bytes = expected.ToByteArray();
            int index = 0;
            int count = bytes.Length;            
            OscBundle actual;
            
            actual = OscBundle.Read(bytes, index, count);
            
            UnitTestHelper.AreEqual(expected, actual);

            //Assert.True(actual.Equals(expected));
        }
        
        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadOffsetTest()
        {
            Random random = new Random();
            
            for (int i = 0; i < 1000; i++)
            {
                OscTimeTag timestamp = new OscTimeTag(14236589681638796952);
                
                List<OscPacket> messages = new List<OscPacket>();

                for (int j = 0; j < 10; j++)
                {
                    messages.Add(new OscMessage("/" + j, (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble() ));
                }

                OscBundle expected = new OscBundle(timestamp, messages.ToArray());

                int index = random.Next(1, 32); 
                int endPadding = random.Next(0, 32);
                int count = expected.SizeInBytes; 
                
                byte[] bytes = new byte[count + index + endPadding]; 
                
                random.NextBytes(bytes);
                
                expected.ToByteArray().CopyTo(bytes, index);
                
                OscBundle actual;

                actual = OscBundle.Read(bytes, index, count);

                UnitTestHelper.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Bad_ToLong()
        {
            try
            {
                byte[] bytes =
                {
                    // #bundle
                    35, 98, 117, 110, 100, 108, 101, 0,

                    // Time-tag
                    197, 146, 134, 227, 3, 18, 110, 152,

                    // length
                    0, 0, 0, 64, // 32,

                    // message body
                    47, 116, 101, 115, 116, 0, 0, 0,
                    44, 105, 91, 105, 105, 105, 93, 0,

                    26, 42, 58, 74, 26, 42, 58, 74,
                    90, 106, 122, 138, 154, 170, 186, 202
                };

                int index = 0;
                int count = bytes.Length;
                OscBundle actual;
                actual = OscBundle.Read(bytes, index, count);

                Assert.True(false, "Exception not thrown");
            }
            catch (OscException ex)
            {
                Assert.Equal(ex.OscError, OscError.InvalidBundleMessageLength);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [Fact]
        public void ReadTest_Bad_ToShort()
        {
            try
            {
                byte[] bytes =
                {
                    // #bundle
                    35, 98, 117, 110, 100, 108, 101, 0,

                    // Time-tag
                    197, 146, 134, 227, 3, 18, 110, 152,

                    // length
                    0, 0, 0, 24, // 32,

                    // message body
                    47, 116, 101, 115, 116, 0, 0, 0,
                    44, 105, 91, 105, 105, 105, 93, 0,

                    26, 42, 58, 74, 26, 42, 58, 74,
                    90, 106, 122, 138, 154, 170, 186, 202
                };

                int index = 0;
                int count = bytes.Length;
                OscBundle actual;
                actual = OscBundle.Read(bytes, index, count);

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

        /// <summary>
        ///A test for TryParse
        ///</summary>
        [Fact]
        public void TryParseTest_Bad()
        {
            bool expected = false;
            foreach (string str in UnitTestHelper.Bundles_Bad)
            {
                OscBundle bundle = null;

                bool actual;
                actual = OscBundle.TryParse(str, out bundle);
	            
                Assert.True(expected == actual, $"While parsing bad bundle '{str}'");
            }
        }

        /// <summary>
        ///A test for TryParse
        ///</summary>
        [Fact]
        public void TryParseTest_Good()
        {
            bool expected = true;
            foreach (string str in UnitTestHelper.Bundles_Good)
            {
                OscBundle bundle = null;

                bool actual;
                actual = OscBundle.TryParse(str, out bundle);
	            
                Assert.True(expected == actual, $"While parsing good bundle '{str}'");
            }
        }
    }
}