using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using AppiumTestExample.Helpers;

namespace AppiumTestExample.Pages
{
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
        public void LoginTeacher()
        {
            helper.EnterTextByIndex(0, "hao.teacher@gmail.com");
            helper.EnterTextByIndex(1, "111111");
            var loginButton = wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().description(\"Đăng nhập\")")));
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
        public bool IsLoginSuccessfulTeacher()
        {
            try
            {
                Console.WriteLine("Kiểm tra đăng nhập giáo viên...");
                var teacherHome = wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().description(\"Trang Chủ Giảng Viên\")")));
                bool isDisplayed = teacherHome.Displayed;
                Console.WriteLine(isDisplayed ? "✓ Đăng nhập thành công với tư cách Giảng viên" : "✗ Không tìm thấy trang chủ giảng viên");
                return isDisplayed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Lỗi khi kiểm tra đăng nhập giáo viên: {ex.Message}");
                return false;
            }
        }
    }
}