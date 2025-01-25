namespace HotelUp.Kitchen.Persistence.EF.Postgres;

public sealed class PostgresOptions
{
    public required string ConnectionString { get; init; }
    public required string SchemaName { get; init; }
}