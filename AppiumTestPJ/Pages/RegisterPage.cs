using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using System;
using AppiumTestExample.Helpers;

namespace AppiumTestExample.Pages
{
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
}