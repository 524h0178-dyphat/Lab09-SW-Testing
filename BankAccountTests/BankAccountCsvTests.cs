using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;
using CsvHelper;
using BankAccountApp; // Đừng quên using project chính

namespace BankAccountTests
{
    public class BankAccountCsvTests
    {
        public static IEnumerable<object[]> GetTestData()
        {
            using (var reader = new StreamReader("BankAccountTestData.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                foreach (var record in csv.GetRecords<TestData>())
                {
                    yield return new object[]
                    {
                        record.CustomerName,
                        record.InitialBalance,
                        record.DebitAmount,
                        record.ExpectedBalance
                    };
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Debit_CsvData_UpdatesBalance(string customerName, decimal initialBalance, decimal debitAmount, string expectedBalance)
        {
            // Arrange
            var account = new BankAccount(customerName, initialBalance);

            // Act & Assert
            if (expectedBalance == "Insufficient funds")
            {
                // Bắt buộc phải văng lỗi InvalidOperationException mới tính là Pass
                Assert.Throws<InvalidOperationException>(() => account.Debit(debitAmount));
            }
            else
            {
                // Nếu không phải là chuỗi báo lỗi, thì ép kiểu về số thập phân (decimal) và so sánh
                account.Debit(debitAmount);
                Assert.Equal(decimal.Parse(expectedBalance), account.Balance);
            }
        }

        public class TestData
        {
            public string CustomerName { get; set; }
            public decimal InitialBalance { get; set; }
            public decimal DebitAmount { get; set; }
            public string ExpectedBalance { get; set; }
        }
    }
}