using Npgsql;

namespace Autonotki.Infrastructure.Services;

public class AuthService
{
    private readonly string _connectionString;

    public AuthService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<string?> LoginAsync(string login, string haslo)
    {
        await using var conn = new NpgsqlConnection(_connectionString);

        await conn.OpenAsync();

        string sql = """
            SELECT rola
            FROM UZYTKOWNICY
            WHERE login = @login
            AND haslo = @haslo
        """;

        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@login", login);
        cmd.Parameters.AddWithValue("@haslo", haslo);

        object? wynik = await cmd.ExecuteScalarAsync();

        return wynik?.ToString();
    }
}