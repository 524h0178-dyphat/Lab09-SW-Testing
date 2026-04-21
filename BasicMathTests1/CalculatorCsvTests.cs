using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;
using BasicMath; // Đảm bảo đã Add Reference project BasicMath vào project Test này

namespace BasicMathTests
{
    public class CalculatorCsvTests
    {
        // 1. Hàm đọc dữ liệu từ file CSV (Dùng chung cho cả 4 phép tính)
        public static IEnumerable<object[]> GetTestData(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                foreach (var record in csv.GetRecords<TestData>())
                {
                    yield return new object[] { record.A, record.B, record.Expected };
                }
            }
        }

        // ==========================================
        // 2. Các hàm Test nhận dữ liệu từ file CSV
        // ==========================================

        [Theory]
        [MemberData(nameof(GetTestData), parameters: "add_testdata.csv")]
        public void Add_CsvData_ReturnsCorrectSum(int a, int b, double expected)
        {
            var calculator = new BasicMaths();
            double result = calculator.Add(a, b);
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetTestData), parameters: "subtract_testdata.csv")]
        public void Subtract_CsvData_ReturnsCorrectDifference(int a, int b, double expected)
        {
            var calculator = new BasicMaths();
            double result = calculator.Subtract(a, b);
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetTestData), parameters: "multiply_testdata.csv")]
        public void Multiply_CsvData_ReturnsCorrectProduct(int a, int b, double expected)
        {
            var calculator = new BasicMaths();
            double result = calculator.Multiply(a, b);
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetTestData), parameters: "divide_testdata.csv")]
        public void Divide_CsvData_ReturnsCorrectQuotient(int a, int b, double expected)
        {
            var calculator = new BasicMaths();
            double result = calculator.Divide(a, b);
            Assert.Equal(expected, result);
        }

        // ==========================================
        // 3. Class Data Model để map cột CSV vào Code
        // ==========================================
        public class TestData
        {
            public int A { get; set; }
            public int B { get; set; }
            public double Expected { get; set; } // Dùng double để bao quát kết quả phép chia
        }
    }
}