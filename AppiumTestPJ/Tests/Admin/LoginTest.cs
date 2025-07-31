using AppiumTestExample.Tests;
using NUnit.Framework;
using System;

namespace AppiumTestPJ.Tests.Admin
{
    [TestFixture]
    public class LoginTests : BaseTest
    {
        [Test, Category("Login"), Timeout(120000)]
        public void LoginWithValidCredentials()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG NHẬP VỚI TÀI KHOẢN HỢP LỆ =====");
                loginPage.Login("tran@gmail.com", "111111");
                Assert.That(loginPage.IsLoginSuccessful(), Is.True, "Home screen should be visible after login");
                Console.WriteLine("✓ Đăng nhập thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Login test failed: {ex.Message}");
            }
        }

        [Test, Category("Login")]
        public void LoginWithInvalidCredentials()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG NHẬP VỚI TÀI KHOẢN KHÔNG HỢP LỆ =====");
                loginPage.Login("invalid@gmail.com", "wrongpassword");

                // Đăng nhập với thông tin sai phải thất bại
                Assert.That(loginPage.IsLoginSuccessful(), Is.False, "Login should fail with invalid credentials");
                Console.WriteLine("✓ Đăng nhập thất bại như mong đợi với thông tin không hợp lệ!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Invalid login test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Invalid login test failed: {ex.Message}");
            }
        }

        [Test, Category("Login")]
        public void LoginWithEmptyCredentials()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG NHẬP VỚI THÔNG TIN TRỐNG =====");
                loginPage.Login("", "");

                // Đăng nhập với thông tin trống phải thất bại
                Assert.That(loginPage.IsLoginSuccessful(), Is.False, "Login should fail with empty credentials");
                Console.WriteLine("✓ Đăng nhập thất bại như mong đợi với thông tin trống!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Empty login test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Empty login test failed: {ex.Message}");
            }
        }

        [Test, Category("Login")]
        public void LoginAsAdmin()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG NHẬP VỚI TÀI KHOẢN QUẢN TRỊ VIÊN =====");
                loginPage.Login("tran@gmail.com", "111111");

                Assert.That(loginPage.IsLoginSuccessful(), Is.True, "Admin login should be successful");
                Console.WriteLine("✓ Đăng nhập quản trị viên thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Admin login test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Admin login test failed: {ex.Message}");
            }
        }
    }
}