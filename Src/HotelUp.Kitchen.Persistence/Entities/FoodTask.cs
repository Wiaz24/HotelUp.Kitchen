using TaskStatus = HotelUp.Kitchen.Persistence.Const.TaskStatus;

namespace HotelUp.Kitchen.Persistence.Entities;

public class FoodTask
{
    public required Guid Id { get; init; }
    public required Reservation Reservation { get; init; }
    public required TaskStatus Status { get; set; } = TaskStatus.Pending;
    public required DateTime CreatedAt { get; init; }
    public required int RoomNumber { get; init; }
    public required List<Dish> Dishes { get; set; }
}