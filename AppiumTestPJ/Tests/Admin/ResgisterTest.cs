using NUnit.Framework;
using System;
using AppiumTestExample.Utilities;
using AppiumTestExample.Tests;

namespace AppiumTestPJ.Tests.Admin
{
    [TestFixture]
    public class RegisterTests : BaseTest
    {
        [Test, Category("Register"), Timeout(300000)]
        public void RegisterMultipleAccounts()
        {
            try
            {
                string csvFilePath = @"D:\HDH_HK2\TSNN\AppiumTestPJ\AppiumTestPJ\Resources\accounts.csv";
                var accounts = CsvReader.ReadAccountsFromCsv(csvFilePath);
                int successCount = 0;
                int totalAccounts = accounts.Count;

                if (totalAccounts == 0)
                {
                    Assert.Fail("No accounts loaded from CSV file.");
                }

                for (int i = 0; i < totalAccounts; i++)
                {
                    var account = accounts[i];
                    Console.WriteLine($"\n===== ĐĂNG KÝ TÀI KHOẢN {i + 1}/{totalAccounts} =====");
                    Console.WriteLine($"Username: {account["username"]}");
                    Console.WriteLine($"Email: {account["email"]}");

                    try
                    {
                        registerPage.RegisterAccount(
                            account["email"],
                            account["username"],
                            account["password"],
                            account["confrimpassword"]
                        );

                        if (registerPage.IsRegistrationSuccessful())
                        {
                            successCount++;
                            Console.WriteLine($"✓ Đăng ký tài khoản {i + 1} thành công!");
                        }
                        else
                        {
                            Console.WriteLine($"✗ Đăng ký tài khoản {i + 1} thất bại!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"✗ Lỗi khi đăng ký tài khoản {i + 1}: {ex.Message}");
                    }
                }

                Console.WriteLine($"\n===== KẾT QUẢ ĐĂNG KÝ =====");
                Console.WriteLine($"Tổng số tài khoản: {totalAccounts}");
                Console.WriteLine($"Đăng ký thành công: {successCount}");
                Console.WriteLine($"Đăng ký thất bại: {totalAccounts - successCount}");
                Console.WriteLine($"Tỷ lệ thành công: {(double)successCount / totalAccounts * 100:F1}%");

                Assert.That(successCount, Is.GreaterThanOrEqualTo(totalAccounts * 0.8),
                    $"Ít nhất 80% tài khoản phải đăng ký thành công. Thực tế: {successCount}/{totalAccounts}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Register test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Register test failed: {ex.Message}");
            }
        }

        [Test, Category("Register")]
        public void RegisterSingleAccount()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG KÝ TÀI KHOẢN ĐƠN LẺ =====");

                registerPage.RegisterAccount(
                    "testuser123@example.com",
                    "testuser123",
                    "password123",
                    "password123"
                );

                bool isSuccess = registerPage.IsRegistrationSuccessful();
                Assert.That(isSuccess, Is.True, "Đăng ký tài khoản đơn lẻ phải thành công");

                Console.WriteLine("✓ Đăng ký tài khoản đơn lẻ thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Single register test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Single register test failed: {ex.Message}");
            }
        }
    }
}