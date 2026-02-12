using vc_workout.api.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

RegisterSlices(app);

app.Run();

// Método local para escaneo de ensamblados
void RegisterSlices(IEndpointRouteBuilder endpointRouteBuilder)
{
    var slices = typeof(Program).Assembly.GetTypes()
        .Where(t => typeof(ISlice).IsAssignableFrom(t) 
                    && !t.IsInterface 
                    && !t.IsAbstract);

    foreach (var sliceType in slices)
    {
        // Creamos una instancia de la clase Slice y llamamos a su MapEndpoint
        var slice = (ISlice)Activator.CreateInstance(sliceType)!;
        slice.MapEndpoint(endpointRouteBuilder);
    }
}
