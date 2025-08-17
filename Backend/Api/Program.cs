using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register Installers from Api
builder.Services.InstallServicesFromAssembly(builder.Configuration, typeof(Program).Assembly);

// Register Services from Application
builder.Services.AddServicesByConvention(typeof(IInstaller).Assembly);

// Register Installers and Services from Infrastructure
var infrastructureAssembly = typeof(DbContextInstaller).Assembly;
builder.Services.InstallServicesFromAssembly(builder.Configuration, infrastructureAssembly);
builder.Services.AddServicesByConvention(infrastructureAssembly);

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi();

var app = builder.Build();

// Seed example data
using (var scope = app.Services.CreateScope())
{
    await ExampleDataSeeder.Seed(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
} else
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
