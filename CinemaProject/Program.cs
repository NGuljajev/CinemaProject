using System.IO;
using System.Reflection;
using CinemaBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------- Kestrel ports ----------
builder.WebHost.UseUrls("http://localhost:5298", "https://localhost:7282");

// ---------- Configuration ----------
var conn = builder.Configuration.GetConnectionString("CinemaDb");

// ---------- Services ----------
builder.Services.AddControllers();

// EF Core (MySQL via Pomelo)
builder.Services.AddDbContext<CinemaDbContext>(opts =>
    opts.UseMySql(conn, ServerVersion.AutoDetect(conn))
);

// CORS: named policy with explicit origins; safe defaults for dev
var corsPolicyName = "FrontendCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName, policy =>
    {
        policy.WithOrigins("http://localhost:4200") // update to your frontend origin(s)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // remove if you don't use credentials/cookies
    });

    // permissive fallback for local testing (only if you need it)
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cinema API V1", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // JWT or other security definitions can be added here if needed
});

var app = builder.Build();

// ---------- Middleware order ----------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cinema API V1");
        c.RoutePrefix = "swagger"; // serve UI at /swagger
    });
}
else
{
    app.UseExceptionHandler("/error"); // production error endpoint (implement as needed)
    app.UseHsts();
}

// Use CORS before routing/authorization/mapping controllers
app.UseCors(corsPolicyName);

// HTTPS redirection after CORS (safe)
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
