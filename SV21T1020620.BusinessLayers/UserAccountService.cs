using SV21T1020620.DataLayers;
using SV21T1020620.DataLayers.SQLServer;
using SV21T1020620.DomainModels;
using static SV21T1020620.BusinessLayers.UserAccountService;

namespace SV21T1020620.BusinessLayers
{
    public class UserAccountService
    {
        private static readonly IUserAccountDAL employeeAccountDB;
        private static readonly IUserAccountDAL customerAccountDB;

        static UserAccountService()
        {
            String connectionString = Configuration.ConnectionString;
            employeeAccountDB = new EmployeeAccountDAL(connectionString);
            customerAccountDB = new CustomerAccountDAL(connectionString);
        }

        public static UserAccount? Authorize(UserTypes userTypes, string username, string password)
        {
            if (userTypes == UserTypes.Employee)
            {
                return employeeAccountDB.Authorize(username, password);
            }
            else
            {
                return customerAccountDB.Authorize(username, password);
            }
        }
        public static bool ChangePassword(UserTypes userTypes, string userName, string oldPassword, string newPassword)
        {
            if (userTypes == UserTypes.Employee)
            {
                return employeeAccountDB.ChangePassword(userName, oldPassword, newPassword);
            }
            else
            {
                return customerAccountDB.ChangePassword(userName, oldPassword, newPassword);
            }
        }

        public enum UserTypes
        {
            Employee,
            Customer
        }

    }
}
