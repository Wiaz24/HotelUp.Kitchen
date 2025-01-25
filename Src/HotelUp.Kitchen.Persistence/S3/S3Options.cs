namespace HotelUp.Kitchen.Persistence.S3;

public class S3Options
{
    public required string Profile { get; init; }
    public required string Region { get; init; }
    public required string BucketName { get; init; }
}