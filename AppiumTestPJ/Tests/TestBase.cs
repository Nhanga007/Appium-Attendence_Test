using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using System;
using AppiumTestExample.Pages;
using AppiumTestExample.Helpers;

namespace AppiumTestExample.Tests
{
    [TestFixture]
    public abstract class BaseTest
    {
        protected AndroidDriver driver;
        protected WebDriverWait wait;
        protected AppiumHelper helper;
        protected RegisterPage registerPage;
        protected LoginPage loginPage;
        protected UserManagementPage userManagementPage;

        [SetUp]
        public virtual void Setup()
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

            // Initialize page objects and helpers
            helper = new AppiumHelper(driver, wait);
            registerPage = new RegisterPage(driver, wait);
            loginPage = new LoginPage(driver, wait);
            userManagementPage = new UserManagementPage(driver, wait);
        }

        [TearDown]
        public virtual void TearDown()
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