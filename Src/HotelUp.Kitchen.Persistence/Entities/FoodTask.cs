using TaskStatus = HotelUp.Kitchen.Persistence.Const.TaskStatus;

namespace HotelUp.Kitchen.Persistence.Entities;

public class FoodTask
{
    public required Guid Id { get; set; }
    public required Guid ReservationId { get; set; }
    public required Reservation Reservation { get; set; }
    public required TaskStatus Status { get; set; } = TaskStatus.Pending;
    public required DateTime CreatedAt { get; set; }
    public required int RoomNumber { get; set; }
    public required List<Dish> Dishes { get; set; }
}