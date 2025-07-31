using NUnit.Framework;
using OpenQA.Selenium.Appium;
using System;

namespace AppiumTestExample.Tests
{
    [TestFixture]
    public class AppLaunchTests : BaseTest
    {
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
    }
}