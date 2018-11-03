// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace OscCore.Address
{
    /// <summary>
    ///     Type of address part
    /// </summary>
    public enum OscAddressPartType
    {
        /// <summary>
        ///     Address separator char i.e. '/'
        /// </summary>
        AddressSeparator,

        /// <summary>
        ///     Address wildcard i.e. '//'
        /// </summary>
        AddressWildcard,

        /// <summary>
        ///     Any string literal i.e [^\s#\*,/\?\[\]\{}]+
        /// </summary>
        Literal,

        /// <summary>
        ///     Either single char or any length wildcard i.e '?' or '*'
        /// </summary>
        Wildcard,

        /// <summary>
        ///     Char span e.g. [a-z]+
        /// </summary>
        CharSpan,

        /// <summary>
        ///     List of literal matches
        /// </summary>
        List,

        /// <summary>
        ///     List of possible char matches e.g. [abcdefg]+
        /// </summary>
        CharList
    }
}