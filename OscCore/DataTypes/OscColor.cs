// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Globalization;
using OscCore.LowLevel;

// ReSharper disable once CheckNamespace
namespace OscCore
{
    /// <summary>
    ///     Represents a 32bit ARGB color
    /// </summary>
    /// <remarks>
    ///     This is a poor replacement for System.Drawing.Color but unfortunately many platforms do not support
    ///     the System.Drawing namespace.
    /// </remarks>
    public struct OscColor
    {
        private const int AlphaMask = 0x18;
        private const int RedMask = 0x10;
        private const int GreenMask = 0x08;
        private const int BlueMask = 0;

        /// <summary>
        ///     Alpha, red, green and blue components packed into a single 32bit int
        /// </summary>
        public int ARGB { get; }

        /// <summary>
        ///     Red component
        /// </summary>
        public byte R => (byte) ((ARGB >> RedMask) & 0xff);

        /// <summary>
        ///     Green component
        /// </summary>
        public byte G => (byte) ((ARGB >> GreenMask) & 0xff);

        /// <summary>
        ///     Blue component
        /// </summary>
        public byte B => (byte) (ARGB & 0xff);

        /// <summary>
        ///     Alpha component
        /// </summary>
        public byte A => (byte) ((ARGB >> AlphaMask) & 0xff);

        /// <summary>
        ///     Initate a new Osc-Color from an ARGB color value
        /// </summary>
        /// <param name="value">An 32bit ARGB integer</param>
        public OscColor(int value)
        {
            ARGB = value;
        }

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case OscColor oscColor:
                    return oscColor.ARGB == ARGB;
                case int intValue:
                    return intValue == ARGB;
                case uint uintValue:
                    return unchecked((int) uintValue) == ARGB;
            }

            return base.Equals(obj);
        }

        public override string ToString()
        {
            //return $"{A}, {R}, {G}, {B}";
            return $"{R}, {G}, {B}, {A}";
        }

        public override int GetHashCode()
        {
            return ARGB;
        }

        /// <summary>
        ///     Create a Osc-Color from an 32bit ARGB integer
        /// </summary>
        /// <param name="argb">An ARGB integer</param>
        /// <returns>An Osc Color</returns>
        public static OscColor FromArgb(int argb)
        {
            return new OscColor(unchecked(argb & (int) 0xffffffff));
        }

        /// <summary>
        ///     Create a Osc-Color from 4 channels
        /// </summary>
        /// <param name="alpha">Alpha channel component</param>
        /// <param name="red">Red channel component</param>
        /// <param name="green">Green channel component</param>
        /// <param name="blue">Blue channel component</param>
        /// <returns>An Osc Color</returns>
        public static OscColor FromArgb(
            int alpha,
            int red,
            int green,
            int blue)
        {
            CheckByte(alpha, "alpha");
            CheckByte(red, "red");
            CheckByte(green, "green");
            CheckByte(blue, "blue");

            return new OscColor(MakeArgb((byte) alpha, (byte) red, (byte) green, (byte) blue));
        }

        private static int MakeArgb(
            byte alpha,
            byte red,
            byte green,
            byte blue)
        {
            return unchecked((int) ((uint) ((red << RedMask) | (green << GreenMask) | blue | (alpha << AlphaMask)) & 0xffffffff));
        }

        private static void CheckByte(int value, string name)
        {
            if (value >= 0 && value <= 0xff)
            {
                return;
            }

            throw new ArgumentException($"The {name} channel has a value of {value}, color channel values must be in the range 0 to {0xff}", name);
        }

        public static OscColor Parse(ref OscStringReader reader, IFormatProvider provider)
        {
            string[] pieces = new string[4];

            OscSerializationToken token = OscSerializationToken.None;

            for (int i = 0; i < 4; i++)
            {
                token = reader.ReadNextToken(out string value);
                pieces[i] = value;
                token = reader.ReadNextToken(out string _);
            }

            if (token != OscSerializationToken.ObjectEnd)
            {
                throw new Exception("Invalid color");
            }

            byte a, r, g, b;

            r = byte.Parse(
                pieces[0]
                    .Trim(),
                NumberStyles.None,
                provider
            );
            g = byte.Parse(
                pieces[1]
                    .Trim(),
                NumberStyles.None,
                provider
            );
            b = byte.Parse(
                pieces[2]
                    .Trim(),
                NumberStyles.None,
                provider
            );
            a = byte.Parse(
                pieces[3]
                    .Trim(),
                NumberStyles.None,
                provider
            );

            return FromArgb(a, r, g, b);
        }

        public static OscColor Parse(string str, IFormatProvider provider)
        {
            string[] pieces = str.Split(',');

            if (pieces.Length != 4)
            {
                throw new Exception($"Invalid color \'{str}\'");
            }

            byte a, r, g, b;

            r = byte.Parse(
                pieces[0]
                    .Trim(),
                NumberStyles.None,
                provider
            );
            g = byte.Parse(
                pieces[1]
                    .Trim(),
                NumberStyles.None,
                provider
            );
            b = byte.Parse(
                pieces[2]
                    .Trim(),
                NumberStyles.None,
                provider
            );
            a = byte.Parse(
                pieces[3]
                    .Trim(),
                NumberStyles.None,
                provider
            );

            return FromArgb(a, r, g, b);
        }
    }
}