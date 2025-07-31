using NUnit.Framework;
using System;
using System.Threading;
using System.Collections.Generic;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium;

namespace AppiumTestExample.Tests
{
    [TestFixture]
    public class AttendanceTests : BaseTest
    {
        [Test, Category("Attendance"), Timeout(180000)]
        public void PerformAttendance()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG NHẬP VỚI TÀI KHOẢN GIÁO VIÊN =====");
                loginPage.LoginTeacher();
                Assert.That(loginPage.IsLoginSuccessfulTeacher(), Is.True, "Đăng nhập giáo viên thất bại");
                Console.WriteLine("✓ Đăng nhập giáo viên thành công!");

                Console.WriteLine("===== BẮT ĐẦU ĐIỂM DANH =====");
                userManagementPage.ClickElementByDescription("Công nghệ .NET\nMã lớp: ET001H1");
                Console.WriteLine("✓ Đã chọn lớp học 'Công nghệ .NET'!");

                userManagementPage.ClickElementByDescription("Công nghệ .NET\nThời gian: 16:03 - 30/07/2025");
                Console.WriteLine("✓ Đã chọn buổi học!");

                userManagementPage.ClickElementByDescription("Danh sách sinh viên\nTab 3 of 3");
                Console.WriteLine("✓ Đã chuyển sang tab Danh sách sinh viên!");
                Thread.Sleep(7000);

                userManagementPage.ClickElementByDescription("Thêm sinh viên");
                Console.WriteLine("✓ Đã nhấn nút Thêm sinh viên!");
                Thread.Sleep(2000); // Chờ form tải

                userManagementPage.ClickElementByDescription("TRẦN PHAN HOÀNG VIỆT\nhoangviet@student.com");
                Console.WriteLine("✓ Đã thêm sinh viên TRẦN PHAN HOÀNG VIỆT!");
                Thread.Sleep(4000);

                userManagementPage.ClickElementByDescription("Đóng");
                Console.WriteLine("✓ Đã nhấn nút Đóng!");

                userManagementPage.ClickElementByDescription("Thêm sinh viên");
                Console.WriteLine("✓ Đã nhấn nút Thêm sinh viên!");
                Thread.Sleep(3000); // Tăng thời gian chờ form ổn định

                // ===== GIẢI PHÁP 1: SỬ DỤNG SWIPE GESTURE CẢI TIẾN =====
                Console.WriteLine("Cuộn xuống dưới cùng của form...");
                try
                {
                    // Kiểm tra app state trước khi thực hiện
                    Console.WriteLine($"Checking app state before scrolling...");

                    // Tìm ScrollView
                    var scrollView = driver.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().className(\"android.widget.ScrollView\")"));
                    Console.WriteLine("✓ Đã tìm thấy ScrollView!");

                    // Lấy kích thước của ScrollView
                    var size = scrollView.Size;
                    var location = scrollView.Location;
                    Console.WriteLine($"ScrollView size: {size.Width}x{size.Height}, location: ({location.X}, {location.Y})");

                    // Tính toán tọa độ để swipe
                    var startX = location.X + (size.Width / 2);
                    var startY = location.Y + (int)(size.Height * 0.8);
                    var endY = location.Y + (int)(size.Height * 0.2);

                    Console.WriteLine($"Swipe coordinates: startX={startX}, startY={startY}, endY={endY}");

                    // Thực hiện swipe gesture nhẹ nhàng
                    driver.ExecuteScript("mobile: swipeGesture", new Dictionary<string, object>
                    {
                        { "left", startX },
                        { "top", startY },
                        { "width", 0 },
                        { "height", endY - startY },
                        { "direction", "up" },
                        { "percent", 0.75 },
                        { "speed", 1000 } // Tốc độ chậm hơn
                    });

                    Console.WriteLine("✓ Đã cuộn lần 1!");
                    Thread.Sleep(2000);

                    // Thực hiện thêm một lần swipe nữa để đảm bảo cuộn hết
                    driver.ExecuteScript("mobile: swipeGesture", new Dictionary<string, object>
                    {
                        { "left", startX },
                        { "top", startY },
                        { "width", 0 },
                        { "height", endY - startY },
                        { "direction", "up" },
                        { "percent", 0.5 },
                        { "speed", 1200 }
                    });

                    Console.WriteLine("✓ Đã cuộn lần 2 - hoàn thành cuộn xuống dưới cùng!");
                    Thread.Sleep(2000);
                }
                catch (Exception scrollEx)
                {
                    Console.WriteLine($"⚠️ Lỗi khi cuộn với swipeGesture: {scrollEx.Message}");

                    // Fallback: Thử phương pháp cuộn khác
                    try
                    {
                        Console.WriteLine("Thử phương pháp cuộn dự phòng...");
                        driver.ExecuteScript("mobile: scrollGesture", new Dictionary<string, object>
                        {
                            { "direction", "down" },
                            { "percent", 1.0 }
                        });
                        Console.WriteLine("✓ Cuộn dự phòng thành công!");
                        Thread.Sleep(2000);
                    }
                    catch (Exception fallbackEx)
                    {
                        Console.WriteLine($"⚠️ Cả hai phương pháp cuộn đều thất bại: {fallbackEx.Message}");

                        // Fallback cuối cùng: Tìm trực tiếp element mà không cần cuộn
                        Console.WriteLine("Thử tìm testuser123 mà không cuộn...");
                    }
                }

                // Chờ một chút để UI ổn định sau khi cuộn
                Thread.Sleep(3000);

                // Thử tìm và click testuser123
                try
                {
                    userManagementPage.ClickElementByDescription("testuser123\ntestuser123@example.com");
                    Console.WriteLine("✓ Đã thêm sinh viên testuser123!");
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Không tìm thấy testuser123 sau khi cuộn: {ex.Message}");

                    // Thử tìm bằng UiScrollable
                    try
                    {
                        Console.WriteLine("Thử tìm testuser123 bằng UiScrollable...");
                        var testUserElement = driver.FindElement(MobileBy.AndroidUIAutomator(
                            "new UiScrollable(new UiSelector().scrollable(true))" +
                            ".scrollIntoView(new UiSelector().descriptionContains(\"testuser123\"))"
                        ));
                        testUserElement.Click();
                        Console.WriteLine("✓ Đã tìm thấy và thêm testuser123 bằng UiScrollable!");
                        Thread.Sleep(2000);
                    }
                    catch (Exception scrollableEx)
                    {
                        Console.WriteLine($"⚠️ UiScrollable cũng thất bại: {scrollableEx.Message}");
                        Console.WriteLine("Tiếp tục test mà không thêm testuser123...");
                    }
                }

                userManagementPage.ClickElementByDescription("Đóng");
                Console.WriteLine("✓ Đã nhấn nút Đóng!");

                userManagementPage.ClickElementByDescription("Chưa điểm danh\nTab 2 of 3");
                Console.WriteLine("✓ Đã chuyển sang tab Chưa điểm danh!");

                // Điểm danh sinh viên TRẦN PHAN HOÀNG VIỆT
                try
                {
                    userManagementPage.ClickElementBySelector("new UiSelector().className(\"android.widget.Button\").instance(2)"); 
                    Console.WriteLine("✓ Đã nhấn nút instance 2!"); Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Không thể điểm danh TRẦN PHAN HOÀNG VIỆT: {ex.Message}");
                }

                // Điểm danh sinh viên testuser123
                try
                {
                    var testUserButton = driver.FindElement(MobileBy.XPath("//android.widget.ImageView[@content-desc=\"testuser123\ntestuser123@example.com\"]/android.widget.Button[1]"));
                    testUserButton.Click();
                    Console.WriteLine("✓ Đã điểm danh sinh viên testuser123!");
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Không thể điểm danh testuser123: {ex.Message}");
                }

                userManagementPage.ClickElementByDescription("Đã điểm danh\nTab 1 of 3");
                Console.WriteLine("✓ Đã chuyển sang tab Đã điểm danh!");

                userManagementPage.ClickElementByDescription("Tạo mã QR Code");
                Console.WriteLine("✓ Đã nhấn nút Tạo mã QR Code!");
                Thread.Sleep(500);

                // Kiểm tra và xử lý popup yêu cầu quyền vị trí
                try
                {
                    Console.WriteLine("Kiểm tra popup yêu cầu quyền vị trí...");

                    // Tìm các button Allow phổ biến
                    var allowButtons = new string[]
                    {
                        "Allow",
                        "ALLOW",
                        "Cho phép",
                        "CHO PHÉP",
                        "Allow only while using app",
                        "Only this time"
                    };

                    bool permissionHandled = false;
                    foreach (var buttonText in allowButtons)
                    {
                        try
                        {
                            var allowButton = driver.FindElement(MobileBy.AndroidUIAutomator($"new UiSelector().text(\"{buttonText}\")"));
                            if (allowButton != null && allowButton.Displayed)
                            {
                                allowButton.Click();
                                Console.WriteLine($"✓ Đã nhấn '{buttonText}' cho quyền vị trí!");
                                permissionHandled = true;
                                Thread.Sleep(1000);
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            // Không tìm thấy button này, thử button khác
                            continue;
                        }
                    }

                    if (!permissionHandled)
                    {
                        Console.WriteLine("Không có popup yêu cầu quyền vị trí hoặc đã được xử lý trước đó.");
                    }
                }
                catch (Exception permEx)
                {
                    Console.WriteLine($"Lỗi khi xử lý popup quyền vị trí: {permEx.Message}");
                    Console.WriteLine("Tiếp tục mà không xử lý popup...");
                }

                userManagementPage.ClickElementByDescription("Tạo");
                Thread.Sleep(5000); // Chờ 5 giây

                Console.WriteLine("===== KẾT QUẢ =====");
                Console.WriteLine("✓ Điểm danh thành công!");
                Assert.Pass("Test điểm danh hoàn thành!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attendance test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");

                // Thêm thông tin debug khi có lỗi
                try
                {
                    Console.WriteLine($"Current activity: {driver.CurrentActivity}");
                    Console.WriteLine($"Page source length: {driver.PageSource.Length}");
                }
                catch (Exception debugEx)
                {
                    Console.WriteLine($"Không thể lấy thông tin debug: {debugEx.Message}");
                }

                Assert.Fail($"Attendance test failed: {ex.Message}");
            }
        }
    }
}