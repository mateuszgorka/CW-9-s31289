using MedicalApi.Data;
using MedicalApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dodaj kontrolery
builder.Services.AddControllers();

// Rejestracja kontekstu bazy danych (SQL Server)
builder.Services.AddDbContext<MedicalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Rejestracja serwisu
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();