// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace OscCore.LowLevel
{
    /// <summary>
    ///     A single osc message token.
    /// </summary>
    public enum OscToken
    {
        /// <summary>
        ///     No token.
        /// </summary>
        None,

        /// <summary>
        ///     Osc address string token.
        /// </summary>
        OscAddress,

        /// <summary>
        ///     Type-tag string token.
        /// </summary>
        TypeTag,

        /// <summary>
        ///     Char/byte token.
        /// </summary>
        Char,

        /// <summary>
        ///     Meta boolean token (actual bools are defined as either true or false in the type-tag).
        /// </summary>
        Bool,

        /// <summary>
        ///     Bool true value token.
        /// </summary>
        True,

        /// <summary>
        ///     Bool false value token.
        /// </summary>
        False,

        /// <summary>
        ///     String token.
        /// </summary>
        String,

        /// <summary>
        ///     Symbol string token.
        /// </summary>
        Symbol,

        /// <summary>
        ///     Impulse / bang token.
        /// </summary>
        Impulse,

        /// <summary>
        ///     Null token.
        /// </summary>
        Null,

        /// <summary>
        ///     Int32 token.
        /// </summary>
        Int,

        /// <summary>
        ///     Int64 token.
        /// </summary>
        Long,

        /// <summary>
        ///     Float / Single token.
        /// </summary>
        Float,

        /// <summary>
        ///     Double token.
        /// </summary>
        Double,

        /// <summary>
        ///     Osc time-tag token.
        /// </summary>
        TimeTag,

        /// <summary>
        ///     Osc time-tag token.
        /// </summary>
        Blob,

        /// <summary>
        ///     Osc color token.
        /// </summary>
        Color,

        /// <summary>
        ///     Osc midi token.
        /// </summary>
        Midi,

        /// <summary>
        ///     Token represents the start of an array.
        /// </summary>
        ArrayStart,

        /// <summary>
        ///     Token represents the end of an array.
        /// </summary>
        ArrayEnd,

        /// <summary>
        ///     Meta token used to indicate multiple types are present in an argument array.
        /// </summary>
        MixedTypes,

        /// <summary>
        ///     Token represents the end of the message.
        /// </summary>
        End,

        /// <summary>
        ///     Bundle message length token.
        /// </summary>
        BundleMessageLength
    }
}