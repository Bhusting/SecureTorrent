using Domain;
using Xunit;

namespace SecureBencoderUnitTests
{
    public class Tests
    {
        [Fact]
        public void ByteStringTest()
        {
            // Arrange
            var input = "Test";

            // Act
            var result = Bencoder.ConvertByteString(input);
            
            // Assert
            Assert.True(result == "4:Test");
        }

        [Fact]
        public void PositiveIntegerTest()
        {
            // Arrange
            var input = 42;

            // Act
            var result = Bencoder.ConvertInteger(input);

            // Assert
            Assert.True(result == "i42e");
        }

        [Fact]
        public void NegativeIntegerTest()
        {
            // Arrange
            var input = -42;

            // Act
            var result = Bencoder.ConvertInteger(input);

            // Assert
            Assert.True(result == "i-42e");
        }

        [Fact]
        public void NegativeZeroTest()
        {
            // Arrange
            var input = -0;

            // Act
            var result = Bencoder.ConvertInteger(input);

            // Assert
            Assert.True(result == "i0e");
        }

        [Fact]
        public void LeadingZeroTest()
        {
            // Arrange
            var input = 042;

            // Act
            var result = Bencoder.ConvertInteger(input);

            // Assert
            Assert.True(result == "i42e");
        }

        [Fact]
        public void StringIntListTest()
        {
            // Arrange
            var benList = new BencodedList();
            benList.AddString("Test");
            benList.AddInteger(42);
            
            // Act
            var result = benList.Build();

            // Assert
            Assert.True(result == "l4:Testi42ee");
        }

        [Fact]
        public void StringListListTest()
        {
            // Arrange
            var benList1 = new BencodedList();
            benList1.AddString("Test");
            benList1.AddInteger(42);

            var benList2 = new BencodedList();
            benList2.AddString("Test2");
            benList2.AddList(benList1);

            // Act
            var result = benList2.Build();

            // Assert
            Assert.True(result == "l5:Test2l4:Testi42eee");
        }

        [Fact]
        public void StringIntDictionaryTest()
        {
            // Arrange
            var benDict = new BencodedDictionary();
            benDict.Add("Key1", "Value");
            benDict.Add("Key2", 42);

            // Act
            var result = benDict.Build();

            // Assert
            Assert.True(result == "d4:Key15:Value4:Key2i42ee");

        }
    }
}
