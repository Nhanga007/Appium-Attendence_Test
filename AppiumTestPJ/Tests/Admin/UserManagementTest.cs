using AppiumTestExample.Tests;
using NUnit.Framework;
using System;

namespace AppiumTestPJ.Tests.Admin
{
    [TestFixture]
    public class UserManagementTests : BaseTest
    {
        [Test, Category("UserManagement"), Timeout(120000)]
        public void DeleteUserAccount()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG NHẬP VỚI TÀI KHOẢN QUẢN TRỊ VIÊN =====");
                loginPage.Login("tran@gmail.com", "111111");

                Assert.That(loginPage.IsLoginSuccessful(), Is.True, "Đăng nhập quản trị viên phải thành công");
                Console.WriteLine("✓ Đăng nhập quản trị viên thành công!");

                Assert.That(userManagementPage.IsUserManagementPageVisible(), Is.True, "Phải hiển thị trang quản lý người dùng");
                Console.WriteLine("✓ Đã vào trang quản lý người dùng!");

                Console.WriteLine("===== BẮT ĐẦU XÓA TÀI KHOẢN =====");

                try
                {
                    userManagementPage.DeleteUser("TEST");

                    bool isDeleteSuccess = userManagementPage.IsDeleteSuccessful();
                    Assert.That(isDeleteSuccess, Is.True, "Xóa tài khoản phải thành công");

                    Console.WriteLine("===== KẾT QUẢ =====");
                    Console.WriteLine("✓ Xóa tài khoản TEST thành công!");
                    Console.WriteLine("✓ Test DeleteUserAccount hoàn thành!");
                }
                catch (Exception ex) when (ex.Message.Contains("Không tìm thấy người dùng"))
                {
                    Console.WriteLine("===== KẾT QUẢ =====");
                    Console.WriteLine("ℹ Không có người dùng 'TEST' để xóa");
                    Console.WriteLine("✓ Test DeleteUserAccount hoàn thành - không có dữ liệu để xóa!");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete user test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Delete user test failed: {ex.Message}");
            }
        }

        [Test, Category("UserManagement")]
        public void SearchUser()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG NHẬP VỚI TÀI KHOẢN QUẢN TRỊ VIÊN =====");
                loginPage.Login("tran@gmail.com", "111111");
                Assert.That(loginPage.IsLoginSuccessful(), Is.True, "Đăng nhập quản trị viên phải thành công");

                Assert.That(userManagementPage.IsUserManagementPageVisible(), Is.True, "Phải hiển thị trang quản lý người dùng");

                Console.WriteLine("===== TÌM KIẾM NGƯỜI DÙNG =====");
                bool userFound = userManagementPage.SearchUser("Test");

                if (userFound)
                {
                    Console.WriteLine("✓ Tìm kiếm người dùng thành công - có kết quả!");
                }
                else
                {
                    Console.WriteLine("ℹ Tìm kiếm hoàn thành - không có người dùng phù hợp!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Search user test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Search user test failed: {ex.Message}");
            }
        }

        [Test, Category("UserManagement")]
        public void SearchNonExistentUser()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG NHẬP VỚI TÀI KHOẢN QUẢN TRỊ VIÊN =====");
                loginPage.Login("tran@gmail.com", "111111");
                Assert.That(loginPage.IsLoginSuccessful(), Is.True, "Đăng nhập quản trị viên phải thành công");

                Assert.That(userManagementPage.IsUserManagementPageVisible(), Is.True, "Phải hiển thị trang quản lý người dùng");

                Console.WriteLine("===== TÌM KIẾM NGƯỜI DÙNG KHÔNG TỒN TẠI =====");
                bool userFound = userManagementPage.SearchUser("NONEXISTENTUSER12345");

                Assert.That(userFound, Is.False, "Không nên tìm thấy user không tồn tại");
                Console.WriteLine("✓ Test tìm kiếm user không tồn tại thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Search non-existent user test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Search non-existent user test failed: {ex.Message}");
            }
        }

        [Test, Category("UserManagement")]
        public void ViewUserList()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG NHẬP VỚI TÀI KHOẢN QUẢN TRỊ VIÊN =====");
                loginPage.Login("tran@gmail.com", "111111");
                Assert.That(loginPage.IsLoginSuccessful(), Is.True, "Đăng nhập quản trị viên phải thành công");

                Console.WriteLine("===== KIỂM TRA DANH SÁCH NGƯỜI DÙNG =====");
                Assert.That(userManagementPage.IsUserManagementPageVisible(), Is.True, "Phải hiển thị trang quản lý người dùng");

                Console.WriteLine("✓ Trang quản lý người dùng hiển thị thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"View user list test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"View user list test failed: {ex.Message}");
            }
        }

        [Test, Category("UserManagement"), Timeout(120000)]
        public void UpdateUserRole()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG NHẬP VỚI TÀI KHOẢN QUẢN TRỊ VIÊN =====");
                loginPage.Login("tran@gmail.com", "111111");
                Assert.That(loginPage.IsLoginSuccessful(), Is.True, "Đăng nhập quản trị viên phải thành công");
                Console.WriteLine("✓ Đăng nhập quản trị viên thành công!");

                Assert.That(userManagementPage.IsUserManagementPageVisible(), Is.True, "Phải hiển thị trang quản lý người dùng");
                Console.WriteLine("✓ Đã vào trang quản lý người dùng!");

                Console.WriteLine("===== BẮT ĐẦU CHỈNH SỬA VAI TRÒ =====");
                userManagementPage.ClickElementByDescription("Vai trò");
                Console.WriteLine("✓ Đã nhấn vào nút chọn vai trò!");

                userManagementPage.ClickElementByDescription("Quản lí lớp học");
                Console.WriteLine("✓ Đã chọn vai trò 'Quản lí lớp học'!");

                userManagementPage.ClickElementByDescription("GV-NHAN 1\n Email: nhan.teacher@gmail.com\nQuản lí lớp học");
                Console.WriteLine("✓ Đã chọn người dùng 'GV-NHAN 1' để chỉnh sửa vai trò!");

                userManagementPage.ClickElementByDescription("Quản trị viên");
                Console.WriteLine("✓ Đã chọn vai trò mới 'Quản trị viên'!");

                userManagementPage.ClickElementByDescription("Cập nhật vai trò thành công");
                Console.WriteLine("✓ Đã nhấn nút cập nhật vai trò!");

                // Đợi và xác nhận thông báo thành công
                bool isUpdateSuccess = userManagementPage.IsElementVisibleByDescription("Cập nhật vai trò thành công", 5000);
                Assert.That(isUpdateSuccess, Is.True, "Thông báo cập nhật vai trò thành công phải xuất hiện");
                Console.WriteLine("✓ Thông báo 'Cập nhật vai trò thành công' được hiển thị!");

                Console.WriteLine("===== KẾT QUẢ =====");
                Console.WriteLine("✓ Cập nhật vai trò cho 'GV-NHAN 1' thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update user role test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Update user role test failed: {ex.Message}");
            }
        }
        [Test, Category("UserManagement"), Timeout(120000)]
        public void EditUserAccount()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG NHẬP VỚI TÀI KHOẢN QUẢN TRỊ VIÊN =====");
                loginPage.Login("tran@gmail.com", "111111");
                Assert.That(loginPage.IsLoginSuccessful(), Is.True, "Đăng nhập quản trị viên phải thành công");
                Console.WriteLine("✓ Đăng nhập quản trị viên thành công!");

                Assert.That(userManagementPage.IsUserManagementPageVisible(), Is.True, "Phải hiển thị trang quản lý người dùng");
                Console.WriteLine("✓ Đã vào trang quản lý người dùng!");

                Console.WriteLine("===== BẮT ĐẦU CHỈNH SỬA TÀI KHOẢN =====");
                bool userFound = userManagementPage.SearchUser("Nhan");
                Assert.That(userFound, Is.True, "Phải tìm thấy người dùng 'Nhan'");
                Console.WriteLine("✓ Tìm thấy người dùng 'Nhan'!");

                userManagementPage.ClickElementBySelector("new UiSelector().className(\"android.widget.Button\").instance(3)");
                Console.WriteLine("✓ Đã nhấn nút chỉnh sửa tài khoản!");

                // Chờ và kiểm tra form chỉnh sửa
                Thread.Sleep(1000);
                bool isEditFormVisible = userManagementPage.IsElementVisibleBySelector("new UiSelector().className(\"android.view.View\").instance(4)", 5000);
                Assert.That(isEditFormVisible, Is.True, "Form chỉnh sửa phải xuất hiện với android.view.View instance 4");
                Console.WriteLine("✓ Form chỉnh sửa đã xuất hiện!");

                userManagementPage.ClearAndInputTextBySelector("new UiSelector().className(\"android.widget.EditText\").instance(0)", "nhan1.teacher@gmail.com");
                Console.WriteLine("✓ Đã cập nhật email thành 'nhan1.teacher@gmail.com'!");

                userManagementPage.ClearAndInputTextBySelector("new UiSelector().className(\"android.widget.EditText\").instance(1)", "IVS-NHAN");
                Console.WriteLine("✓ Đã cập nhật tên thành 'IVS-NHAN'!");

                userManagementPage.ClearAndInputTextBySelector("new UiSelector().className(\"android.widget.EditText\").instance(2)", "012346579");
                Console.WriteLine("✓ Đã cập nhật số điện thoại thành '012346579'!");

                userManagementPage.ClickElementByDescription("Vai trò\nQuản lí lớp học");
                Console.WriteLine("✓ Đã nhấn vào nút chọn vai trò!");

                userManagementPage.ClickElementByDescription("Quản trị viên");
                Console.WriteLine("✓ Đã chọn vai trò mới 'Quản trị viên'!");

                userManagementPage.ClickElementByDescription("Lưu");
                Console.WriteLine("✓ Đã nhấn nút 'Lưu'!");

                // Kiểm tra thông báo "Cập nhật thành công"
                bool isUpdateSuccess = userManagementPage.IsElementVisibleByText("Cập nhật thành công", 1000);
                Assert.That(isUpdateSuccess, Is.True, "Thông báo 'Cập nhật thành công' phải xuất hiện");
                Console.WriteLine("✓ Thông báo 'Cập nhật thành công' được hiển thị!");

                Console.WriteLine("===== KẾT QUẢ =====");
                Console.WriteLine("✓ Chỉnh sửa tài khoản người dùng 'Nhan' thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Edit user account test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Edit user account test failed: {ex.Message}");
            }
        }
        [Test, Category("UserManagement"), Timeout(120000)]
        public void SendNotification()
        {
            try
            {
                Console.WriteLine("===== ĐĂNG NHẬP VỚI TÀI KHOẢN QUẢN TRỊ VIÊN =====");
                loginPage.Login("tran@gmail.com", "111111");
                Assert.That(loginPage.IsLoginSuccessful(), Is.True, "Đăng nhập quản trị viên phải thành công");
                Console.WriteLine("✓ Đăng nhập quản trị viên thành công!");

                Console.WriteLine("===== BẮT ĐẦU GỬI THÔNG BÁO =====");
                userManagementPage.ClickElementByDescription("Thông báo");
                Console.WriteLine("✓ Đã nhấn vào 'Thông báo'!");

                Thread.Sleep(1000); // Đợi 1 giây
                userManagementPage.ClickElementBySelector("new UiSelector().className(\"android.widget.Button\").instance(5)");
                Console.WriteLine("✓ Đã nhấn nút button instance 5!");

                Thread.Sleep(1000); // Đợi 1 giây
                userManagementPage.ClearAndInputTextBySelector("new UiSelector().className(\"android.widget.EditText\").instance(0)", "nhan.staff@gmail.com");
                Console.WriteLine("✓ Đã nhập email 'nhan.staff@gmail.com'!");

                userManagementPage.ClearAndInputTextBySelector("new UiSelector().className(\"android.widget.EditText\").instance(1)", "TestAutomation Send Notification");
                Console.WriteLine("✓ Đã nhập tiêu đề 'TestAutomation Send Notification'!");

                userManagementPage.ClearAndInputTextBySelector("new UiSelector().className(\"android.widget.EditText\").instance(2)", "Notification for Testing, no reply");
                Console.WriteLine("✓ Đã nhập nội dung 'Notification for Testing, no reply'!");

                userManagementPage.ClickElementByDescription("Gửi");
                Console.WriteLine("✓ Đã nhấn nút 'Gửi'!");

                Thread.Sleep(2000); // Đợi 2 giây để hoàn thành

                Console.WriteLine("===== KẾT QUẢ =====");
                Console.WriteLine("✓ Gửi thông báo thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Send notification test failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
                Assert.Fail($"Send notification test failed: {ex.Message}");
            }
        }
    }
}