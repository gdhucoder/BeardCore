using CommonUtil;
using System;
using Xunit;

namespace Datalibrary.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void Add_SimpleValueShouldCalculate()
        {
            // Arrange
            double expected = 6;

            // Act
            double actual = Calculator.Add(2, 3);

            Console.WriteLine(actual);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
