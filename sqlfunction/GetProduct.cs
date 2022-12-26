using System;
using System.IO;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace sqlfunction
{
    public static class GetProduct
    {
        [FunctionName("GetProducts")]
        public static async Task<IActionResult> RunProducts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            SqlConnection conn = GetConnection();
            var products = new List<Product>();
            string statement = "SELECT ProductID, ProductName, Quantity FROM Products";
            conn.Open();

            using SqlCommand cmd = new(statement, conn);

            using SqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                Product product = new()
                {
                    ProductID = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    Quantity = reader.GetInt32(2),
                };

                products.Add(product);
            }

            conn.Close();

            return new OkObjectResult(JsonConvert.SerializeObject(products));
        }

        private static SqlConnection GetConnection()
        {
            string connectionString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_SQLConnectionString");
            return new SqlConnection(connectionString);
        }

        [FunctionName("GetProduct")]
        public static async Task<IActionResult> RunProduct(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            int productId = Convert.ToInt32(req.Query["id"]);

            SqlConnection conn = GetConnection();
            string statement = $"SELECT ProductID, ProductName, Quantity FROM Products WHERE ProductID = {productId}";
            conn.Open();

            using SqlCommand cmd = new(statement, conn);
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                Product product = new()
                {
                    ProductID = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    Quantity = reader.GetInt32(2),
                };

                conn.Close();

                var response = product;
                return new OkObjectResult(response);
            }
            catch (Exception)
            {
                var response = "No records found";
                conn.Close();
                return new OkObjectResult(response);
            }
        }
    }
}
