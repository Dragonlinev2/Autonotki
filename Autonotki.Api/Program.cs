using Autonotki.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Rejestracja AuthService
builder.Services.AddSingleton<AuthService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    string conn =
        config.GetConnectionString("Postgres")!;

    return new AuthService(conn);
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Endpoint logowania
app.MapPost("/login", async (
    LoginRequest request,
    AuthService authService) =>
{
    // Walidacja
    if (string.IsNullOrWhiteSpace(request.Login) ||
        string.IsNullOrWhiteSpace(request.Haslo))
    {
        return Results.BadRequest(new
        {
            message = "Login i hasło są wymagane"
        });
    }

    try
    {
        var rola = await authService.LoginAsync(
            request.Login,
            request.Haslo
        );

        if (rola == null)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(new
        {
            message = "Zalogowano poprawnie",
            rola = rola
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: "Błąd połączenia z bazą",
            detail: ex.Message
        );
    }
});

app.Run();


// DTO request
record LoginRequest(string Login, string Haslo);