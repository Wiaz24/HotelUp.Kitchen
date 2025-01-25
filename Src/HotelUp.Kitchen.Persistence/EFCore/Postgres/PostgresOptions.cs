namespace HotelUp.Kitchen.Persistence.EFCore.Postgres;

public sealed class PostgresOptions
{
    public required string ConnectionString { get; init; }
    public required string SchemaName { get; init; }
}