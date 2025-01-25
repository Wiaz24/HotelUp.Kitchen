using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelUp.Kitchen.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "kitchen");

            migrationBuilder.CreateTable(
                name: "Cooks",
                schema: "kitchen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                schema: "kitchen",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImageUrl_Value = table.Column<string>(type: "text", nullable: false),
                    Price_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Price_Currency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "FoodTasks",
                schema: "kitchen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reservation_ReservationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reservation_RoomNumbers = table.Column<List<int>>(type: "integer[]", nullable: false),
                    Reservation_StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reservation_EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoomNumber = table.Column<int>(type: "integer", nullable: false),
                    CookId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodTasks_Cooks_CookId",
                        column: x => x.CookId,
                        principalSchema: "kitchen",
                        principalTable: "Cooks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                schema: "kitchen",
                columns: table => new
                {
                    ServingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Published = table.Column<bool>(type: "boolean", nullable: false),
                    CookId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.ServingDate);
                    table.ForeignKey(
                        name: "FK_Menus_Cooks_CookId",
                        column: x => x.CookId,
                        principalSchema: "kitchen",
                        principalTable: "Cooks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DishFoodTask",
                schema: "kitchen",
                columns: table => new
                {
                    DishesName = table.Column<string>(type: "text", nullable: false),
                    FoodTaskId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishFoodTask", x => new { x.DishesName, x.FoodTaskId });
                    table.ForeignKey(
                        name: "FK_DishFoodTask_Dishes_DishesName",
                        column: x => x.DishesName,
                        principalSchema: "kitchen",
                        principalTable: "Dishes",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishFoodTask_FoodTasks_FoodTaskId",
                        column: x => x.FoodTaskId,
                        principalSchema: "kitchen",
                        principalTable: "FoodTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DishMenu",
                schema: "kitchen",
                columns: table => new
                {
                    DishesName = table.Column<string>(type: "text", nullable: false),
                    MenuServingDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishMenu", x => new { x.DishesName, x.MenuServingDate });
                    table.ForeignKey(
                        name: "FK_DishMenu_Dishes_DishesName",
                        column: x => x.DishesName,
                        principalSchema: "kitchen",
                        principalTable: "Dishes",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishMenu_Menus_MenuServingDate",
                        column: x => x.MenuServingDate,
                        principalSchema: "kitchen",
                        principalTable: "Menus",
                        principalColumn: "ServingDate",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishFoodTask_FoodTaskId",
                schema: "kitchen",
                table: "DishFoodTask",
                column: "FoodTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DishMenu_MenuServingDate",
                schema: "kitchen",
                table: "DishMenu",
                column: "MenuServingDate");

            migrationBuilder.CreateIndex(
                name: "IX_FoodTasks_CookId",
                schema: "kitchen",
                table: "FoodTasks",
                column: "CookId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodTasks_Reservation_ReservationId",
                schema: "kitchen",
                table: "FoodTasks",
                column: "Reservation_ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_CookId",
                schema: "kitchen",
                table: "Menus",
                column: "CookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishFoodTask",
                schema: "kitchen");

            migrationBuilder.DropTable(
                name: "DishMenu",
                schema: "kitchen");

            migrationBuilder.DropTable(
                name: "FoodTasks",
                schema: "kitchen");

            migrationBuilder.DropTable(
                name: "Dishes",
                schema: "kitchen");

            migrationBuilder.DropTable(
                name: "Menus",
                schema: "kitchen");

            migrationBuilder.DropTable(
                name: "Cooks",
                schema: "kitchen");
        }
    }
}
