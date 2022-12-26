using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace sqlfunction
{
    public static class AddProduct
    {
        [FunctionName("AddProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string reqBody = await new StreamReader(req.Body).ReadToEndAsync();
            Product product = JsonConvert.DeserializeObject<Product>(reqBody);

            SqlConnection conn = GetConnection();
            conn.Open();

            string statement = $"INSERT INTO Products(ProductID,ProductName,Quantity) VALUES (@param1, @param2, @param3)";

            try
            {
                using SqlCommand cmd = new(statement, conn);
                cmd.Parameters.Add("@param1", SqlDbType.Int).Value = product.ProductID;
                cmd.Parameters.Add("@param2", SqlDbType.VarChar, 1000).Value = product.ProductName;
                cmd.Parameters.Add("@param3", SqlDbType.Int).Value = product.Quantity;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                conn.Close();
                return new OkObjectResult("Product added");
            }
            catch (Exception ex)
            {
                conn.Close();
                return new OkObjectResult(ex.Message);
            }
        }

        private static SqlConnection GetConnection()
        {
            string connectionString = "Server=tcp:gabundisaz204.database.windows.net,1433;Initial Catalog=az204db;Persist Security Info=False;User ID=gabundis;Password=Gsam_8906*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            return new SqlConnection(connectionString);
        }
    }
}
