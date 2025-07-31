using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using AppiumTestExample.Helpers;

namespace AppiumTestExample.Pages
{
    public class UserManagementPage
    {
        private readonly AndroidDriver driver;
        private readonly WebDriverWait wait;
        private readonly AppiumHelper helper;

        private readonly string searchEditTextSelector = "new UiSelector().className(\"android.widget.EditText\")";
        private readonly string deleteButtonSelector = "new UiSelector().className(\"android.widget.Button\").instance(4)";
        private readonly string confirmDeleteSelector = "new UiSelector().description(\"Xóa\")";
        private readonly string successMessageSelector = "new UiSelector().text(\"Xóa người dùng thành công\")";

        public UserManagementPage(AndroidDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
            this.helper = new AppiumHelper(driver, wait);
        }

        public bool SearchUser(string searchTerm)
        {
            try
            {
                Console.WriteLine($"Tìm kiếm người dùng với từ khóa: {searchTerm}");
                var searchEditText = wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator(searchEditTextSelector)));
                searchEditText.Click();
                searchEditText.Clear();
                searchEditText.SendKeys(searchTerm);
                Console.WriteLine($"Đã nhập từ khóa tìm kiếm: {searchTerm}");

                Thread.Sleep(3000);

                if (IsNoUserFoundMessageDisplayed())
                {
                    Console.WriteLine("Không tìm thấy người dùng nào phù hợp");
                    return false;
                }

                Console.WriteLine("Tìm thấy người dùng phù hợp");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tìm kiếm người dùng: {ex.Message}");
                throw;
            }
        }

        public void ClickDeleteButton()
        {
            try
            {
                Console.WriteLine("Nhấn nút xóa tài khoản...");
                var deleteButton = wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator(deleteButtonSelector)));
                deleteButton.Click();
                Console.WriteLine("Đã nhấn nút xóa");

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi nhấn nút xóa: {ex.Message}");
                throw;
            }
        }

        public void ConfirmDelete()
        {
            try
            {
                Console.WriteLine("Xác nhận xóa tài khoản...");
                var confirmButton = wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator(confirmDeleteSelector)));
                confirmButton.Click();
                Console.WriteLine("Đã xác nhận xóa");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xác nhận xóa: {ex.Message}");
                throw;
            }
        }

        public bool IsDeleteSuccessful()
        {
            try
            {
                Console.WriteLine("Chờ thông báo xóa thành công...");

                // Thông báo xuất hiện nhanh và biến mất, cần kiểm tra liên tục trong khoảng thời gian ngắn
                int maxAttempts = 20; // Kiểm tra 10 lần
                int delayBetweenAttempts = 50; // Mỗi lần cách nhau 100ms

                for (int i = 0; i < maxAttempts; i++)
                {
                    try
                    {
                        var successMessage = driver.FindElement(MobileBy.AndroidUIAutomator(successMessageSelector));
                        if (successMessage.Displayed)
                        {
                            Console.WriteLine("✓ Thông báo 'Xóa người dùng thành công' đã xuất hiện!");
                            return true;
                        }
                    }
                    catch
                    {
                    }

                    Thread.Sleep(delayBetweenAttempts);
                }

                Console.WriteLine("✗ Không tìm thấy thông báo xóa thành công sau 1 giây");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi kiểm tra thông báo xóa thành công: {ex.Message}");
                return false;
            }
        }

        public void DeleteUser(string searchTerm)
        {
            bool userFound = SearchUser(searchTerm);
            if (!userFound)
            {
                throw new Exception($"Không tìm thấy người dùng với từ khóa '{searchTerm}' để xóa");
            }

            ClickDeleteButton();
            ConfirmDelete();
        }

        public bool IsUserManagementPageVisible()
        {
            try
            {
                var searchBox = driver.FindElement(MobileBy.AndroidUIAutomator(searchEditTextSelector));
                return searchBox.Displayed;
            }
            catch
            {
                return false;
            }
        }

        public bool IsNoUserFoundMessageDisplayed()
        {
            try
            {
                var noUserMessage = driver.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().description(\"Không có người dùng\")"));
                bool isDisplayed = noUserMessage.Displayed;
                if (isDisplayed)
                {
                    Console.WriteLine("Hiển thị thông báo: Không có người dùng");
                }
                return isDisplayed;
            }
            catch
            {
                return false;
            }
        }
            public void ClickElementByDescription(string description)
            {
                try
                {
                    Console.WriteLine($"Nhấn vào phần tử với description: {description}");
                    var element = wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator($"new UiSelector().description(\"{description}\")")));
                    element.Click();
                    Console.WriteLine($"Đã nhấn phần tử với description: {description}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi nhấn phần tử với description '{description}': {ex.Message}");
                    throw;
                }
            }
        public bool IsElementVisibleByDescription(string description, int timeoutMs)
        {
            try
            {
                Console.WriteLine($"Kiểm tra sự xuất hiện của phần tử với description: {description}");
                var tempWait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeoutMs));
                var element = tempWait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator($"new UiSelector().description(\"{description}\")")));
                bool isDisplayed = element.Displayed;
                Console.WriteLine(isDisplayed ? $"✓ Phần tử với description '{description}' được hiển thị!" : $"✗ Phần tử với description '{description}' không được hiển thị!");
                return isDisplayed;
            }
            catch
            {
                Console.WriteLine($"✗ Không tìm thấy phần tử với description '{description}' trong {timeoutMs}ms");
                return false;
            }
        }
        public void ClickElementBySelector(string selector)
        {
            try
            {
                Console.WriteLine($"Nhấn vào phần tử với selector: {selector}");
                var element = wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator(selector)));
                element.Click();
                Console.WriteLine($"Đã nhấn phần tử với selector: {selector}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi nhấn phần tử với selector '{selector}': {ex.Message}");
                throw;
            }
        }
        public void ClearAndInputTextBySelector(string selector, string text)
        {
            try
            {
                Console.WriteLine($"Xóa và nhập '{text}' vào phần tử với selector: {selector}");
                var element = wait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator(selector)));
                element.Click();
                element.Clear();
                element.SendKeys(text);
                Console.WriteLine($"Đã nhập '{text}' vào phần tử với selector: {selector}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa và nhập text vào phần tử với selector '{selector}': {ex.Message}");
                throw;
            }
        }
        public bool IsElementVisibleByText(string text, int timeoutMs)
        {
            try
            {
                Console.WriteLine($"Kiểm tra sự xuất hiện của phần tử với text: {text}");
                var tempWait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeoutMs));
                var element = tempWait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator($"new UiSelector().text(\"{text}\")")));
                bool isDisplayed = element.Displayed;
                Console.WriteLine(isDisplayed ? $"✓ Phần tử với text '{text}' được hiển thị!" : $"✗ Phần tử với text '{text}' không được hiển thị!");
                return isDisplayed;
            }
            catch
            {
                Console.WriteLine($"✗ Không tìm thấy phần tử với text '{text}' trong {timeoutMs}ms");
                return false;
            }
        }
        public bool IsElementVisibleBySelector(string selector, int timeoutMs)
        {
            try
            {
                Console.WriteLine($"Kiểm tra sự xuất hiện của phần tử với selector: {selector}");
                var tempWait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeoutMs));
                var element = tempWait.Until(d => d.FindElement(MobileBy.AndroidUIAutomator(selector)));
                bool isDisplayed = element.Displayed;
                Console.WriteLine(isDisplayed ? $"✓ Phần tử với selector '{selector}' được hiển thị!" : $"✗ Phần tử với selector '{selector}' không được hiển thị!");
                return isDisplayed;
            }
            catch
            {
                Console.WriteLine($"✗ Không tìm thấy phần tử với selector '{selector}' trong {timeoutMs}ms");
                return false;
            }
        }
    }
}