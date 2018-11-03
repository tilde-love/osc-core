// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using OscCore;
using Xunit;

namespace OscCoreTests
{
    /// <summary>
    /// This is a test class for OscColorTest and is intended
    /// to contain all OscColorTest Unit Tests
    ///</summary>
    public class OscColorTest
    {
        /// <summary>
        ///A test for FromArgb
        ///</summary>
        [Fact]
        public void FromArgbTest_B()
        {
            byte alpha = 255;
            byte red = 0;
            byte green = 0;
            byte blue = 255;
            int argb = unchecked((int) 0xFF0000FF);

            OscColor expected = new OscColor(argb);
            OscColor actual;
            actual = OscColor.FromArgb(alpha, red, green, blue);

            Assert.Equal(expected, actual);

            Assert.Equal(argb, actual.ARGB);

            Assert.Equal(alpha, actual.A);
            Assert.Equal(red, actual.R);
            Assert.Equal(green, actual.G);
            Assert.Equal(blue, actual.B);
        }

        /// <summary>
        ///A test for FromArgb
        ///</summary>
        [Fact]
        public void FromArgbTest_G()
        {
            byte alpha = 255;
            byte red = 0;
            byte green = 255;
            byte blue = 0;
            int argb = unchecked((int) 0xFF00FF00);

            OscColor expected = new OscColor(argb);
            OscColor actual;
            actual = OscColor.FromArgb(alpha, red, green, blue);

            Assert.Equal(expected, actual);

            Assert.Equal(argb, actual.ARGB);

            Assert.Equal(alpha, actual.A);
            Assert.Equal(red, actual.R);
            Assert.Equal(green, actual.G);
            Assert.Equal(blue, actual.B);
        }

        /// <summary>
        ///A test for FromArgb
        ///</summary>
        [Fact]
        public void FromArgbTest_R()
        {
            byte alpha = 255;
            byte red = 255;
            byte green = 0;
            byte blue = 0;
            int argb = unchecked((int) 0xFFFF0000);

            OscColor expected = new OscColor(argb);
            OscColor actual;
            actual = OscColor.FromArgb(alpha, red, green, blue);

            Assert.Equal(expected, actual);

            Assert.Equal(argb, actual.ARGB);

            Assert.Equal(alpha, actual.A);
            Assert.Equal(red, actual.R);
            Assert.Equal(green, actual.G);
            Assert.Equal(blue, actual.B);
        }

        /// <summary>
        ///A test for OscColor Constructor
        ///</summary>
        [Fact]
        public void OscColorConstructorTest()
        {
            int value = unchecked((int) 0xFFFFFFFF);

            OscColor target = new OscColor(value);

            Assert.Equal(value, target.ARGB);

            Assert.Equal(255, target.A);
            Assert.Equal(255, target.R);
            Assert.Equal(255, target.G);
            Assert.Equal(255, target.B);
        }
    }
}