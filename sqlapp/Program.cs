using Microsoft.FeatureManagement;
using sqlapp.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Endpoint=https://gabundisaz204config.azconfig.io;Id=qoA/-l5-s0:wNuNKsPnPB3zcNADmiGh;Secret=AxiUJzmBiQf+kpAv2967t3qzeyyEQOIst+or7Cw00uU=";

builder.Host.ConfigureAppConfiguration(app =>
{
    app.AddAzureAppConfiguration(options =>
        options.Connect(connectionString).UseFeatureFlags());
});

builder.Services.AddTransient<IProductService, ProductService>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddFeatureManagement();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
