namespace HotelUp.Kitchen.Persistence.Entities;

public class Reservation
{
    public required Guid Id { get; init; }
    public required List<int> RoomNumbers { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
}