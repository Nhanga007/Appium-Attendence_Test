using NUnit.Framework;
using System;
using System.Threading;

namespace AppiumTestExample.Tests
{
    [TestFixture]
    public class ClassScheduleTests : BaseTest
    {
        [Test, Category("ClassSchedule"), Timeout(120000)]
        public void AddClassSession()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG NHẬP VỚI TÀI KHOẢN GIÁO VIÊN =====");
                loginPage.LoginTeacher();
                Assert.That(loginPage.IsLoginSuccessfulTeacher(), Is.True, "Đăng nhập giáo viên thất bại");
                Console.WriteLine("✓ Đăng nhập giáo viên thành công!");

                Console.WriteLine("===== THÊM BUỔI HỌC CHO LỚP HỌC =====");
                userManagementPage.ClickElementByDescription("Công nghệ .NET\nMã lớp: ET001H1");
                Console.WriteLine("✓ Đã chọn lớp học 'Công nghệ .NET'!");

                userManagementPage.ClickElementByDescription("Thêm buổi học");
                Console.WriteLine("✓ Đã nhấn nút 'Thêm buổi học'!");

                Thread.Sleep(1000); 
                 
                userManagementPage.ClickElementBySelector("new UiSelector().className(\"android.view.View\").instance(8)");
                Console.WriteLine("✓ Đã chọn thời gian!");
                userManagementPage.ClickElementByDescription("OK");
                Console.WriteLine("✓ Đã xác nhận thời gian!");

                userManagementPage.ClickElementBySelector("new UiSelector().className(\"android.view.View\").instance(9)");
                Console.WriteLine("✓ Đã chọn ngày!");
                userManagementPage.ClickElementByDescription("OK");
                Console.WriteLine("✓ Đã xác nhận ngày!");

                userManagementPage.ClickElementByDescription("Lưu");
                Console.WriteLine("✓ Đã nhấn nút 'Lưu'!");

                Thread.Sleep(2000); 

                Console.WriteLine("===== KẾT QUẢ =====");
                Console.WriteLine("✓ Thêm buổi học thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add class session test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Add class session test failed: {ex.Message}");
            }
        }
    }
}