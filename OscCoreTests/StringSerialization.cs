// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using OscCore;
using Xunit;

namespace OscCoreTests
{
    public class StringSerialization
    {     
        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ToStringTest_NestedQuoteString()
        {
            string expected = UnitTestHelper.MessageString_NestedQuoteString;
            OscMessage message = UnitTestHelper.Message_NestedQuoteString();
            string actual;
            actual = message.ToString();
            Assert.Equal(expected, actual);
        }
        
        /// <summary>
        ///A test for Parse
        ///</summary>
        [Fact]
        public void ParseTest_NestedQuoteString()
        {
            string str = UnitTestHelper.MessageString_NestedQuoteString;
            OscMessage expected = UnitTestHelper.Message_NestedQuoteString();
            OscMessage actual;
            actual = OscMessage.Parse(str);
            UnitTestHelper.AreEqual(expected, actual);
        }
    }
}