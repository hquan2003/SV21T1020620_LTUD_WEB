using SV21T1020620.DataLayers;
using SV21T1020620.DataLayers.SQLServer;
using SV21T1020620.DomainModels;

namespace SV21T1020620.BusinessLayers
{
    public static class UserAccountService
    {
        private static readonly IUserAccountDAL employeeAccountDB;
        private static readonly IUserAccountDAL customerAccountDB;
        private static readonly IUserAccountDAL shipperAccountDB;
        static UserAccountService()
        {
            String connectionString = Configuration.ConnectionString;
            employeeAccountDB = new EmployeeAccountDAL(connectionString);
            customerAccountDB = new CustomerAccountDAL(connectionString);
        }

        public static UserAccount? Authorize(UserTypes userType, string username, string password)
        {
            if (userType == UserTypes.employee)
                return employeeAccountDB.Authorize(username, password);
            else if (userType == UserTypes.customer)
            {
                return customerAccountDB.Authorize(username, password);

            }
            else
            {
                return shipperAccountDB.Authorize(username, password);

            }

        }
        public static bool UpdateUserPassword(string username, string oldPassword, string newPassword)
        {
            // Kiểm tra tài khoản và mật khẩu cũ cho từng loại người dùng
            var user = Authorize(UserTypes.employee, username, oldPassword)
                       ?? Authorize(UserTypes.customer, username, oldPassword)
                       ?? Authorize(UserTypes.shipper, username, oldPassword);

            // Nếu không tìm thấy tài khoản hoặc mật khẩu cũ không đúng
            if (user == null)
                return false; // Tài khoản không tồn tại hoặc mật khẩu cũ không đúng


            // Cập nhật mật khẩu mới, gọi đến phương thức thay đổi mật khẩu trong cơ sở dữ liệu
            return employeeAccountDB.ChangePassword(username, newPassword);
        }

    }
    public enum UserTypes
    {
        employee,
        customer,
        shipper,

    }
}
