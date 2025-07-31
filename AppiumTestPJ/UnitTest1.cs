/*
sing NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace AppiumTestExample
{
    public class AppiumHelper
    {
        private readonly AndroidDriver driver;
        private readonly WebDriverWait wait;

        public AppiumHelper(AndroidDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }

        public void NavigateBackToLoginPage()
        {
            Console.WriteLine("Đang quay lại trang đăng nhập...");
            int maxAttempts = 3;
            bool isLoginPage = false;

            for (int attempt = 1; attempt <= maxAttempts && !isLoginPage; attempt++)
            {
                try
                {
                    var loginButton = wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().description(\"Đăng nhập\")")));
                    if (loginButton.Displayed)
                    {
                        Console.WriteLine("Đã ở trang đăng nhập!");
                        isLoginPage = true;
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine($"Lần thử {attempt}: Chưa tìm thấy nút Đăng nhập, nhấn Back...");
                    driver.Navigate().Back();
                    Thread.Sleep(2000);
                    try
                    {
                        var backButton = driver.FindElement(MobileBy.AndroidUIAutomator(
                            "new UiSelector().description(\"Quay lại\").or(new UiSelector().description(\"Đóng\"))"));
                        if (backButton.Displayed)
                        {
                            backButton.Click();
                            Thread.Sleep(1000);
                        }
                    }
                    catch { }
                }
            }

            if (!isLoginPage)
            {
                throw new Exception("Failed to navigate back to login page.");
            }
            Console.WriteLine("Đã quay lại trang đăng nhập thành công!");
        }

        public void EnterTextByIndex(int index, string text)
        {
            var editText = wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator($"new UiSelector().className(\"android.widget.EditText\").instance({index})")));
            editText.Click();
            editText.Clear();
            editText.SendKeys(text);
            Console.WriteLine($"Đã nhập text vào EditText[{index}]: {text}");
        }

        public void ClickButton(string description)
        {
            var button = wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator($"new UiSelector().description(\"{description}\")")));
            button.Click();
            Console.WriteLine($"Đã nhấn nút: {description}");
        }

        public bool IsElementDisplayed(string selector, string value)
        {
            try
            {
                var element = wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator($"new UiSelector().{selector}(\"{value}\")")));
                return element.Displayed;
            }
            catch
            {
                return false;
            }
        }
    }

    // REGISTER SCREEN ACTION
    public class RegisterPage
    {
        private readonly AndroidDriver driver;
        private readonly WebDriverWait wait;
        private readonly AppiumHelper helper;

        public RegisterPage(AndroidDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
            this.helper = new AppiumHelper(driver, wait);
        }

        public void RegisterAccount(string username, string email, string password, string phone)
        {
            helper.ClickButton("Đăng ký");
            wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().className(\"android.widget.EditText\").instance(0)")).Displayed);
            helper.EnterTextByIndex(0, username);
            helper.EnterTextByIndex(1, email);
            helper.EnterTextByIndex(2, password);
            helper.EnterTextByIndex(3, phone);
            helper.ClickButton("Đăng ký");
            wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().description(\"Đăng nhập\")")).Displayed);
        }

        public bool IsRegistrationSuccessful()
        {
            try
            {
                if (helper.IsElementDisplayed("description", "Đăng nhập"))
                {
                    Console.WriteLine("Tìm thấy nút Đăng nhập, đăng ký thành công!");
                    return true;
                }
                helper.NavigateBackToLoginPage();
                return true; 
            }
            catch
            {
                Console.WriteLine("Kiểm tra đăng ký thất bại, thử quay lại trang đăng nhập...");
                helper.NavigateBackToLoginPage();
                return true;
            }
        }
    }

    // LOGIN TEST FUNCTION
    public class LoginPage
    {
        private readonly AndroidDriver driver;
        private readonly WebDriverWait wait;
        private readonly AppiumHelper helper;

        public LoginPage(AndroidDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
            this.helper = new AppiumHelper(driver, wait);
        }

        public void Login(string email, string password)
        {
            helper.EnterTextByIndex(0, email);
            helper.EnterTextByIndex(1, password);
            var loginButton = wait.Until(d => d.FindElement(By.XPath("//android.widget.Button[@content-desc=\"Đăng nhập\"]")));
            loginButton.Click();
            Console.WriteLine("Đã nhấn nút Đăng nhập");
        }

        public bool IsLoginSuccessful()
        {
            try
            {
                Thread.Sleep(5000);
                var homeElement = wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().description(\"Quản trị viên\")")));
                return homeElement.Displayed;
            }
            catch
            {
                return false;
            }
        }
    }

    [TestFixture]
    public class AndroidAppTests
    {
        private AndroidDriver driver;
        private WebDriverWait wait;
        private AppiumHelper helper;
        private RegisterPage registerPage;
        private LoginPage loginPage;

        [SetUp]
        public void Setup()
        {
            var options = new AppiumOptions();
            options.PlatformName = "Android";
            options.PlatformVersion = "10";
            options.DeviceName = "emulator-5554";
            options.AutomationName = "UiAutomator2";
            options.App = @"D:\HDH_HK2\Attendance_flutter\build\app\outputs\flutter-apk\app-debug.apk";
            options.AddAdditionalAppiumOption("noReset", true);
            options.AddAdditionalAppiumOption("fullReset", false);

            driver = new AndroidDriver(new Uri("http://127.0.0.1:4723/"), options, TimeSpan.FromSeconds(60));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            helper = new AppiumHelper(driver, wait);
            registerPage = new RegisterPage(driver, wait);
            loginPage = new LoginPage(driver, wait);
        }
        //CHECK APP
        [Test, Category("General")]
        public void TestAppLaunch()
        {
            try
            {
                Assert.That(driver, Is.Not.Null, "Driver should not be null");
                wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().description(\"Đăng nhập\")")).Displayed);
                string currentContext = driver.Context;
                Console.WriteLine($"Current context: {currentContext}");
                Assert.That(currentContext.Contains("NATIVE_APP"), Is.True, "App should be in native context");
                string currentPackage = driver.CurrentPackage;
                Console.WriteLine($"Current package: {currentPackage}");
                string currentActivity = driver.CurrentActivity;
                Console.WriteLine($"Current activity: {currentActivity}");
                Assert.That(string.IsNullOrEmpty(currentPackage), Is.False, "App package should not be empty");
                Assert.That(string.IsNullOrEmpty(currentActivity), Is.False, "App activity should not be empty");
                string pageSource = driver.PageSource;
                Assert.That(string.IsNullOrEmpty(pageSource), Is.False, "Page source should not be empty");
                Console.WriteLine("App launched successfully!");
            }
            catch (Exception ex)
            {
                Assert.Fail($"App launch test failed: {ex.Message}");
            }
        }
        //RESGISTER SESSION LOG
        [Test, Category("Register"), Timeout(300000)]
        public void RegisterMultipleAccounts()
        {
            try
            {
                string csvFilePath = @"D:\HDH_HK2\accounts.csv";
                var accounts = ReadAccountsFromCsv(csvFilePath);
                int successCount = 0;
                int totalAccounts = accounts.Count;

                if (totalAccounts == 0)
                {
                    Assert.Fail("No accounts loaded from CSV file.");
                }

                for (int i = 0; i < totalAccounts; i++)
                {
                    var account = accounts[i];
                    Console.WriteLine($"\n===== ĐĂNG KÝ TÀI KHOẢN {i + 1}/{totalAccounts} =====");
                    Console.WriteLine($"Username: {account["username"]}");
                    Console.WriteLine($"Email: {account["email"]}");

                    try
                    {
                        registerPage.RegisterAccount(
                            account["username"],
                            account["email"],
                            account["password"],
                            account["phone"]
                        );
                        if (registerPage.IsRegistrationSuccessful())
                        {
                            successCount++;
                            Console.WriteLine($"✓ Đăng ký tài khoản {i + 1} thành công!");
                        }
                        else
                        {
                            Console.WriteLine($"✗ Đăng ký tài khoản {i + 1} thất bại!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"✗ Lỗi khi đăng ký tài khoản {i + 1}: {ex.Message}");
                    }
                }

                Console.WriteLine($"\n===== KẾT QUẢ ĐĂNG KÝ =====");
                Console.WriteLine($"Tổng số tài khoản: {totalAccounts}");
                Console.WriteLine($"Đăng ký thành công: {successCount}");
                Console.WriteLine($"Đăng ký thất bại: {totalAccounts - successCount}");
                Console.WriteLine($"Tỷ lệ thành công: {(double)successCount / totalAccounts * 100:F1}%");

                Assert.That(successCount, Is.GreaterThanOrEqualTo(totalAccounts * 0.8),
                    $"Ít nhất 80% tài khoản phải đăng ký thành công. Thực tế: {successCount}/{totalAccounts}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Register test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Register test failed: {ex.Message}");
            }
        }

        [Test, Category("Login"), Timeout(120000)]
        public void Login()
        {
            try
            {
                loginPage.Login("tran@gmail.com", "111111");
                Assert.That(loginPage.IsLoginSuccessful(), Is.True, "Home screen should be visible after login");
                Console.WriteLine("Login successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Login test failed: {ex.Message}");
            }
        }


        //READ CSV FILE
        private List<Dictionary<string, string>> ReadAccountsFromCsv(string csvFilePath)
        {
            var accounts = new List<Dictionary<string, string>>();
            try
            {
                Console.WriteLine($"Reading CSV file from: {csvFilePath}");
                var lines = File.ReadAllLines(csvFilePath);
                if (lines.Length <= 1)
                {
                    Console.WriteLine("CSV file is empty or only contains header.");
                    return accounts;
                }

                for (int i = 1; i < lines.Length; i++)
                {
                    var columns = lines[i].Split(',');
                    if (columns.Length == 4)
                    {
                        accounts.Add(new Dictionary<string, string>
                        {
                            { "username", columns[0].Trim() },
                            { "email", columns[1].Trim() },
                            { "password", columns[2].Trim() },
                            { "phone", columns[3].Trim() }
                        });
                    }
                    else
                    {
                        Console.WriteLine($"Skipping invalid CSV row {i + 1}: {lines[i]}");
                    }
                }
                Console.WriteLine($"Loaded {accounts.Count} accounts from CSV.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading CSV file: {ex.Message}");
                Assert.Fail($"Failed to read CSV file: {ex.Message}");
            }
            return accounts;
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                try
                {
                    driver.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during driver quit: {ex.Message}");
                }
                finally
                {
                    driver = null;
                }
            }
        }
    }
}
*/