using IGwApi.Extensions;
using HubAcoApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMyDependencyGroup();
builder.Services.AddAppSettings(builder.Configuration);
builder.Services.AddResponseCompressionGw();
builder.Services.AddHttpClientPPN();
builder.Services.AddCustomizedInvalidResponse();
builder.Services.AddJsonSettings();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.ConfigureExceptionHandler();
app.UseAuthorization();
app.UseBasicAuthentication();
app.UseResponseCompression();
app.MapControllers();

app.Run();

public partial class Program { }