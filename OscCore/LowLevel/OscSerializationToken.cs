// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace OscCore.LowLevel
{
    public enum OscSerializationToken
    {
        None,

        Literal,
        String,
        Symbol,
        Char,

        Separator,

        ArrayStart,
        ArrayEnd,

        ObjectStart,
        ObjectEnd,

        End
    }
}