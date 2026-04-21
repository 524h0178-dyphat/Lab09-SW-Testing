using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;

namespace LoginAutomation
{
    public class LoginTests
    {
        // 1. Đọc dữ liệu tài khoản từ file CSV
        public static IEnumerable<object[]> GetTestData()
        {
            using (var reader = new StreamReader("LoginTestData.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                foreach (var record in csv.GetRecords<TestData>())
                {
                    yield return new object[] { record.Username, record.Password };
                }
            }
        }

        // 2. Kịch bản Test tự động
        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Login_WithValidCredentials_ShouldSucceed(string username, string password)
        {
            // Khởi tạo trình duyệt Chrome ẩn danh để test
            using (IWebDriver driver = new ChromeDriver())
            {
                // Thay thế bằng đường dẫn tới file login.html của bạn !!!
                driver.Navigate().GoToUrl("file:///D:/Study/HK2_2526/CNPM/Lab/Lab09/login.html");

                // Tìm ô Username và điền chữ vào
                driver.FindElement(By.Id("username")).SendKeys(username);

                // Tìm ô Password và điền chữ vào
                driver.FindElement(By.Id("password")).SendKeys(password);

                // Tìm nút Login và click
                driver.FindElement(By.Id("loginButton")).Click();

                // Dừng 2 giây để bạn kịp nhìn thấy nó tự động gõ chữ như thế nào
                System.Threading.Thread.Sleep(2000);
            }
        }

        // 3. Class Data Model
        public class TestData
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}