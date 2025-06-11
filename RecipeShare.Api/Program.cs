using Microsoft.EntityFrameworkCore;
using RecipeShare.Api.Middleware;
using RecipeShare.Application.Contracts.Persistence;
using RecipeShare.Application.Contracts.Persistence.Service;
using RecipeShare.Persistence.DatabaseContext;
using RecipeShare.Persistence.Repositories;
using RecipeShare.Persistence.Repositories.Service;
//using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("RecipeDatabaseConnectionString");
Console.WriteLine($"Connection String: {connectionString}");

// Add services to the container.
builder.Services.AddTransient<IRecipeService, RecipeService>();
builder.Services.AddTransient<IRecipeRepository, RecipeRepository>();
builder.Services.AddTransient<IDietaryTagRepository, DietaryTagRepository>();
builder.Services.AddDbContext<RecipeDatabaseContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("RecipeDatabaseConnectionString")));

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("all", builder => builder.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyHeader());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

//app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("all");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RecipeDatabaseContext>();
    try
    {
        var canConnect = context.Database.CanConnect();
        Console.WriteLine($"Database connection success: {canConnect}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection failed: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
