using Gateway.Middleware;
using Gateway.Utils;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the ciwaniontainer.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile(@"RouteConfig\ocelot.json", optional: false, reloadOnChange: true);
//builder.Services.AddJwt();
builder.Services.AddOcelot();
builder.Services.Register(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder.AllowAnyOrigin()
         .AllowAnyMethod()
                .AllowAnyHeader()
                ); // Replace with your Angular app's URL
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwaggerForOcelotUI();
app.UseSwagger();
app.UseCors("AllowOrigin");
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<TrafficInterceptorMiddleware>();
await app.UseOcelot();

app.Run();
