// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;

namespace OscCore.LowLevel
{
    public static class ArraySegmentByteExt
    {
        public static byte[] ToArray(this ArraySegment<byte> arraySegment)
        {
            byte[] buffer = new byte[arraySegment.Count];

            Buffer.BlockCopy(arraySegment.Array, arraySegment.Offset, buffer, 0, arraySegment.Count);

            return buffer;
        }
    }
}