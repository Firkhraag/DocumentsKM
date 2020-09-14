using System.IO;
using Xunit;

namespace ProjectKM.Tests
{
    public class SimpleTests
    {
        [Theory]
        [InlineData(3, false)]
        [InlineData(11, true)]
        public void IsOdd(int value, bool expected)
        {
            Assert.Equal(expected, value > 10);
        }

        [Fact]
        public void Add_SimpleValuesShouldCalculate()
        {
            // Arrange
            double expected = 5;

            // Act
            double actual = Calculator.Add(3, 2);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_MaxValuesShouldCalculate()
        {
            // Arrange
            double expected = double.MaxValue;

            // Act
            double actual = Calculator.Add(double.MaxValue, 2);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_ShouldFail()
        {
            // Arrange
            double wrong = 6;

            // Act
            double actual = Calculator.Add(3, 2);

            // Assert
            Assert.NotEqual(wrong, actual);
        }

        [Fact]
        public void ShouldFail()
        {
            Assert.ThrowsAsync<FileNotFoundException>(() => throw new FileNotFoundException());
        }
    }
}