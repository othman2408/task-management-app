using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;

var builder = WebApplication.CreateBuilder(args);

// ===== DATABASE =====
builder.Services.AddDbContext<TaskManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") 
                         ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// ===== API SERVICES =====
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Automatic 400 responses for validation errors
        options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(context.ModelState);
    });

// ===== Circular Reference =====
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// ===== SWAGGER =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== PIPELINE =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

// ===== MAIN ROUTE =====
app.MapGet("/", () => "Task Management API!");

app.Run();