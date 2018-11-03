// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace OscCore.LowLevel
{
    public static class OscUtils
    {
        /// <summary>
        ///     Are the contents of 2 argument arrays the equivalent
        /// </summary>
        /// <param name="array1">An array containing argument objects</param>
        /// <param name="array2">An array containing argument objects</param>
        /// <returns>true if the object arrays are equivalent</returns>
        public static bool ArgumentsAreEqual(object[] array1, object[] array2)
        {
            // ensure the arrays the same length
            if (array1.Length != array2.Length)
            {
                return false;
            }

            // iterate through the arrays
            for (int i = 0; i < array1.Length; i++)
            {
                // ensure the objects at index i of the same type? 
                if (array1[i]
                        .GetType() != array2[i]
                        .GetType())
                {
                    return false;
                }

                // is the argument an object array
                if (array1[i] is object[])
                {
                    object[] expectedArg = (object[]) array1[i];
                    object[] actualArg = (object[]) array2[i];

                    // ensure the argument object arrays are the same
                    if (ArgumentsAreEqual(expectedArg, actualArg) == false)
                    {
                        return false;
                    }
                }
                // is the argument an byte array
                else if (array1[i] is byte[])
                {
                    byte[] expectedArg = (byte[]) array1[i];
                    byte[] actualArg = (byte[]) array2[i];

                    // ensure the byte arrays are the same
                    if (BytesAreEqual(expectedArg, actualArg) == false)
                    {
                        return false;
                    }
                }
                // is the argument a color
                else if (array1[i] is OscColor)
                {
                    OscColor expectedArg = (OscColor) array1[i];
                    OscColor actualArg = (OscColor) array2[i];

                    // check the RGBA values
                    if (expectedArg.R != actualArg.R ||
                        expectedArg.G != actualArg.G ||
                        expectedArg.B != actualArg.B ||
                        expectedArg.A != actualArg.A)
                    {
                        return false;
                    }
                }
                // anything else
                else
                {
                    // just check the value
                    if (array1[i]
                            .Equals(array2[i]) == false)
                    {
                        return false;
                    }
                }
            }

            // we are good
            return true;
        }

        /// <summary>
        ///     Check the contents of 2 arrays of bytes are the same
        /// </summary>
        /// <param name="expected">The expected contents</param>
        /// <param name="actual">The actual contents</param>
        /// <returns>True if the contents are the same</returns>
        public static bool BytesAreEqual(byte[] expected, byte[] actual)
        {
            if (expected.Length != actual.Length)
            {
                return false;
            }

            for (int i = 0; i < expected.Length; i++)
            {
                if (expected[i] != actual[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Are 2 messages equivalent
        /// </summary>
        /// <param name="message1">A message</param>
        /// <param name="message2">A message</param>
        /// <returns>true if the objects are equivalent</returns>
        public static bool MessagesAreEqual(OscMessage message1, OscMessage message2)
        {
            // ensure the address is the same
            if (message1.Address != message2.Address)
            {
                return false;
            }

            // ensure the argument arrays are the same
            return ArgumentsAreEqual(message1.ToArray(), message2.ToArray());
        }


        /// <summary>
        ///     Calculate the size of the an object array in bytes
        /// </summary>
        /// <param name="args">the array</param>
        /// <returns>the size of the array in bytes</returns>
        public static int SizeOfObjectArray(object[] args)
        {
            int size = 0;
            int nullCount = 0;

            foreach (object obj in args)
            {
                if (obj is object[])
                {
                    size += SizeOfObjectArray(obj as object[]);
                }
                else if (
                    obj is int ||
                    obj is float ||
                    obj is byte ||
                    obj is OscMidiMessage ||
                    obj is OscColor)
                {
                    size += 4;
                }
                else if (
                    obj is long ||
                    obj is double ||
                    obj is OscTimeTag)
                {
                    size += 8;
                }
                else if (
                    obj is string ||
                    obj is OscSymbol)
                {
                    string value = obj.ToString();

                    // string and terminator
                    size += value.Length + 1;

                    // padding 
                    nullCount = 4 - size % 4;

                    if (nullCount < 4)
                    {
                        size += nullCount;
                    }
                }
                else if (obj is byte[])
                {
                    byte[] value = (byte[]) obj;

                    // length integer 
                    size += 4;

                    // content 
                    size += value.Length;

                    // padding 
                    nullCount = 4 - size % 4;

                    if (nullCount < 4)
                    {
                        size += nullCount;
                    }
                }
                else if (
                    obj is bool ||
                    obj is OscNull ||
                    obj is OscImpulse)
                {
                    size += 0;
                }
            }

            return size;
        }

        /// <summary>
        ///     Calculate the size of the type tag of an object array
        /// </summary>
        /// <param name="args">the array</param>
        /// <returns>the size of the type tag for the array</returns>
        public static int SizeOfObjectArray_TypeTag(object[] args)
        {
            int size = 0;

            // typetag
            foreach (object obj in args)
            {
                if (obj is object[])
                {
                    size += SizeOfObjectArray_TypeTag(obj as object[]);
                    size += 2; // for the [ ] 
                }
                else
                {
                    size++;
                }
            }

            return size;
        }
    }
}