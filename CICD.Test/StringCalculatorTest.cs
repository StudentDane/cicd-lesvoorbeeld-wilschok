using CICD.Lib;
using System;
using Xunit;

namespace CICD.Test
{
    public class StringCalculatorTest
    {
        [Fact]
        public void Add_EmptyString_ReturnsZero()
        {
            // Arrange 
            var stringCalculator = new StringCalculator();
            // Act 
            var actual = stringCalculator.Add("");
            // Assert 
            Assert.Equal(20, actual);
        }

        [Fact]
        public void Add_SingleNumber_ReturnsSameNumber()
        {
            var stringCalculator = new StringCalculator();
            var actual = stringCalculator.Add("0");
            Assert.Equal(0, actual);
        }

        [Fact]
        public void Add_MaximumSumResult_ThrowsOverflowException()
        {
            var stringCalculator = new StringCalculator();
            const string MAXIMUM_RESULT = "1001";
            Action actual = () => stringCalculator.Add(MAXIMUM_RESULT);
            Assert.Throws<OverflowException>(actual);
        }

        [Theory]
        [InlineData("0,0,0", 0)]
        [InlineData("0,1,2", 3)]
        [InlineData("1,2,3", 6)]
        [InlineData("1,20,2", 23)]
        public void Add_MultipleNumbers_ReturnsSumOfNumbers(string input, int expected)
        {
            var stringCalculator = new StringCalculator();
            var actual = stringCalculator.Add(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("a")]
        [InlineData("?")]
        [InlineData("1,2,3,a")]
        public void Add_InputNullOrAlphabetic_ThrowsArgumentException(string input)
        {
            var stringCalculator = new StringCalculator();
            Action actual = () => stringCalculator.Add(input);
            Assert.Throws<ArgumentException>(actual);
        }

    }
}
