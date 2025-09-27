using CinemaBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// 1) Bind Kestrel to HTTP & HTTPS ports you’ll actually use
builder.WebHost.UseUrls(
    "http://localhost:5298",  // match your swagger URL
    "https://localhost:7282"
);

var conn = builder.Configuration.GetConnectionString("CinemaDb");

// 2) Register services
builder.Services.AddDbContext<CinemaDbContext>(opts =>
    opts.UseMySql(conn, ServerVersion.AutoDetect(conn))
);

builder.Services.AddControllers();

// 3) CORS - wide open for dev
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader()
));

// 4) Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cinema API", Version = "v1" });

    // XML comments if you have them
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// 5) Middleware pipeline

// Always serve Swagger UI at root for dev
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cinema API V1");
    c.RoutePrefix = "";
    // now Swagger UI is at http://localhost:5298/
});

// Redirect root → Swagger if you like
app.MapGet("/", () => Results.Redirect("/"));

app.UseHttpsRedirection();

app.UseCors();       // apply default CORS policy

app.UseAuthorization();

app.MapControllers();

app.Run();
