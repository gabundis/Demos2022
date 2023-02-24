using Azure;
using Azure.Data.Tables;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=az204-cosmos-table-api;AccountKey=aYB490SvmMmHSEBnSxOhx2sghaem14cjjWutF4c6SUYtrb0Pk34cA7h9k9lSNAwP9dsynVZJWlN8ACDbOSCtLQ==;TableEndpoint=https://az204-cosmos-table-api.table.cosmos.azure.com:443/;";
string tableName = "Orders";

AddEntity("O1", "Mobile", 100);
AddEntity("O2", "Laptop", 50);
AddEntity("O3", "Desktop", 70);
AddEntity("O4", "Laptop", 200);

//QueryEntity("Laptop");

//DeleteEntity("Laptop", "O2");

//UpdateEntity("O3", "Desktop", 300);

void AddEntity(string orderId, string category, int quantity)
{
    TableClient tableClient = new(connectionString, tableName);

    TableEntity entity = new(category, orderId)
    {
        { "quantity", quantity }
    };

    tableClient.AddEntity(entity);
    Console.WriteLine($"Entity added with order Id {orderId}");
}

void QueryEntity(string category)
{
    TableClient tableClient = new(connectionString, tableName);

    Pageable<TableEntity> results = tableClient.Query<TableEntity>(ent => ent.PartitionKey == category);

    foreach (TableEntity tblEntity in results)
    {
        int quantity = tblEntity.GetInt32("quantity").GetValueOrDefault();
        Console.WriteLine($"Order Id {tblEntity.RowKey}");
        Console.WriteLine($"Quantity is {quantity}");
    }
}

void DeleteEntity(string category, string orderId)
{
    TableClient tableClient = new(connectionString, tableName);

    tableClient.DeleteEntity(category, orderId);

    Console.WriteLine("The entity is deleted");
}

void UpdateEntity(string orderId, string category, int quantity)
{
    TableClient tableClient = new(connectionString, tableName);

    TableEntity order = tableClient.GetEntity<TableEntity>(category, orderId).Value;

    order.Add("quantity", quantity);

    tableClient.UpdateEntity(order, ETag.All, TableUpdateMode.Replace);

    Console.WriteLine("The entity is updated");
}