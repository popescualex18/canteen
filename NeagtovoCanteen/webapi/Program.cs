using Microsoft.EntityFrameworkCore;
using SCNeagtovo.Api.Authorization.Helpers;
using SCNeagtovo.Api.Infrastructure;
using SCNeagtovo.BusinessLogic.Implementations;
using SCNeagtovo.BusinessLogic.Interfaces;
using SCNeagtovo.DataAccessLayer.Implementations;
using SCNeagtovo.DataAccessLayer.Interfaces;
using SCNeagtovo.DataModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddDbContext<NeagtovoDbContext>(options =>
{
    var connectionString = configuration.GetConnectionString("NeagtovoDatabase");
    options.UseSqlServer(connectionString);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR(e =>
{
    e.MaximumReceiveMessageSize = 102400000;
});
// Register repositories and business services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IBaseBusinessService<>), typeof(BaseBusinessService<>));
builder.Services.AddScoped<ITokenHelper, TokenHelper>();

//builder.Services.AddCors(options =>
//{
//    var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

//    options.AddDefaultPolicy(builder =>
//    {
//        builder.WithOrigins(allowedOrigins)
//               .AllowAnyHeader()
//               .AllowAnyMethod()
//               .AllowCredentials();
//    });
//});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var signalRClientService = new SignalRClientService(scope);
    await signalRClientService.StartAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
//app.UseCors();
app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger/index.html");
        return Task.CompletedTask;
    });
    endpoints.MapControllers();
    // Map the SignalR hub
    endpoints.MapHub<OrderHub>("/orderHub");
});

app.Run();
