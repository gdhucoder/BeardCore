using CommonUtil;
using System;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(2,3,5)]
        [InlineData(2.1,3,5.1)]
        [InlineData(double.MaxValue,-1,double.MaxValue)]
        [InlineData(double.MaxValue,double.MinValue,double.MaxValue)]
        public void Add_SimpleValueShouldCalculate(double x, double y,double expected)
        {
            // Arrange

            // Act
            double actual = Calculator.Add(x, y);

            // Assert
            Assert.Equal(expected, actual);
        }



        [Fact]
        public void Fail_SimpleValueShouldCalculate()
        {
            // Arrange
            double expected = 6;

            // Act
            double actual = Calculator.Add(2, 3);

            Console.WriteLine(actual);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void MyFirstTheory(int a)
        {
            Assert.True(IsOdd(a));
        }

        bool IsOdd(int v)
        {
            return v % 2 == 1;
        }
    }
}
