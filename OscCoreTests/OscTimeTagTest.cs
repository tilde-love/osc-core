// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using OscCore;
using Xunit;

namespace OscCoreTests
{
    /// <summary>
    ///This is a test class for OscTimeTagTest and is intended
    ///to contain all OscTimeTagTest Unit Tests
    ///</summary>
    public class OscTimeTagTest
    {
        /// <summary>
        ///A test for FromDataTime
        ///</summary>
        [Fact]
        public void FromDataTimeTest()
        {
            DateTime datetime = new DateTime(632413223390120000, DateTimeKind.Utc);
            OscTimeTag expected = new OscTimeTag(14236589681638796952);
            OscTimeTag actual;
            actual = OscTimeTag.FromDataTime(datetime);
            
            Assert.True(expected.Value <= actual.Value + 1 && expected.Value >= actual.Value - 1);
        }


        /// <summary>
        ///A test for OscTimeTag Constructor
        ///</summary>
        [Fact]
        public void OscTimeTagConstructorTest()
        {
            DateTime expected = new DateTime(632413223390120000, DateTimeKind.Utc);
            ulong value = 14236589681638796952;
            OscTimeTag target = new OscTimeTag(value);

            DateTime datetime = target.ToDataTime();

            string valueString = datetime.ToString("dd/MM/yyyy HH:mm:ss") + " " + datetime.Millisecond;
            string expectedString = expected.ToString("dd/MM/yyyy HH:mm:ss") + " " + datetime.Millisecond;

            Assert.Equal(expectedString, valueString); // , "Date resolved to '{0}'", valueString);
        }

        /// <summary>
        ///A test for ToDataTime
        ///</summary>
        [Fact]
        public void ToDataTimeTest()
        {
            OscTimeTag target = new OscTimeTag(14236589681638796952);
            DateTime expected = new DateTime(632413223390120000, DateTimeKind.Utc);
            DateTime actual;
            actual = target.ToDataTime();
            Assert.Equal(expected, actual);
        }
    }
}