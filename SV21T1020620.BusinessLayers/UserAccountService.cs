using SV21T1020620.DataLayers;
using SV21T1020620.DataLayers.SQLServer;
using SV21T1020620.DomainModels;

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
        public static bool ChangePassword(string userName, string oldPass, string newPass)
        {
            return employeeAccountDB.ChangePassword(userName, oldPass, newPass);
        }

        public enum UserTypes
        {
            Employee,
            Customer
        }

    }
}
