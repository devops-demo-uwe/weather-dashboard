using WeatherDashboard.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add weather services with configuration
builder.Services.AddWeatherServices(builder.Configuration);

// Add Azure Storage services with configuration
builder.Services.AddAzureStorageServices(builder.Configuration);

// Add enhanced logging configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddFilter("WeatherDashboard", LogLevel.Debug);
}

// Add options validation
builder.Services.AddOptionsWithValidateOnStart<WeatherDashboard.Configuration.WeatherApiOptions>();
builder.Services.AddOptionsWithValidateOnStart<WeatherDashboard.Configuration.AzureStorageOptions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

// Add health checks endpoint (JSON format for monitoring)
app.MapHealthChecks("/api/health");

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
