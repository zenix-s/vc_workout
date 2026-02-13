using Microsoft.EntityFrameworkCore;
using vc_workout.api.Features;
using vc_workout.infraestructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5000",     // Local Blazor dev
            "https://localhost:5001",    // Local Blazor dev HTTPS
            "http://localhost:8080",     // Docker frontend
            "http://web:8080"            // Docker service name
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Add DbContext
builder.Services.AddDbContext<WorkoutDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register slice services
RegisterSliceServices(builder.Services);

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WorkoutDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowFrontend");

// Only use HTTPS redirection in Development (not in Docker)
if (!app.Environment.EnvironmentName.Equals("Docker", StringComparison.OrdinalIgnoreCase))
{
    app.UseHttpsRedirection();
}

// Register slice endpoints
RegisterSlices(app);

app.Run();

// Método local para registro de servicios de slices
void RegisterSliceServices(IServiceCollection services)
{
    var slices = typeof(Program).Assembly.GetTypes()
        .Where(t => typeof(ISlice).IsAssignableFrom(t) 
                    && !t.IsInterface 
                    && !t.IsAbstract);

    foreach (var sliceType in slices)
    {
        // Register slice in DI container
        services.AddScoped(sliceType);
    }
}

// Método local para escaneo de ensamblados
void RegisterSlices(IEndpointRouteBuilder endpointRouteBuilder)
{
    var slices = typeof(Program).Assembly.GetTypes()
        .Where(t => typeof(ISlice).IsAssignableFrom(t) 
                    && !t.IsInterface 
                    && !t.IsAbstract);

    foreach (var sliceType in slices)
    {
        // Create scope to resolve slice and map endpoints
        using var scope = app.Services.CreateScope();
        var slice = (ISlice)scope.ServiceProvider.GetRequiredService(sliceType);
        slice.MapEndpoint(endpointRouteBuilder);
    }
}

