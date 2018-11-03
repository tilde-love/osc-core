using OscCore;
using Xunit;

namespace OscCoreTests
{
    /// <summary>
    ///This is a test class for OscMidiMessageTest and is intended
    ///to contain all OscMidiMessageTest Unit Tests
    ///</summary>
    public class OscMidiMessageTest
    {
        /// <summary>
        ///A test for Data14BitValue
        ///</summary>
        [Fact]
        public void Data14BitValueTest()
        {
            ushort expected = 0x1356;
            OscMidiMessage target = new OscMidiMessage(0x03F35626);
            ushort actual;
            actual = target.Data14BitValue;
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [Fact]
        public void EqualsTest()
        {
            OscMidiMessage target = new OscMidiMessage(0x03F35626);
            uint obj = 0x03F35626;
            bool expected = true;
            bool actual;
            actual = target.Equals(obj);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///A test for Equals
        ///</summary>
        [Fact]
        public void EqualsTest2()
        {
            OscMidiMessage target = new OscMidiMessage(0x03F35626);
            uint obj = 0x0832626;
            bool expected = false;
            bool actual;
            actual = target.Equals(obj);
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [Fact]
        public void GetHashCodeTest()
        {
            OscMidiMessage target = new OscMidiMessage(0x03F35626);
            int expected = 0x03F35626;
            int actual;
            actual = target.GetHashCode();
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///A test for OscMidiMessage Constructor
        ///</summary>
        [Fact]
        public void OscMidiMessageConstructorTest()
        {
            OscMidiMessage expected = new OscMidiMessage(0x03962200);

            byte portID = 3;
            OscMidiMessageType type = OscMidiMessageType.NoteOn;
            byte channel = 6;
            byte data1 = 34;
            OscMidiMessage target = new OscMidiMessage(portID, type, channel, data1);

            Assert.Equal(expected, target);
            Assert.Equal(target.PortID, portID);
            Assert.Equal(target.Channel, channel);
            Assert.Equal(target.MessageType, type);
            Assert.Equal(target.Data1, data1);
            Assert.Equal(target.Data2, 0);
        }

        /// <summary>
        ///A test for OscMidiMessage Constructor
        ///</summary>
        [Fact]
        public void OscMidiMessageConstructorTest1()
        {
            OscMidiMessage expected = new OscMidiMessage(0x03F35626);

            byte portID = 3;
            OscMidiSystemMessageType type = OscMidiSystemMessageType.SongSelect;
            ushort value = 0x1356;

            OscMidiMessage target = new OscMidiMessage(portID, type, value);

            Assert.Equal(expected, target);
            Assert.Equal(target.PortID, portID);
            Assert.Equal(target.MessageType, OscMidiMessageType.SystemExclusive);
            Assert.Equal(target.SystemMessageType, type);
            Assert.Equal(target.Data14BitValue, value);
        }

        /// <summary>
        ///A test for OscMidiMessage Constructor
        ///</summary>
        [Fact]
        public void OscMidiMessageConstructorTest2()
        {
            uint value = 0x03F35626;
            OscMidiMessage target = new OscMidiMessage(value);

            byte portID = 3;
            OscMidiSystemMessageType type = OscMidiSystemMessageType.SongSelect;
            ushort data14BitValue = 0x1356;

            OscMidiMessage expected = new OscMidiMessage(portID, type, data14BitValue);

            Assert.Equal(expected, target);
            Assert.Equal(target.PortID, portID);
            Assert.Equal(target.MessageType, OscMidiMessageType.SystemExclusive);
            Assert.Equal(target.SystemMessageType, type);
            Assert.Equal(target.Data14BitValue, data14BitValue);
            Assert.Equal(target.Data1, 0x56);
            Assert.Equal(target.Data2, 0x26);
        }
    }
}