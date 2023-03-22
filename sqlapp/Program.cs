using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.FeatureManagement;
using sqlapp.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Endpoint=https://bnextaz204config.azconfig.io;Id=M34V-l5-s0:UJzJRIqNX/9fNsoFvFZJ;Secret=S21/HNtwp9cYoWkLJzjGWYUlE6yOoyxEqmmY807AjJc=";

builder.Host.ConfigureAppConfiguration(app =>
{
    app.AddAzureAppConfiguration(options =>
        options.Connect(connectionString));
    //app.AddAzureAppConfiguration(options =>
    //    options.Connect(connectionString).UseFeatureFlags());
});

builder.Services.AddTransient<IProductService, ProductService>();

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddFeatureManagement();
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) => { module.EnableSqlCommandTextInstrumentation = true; });

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
