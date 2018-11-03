// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OscCore.Address
{
    /// <summary>
    ///     Encompasses an entire osc address
    /// </summary>
    public sealed class OscAddress : IEnumerable<OscAddressPart>
    {
        private static readonly char[] AddressSeperatorChar = {'/'};
        private static readonly Regex LiteralAddressValidator = new Regex(@"^/[^\s#\*,/\?\[\]\{}]+((/[^\s#\*,/\?\[\]\{}]+)*)$", RegexOptions.Compiled);

        private static readonly Regex PatternAddressPartExtractor = new Regex(
            @"
			  (?<Literal>([^\s#\*,/\?\[\]\{}]+)) |
			  (?<Wildcard>([\*\?]+)) |
			  (?<CharSpan>(\[(!?)[^\s#\*,/\?\[\]\{}-]-[^\s#\*,/\?\[\]\{}-]\])) |
			  (?<CharList>(\[(!?)[^\s#\*,/\?\[\]\{}]+\])) |
			  (?<List>{([^\s#\*/\?\,[\]\{}]+)((,[^\s#\*/\?\,[\]\{}]+)*)})
			  ",
            RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace
        );

        private static readonly Regex PatternAddressPartValidator = new Regex(
            @"^((
			  (?<Literal>([^\s#\*,/\?\[\]\{}]+)) |
			  (?<Wildcard>([\*\?]+)) |
			  (?<CharSpan>(\[(!?)[^\s#\*,/\?\[\]\{}-]-[^\s#\*,/\?\[\]\{}-]\])) |
			  (?<CharList>(\[(!?)[^\s#\*,/\?\[\]\{}]+\])) |
			  (?<List>{([^\s#\*/\?\,[\]\{}]+)((,[^\s#\*/\?\,[\]\{}]+)*)})
			  )+)$",
            RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace
        );

        private static readonly Regex PatternAddressValidator = new Regex(@"^(//|/)[^\s#/]+((/[^\s#/]+)*)$", RegexOptions.Compiled);

        private OscAddressPart[] parts;
        private Regex regex;
        private OscAddressType type;

        /// <summary>
        ///     Address parts
        /// </summary>
        /// <param name="index">the index of the part</param>
        /// <returns>the address part at the given index</returns>
        public OscAddressPart this[int index] => parts[index];

        /// <summary>
        ///     The number of parts in the address
        /// </summary>
        public int Count => parts.Length;

        /// <summary>
        ///     Is this address a literal
        /// </summary>
        public bool IsLiteral => type == OscAddressType.Literal;

        /// <summary>
        ///     The string used to create the address
        /// </summary>
        public string OrigialString { get; private set; }

        /// <summary>
        ///     Create an osc address from a string, must follow the rules set out in http://opensoundcontrol.org/spec-1_0 and
        ///     http://opensoundcontrol.org/spec-1_1
        /// </summary>
        /// <param name="address">the address string</param>
        public OscAddress(string address)
        {
            ParseAddress(address);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return parts.GetEnumerator();
        }

//        /// <summary>
//        /// Only used for testing
//        /// </summary>
//        /// <returns>a string that would produce the same address pattern but not a copy of the original string</returns>
//        internal string ToString_Rebuild()
//        {
//            StringBuilder sb = new StringBuilder();
//
//            foreach (OscAddressPart part in parts)
//            {
//                sb.Append(part.Interpreted);
//            }
//
//            return sb.ToString();
//        }

        /// <inheritdoc />
        public IEnumerator<OscAddressPart> GetEnumerator()
        {
            return (parts as IEnumerable<OscAddressPart>).GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            return OrigialString.Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return OrigialString.GetHashCode();
        }

        /// <summary>
        ///     Does a address match a address pattern
        /// </summary>
        /// <param name="addressPattern">address pattern (may include wildcards and lists)</param>
        /// <param name="address">literal address</param>
        /// <returns>true if the addess matches the pattern</returns>
        public static bool IsMatch(string addressPattern, string address)
        {
            if (IsValidAddressLiteral(address) == false)
            {
                return false;
            }

            // are they both literals
            if (IsValidAddressLiteral(addressPattern))
            {
                // preform a string match
                return addressPattern.Equals(address);
            }

            if (IsValidAddressPattern(addressPattern) == false)
            {
                return false;
            }

            // create a new pattern for the match
            OscAddress pattern = new OscAddress(addressPattern);

            // return the result
            return pattern.Match(address);
        }

        /// <summary>
        ///     Is the supplied address a valid literal address (no wildcards or lists)
        /// </summary>
        /// <param name="address">the address to check</param>
        /// <returns>true if the address is valid</returns>
        public static bool IsValidAddressLiteral(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return false;
            }

            return LiteralAddressValidator.IsMatch(address);
        }

        /// <summary>
        ///     Is the supplied address a valid address pattern (may include wildcards and lists)
        /// </summary>
        /// <param name="addressPattern">the address pattern to check</param>
        /// <returns>true if the address pattern is valid</returns>
        public static bool IsValidAddressPattern(string addressPattern)
        {
            if (string.IsNullOrWhiteSpace(addressPattern))
            {
                return false;
            }

            if (PatternAddressValidator.IsMatch(addressPattern) == false)
            {
                return false;
            }

            // is this address a liternal address?
            if (IsValidAddressLiteral(addressPattern))
            {
                return true;
            }

            string[] parts = addressPattern.Split(AddressSeperatorChar, StringSplitOptions.RemoveEmptyEntries);

            bool isMatch = true;

            // scan for wild chars and lists
            foreach (string part in parts)
            {
                isMatch &= PatternAddressPartValidator.IsMatch(part);
            }

            return isMatch;
        }

        /// <summary>
        ///     Match this address against an address string
        /// </summary>
        /// <param name="address">the address string to match against</param>
        /// <returns>true if the addresses match, otherwise false</returns>
        public bool Match(string address)
        {
            // if this address in a literal
            if (type == OscAddressType.Literal)
            {
                // if the original string is the same then we are good
                return OrigialString.Equals(address);
            }

            // use the pattern regex to determin a match
            return regex.IsMatch(address);
        }

        /// <summary>
        ///     Match this address against another
        /// </summary>
        /// <param name="address">the address to match against</param>
        /// <returns>true if the addresses match, otherwise false</returns>
        public bool Match(OscAddress address)
        {
            // if both addresses are literals then we can match on original string
            if (type == OscAddressType.Literal &&
                address.type == OscAddressType.Literal)
            {
                return OrigialString.Equals(address.OrigialString);
            }
            // if this address is a literal then use the others regex

            if (type == OscAddressType.Literal)
            {
                return address.regex.IsMatch(OrigialString);
            }
            // if the other is a literal use this ones regex

            if (address.type == OscAddressType.Literal)
            {
                return regex.IsMatch(address.OrigialString);
            }
            // if both are patterns then we just match on pattern original strings

            return OrigialString.Equals(address.OrigialString);
        }

        public override string ToString()
        {
            return OrigialString;
        }

        private void ParseAddress(string address)
        {
            // Ensure address is valid
            if (IsValidAddressPattern(address) == false)
            {
                throw new ArgumentException($"The address '{address}' is not a valid osc address", nameof(address));
            }

            // stash the original string
            OrigialString = address;

            // is this address non-literal (an address pattern)
            bool nonLiteral = false;
            bool skipNextSeparator = false;

            // create a list for the parsed parts
            List<OscAddressPart> addressParts = new List<OscAddressPart>();

            if (address.StartsWith("//"))
            {
                // add the wildcard
                addressParts.Add(OscAddressPart.AddressWildcard());

                // strip off the "//" from the address
                address = address.Substring(2);

                // this address in not a literal
                nonLiteral = true;

                // do not add a Separator before the next token
                skipNextSeparator = true;
            }

            // the the bits of the path, split by the '/' char
            string[] parts = address.Split(AddressSeperatorChar, StringSplitOptions.RemoveEmptyEntries);

            // loop through all the parts
            foreach (string part in parts)
            {
                if (skipNextSeparator == false)
                {
                    // add a separator
                    addressParts.Add(OscAddressPart.AddressSeparator());
                }
                else
                {
                    // we dont want to skip the next one
                    skipNextSeparator = false;
                }

                // get the matches within the part
                MatchCollection matches = PatternAddressPartExtractor.Matches(part);

                // loop through all matches
                foreach (Match match in matches)
                {
                    if (match.Groups["Literal"]
                        .Success)
                    {
                        addressParts.Add(
                            OscAddressPart.Literal(
                                match.Groups["Literal"]
                                    .Value
                            )
                        );
                    }
                    else if (match.Groups["Wildcard"]
                        .Success)
                    {
                        addressParts.Add(
                            OscAddressPart.Wildcard(
                                match.Groups["Wildcard"]
                                    .Value
                            )
                        );
                        nonLiteral = true;
                    }
                    else if (match.Groups["CharSpan"]
                        .Success)
                    {
                        addressParts.Add(
                            OscAddressPart.CharSpan(
                                match.Groups["CharSpan"]
                                    .Value
                            )
                        );
                        nonLiteral = true;
                    }
                    else if (match.Groups["CharList"]
                        .Success)
                    {
                        addressParts.Add(
                            OscAddressPart.CharList(
                                match.Groups["CharList"]
                                    .Value
                            )
                        );
                        nonLiteral = true;
                    }
                    else if (match.Groups["List"]
                        .Success)
                    {
                        addressParts.Add(
                            OscAddressPart.List(
                                match.Groups["List"]
                                    .Value
                            )
                        );
                        nonLiteral = true;
                    }
                    else
                    {
                        throw new Exception($"Unknown address part '{match.Value}'");
                    }
                }
            }

            // set the type
            type = nonLiteral ? OscAddressType.Pattern : OscAddressType.Literal;

            // set the parts array
            this.parts = addressParts.ToArray();

            // build the regex if one is needed
            if (type == OscAddressType.Literal)
            {
                return;
            }

            StringBuilder regex = new StringBuilder();

            if (this.parts[0]
                    .Type == OscAddressPartType.AddressWildcard)
            {
                // dont care where the start is
                regex.Append("(");
            }
            else
            {
                // match the start of the string
                regex.Append("^(");
            }

            foreach (OscAddressPart part in this.parts)
            {
                // match the part
                regex.Append(part.PartRegex);
            }

            // match the end of the string
            regex.Append(")$");

            // aquire the regex
            this.regex = OscAddressRegexCache.Aquire(regex.ToString());
        }
    }
}