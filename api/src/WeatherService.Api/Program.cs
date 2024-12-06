using Polly;
using Polly.Extensions.Http;
using WeatherService.WeatherApiClient.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
// TODO: use persistent cache for multiple instances
builder.Services.AddDistributedMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: Environments.Development,
        policy  =>
        {
            policy.AllowAnyOrigin();
        });
    
    options.AddPolicy(name: Environments.Staging,
        policy  =>
        {
            policy.WithOrigins("https://code-test-ui-staging.com");
        });
    
    options.AddPolicy(name: Environments.Production,
        policy  =>
        {
            policy.WithOrigins("https://code-test-ui.com");
        });
});
// Add services to the container.
builder.Services.AddHttpClient<IWeatherApiClient, WeatherApiClient>((client, sp) =>
{
    client.BaseAddress = new Uri(builder.Configuration["WeatherApiUrl"] ?? throw new Exception("Missing WeatherApiUrl"));
    var logger = sp.GetRequiredService<ILogger<WeatherApiClient>>();
    // TODO: move ApiKey to app service config or keyvault instead of source control
    var apiKey = builder.Configuration["WeatherApiKey"] ?? throw new Exception("Missing WeatherApiKey");
    return new WeatherApiClient(client, apiKey, logger);
})
.AddPolicyHandler(GetRetryPolicy())
.AddPolicyHandler(GetCircuitBreakerPolicy());;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();
app.MapHealthChecks("/api/status").AllowAnonymous();
app.UseCors(app.Environment.EnvironmentName);

app.Run();

// Retry Policy
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

// Circuit Breaker Policy
static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}

namespace WeatherService.Api
{
    public abstract partial class Program { }
}