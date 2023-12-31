﻿using SalesTaxes.Core.Services;
using Xunit;

namespace SalesTaxesTests.Services
{
    public class CoreServiceTests
    {
        private readonly StringWriter _stringWriter;

        public CoreServiceTests()
        {
            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);
        }

        [Fact]
        public void Test_Input1()
        {

            var input = new string[] {
                                        "1 Book at 12.49|3|n",
                                        "1 Book at 12.49|3|n",
                                        "1 Music CD at 14.99||n",
                                        "1 Chocolate bar at 0.85|1|n"
                                    };

            CoreService.Run(input);

            var output = _stringWriter.ToString().Split("\r\n");

            Assert.Equal("Book: 24.98 (2 @ 12.49)", output[0]);
            Assert.Equal("Music CD: 16.49", output[1]);
            Assert.Equal("Chocolate bar: 0.85", output[2]);
            Assert.Equal("Sales Taxes: 1.50", output[3]);
            Assert.Equal("Total: 42.32", output[4]);
        }

        [Fact]
        public void Test_Input2()
        {

            var input = new string[] {
                                       "1 Imported box of chocolates at 10.00|1|y",
                                       "1 Imported bottle of perfume at 47.50||y"
                                     };
            CoreService.Run(input);

            var output = _stringWriter.ToString().Split("\r\n");


            Assert.Equal("Imported box of chocolates: 10.50", output[0]);
            Assert.Equal("Imported bottle of perfume: 54.65", output[1]);
            Assert.Equal("Sales Taxes: 7.65", output[2]);
            Assert.Equal("Total: 65.15", output[3]);
        }

        [Fact]
        public void Test_Input3()
        {

            var input = new string[] {
                                        "1 Imported bottle of perfume at 27.99||y",
                                        "1 Bottle of perfume at 18.99||n",
                                        "1 Packet of headache pills at 9.75|2|n",
                                        "1 Imported box of chocolates at 11.25|1|y",
                                        "1 Imported box of chocolates at 11.25|1|y"
                                     };

            CoreService.Run(input);

            var output = _stringWriter.ToString().Split("\r\n");

            Assert.Equal("Imported bottle of perfume: 32.19", output[0]);
            Assert.Equal("Bottle of perfume: 20.89", output[1]);
            Assert.Equal("Packet of headache pills: 9.75", output[2]);
            Assert.Equal("Imported box of chocolates: 23.70 (2 @ 11.85)", output[3]);
            Assert.Equal("Sales Taxes: 7.30", output[4]);
            Assert.Equal("Total: 86.53", output[5]);
        }

        [Fact]
        public void Custom_Input1()
        {

            var input = new string[] {
                                        "1 Imported bottle of perfume at 27.99||y",
                                        "1 Imported box of chocolates at 11.25|1|y",
                                        "1 Book at 12.49|3|n",
                                        "1 Book at 12.49|3|n",
                                        "1 Book at 12.49|3|n",
                                        "1 Music CD at 14.99||n",
                                        "1 Chocolate bar at 0.85|1|n"
                                     };

            CoreService.Run(input);

            var output = _stringWriter.ToString().Split("\r\n");

            Assert.Equal("Imported bottle of perfume: 32.19", output[0]);
            Assert.Equal("Imported box of chocolates: 11.85", output[1]);
            Assert.Equal("Book: 37.47 (3 @ 12.49)", output[2]);
            Assert.Equal("Music CD: 16.49", output[3]);
            Assert.Equal("Chocolate bar: 0.85", output[4]);
            Assert.Equal("Sales Taxes: 6.30", output[5]);
            Assert.Equal("Total: 98.85", output[6]);
        }

        [Fact]
        public void Custom_Input2()
        {

            var input = new string[] {
                                        "1 Music CD at 14.99||n",
                                     };

            CoreService.Run(input);

            var output = _stringWriter.ToString().Split("\r\n");

            Assert.Equal("Music CD: 16.49", output[0]);
            Assert.Equal("Sales Taxes: 1.50", output[1]);
            Assert.Equal("Total: 16.49", output[2]);
        }

        [Fact]
        public void Custom_Input3()
        {

            var input = new string[] {
                                        "1 Imported bottle of perfume at 27.99||y",
                                        "2 Imported box of chocolates at 11.25|1|y",
                                     };


            CoreService.Run(input);

            var output = _stringWriter.ToString().Split("\r\n");

            Assert.Equal("Imported bottle of perfume: 32.19", output[0]);
            Assert.Equal("Imported box of chocolates: 23.70 (2 @ 11.85)", output[1]);
            Assert.Equal("Sales Taxes: 5.40", output[2]);
            Assert.Equal("Total: 55.89", output[3]);
        }

        [Fact]
        public void Custom_Input4()
        {

            var input = new string[] {
                                        "1 cd at 10.00||n",
                                        "1 cd at 11.00||n",
                                     };


            CoreService.Run(input);

            var output = _stringWriter.ToString().Split("\r\n");

            Assert.Equal("cd: 11.00", output[0]);
            Assert.Equal("cd: 12.10", output[1]);
            Assert.Equal("Sales Taxes: 2.10", output[2]);
            Assert.Equal("Total: 23.10", output[3]);
        }

        [Theory]
        [InlineData("1 Music CD  at 9.99", true)]
        [InlineData("4 Chocolate bar at 0.10", true)]
        [InlineData("2 Pack of Bacon at 1.99", true)]
        [InlineData("2 Pack of Bacon at 100.99", true)]
        [InlineData("2 Pack of Bacon at 1000.99", true)]
        [InlineData("x anything  at 3.99", false)]
        [InlineData("1 anything  at 3.9", false)]
        [InlineData("10 anything  at 3x9", false)]
        [InlineData("1anythingat32.9", false)]
        public void ValidateString(string input, bool expectedResult)
        {
            Assert.Equal(expectedResult, CoreService.ValidateString(input));
        }
    }
}
