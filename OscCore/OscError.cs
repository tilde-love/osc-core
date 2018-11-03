// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace OscCore
{
    /// <summary>
    ///     All errors that can occur while parsing or reading osc packets, messages and bundles
    /// </summary>
    public enum OscError
    {
        /// <summary>
        ///     No error
        /// </summary>
        None,

        /// <summary>
        ///     An invalid number or bytes has been read
        /// </summary>
        InvalidSegmentLength,

        /// <summary>
        ///     The address string is empty
        /// </summary>
        MissingAddress,

        /// <summary>
        ///     Missing comma after the address string
        /// </summary>
        MissingComma,

        /// <summary>
        ///     Missing type-tag
        /// </summary>
        MissingTypeTag,

        /// <summary>
        ///     Invalid type-tag
        /// </summary>
        MalformedTypeTag,

        /// <summary>
        ///     Error parsing arguemnt
        /// </summary>
        ErrorParsingArgument,

        /// <summary>
        ///     Error parsing blob argument
        /// </summary>
        ErrorParsingBlob,

        /// <summary>
        ///     Error parsing string argument
        /// </summary>
        ErrorParsingString,

        /// <summary>
        ///     Error parsing symbol argument
        /// </summary>
        ErrorParsingSymbol,

        /// <summary>
        ///     Error parsing int argument
        /// </summary>
        ErrorParsingInt32,

        /// <summary>
        ///     Error parsing long argument
        /// </summary>
        ErrorParsingInt64,

        /// <summary>
        ///     Error parsing float argument
        /// </summary>
        ErrorParsingSingle,

        /// <summary>
        ///     Error parsing double argument
        /// </summary>
        ErrorParsingDouble,

        /// <summary>
        ///     Error parsing osc-color argument
        /// </summary>
        ErrorParsingColor,

        /// <summary>
        ///     Error parsing char argument
        /// </summary>
        ErrorParsingChar,

        /// <summary>
        ///     Error parsing midi message argument
        /// </summary>
        ErrorParsingMidiMessage,

        /// <summary>
        ///     Error parsing midi message argument
        /// </summary>
        ErrorParsingOscTimeTag,

        /// <summary>
        ///     The type of an argument is unsupported
        /// </summary>
        UnknownArguemntType,

        /// <summary>
        ///     Bundle with missing ident
        /// </summary>
        MissingBundleIdent,

        /// <summary>
        ///     Bundle with invalid ident
        /// </summary>
        InvalidBundleIdent,

        /// <summary>
        ///     Invalid bundle message header
        /// </summary>
        InvalidBundleMessageHeader,

        /// <summary>
        ///     An error occured while parsing a packet
        /// </summary>
        ErrorParsingPacket,

        /// <summary>
        ///     Invalid bundle message length
        /// </summary>
        InvalidBundleMessageLength,

        /// <summary>
        /// </summary>
        UnexpectedToken,

        /// <summary>
        /// </summary>
        UnexpectedWriterState,
        ErrorParsingOscAdress,
        InvalidObjectName
    }
}