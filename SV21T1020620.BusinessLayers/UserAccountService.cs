using SV21T1020620.DataLayers;
using SV21T1020620.DomainModels;

namespace SV21T1020620.BusinessLayers
{
    public static class UserAccountService
    {
        private static readonly IUserAccountDAL employeeAccountDB;
        private static readonly IUserAccountDAL customerAccountDB;

        static UserAccountService()
        {
            string connectionString = Configuration.ConnectionString;

            employeeAccountDB = new DataLayers.SQLServer.EmployeeAccountDAL(connectionString);
            customerAccountDB = new DataLayers.SQLServer.CustomerAccountDAL(connectionString);
        }
        public static UserAccount? Authorize(UserTypes userType, string username, string password)
        {
            if(userType == UserTypes.Employee)
                return employeeAccountDB.Authorize(username, password);
            else
                return customerAccountDB.Authorize(username, password);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum UserTypes
    {
        Employee,
        Customer
    }
}
