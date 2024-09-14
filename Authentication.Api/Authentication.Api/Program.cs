using Authentication.Api.Core.NotificationHub;
using Authentication.Api.Startup;
using Authentication.BusinessLogic.Implementations;
using Authentication.BusinessLogic.Interfaces;
using Authentication.Core.Helpers;
using Authentication.Core.Implementations;
using AuthenticationCore.Interfaces;
using AuthenticationDataModels;
using DataAccessLayer.Implementations;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json",
        false,
        true
    );
// Add services to the container.
var configuration = builder.Configuration;
var appSettingsSection = builder.Configuration.GetSection("ApiOptions");
var appSettings = appSettingsSection.Get<ApiOptions>()!;
builder.Services.Configure<ApiOptions>(appSettingsSection);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
builder.Services.AddDbContext<GlobalServDbContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DatabaseConnectionString");
    options.UseSqlServer(connectionString);
});
builder.Services.AddControllers();
//builder.Services.AddAuthorizationAndAuthentication(appSettings);
builder.Services.AddEndpointsApiExplorer();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Configure Swagger to use the Bearer token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter the Bearer token in the input box below.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    // Use the Bearer token for operations that require authorization
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

});
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddSignalR();
builder.Services.AddScoped(typeof(IBaseBusinessService<>), typeof(BaseBusinessService<>));
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped<IUserBusinessService, UserBusinessService>();
builder.Services.AddScoped<IGeneratePasswordHelper, GeneratePasswordHelper>();
builder.Services.AddScoped<ITokenHelper, TokenHelper>();
var app = builder.Build();
DataInitizalizer.Initialize(builder.Services.BuildServiceProvider());
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<RegisterHub>("/registerHub");
});

app.Run();
