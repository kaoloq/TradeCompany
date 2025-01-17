﻿using System;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeCompany_DAL.DTOs;
using System.Data;

namespace TradeCompany_DAL
{
    public class ProductsData: ProductsDataInterface
    {
        public string ConnectionString { get; set; }
        public ProductsData()
        {
            ConnectionString = null;
        }
        public ProductsData(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<ProductDTO> GetProducts()
        {
            List<ProductDTO> products = new List<ProductDTO>();
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.GetProducts";
                dbConnection.Query<ProductDTO, ProductGroupDTO, ProductDTO>(query,
                    (product, group) =>
                    {
                        ProductDTO crntProduct = null;
                        foreach (var p in products)
                        {
                            if (p.ID == product.ID)
                            {
                                crntProduct = p;
                                break;
                            }
                        }
                        if (crntProduct is null)
                        {
                            crntProduct = product;
                            products.Add(crntProduct);
                        }
                        if (!(group is null))
                        {
                            crntProduct.Group.Add(group);
                        }
                        return crntProduct;
                    },
                    splitOn: "ID");
            }
            return products;
        }

        public List<ProductDTO> GetProductsByAllParams(string inputString, int? ProductGroupID, float? fromStockAmount, float? toStockAmount,
            float? fromWholesalePrice, float? toWholesalePrice, float? fromRetailPrice, float? toRetailPrice,
            DateTime? minDateTime, DateTime? maxDateTime)
        {
            List<ProductDTO> products = new List<ProductDTO>();
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.GetProductsByAllParams @InputString, @ProductGroupID, " +
                    "@FromStockAmount, @ToStockAmount, @FromWholesalePrice, @ToWholesalePrice, @FromRetailPrice, " +
                    "@ToRetailPrice, @MinDateTime, @MaxDateTime";
                dbConnection.Query<ProductDTO, ProductGroupDTO, ProductDTO>(query,
                    (product, group) =>
                    {
                        ProductDTO crntProduct = null;
                        foreach (var p in products)
                        {
                            if (p.ID == product.ID)
                            {
                                crntProduct = p;
                                break;
                            }
                        }
                        if (crntProduct is null)
                        {
                            crntProduct = product;
                            products.Add(crntProduct);
                        }
                        if (!(group is null))
                        {
                            crntProduct.Group.Add(group);
                        }
                        return crntProduct;
                    },
                    new
                    {
                        inputString,
                        ProductGroupID,
                        fromStockAmount,
                        toStockAmount,
                        fromWholesalePrice,
                        toWholesalePrice,
                        fromRetailPrice,
                        toRetailPrice,
                        minDateTime,
                        maxDateTime
                    },
                    splitOn: "ID");
            }
            return products;
        }

        public ProductDTO GetProductByID(int id)
        {
            List<ProductDTO> products = new List<ProductDTO>();
            ProductDTO crntProduct = null;
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.GetProductByID @ID";
                dbConnection.Query<ProductDTO, ProductGroupDTO, ProductDTO>(query,
                    (product, group) =>
                    {
                        if (crntProduct is null)
                        {
                            crntProduct = product;
                            products.Add(crntProduct);
                        }
                        if (!(group is null))
                        {
                            crntProduct.Group.Add(group);
                        }
                        return crntProduct;
                    }, new { id },
                    splitOn: "ID");
            }
            return crntProduct;
        }

        public int GetLastProductID()
        {
            int output;
            var p = new DynamicParameters();
            p.Add("Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
            string query = "TradeCompany_DataBase.GetLastProductID";
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                dbConnection.Query<int>(query, p, commandType: CommandType.StoredProcedure);
                output = p.Get<int>("Output");
            }
            return output;
        }
        public List<ProductDTO> GetProductsByLetter(string inputString)
        {
            List<ProductDTO> products = new List<ProductDTO>();
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.GetProductByLetter @InputString";
                dbConnection.Query<ProductDTO, ProductGroupDTO, ProductDTO>(query,
                    (product, group) =>
                    {
                        ProductDTO crntProduct = null;
                        foreach (var p in products)
                        {
                            if (p.ID == product.ID)
                            {
                                crntProduct = p;
                                break;
                            }
                        }
                        if (crntProduct is null)
                        {
                            crntProduct = product;
                            products.Add(crntProduct);
                        }
                        if (!(group is null))
                        {
                            crntProduct.Group.Add(group);
                        }
                        return crntProduct;
                    }, new { inputString },
                    splitOn: "ID");
            }
            return products;
        }

        public void DeleteProductByID(int id)
        {
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.DeleteProductByID @ID";
                dbConnection.Query(query, new { id });
            }
        }

        public void SoftDeleteProductByID(int id)
        {
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.SoftDeleteProductByID @ID";
                dbConnection.Query(query, new { id });
            }
        }

        public void DeleteGroupFromProduct(int id, int productGroupID)
        {
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.DeleteGroupFromProduct @ID, @ProductGroupID";
                dbConnection.Query(query, new { id, productGroupID });
            }
        }

        public void AddProduct(ProductDTO product)
        {
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.AddProduct @Name, @StockAmount, @MeasureUnit, @WholesalePrice, @RetailPrice, @LastSupplyDate, @Description, @Comments";
                dbConnection.Query<ProductDTO>(query, new
                {
                    product.Name,
                    product.StockAmount,
                    product.MeasureUnit,
                    product.WholesalePrice,
                    product.RetailPrice,
                    product.LastSupplyDate,
                    product.Description,
                    product.Comments
                }) ;
            }
        }

        public void AddProductToProductGroup(int id, int productGroupID)
        {
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.AddProductToProductGroup @ID, @ProductGroupID";
                dbConnection.Query(query, new { id, productGroupID});
            }
        }

        public void UpdateProductByID(ProductDTO product)
        {
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec TradeCompany_DataBase.UpdateProductByID @ID, @Name, @StockAmount, @MeasureUnit, @WholesalePrice, @RetailPrice, @LastSupplyDate, @Description, @Comments";
                dbConnection.Query<ProductDTO>(query, new
                {
                    product.ID,
                    product.Name,
                    product.StockAmount,
                    product.MeasureUnit,
                    product.WholesalePrice,
                    product.RetailPrice,
                    product.LastSupplyDate,
                    product.Description,
                    product.Comments
                });
            }
        }

        public List<MeasureUnitsDTO> GetAllMeasureUnits()
        {
            List<MeasureUnitsDTO> measureUnits = new List<MeasureUnitsDTO>();
            string query = "exec TradeCompany_DataBase.GetAllMeasureUnits";
            using(IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                measureUnits = dbConnection.Query<MeasureUnitsDTO>(query).AsList<MeasureUnitsDTO>();
            }
            return measureUnits;
        }
        public void ReduceProductAmountInStockByID(int id , int amount)
        {
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec [TradeCompany_DataBase].[ReduceProductAmountInStockByID] @id , @amount";
                dbConnection.Query(query, new { id , amount });
            }

        }
        public void IncreaseProductAmountInStockByID(int id, int amount)
        {
            string query;
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                query = "exec [TradeCompany_DataBase].[IncreaseProductAmountInStockByID] @id , @amount";
                dbConnection.Query(query, new { id, amount });
            }
        }
    }
}
