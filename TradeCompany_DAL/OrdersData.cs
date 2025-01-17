﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeCompany_DAL.DTOs;

namespace TradeCompany_DAL
{
    public class OrdersData
    {
        public string ConnectionString { get; set; }
        public OrdersData()
        {
            ConnectionString = null;
        }
        public OrdersData(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public OrdersDTO AddOrder(OrdersDTO ordersDTO)
        {
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec [TradeCompany_DataBase].[AddOrder] @ClientsID, @Datetime, @AddressID, @Comment";
                ordersDTO.ID = dbConnection.Query<int>(query, new
                {
                    ordersDTO.ClientsID,
                    ordersDTO.DateTime,
                    ordersDTO.AddressID,
                    ordersDTO.Comment
                }).AsList<int>()[0];
                query = "exec TradeCompany_DataBase.AddOrderList @OrderID, @ProductID, @Amount, @Price";
                foreach (OrderListsDTO orderListDTO in ordersDTO.OrderLists)
                {
                    orderListDTO.OrderID = ordersDTO.ID;
                    orderListDTO.ID = dbConnection.Query<int>(query, new
                    {
                        orderListDTO.OrderID,
                        orderListDTO.ProductID,
                        orderListDTO.Amount,
                        orderListDTO.Price
                    }).AsList<int>()[0];
                }
            }
            return ordersDTO;
        }

        public List<OrderListsDTO> AddOrderList(List<OrderListsDTO> orderListsDTO)
        {
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.AddOrderList @OrderID, @ProductID, @Amount, @Price";
                foreach (OrderListsDTO orderListDTO in orderListsDTO)
                {
                    orderListDTO.ID = dbConnection.Query<int>(query, new
                    {
                        orderListDTO.OrderID,
                        orderListDTO.ProductID,
                        orderListDTO.Amount,
                        orderListDTO.Price
                    }).AsList<int>()[0];
                }
            }
            return orderListsDTO;
        }
        public void AddSpecificProductInOrder(SpecificProductDTO specificProductDTO)
        {
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.AddOrderList @OrderID, @ProductID, @Amount, @Price";
                dbConnection.Query(query, new
                {
                    specificProductDTO.OrderID,
                    specificProductDTO.ProductID,
                    specificProductDTO.Amount,
                    specificProductDTO.Price
                });
            }
            
        }

        public void DeleteOrderByID(int id)
        {
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.DeleteOrderByID @ID";
                dbConnection.Query(query, new { id });
            }
        }
        public void DeleteOrderListByID(int id)
        {
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.DeleteOrderListByID @ID";
                dbConnection.Query(query, new { id });
            }
        }

        public List<OrdersDTO> GetOrders()
        {
            List<OrdersDTO> result = new List<OrdersDTO>();
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.GetOrders";
                dbConnection.Query<OrdersDTO, OrderListsDTO, ClientDTO, ProductDTO, OrdersDTO>(query,
                    (order, orderList, client, product) => MapsOrdersDTO(order, orderList, client, product, result));
            }
            return result;
        }

        public List<OrdersDTO> GetOrdersByID(int id)
        {
            List<OrdersDTO> result = new List<OrdersDTO>();
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.GetOrdersByID @ID";
                dbConnection.Query<OrdersDTO, OrderListsDTO, ClientDTO, ProductDTO, OrdersDTO>(query,
                    (order, orderList, client, product) => MapsOrdersDTO(order, orderList, client, product, result),
                    new { id });
            }
            return result;
        }

        public List<ProductForOrderDTO> GetProductsByOrderId(int ID)
        {
            List<ProductForOrderDTO> result = new List<ProductForOrderDTO>();
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                //query = "exec TradeCompany_DataBase.GetProductsInOrderByOrderId @ID";
                query = "exec TradeCompany_DataBase.GetProductsByOrderId @ID";
                result = dbConnection.Query<ProductForOrderDTO>(query, new { ID }).ToList();
            }
            return result;
        }

        public List<OrdersDTO> GetOrdersByParams(string client, DateTime? minDateTime, DateTime? maxDateTime, string address)
        {
            List<OrdersDTO> result = new List<OrdersDTO>();
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.GetOrdersByParams @Client, @MinDateTime, @MaxDateTime, @Address";
                dbConnection.Query<OrdersDTO, OrderListsDTO, ClientDTO, ProductDTO, OrdersDTO>(query,
                    (order, orderList, client, product) => MapsOrdersDTO(order, orderList, client, product, result),
                    new
                    {
                        client,
                        minDateTime,
                        maxDateTime,
                        address
                    });
            }
            return result;
        }

        public List<OrdersDTO> SearchOrders(string str)
        {
            List<OrdersDTO> result = new List<OrdersDTO>();
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "TradeCompany_DataBase.SearchOrders";
                dbConnection.Query<OrdersDTO, OrderListsDTO, ClientDTO, ProductDTO, OrdersDTO>(query,
                    (order, orderList, client, product) => MapsOrdersDTO(order, orderList, client, product, result),
                    new { str }, commandType: CommandType.StoredProcedure);
            }
            return result;
        }
        public void UpdateOrdersByID(OrdersDTO ordersDTO)
        {
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.UpdateOrder @ID, @ClientsID, @Datetime, @AddressID, @Comment";
                dbConnection.Query(query, new
                {
                    ordersDTO.ID,
                    ordersDTO.ClientsID,
                    ordersDTO.DateTime,
                    ordersDTO.AddressID,
                    ordersDTO.Comment
                });
                query = "exec TradeCompany_DataBase.AddOrderList @OrderID, @ProductID, @Amount, @Price";
                foreach (OrderListsDTO orderListDTO in ordersDTO.OrderLists)
                {
                    dbConnection.Query<int>(query, new
                    {
                        orderListDTO.OrderID,
                        orderListDTO.ProductID,
                        orderListDTO.Amount,
                        orderListDTO.Price
                    });
                }
            }
        }
        public OrdersDTO MapsOrdersDTO(OrdersDTO order, OrderListsDTO orderList, ClientDTO client, ProductDTO product, List<OrdersDTO> result)
        {
            OrdersDTO crntOrder = null;
            foreach (var o in result)
            {
                if (o.ID == order.ID)
                {
                    crntOrder = o;
                }
            }
            if (crntOrder == null)
            {
                crntOrder = order;
                crntOrder.ClientDTO = client;
                result.Add(crntOrder);
            }
            if (orderList != null)
            {
                orderList.productDTO = product;
                crntOrder.OrderLists.Add(orderList);
                
            }
            return order;
        }

        public List<OrdersDTO> GetOrdersByClientID(int id)
        {
            List<OrdersDTO> result = new List<OrdersDTO>();
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.GetOrderByClientID @id";
                dbConnection.Query<OrdersDTO, OrderListsDTO, ClientDTO, ProductDTO, OrdersDTO>(query,
                    (order, orderList, client, product) => MapsOrdersDTO(order, orderList, client, product, result),
                    new{ id });
            }
            return result;


        }


    }
}
