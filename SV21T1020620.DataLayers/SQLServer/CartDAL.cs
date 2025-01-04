﻿using Dapper;
using SV21T1020620.DomainModels;

namespace SV21T1020620.DataLayers.SQLServer
{
    public class CartDAL : BaseDAL, ICartDAL
    {
        public CartDAL(string connectString) : base(connectString)
        {
        }

        public int Add(Cart Cart)
        {
            int id = 0;

            using (var connection = OpenConnection())
            {
                var sql = @"insert into Carts(Count, CustomerID)
                                    values(@Count, @CustomerID);
                                    select SCOPE_IDENTITY();";
                var parameters = new
                {
                    Count = Cart.Count,
                    CustomerID = Cart.CustomerID
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int AddCartDetail(Cartdetail data)
        {
            int id = 0;

            using (var connection = OpenConnection())
            {
                var sql = @"insert into Cartdetails(Quantity, Price, CartID, ProductID)
                                    values(@Quantity, @Price, @CartID, @ProductID);
                                    select SCOPE_IDENTITY();";
                var parameters = new
                {
                    Quantity = data.Quantity,
                    Price = data.Price,
                    CartID = data.CartID,
                    ProductID = data.ProductID

                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public Cartdetail? checkProductExists(int CartID, int productID)
        {
            Cartdetail? data = new Cartdetail();

            using (var connection = OpenConnection())
            {
                var sql = @"select * from Cartdetails where CartID = @CartID and ProductID = @ProductID";
                var parameters = new
                {
                    CartID = CartID,
                    ProductID = productID

                };
                data = connection.QueryFirstOrDefault<Cartdetail>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;

        }

        public int Count(int customerID)
        {
            int count = 0;
            using (var connection = OpenConnection())
            {
                string sql = @" select count(*)
                                from Carts
                                where CustomerID = @CustomerID;
                               ";
                var parameters = new { CustomerID = customerID };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public int Delete(int customerID)
        {
            throw new NotImplementedException();
        }

        public int DeleteAll(int customerID)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDetail(int cartID, int productID)
        {
            throw new NotImplementedException();
        }

        public Cart Get(int customerID)
        {
            var cart = new Cart();
            using (var connection = OpenConnection())
            {
                string sql = @" select * from Carts
                                where CustomerID = @CustomerID";
                var parameters = new
                {
                    CustomerID = customerID
                };
                cart = connection.QueryFirstOrDefault<Cart>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return cart;
        }

        public Cartdetail GetDetail(int cartID, int productID)
        {
            throw new NotImplementedException();
        }

        public List<Cartdetail> GetDetailList(int cartID)
        {
            throw new NotImplementedException();
        }

        public bool SaveCart(int customerID, int sum)
        {
            throw new NotImplementedException();
        }

        public bool SaveDetail(int CartID, int ProductID, int Quantity)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"update Cartdetails set Quantity = @Quantity 
                            where CartID = @CartID and ProductID = @ProductID";
                var parameters = new
                {
                    Quantity = Quantity,
                    CartID = CartID,
                    ProductID = ProductID

                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool Update(Cart Cart)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"update Carts set Count = @Count 
                            where CustomerID = @CustomerID and CartID = @CartID";
                var parameters = new
                {
                    Count = Cart.Count,
                    CustomerID = Cart.CustomerID,
                    CartID = Cart.CartID

                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
        public List<ViewCart> GetViewCarts(int userID)
        {
            List<ViewCart> viewCarts = new List<ViewCart>();
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT TOP (1000) [CartID]
                              ,[CartdetailID]
                              ,[ProductID]
                              ,[CustomerID]
                              ,[ProductName]
                              ,[Photo]
                              ,[Quantity]
                              ,[Price]
                              ,[Total]
                          FROM [LiteCommerceDB].[dbo].[ViewCart]
                          Where CustomerID = @CútomẻID";
                var parameters = new
                {


                };

            }
            return viewCarts;
        }
    }
}
