using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace AppiumTestExample.Helpers
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
}