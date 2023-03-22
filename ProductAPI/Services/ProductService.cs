using Microsoft.Data.SqlClient;
using ProductAPI.Models;

namespace ProductAPI.Services
{
    public class ProductService
    {
        private SqlConnection GetConnection()
        {
            return new SqlConnection("Server=tcp:az204-appserver.database.windows.net,1433;Initial Catalog=az204-appdb;Persist Security Info=False;User ID=gabundis;Password=Gsam_8906*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        public List<Product> GetProducts()
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

            return products;
        }

        public Product GetProduct(string _productId)
        {
            int productId = int.Parse(_productId);
            SqlConnection conn = GetConnection();
            string statement = $"SELECT ProductID, ProductName, Quantity FROM Products WHERE ProductID = {productId}";
            conn.Open();

            using SqlCommand cmd = new(statement, conn);
            using SqlDataReader reader = cmd.ExecuteReader();

            reader.Read();
            Product product = new()
            {
                ProductID = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                Quantity = reader.GetInt32(2),
            };

            conn.Close();

            return product;
        }
    }
}
