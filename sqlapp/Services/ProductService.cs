using System.Data.SqlClient;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.FeatureManagement;
using MySql.Data.MySqlClient;
using sqlapp.Models;

namespace sqlapp.Services
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        //private readonly IFeatureManager _featureManager;

        public ProductService(
            IConfiguration configuration
            //IFeatureManager featureManager
        )
        {
            _configuration = configuration;
            //_featureManager = featureManager;
        }

        //public async Task<bool> IsBeta()
        //{
        //    if (await _featureManager.IsEnabledAsync("beta"))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        private SqlConnection GetConnection()
        {
            //var _builder = new SqlConnectionStringBuilder
            //{
            //    DataSource = db_source,
            //    UserID = db_user,
            //    Password = db_password,
            //    InitialCatalog = db_database
            //};
            //return new SqlConnection("Server=tcp:gabundisaz204.database.windows.net,1433;Initial Catalog=az204db;Persist Security Info=False;User ID=gabundis;Password=Gsam_8906*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            //return new SqlConnection(_configuration.GetConnectionString("SQLConnection")); //WebApp Configuration
            return new SqlConnection(_configuration["SQLConnection"]); // Azure Configuration
        }

        //private MySqlConnection GetConnection()
        //{
        //    return new MySqlConnection("Server=localhost;Port=3306;Database=az204db;Uid=root;Pwd=Gsam_8906*;SslMode=Preferred;");
        //}

        public List<Product> GetProducts()
        {
            //MySqlConnection conn = GetConnection();
            SqlConnection conn = GetConnection();
            var products = new List<Product>();
            string statement = "SELECT ProductID, ProductName, Quantity FROM Products";
            conn.Open();

            //using MySqlCommand cmd = new(statement, conn);
            using SqlCommand cmd = new(statement, conn);

            //using MySqlDataReader reader = cmd.ExecuteReader();
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

            return products;
        }

        //public async Task<List<Product>> GetProducts()
        //{
        //    string FunctionURL = "https://gabundisaz204functionapp.azurewebsites.net/api/GetProducts?code=v953a6FlgoxF3GmIP_XcAiG7XACfT2bzClx1xgevIUzSAzFuHgehdg==";

        //    using HttpClient client = new();
        //    HttpResponseMessage response = await client.GetAsync(FunctionURL);

        //    string content = await response.Content.ReadAsStringAsync();

        //    return JsonSerializer.Deserialize<List<Product>>(content);
        //}
    }
}
