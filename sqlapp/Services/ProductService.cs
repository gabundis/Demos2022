using System.Data.SqlClient;
using sqlapp.Models;

namespace sqlapp.Services
{
    public class ProductService
    {
        private static readonly string db_source = "gabundisaz204.database.windows.net";
        private static readonly string db_user = "gabundis";
        private static readonly string db_password = "Gsam_8906*";
        private static readonly string db_database = "az204db";

        private SqlConnection GetConnection()
        {
            var _builder = new SqlConnectionStringBuilder
            {
                DataSource = db_source,
                UserID = db_user,
                Password = db_password,
                InitialCatalog = db_database
            };
            return new SqlConnection(_builder.ConnectionString);
        }

        public List<Product> GetProducts()
        {
            SqlConnection conn = GetConnection();
            var products = new List<Product>();

            string statement = "SELECT ProductID, ProductName, Quantity FROM Products";

            conn.Open();

            SqlCommand cmd = new(statement, conn);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    Product product = new()
                    {
                        ProductID = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        Quantity = reader.GetInt32(2),
                    };

                    products.Add(product);
                }
            }
            conn.Close();
            return products;
        }
    }
}
