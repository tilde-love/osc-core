// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace OscCore.Address
{
    /// <summary>
    ///     Encompasses a single part of an osc address
    /// </summary>
    public struct OscAddressPart
    {
        /// <summary>
        ///     The regex representation of this part
        /// </summary>
        public readonly string PartRegex;

        /// <summary>
        ///     The address part type
        /// </summary>
        public readonly OscAddressPartType Type;

        /// <summary>
        ///     The original string value of this part
        /// </summary>
        public readonly string Value;

        /// <summary>
        ///     How the string was interpreted (only used for testing)
        /// </summary>
        public readonly string Interpreted;

        private static readonly Regex CharMatcher = new Regex(@"[\.\$\^\{\[\(\|\)\*\+\?\\]", RegexOptions.Compiled);

        /// <summary>
        ///     Create a address part
        /// </summary>
        /// <param name="type">the type of part</param>
        /// <param name="value">the original string value</param>
        /// <param name="interpreted">the representation of the original value as interpreted by the parser</param>
        /// <param name="partRegex">the part as a regex expression</param>
        private OscAddressPart(
            OscAddressPartType type,
            string value,
            string interpreted,
            string partRegex)
        {
            Type = type;
            Value = value;
            Interpreted = interpreted;
            PartRegex = partRegex;
        }

        /// <summary>
        ///     Create a address separator part '/'
        /// </summary>
        /// <returns>the part</returns>
        internal static OscAddressPart AddressSeparator()
        {
            return new OscAddressPart(OscAddressPartType.AddressSeparator, "/", "/", "/");
        }

        /// <summary>
        ///     Create a address wildcard part "//"
        /// </summary>
        /// <returns>the part</returns>
        internal static OscAddressPart AddressWildcard()
        {
            return new OscAddressPart(OscAddressPartType.AddressWildcard, "//", "//", "/");
        }

        /// <summary>
        ///     Character list e.g. [abcde]
        /// </summary>
        /// <param name="value">the original string</param>
        /// <returns>the part</returns>
        internal static OscAddressPart CharList(string value)
        {
            bool isNot = false;
            int index = 1;

            if (value[index] == '!')
            {
                isNot = true;
                index++;
            }

            string list = value.Substring(index, value.Length - 1 - index);

            string regex = $"[{(isNot ? "^" : string.Empty)}{EscapeString(list)}]+";
            string rebuild = $"[{(isNot ? "!" : string.Empty)}{list}]";

            return new OscAddressPart(OscAddressPartType.CharList, value, rebuild, regex);
        }

        /// <summary>
        ///     Character span e.g. [a-e]
        /// </summary>
        /// <param name="value">the original string</param>
        /// <returns>the part</returns>
        internal static OscAddressPart CharSpan(string value)
        {
            bool isNot = false;
            int index = 1;

            if (value[index] == '!')
            {
                isNot = true;
                index++;
            }

            char low = value[index++];
            index++;
            char high = value[index++];

            string rebuild = $"[{(isNot ? "!" : string.Empty)}{low}-{high}]";

            // if the range is the wrong way round then swap them
            if (low > high)
            {
                char temp = high;

                high = low;
                low = temp;
            }

            string regex = $"[{(isNot ? "^" : string.Empty)}{EscapeChar(low)}-{EscapeChar(high)}]+";

            return new OscAddressPart(OscAddressPartType.CharSpan, value, rebuild, regex);
        }

        /// <summary>
        ///     Literal list e.g. {thing1,THING1}
        /// </summary>
        /// <param name="value">the original string</param>
        /// <returns>the part</returns>
        internal static OscAddressPart List(string value)
        {
            string[] list = value.Substring(1, value.Length - 2)
                .Split(',');

            StringBuilder regSb = new StringBuilder();
            StringBuilder listSb = new StringBuilder();

            bool first = true;

            regSb.Append("(");
            listSb.Append("{");

            foreach (string str in list)
            {
                if (first == false)
                {
                    regSb.Append("|");
                    listSb.Append(",");
                }
                else
                {
                    first = false;
                }

                regSb.Append("(" + EscapeString(str) + ")");
                listSb.Append(str);
            }

            listSb.Append("}");
            regSb.Append(")");

            return new OscAddressPart(OscAddressPartType.List, value, listSb.ToString(), regSb.ToString());
        }

        /// <summary>
        ///     Create a literal address part
        /// </summary>
        /// <param name="value">the literal</param>
        /// <returns>the part</returns>
        internal static OscAddressPart Literal(string value)
        {
            return new OscAddressPart(OscAddressPartType.Literal, value, value, "(" + EscapeString(value) + ")");
        }

        /// <summary>
        ///     Create a part for a wildcard part
        /// </summary>
        /// <param name="value">the original string</param>
        /// <returns>the part</returns>
        internal static OscAddressPart Wildcard(string value)
        {
            string regex = value;

            // reduce needless complexity
            while (regex.Contains("**"))
            {
                regex = regex.Replace("**", "*");
            }

            StringBuilder sb = new StringBuilder();

            // single char mode indicates that 1 or more '?' has been encountered while parsing
            bool singleCharMode = false;

            // the number of '?' that have been encountered sequentially
            int count = 0;

            // replace with wildcard regex
            foreach (char c in regex)
            {
                switch (c)
                {
                    case '*':
                        // if we are in single char mode the output the match for the current count of sequential chars
                        if (singleCharMode)
                        {
                            sb.Append($@"([^\s#\*,/\?\[\]\{{}}]{{{count}}})");
                        }

                        // no longer in single char mode
                        singleCharMode = false;

                        // reset the count
                        count = 0;

                        // output the zero or more chars matcher
                        sb.Append(@"([^\s#\*,/\?\[\]\{}]*)");
                        break;
                    case '?':
                        // indicate that a '?' has been encountered
                        singleCharMode = true;

                        // increment the count
                        count++;
                        break;
                }
            }

            // if we are in single char mode then output the match for the current count of sequential chars
            if (singleCharMode)
            {
                sb.Append($@"([^\s#\*,/\?\[\]\{{}}]{{{count}}})");
            }

            return new OscAddressPart(OscAddressPartType.Wildcard, value, value, sb.ToString());
        }

        private static string EscapeChar(char c)
        {
            return EscapeString(c.ToString());
        }

        private static string EscapeString(string str)
        {
            return CharMatcher.Replace(
                str,
                match =>
                {
                    switch (match.Value)
                    {
                        case ".":
                            return @"\.";

                        case "$":
                            return @"\$";

                        case "^":
                            return @"\^";

                        case "{":
                            return @"\{";

                        case "[":
                            return @"\[";

                        case "(":
                            return @"\(";

                        case "|":
                            return @"\|";

                        case ")":
                            return @"\)";

                        case "*":
                            return @"\*";

                        case "+":
                            return @"\+";

                        case "?":
                            return @"\?";

                        case "\\":
                            return @"\\";

                        default:
                            throw new Exception("Unexpected match");
                    }
                }
            );

            //#pragma warning disable
            //            // This should never be reached but should shut up the Unity compiler.
            //            return null;
            //#pragma warning reset
        }
    }
}