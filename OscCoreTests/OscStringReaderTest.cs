// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System.Collections.Generic;
using OscCore.LowLevel;
using Xunit;

namespace OscCoreTests
{
    public class OscStringReaderTest
    {
        [Fact]
        public void ArrayEnd()
        {
            string original = @"]";
            string expectedValue = null;
            OscSerializationToken expectedToken = OscSerializationToken.ArrayEnd;

            OscStringReader reader = new OscStringReader(original);

            OscSerializationToken token = reader.ReadNextToken(out string value);

            Assert.Equal(expectedToken, token);
            Assert.Equal(expectedValue, value);
        }

        [Fact]
        public void ArrayStart()
        {
            string original = @"[";
            string expectedValue = null;
            OscSerializationToken expectedToken = OscSerializationToken.ArrayStart;

            OscStringReader reader = new OscStringReader(original);

            OscSerializationToken token = reader.ReadNextToken(out string value);

            Assert.Equal(expectedToken, token);
            Assert.Equal(expectedValue, value);
        }

        [Fact]
        public void Char()
        {
            string original = @"'hello'";
            string expectedValue = "hello";
            OscSerializationToken expectedToken = OscSerializationToken.Char;

            OscStringReader reader = new OscStringReader(original);

            OscSerializationToken token = reader.ReadNextToken(out string value);

            Assert.Equal(expectedToken, token);
            Assert.Equal(expectedValue, value);
        }

//        [Fact]
//        public void Colon()
//        {
//            string original = @":";
//            string expectedValue = null;
//            OscSerializationToken expectedToken = OscSerializationToken.Colon;
//
//            OscStringReader reader = new OscStringReader(original);
//
//            OscSerializationToken token = reader.ReadNextToken(out string value);
//
//            Assert.Equal(expectedToken, token);
//            Assert.Equal(expectedValue, value);
//        }

        [Fact]
        public void End()
        {
            string original = @"";
            string expectedValue = null;
            OscSerializationToken expectedToken = OscSerializationToken.End;

            OscStringReader reader = new OscStringReader(original);

            OscSerializationToken token = reader.ReadNextToken(out string value);

            Assert.Equal(expectedToken, token);
            Assert.Equal(expectedValue, value);
        }

        [Fact]
        public void Literal()
        {
            string original = "hello";
            string expectedValue = "hello";
            OscSerializationToken expectedToken = OscSerializationToken.Literal;

            OscStringReader reader = new OscStringReader(original);

            OscSerializationToken token = reader.ReadNextToken(out string value);

            Assert.Equal(expectedToken, token);
            Assert.Equal(expectedValue, value);
        }

        [Fact]
        public void ObjectEnd()
        {
            string original = @"}";
            string expectedValue = null;
            OscSerializationToken expectedToken = OscSerializationToken.ObjectEnd;

            OscStringReader reader = new OscStringReader(original);

            OscSerializationToken token = reader.ReadNextToken(out string value);

            Assert.Equal(expectedToken, token);
            Assert.Equal(expectedValue, value);
        }

        [Fact]
        public void ObjectStart()
        {
            string original = @"{";
            string expectedValue = null;
            OscSerializationToken expectedToken = OscSerializationToken.ObjectStart;

            OscStringReader reader = new OscStringReader(original);

            OscSerializationToken token = reader.ReadNextToken(out string value);

            Assert.Equal(expectedToken, token);
            Assert.Equal(expectedValue, value);
        }

        [Fact]
        public void Separator()
        {
            string original = @",";
            string expectedValue = null;
            OscSerializationToken expectedToken = OscSerializationToken.Separator;

            OscStringReader reader = new OscStringReader(original);

            OscSerializationToken token = reader.ReadNextToken(out string value);

            Assert.Equal(expectedToken, token);
            Assert.Equal(expectedValue, value);
        }

        [Fact]
        public void SimpleString()
        {
            List<string> originals = new List<string>
            {
                @"/moop, ""thing"", 123, '!', [ 1, 2, 3, 4, 5]",
                @"/moop,""thing"",123,'!',[1,2,3,4,5]"
            };

            List<OscSerializationToken> expectedTokens = new List<OscSerializationToken>
            {
                OscSerializationToken.Literal,
                OscSerializationToken.Separator,
                OscSerializationToken.String,
                OscSerializationToken.Separator,
                OscSerializationToken.Literal,
                OscSerializationToken.Separator,
                OscSerializationToken.Char,
                OscSerializationToken.Separator,

                OscSerializationToken.ArrayStart,

                OscSerializationToken.Literal,
                OscSerializationToken.Separator,
                OscSerializationToken.Literal,
                OscSerializationToken.Separator,
                OscSerializationToken.Literal,
                OscSerializationToken.Separator,
                OscSerializationToken.Literal,
                OscSerializationToken.Separator,
                OscSerializationToken.Literal,

                OscSerializationToken.ArrayEnd,
                OscSerializationToken.End
            };

            List<string> expectedValues = new List<string>
            {
                "/moop",
                null,
                "thing",
                null,
                "123",
                null,
                "!",
                null,

                null,

                "1",
                null,
                "2",
                null,
                "3",
                null,
                "4",
                null,
                "5",
                null,

                null,
                null
            };

            foreach (string orginal in originals)
            {
                OscStringReader reader = new OscStringReader(orginal);

                OscSerializationToken token;

                int index = 0;

                do
                {
                    token = reader.ReadNextToken(out string value);

                    Assert.Equal(expectedTokens[index], token);
                    Assert.Equal(expectedValues[index], value);

                    index++;
                }
                while (token != OscSerializationToken.End);
            }
        }

        [Fact]
        public void String()
        {
            string original = @"""hello""";
            string expectedValue = "hello";
            OscSerializationToken expectedToken = OscSerializationToken.String;

            OscStringReader reader = new OscStringReader(original);

            OscSerializationToken token = reader.ReadNextToken(out string value);

            Assert.Equal(expectedToken, token);
            Assert.Equal(expectedValue, value);
        }
        
        [Fact]
        public void StringWithEscape()
        {
            string original = @"""let's say \""hello\""""";
            string expectedValue = @"let's say \""hello\""";
            OscSerializationToken expectedToken = OscSerializationToken.String;

            OscStringReader reader = new OscStringReader(original);

            OscSerializationToken token = reader.ReadNextToken(out string value);

            Assert.Equal(expectedToken, token);
            Assert.Equal(expectedValue, value);
        }
    }
}