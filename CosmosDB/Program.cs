
#region CosmosDB

using CosmosDB;
using Microsoft.Azure.Cosmos;

string cosmosEndpointUri = "https://az204-cosmos-db-core.documents.azure.com:443/";
string cosmosDBKey = "HVXL2r1ZN6QFsiFmAgNCcE7Zyct075oECjjwDmINIlb1dUVa0B6j3Z9UAFe8cpX7Or3nebSo6gjHACDbnID97g==";
string databaseName = "appdb";
string containerName = "Customers";

#region Create Database and Container

//await CreateDatabase("appdb");
//await CreateContainer(databaseName, "Customers", "/customerCity");

async Task CreateDatabase(string databaseName)
{
    CosmosClient cosmosClient = new(cosmosEndpointUri, cosmosDBKey);

    await cosmosClient.CreateDatabaseAsync(databaseName);
    Console.WriteLine("Database created!");
}

async Task CreateContainer(string databaseName, string containerName, string partitionKey)
{
    CosmosClient cosmosClient = new(cosmosEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);

    await database.CreateContainerAsync(containerName, partitionKey);

    Console.WriteLine("Container created!");
}

#endregion Create Database and Container

#region Add items

#region Orders

//await AddOrder("O1", "Laptop", 100);
//await AddOrder("O2", "Mobiles", 200);
//await AddOrder("O3", "Desktop", 75);
//await AddOrder("O4", "Laptop", 25);

async Task AddOrder(string orderId, string category, int quantity)
{
    CosmosClient cosmosClient = new(cosmosEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);

    Order order = new()
    {
        id = Guid.NewGuid().ToString(),
        orderId = orderId,
        category = category,
        quantity = quantity
    };

    ItemResponse<Order> response = await container.CreateItemAsync<Order>(order, new PartitionKey(category));

    Console.WriteLine($"Item added with Id {orderId}");
    Console.WriteLine($"Request units {response.RequestCharge}");
}

#endregion Orders

#region Customers

//await AddCustomer("C1", "CustomerA", "New York",
//    new List<Order>
//    {
//        new Order
//        {
//            orderId = "O1",
//            category = "Laptop",
//            quantity = 100
//        },
//        new Order
//        {
//            orderId = "O3",
//            category = "Desktop",
//            quantity = 75
//        }
//    });

//await AddCustomer("C2", "CustomerB", "Chicago",
//    new List<Order>
//    {
//        new Order
//        {
//            orderId = "O2",
//            category = "Mobiles",
//            quantity = 200
//        }
//    });

//await AddCustomer("C3", "CustomerC", "Florida",
//    new List<Order>
//    {
//        new Order
//        {
//            orderId = "O4",
//            category = "Laptop",
//            quantity = 25
//        }
//    });

async Task AddCustomer(string customerId, string customerName, string customerCity, List<Order> orders)
{
    CosmosClient cosmosClient = new(cosmosEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);

    Customer customer = new()
    {
        customerId = customerId,
        customerName = customerName,
        customerCity = customerCity,
        orders = orders
    };

    ItemResponse<Customer> response = await container.CreateItemAsync<Customer>(customer, new PartitionKey(customerCity));

    Console.WriteLine($"Item added with Id {customerId}");
    Console.WriteLine($"Request units {response.RequestCharge}");
}

#endregion Customers

#endregion Add items

#region Read items

#region Orders

//await ReadOrderItem();

async Task ReadOrderItem()
{
    CosmosClient cosmosClient = new(cosmosEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);

    string sqlQuery = "SELECT o.orderId, o.category, o.quantity FROM Orders o";

    QueryDefinition queryDef = new(sqlQuery);

    FeedIterator<Order> feedIterator = container.GetItemQueryIterator<Order>(queryDef);

    while (feedIterator.HasMoreResults)
    {
        FeedResponse<Order> orders = await feedIterator.ReadNextAsync();
        foreach (Order order in orders)
        {
            Console.WriteLine($"Order Id {order.orderId}");
            Console.WriteLine($"Category {order.category}");
            Console.WriteLine($"Quantity {order.quantity}");
        }
    }
}

#endregion Orders

#region Customers

//await ReadCustomerItem();

async Task ReadCustomerItem()
{
    CosmosClient cosmosClient = new(cosmosEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);

    string sqlQuery = "SELECT c.id, c.customerName, c.customerCity, c.orders FROM Customers c";

    QueryDefinition queryDef = new(sqlQuery);

    FeedIterator<Customer> feedIterator = container.GetItemQueryIterator<Customer>(queryDef);

    while (feedIterator.HasMoreResults)
    {
        FeedResponse<Customer> customers = await feedIterator.ReadNextAsync();
        foreach (Customer customer in customers)
        {
            Console.WriteLine($"Customer Id {customer.customerId}");
            Console.WriteLine($"Customer name {customer.customerName}");

            Console.WriteLine($"Customer city {customer.customerCity}");

            foreach (Order order in customer.orders)
            {
                Console.WriteLine($"\tOrder Id {order.orderId}");
                Console.WriteLine($"\tCategory {order.category}");
                Console.WriteLine($"\tQuantity {order.quantity}");
            }
            Console.WriteLine();
        }
    }
}

#endregion Customers

#endregion Read items

#region Update items

#region Orders

//await UpdateOrder();

async Task UpdateOrder()
{
    CosmosClient cosmosClient = new(cosmosEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);

    string orderId = "O1";
    string sqlQuery = $"SELECT o.id, o.category FROM Orders o WHERE o.orderId = '{orderId}'";

    string id = "";
    string category = "";

    QueryDefinition queryDef = new(sqlQuery);

    FeedIterator<Order> feedIterator = container.GetItemQueryIterator<Order>(queryDef);

    while (feedIterator.HasMoreResults)
    {
        FeedResponse<Order> orders = await feedIterator.ReadNextAsync();
        foreach (Order order in orders)
        {
            id = order.id;
            category = order.category;
        }
    }

    ItemResponse<Order> response = await container.ReadItemAsync<Order>(id, new PartitionKey(category));

    var item = response.Resource;
    item.quantity = 150;

    await container.ReplaceItemAsync<Order>(item, id, new PartitionKey(category));

    Console.WriteLine("Item was updated");
}

#endregion Orders

#region Customers

//await UpdateCustomer();

async Task UpdateCustomer()
{
    CosmosClient cosmosClient = new(cosmosEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);

    string customerId = "C2";
    string sqlQuery = $"SELECT c.id, c.customerCity FROM Customers c WHERE c.id = '{customerId}'";

    string customerCity = "";

    QueryDefinition queryDef = new(sqlQuery);

    FeedIterator<Customer> feedIterator = container.GetItemQueryIterator<Customer>(queryDef);

    while (feedIterator.HasMoreResults)
    {
        FeedResponse<Customer> customers = await feedIterator.ReadNextAsync();
        foreach (Customer customer in customers)
        {
            customerCity = customer.customerCity;
        }
    }

    ItemResponse<Customer> response = await container.ReadItemAsync<Customer>(customerId, new PartitionKey(customerCity));

    var item = response.Resource;
    item.orders.Add(
        new Order
        {
            orderId = "O4",
            category = "Desktop",
            quantity = 5
        }
    );

    await container.ReplaceItemAsync<Customer>(item, customerId, new PartitionKey(customerCity));

    Console.WriteLine("Item was updated");
}

#endregion Customers

#endregion Update items

#region Delete items

//await DeleteItem();

async Task DeleteItem()
{
    CosmosClient cosmosClient = new(cosmosEndpointUri, cosmosDBKey);

    Database database = cosmosClient.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);

    string orderId = "O1";
    string sqlQuery = $"SELECT o.id, o.category FROM Orders o WHERE o.orderId = '{orderId}'";

    string id = "";
    string category = "";

    QueryDefinition queryDef = new(sqlQuery);

    FeedIterator<Order> feedIterator = container.GetItemQueryIterator<Order>(queryDef);

    while (feedIterator.HasMoreResults)
    {
        FeedResponse<Order> orders = await feedIterator.ReadNextAsync();
        foreach (Order order in orders)
        {
            id = order.id;
            category = order.category;
        }
    }

    ItemResponse<Order> response = await container.DeleteItemAsync<Order>(id, new PartitionKey(category));



    Console.WriteLine("Item was deleted");
}

#endregion Delete items

#endregion CosmosDB