using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ScannerUnderTest = Receipt.Scanner;

namespace Receipt.Test.Scanner
{
    [TestClass]
    public class TestScanner
    {
        private class StringValueTestData
        {
            public string UnderTest { get; set; }
            public string ExpectedString { get; set; }
            public decimal ExpectedValue { get; set; }
        }

        [TestMethod]
        public void TestConvertCorrect()
        {
            var data = GetCorrectTestData();

            foreach (var testData in data)
            {
                var result = ScannerUnderTest.Scanner.Convert(testData.UnderTest);
                Assert.AreEqual(1, result.Count, "Conver should return 1 match");
                Assert.IsTrue(result.ContainsKey(testData.ExpectedString), "Name should be \"" + testData.ExpectedString + "\" but is \"" + result.Keys.First() + "\"");
                Assert.AreEqual(testData.ExpectedValue, result[testData.ExpectedString], "Value should be " + testData.ExpectedValue + " but is " + result[testData.ExpectedString]);
            }

        }

        [TestMethod]
        public void TestConvertIncorrect()
        {
            var data = GetIncorrectTestData();

            foreach (var testData in data)
            {
                var result = ScannerUnderTest.Scanner.Convert(testData.UnderTest);
                Assert.AreEqual(0, result.Count, "Conver should return no match");
            }

        }

        private IEnumerable<StringValueTestData> GetCorrectTestData()
        {
            List<StringValueTestData> data = new List<StringValueTestData>(){
                new StringValueTestData() {
                    UnderTest = "JASDFFDSA sdf 123123.41",
                    ExpectedString = "JASDFFDSA sdf",
                    ExpectedValue = 123123.41M
                },
                new StringValueTestData() {
                    UnderTest = "JASDFFDSA sdf 3123.4",
                    ExpectedString = "JASDFFDSA sdf",
                    ExpectedValue = 3123.4M
                },
                new StringValueTestData() {
                    UnderTest = "JASDFFDSA sdf 1.41",
                    ExpectedString = "JASDFFDSA sdf",
                    ExpectedValue = 1.41M
                },
                new StringValueTestData() {
                    UnderTest = "ENERGY DRINK XEOOML 2.05",
                    ExpectedString = "ENERGY DRINK XEOOML",
                    ExpectedValue = 2.05M
                },
                new StringValueTestData() {
                    UnderTest = "STRTIEGELD - U 25",
                    ExpectedString = "STRTIEGELD - U",
                    ExpectedValue = 25M
                },
                new StringValueTestData() {
                    UnderTest = "MONSTER ENERU RIPPLR 0 99",
                    ExpectedString = "MONSTER ENERU RIPPLR",
                    ExpectedValue = 0.99M
                },
                new StringValueTestData() {
                    UnderTest = "EURO HV MELK 0.59",
                    ExpectedString = "EURO HV MELK",
                    ExpectedValue = 0.59M
                },
                new StringValueTestData() {
                    UnderTest = "JUMBO 30 LTR H008 1.53",
                    ExpectedString = "JUMBO 30 LTR H008",
                    ExpectedValue = 1.53M
                },
                new StringValueTestData() {
                    UnderTest = "SMITHS NIBB~IT STICK 1.03",
                    ExpectedString = "SMITHS NIBB~IT STICK",
                    ExpectedValue = 1.03M
                },
                new StringValueTestData() {
                    UnderTest = "JUMBO SCHRRRELEI 10 P 34",
                    ExpectedString = "JUMBO SCHRRRELEI 10 P",
                    ExpectedValue = 34M
                },
            };

            return data;
        }

        private IEnumerable<StringValueTestData> GetIncorrectTestData()
        {
            List<StringValueTestData> data = new List<StringValueTestData>(){
                new StringValueTestData() {
                    UnderTest = "KIPS PEPERPRTE 1 C5"
                },
                new StringValueTestData() {
                    UnderTest = "ZRND GER SPEK PLRKJS I 4%"
                },
                new StringValueTestData() {
                    UnderTest = "1.322235"
                },
                new StringValueTestData() {
                    UnderTest = "1,23"
                },
                new StringValueTestData() {
                    UnderTest = "123."
                },
                new StringValueTestData() {
                    UnderTest = ".89"
                },
            };

            return data;
        }

    }
}
