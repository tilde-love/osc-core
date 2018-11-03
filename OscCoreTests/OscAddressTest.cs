// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using OscCore.Address;
using Xunit;

namespace OscCoreTests
{
    /// <summary>
    /// This is a test class for OscAddressTest and is intended
    /// to contain all OscAddressTest Unit Tests
    /// </summary>
    public class OscAddressTest
    {     
        #region Match

        /// <summary>
        ///A test for IsMatch
        ///</summary>
        [Fact]
        public void IsMatchTest_LiteralMatch()
        {
            string addressPattern = "/container_A/method_A";
            string address = "/container_A/method_A";

            Assert.True(OscAddress.IsMatch(addressPattern, address));
        }

        /// <summary>
        ///A test for IsMatch
        ///</summary>
        [Fact]
        public void IsMatchTest_LiteralMissmatch1()
        {
            string addressPattern = "/container_A/method_A";
            string address = "/container_A/method_B";

            Assert.False(OscAddress.IsMatch(addressPattern, address));
        }

        /// <summary>
        ///A test for IsMatch
        ///</summary>
        [Fact]
        public void IsMatchTest_LiteralMissmatch2()
        {
            string addressPattern = "/container_A/method_A";
            string address = "/container_B/method_A";

            Assert.False(OscAddress.IsMatch(addressPattern, address));
        }

        /// <summary>
        ///A test for IsMatch
        ///</summary>
        [Fact]
        public void IsMatchTest_PossibleIssueWithAddressPatternWildcard1()
        {
            string addressPattern = "/?*test";
            string address = "/test";

            Assert.False(OscAddress.IsMatch(addressPattern, address));
        }

        /// <summary>
        ///A test for IsMatch
        ///</summary>
        [Fact]
        public void IsMatchTest_PossibleIssueWithAddressPatternWildcard2()
        {
            string addressPattern = "/?*?test";
            string address = "/test";

            Assert.False(OscAddress.IsMatch(addressPattern, address));
        }

        /// <summary>
        ///A test for IsMatch
        ///</summary>
        [Fact]
        public void IsMatchTest_PossibleIssueWithAddressPatternWildcard3()
        {
            string addressPattern = "/*?test";
            string address = "/test";

            Assert.False(OscAddress.IsMatch(addressPattern, address));
        }

        /// <summary>
        ///A test for IsMatch
        ///</summary>
        [Fact]
        public void IsMatchTest_PossibleIssueWithAddressPatternWildcard4()
        {
            string addressPattern = "/?*test";
            string address = "/1test";

            Assert.True(OscAddress.IsMatch(addressPattern, address));
        }

        /// <summary>
        ///A test for IsMatch
        ///</summary>
        [Fact]
        public void IsMatchTest_PossibleIssueWithAddressPatternWildcard5()
        {
            string addressPattern = "/?*test";
            string address = "/1_test";

            Assert.True(OscAddress.IsMatch(addressPattern, address));
        }

        /// <summary>
        ///A test for IsMatch
        ///</summary>
        [Fact]
        public void IsMatchTest_PossibleIssueWithAddressPatternWildcard6()
        {
            string addressPattern = "/???test";
            string address = "/123test";

            Assert.True(OscAddress.IsMatch(addressPattern, address));
        }

        /// <summary>
        ///A test for IsMatch
        ///</summary>
        [Fact]
        public void IsMatchTest_PossibleIssueWithAddressPatternWildcard7()
        {
            string addressPattern = "/???test";
            string address = "/test";

            Assert.False(OscAddress.IsMatch(addressPattern, address));
        }

        /// <summary>
        ///A test for IsMatch
        ///</summary>
        [Fact]
        public void IsMatchTest_PossibleIssueWithAddressPatternWildcard8()
        {
            string addressPattern = "/???test??";
            string address = "/test";

            Assert.False(OscAddress.IsMatch(addressPattern, address));
        }

        /// <summary>
        ///A test for IsMatch
        ///</summary>
        [Fact]
        public void IsMatchTest_PossibleIssueWithAddressPatternWildcard9()
        {
            string addressPattern = "/???test??";
            string address = "/123test45";

            Assert.True(OscAddress.IsMatch(addressPattern, address));
        }

        /// <summary>
        ///A test for IsMatch
        ///</summary>
        [Fact]
        public void IsMatchTest_PossibleIssueWithAddressPatternWildcard10()
        {
            string addressPattern = "/???test*?";
            string address = "/123test9";

            Assert.True(OscAddress.IsMatch(addressPattern, address));
        }

        #endregion Match

        #region Validate Address

        /// <summary>
        ///A test for IsValidAddressPattern
        ///</summary>
        [Fact]
        public void IsValidAddressPatternTest_Good()
        {
            for (int i = 0; i < UnitTestHelper.Good_AddressPatterns.Length; i++)
            {
                string address = UnitTestHelper.Good_AddressPatterns[i];

                bool result = OscAddress.IsValidAddressPattern(address);

                Assert.True(result, $"Failed to validate address pattern {i} '{address}'");
            }
        }

        /// <summary>
        ///A test for IsValidAddressPattern
        ///</summary>
        [Fact]
        public void IsValidAddressPatternTest_Bad()
        {
            for (int i = 0; i < UnitTestHelper.Bad_AddressPatterns.Length; i++)
            {
                string address = UnitTestHelper.Bad_AddressPatterns[i];

                bool result = OscAddress.IsValidAddressPattern(address);

                Assert.False(result, $"Incorrectly validated address pattern {i} '{address}'");
            }
        }

        #endregion Validate Address

        #region Parse Address

        /// <summary>
        ///A test for Constructor
        ///</summary>
        [Fact]
        public void OscAddress_Constructor_Good()
        {
            for (int i = 0; i < UnitTestHelper.Good_AddressPatterns.Length; i++)
            {
                string address = UnitTestHelper.Good_AddressPatterns[i];

                OscAddress result = new OscAddress(address);

                Assert.Equal(address, UnitTestHelper.RebuildOscAddress(result)); // , $"Failed to parse address pattern {i} '{address}'");
            }
        }

        #endregion Parse Address

        #region Parse Address

        /// <summary>
        ///A test for Constructor
        ///</summary>
        [Fact]
        public void AddressPatternMatches()
        {
            for (int i = 0; i < UnitTestHelper.Good_AddressPatterns.Length; i++)
            {
                string pattern = UnitTestHelper.Good_AddressPatterns[i];
                string address = UnitTestHelper.Good_AddressPatternMatches[i];

                OscAddress target = new OscAddress(pattern);

                bool result = target.Match(address);

                Assert.True(result, String.Format("Failed to match address pattern {0} '{1}' to '{2}'", i, pattern, address));
            }
        }

        #endregion Parse Address
    }
}
