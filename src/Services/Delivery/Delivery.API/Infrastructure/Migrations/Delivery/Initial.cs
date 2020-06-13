using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Delivery.API.Infrastructure.Migrations.Delivery
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                "Delivery");

            migrationBuilder.CreateTable(
                "Clients",
                schema: "Delivery",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Clients", x => x.Id); });

            migrationBuilder.CreateTable(
                "DeliveryStatus",
                schema: "Delivery",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_DeliveryStatus", x => x.Id); });

            migrationBuilder.CreateTable(
                "Requests",
                schema: "Delivery",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Requests", x => x.Id); });

            migrationBuilder.CreateTable(
                "Deliveries",
                schema: "Delivery",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<long>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    FinishedDateTime = table.Column<DateTime>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Weight = table.Column<short>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    DeliveryStatusId = table.Column<int>(nullable: false),
                    PickUpLocation_Address = table.Column<string>(nullable: true),
                    PickUpLocation_BuildingNumber = table.Column<string>(nullable: true),
                    PickUpLocation_EntranceNumber = table.Column<string>(nullable: true),
                    PickUpLocation_FloorNumber = table.Column<string>(nullable: true),
                    PickUpLocation_ApartmentNumber = table.Column<string>(nullable: true),
                    PickUpLocation_Latitude = table.Column<double>(nullable: true),
                    PickUpLocation_Longitude = table.Column<double>(nullable: true),
                    PickUpLocation_Note = table.Column<string>(nullable: true),
                    PickUpLocation_ContactPerson_Name = table.Column<string>(nullable: true),
                    PickUpLocation_ContactPerson_Phone = table.Column<string>(nullable: true),
                    PickUpLocation_ArrivalStartDateTime = table.Column<DateTime>(nullable: true),
                    PickUpLocation_ArrivalFinishDateTime = table.Column<DateTime>(nullable: true),
                    PickUpLocation_CourierArrivedDateTime = table.Column<DateTime>(nullable: true),
                    DropOffLocation_Address = table.Column<string>(nullable: true),
                    DropOffLocation_BuildingNumber = table.Column<string>(nullable: true),
                    DropOffLocation_EntranceNumber = table.Column<string>(nullable: true),
                    DropOffLocation_FloorNumber = table.Column<string>(nullable: true),
                    DropOffLocation_ApartmentNumber = table.Column<string>(nullable: true),
                    DropOffLocation_Latitude = table.Column<double>(nullable: true),
                    DropOffLocation_Longitude = table.Column<double>(nullable: true),
                    DropOffLocation_Note = table.Column<string>(nullable: true),
                    DropOffLocation_ContactPerson_Name = table.Column<string>(nullable: true),
                    DropOffLocation_ContactPerson_Phone = table.Column<string>(nullable: true),
                    DropOffLocation_ArrivalStartDateTime = table.Column<DateTime>(nullable: true),
                    DropOffLocation_ArrivalFinishDateTime = table.Column<DateTime>(nullable: true),
                    DropOffLocation_CourierArrivedDateTime = table.Column<DateTime>(nullable: true),
                    ClientId = table.Column<Guid>(nullable: false),
                    CourierId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                    table.ForeignKey(
                        "FK_Deliveries_Clients_ClientId",
                        x => x.ClientId,
                        principalSchema: "Delivery",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Deliveries_DeliveryStatus_DeliveryStatusId",
                        x => x.DeliveryStatusId,
                        principalSchema: "Delivery",
                        principalTable: "DeliveryStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Deliveries_ClientId",
                schema: "Delivery",
                table: "Deliveries",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                "IX_Deliveries_DeliveryStatusId",
                schema: "Delivery",
                table: "Deliveries",
                column: "DeliveryStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Deliveries",
                "Delivery");

            migrationBuilder.DropTable(
                "Requests",
                "Delivery");

            migrationBuilder.DropTable(
                "Clients",
                "Delivery");

            migrationBuilder.DropTable(
                "DeliveryStatus",
                "Delivery");
        }
    }
}