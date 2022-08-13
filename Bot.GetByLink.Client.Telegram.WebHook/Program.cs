var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/weatherforecast", () => { });

app.Run();