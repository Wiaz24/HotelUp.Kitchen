using HotelUp.Kitchen.Persistence.Const;

namespace HotelUp.Kitchen.Persistence.Entities;

public class Dish
{
    public string Id => Name;
    public required string Name { get; set; }
    public required Money Price { get; set; }
    public required ImageUrl ImageUrl { get; set; }
}