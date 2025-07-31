using NUnit.Framework;
using System;

namespace AppiumTestExample.Tests
{
    [TestFixture]
    public class LoginTeacherTest : BaseTest
    {
        [Test, Category("TeacherLogin"), Timeout(60000)]
        public void LoginTeacher()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG NHẬP VỚI TÀI KHOẢN HỢP LỆ =====");
                loginPage.LoginTeacher();
                Assert.That(loginPage.IsLoginSuccessfulTeacher(), Is.True, "Home screen should be visible after login");
                Console.WriteLine("✓ Đăng nhập thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Login test failed: {ex.Message}");
            }
        }
    }
}