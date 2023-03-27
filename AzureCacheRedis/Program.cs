using AzureCacheRedis;
using Newtonsoft.Json;
using StackExchange.Redis;

string connectionString = "gabundisaz204.redis.cache.windows.net:6380,password=pSgi7UqN9kcXw6jBbdobs9vEjZLq9YxxjAzCaLf4eHk=,ssl=True,abortConnect=False";

ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);

#region Exercise 1

//SetCacheData("top:3:courses", "AZ-104,AZ-305,AZ-204");
//GetCacheData("top:3:coursess");

//void SetCacheData(string key, string value)
//{
//    IDatabase database = redis.GetDatabase();

//    database.StringSet(key, value);
//    Console.WriteLine("Cache data has been set");
//}

//void GetCacheData(string key)
//{
//    IDatabase database = redis.GetDatabase();

//    if (database.KeyExists(key))
//        Console.WriteLine(database.StringGet(key));
//    else
//        Console.WriteLine($"Key '{key}' does not exist");
//}

#endregion Exercise 1

#region Exercise 2

//await SetCacheData("u1", 10, 100);
//await SetCacheData("u1", 20, 200);
//await SetCacheData("u1", 30, 300);

await GetCacheData("u1:cartItems");

//await DeleteCacheData("u1:cartItems");

//await SetKeyExpiry("u1:cartItems", new TimeSpan(0, 0, 30));

async Task SetCacheData(string userId, int productId, int quantity)
{
    IDatabase database = redis.GetDatabase();

    CartItem cartItem = new() { ProductID = productId, Quantity = quantity };

    string key = String.Concat(userId, ":cartItems");

    await database.ListRightPushAsync(key, JsonConvert.SerializeObject(cartItem));
    Console.WriteLine("Cache data has been set");
}

async Task GetCacheData(string key)
{
    IDatabase database = redis.GetDatabase();

    if (database.KeyExists(key))
    {
        var list = await database.ListRangeAsync(key, 0, -1);

        foreach (var itm in list)
        {
            CartItem cartItem = JsonConvert.DeserializeObject<CartItem>(itm);

            Console.WriteLine($"ProductID: {cartItem.ProductID}");
            Console.WriteLine($"Quantuty: {cartItem.Quantity}");
        }
    }
    else
        Console.WriteLine($"Key '{key}' does not exist");
}

async Task DeleteCacheData(string key)
{
    IDatabase database = redis.GetDatabase();

    if (database.KeyExists(key))
    {
        await database.KeyDeleteAsync(key);
        Console.WriteLine("Key deleted");
    }
    else
        Console.WriteLine("Key does not exist");
}

async Task SetKeyExpiry(string key, TimeSpan expiry)
{
    IDatabase database = redis.GetDatabase();
    await database.KeyExpireAsync(key, expiry);
    Console.WriteLine("Set the key expiry to 30 seconds");
}

#endregion Exercise 2