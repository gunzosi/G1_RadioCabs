using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyServices.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyTaxCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPersonMobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyTelephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyWard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyDistrict = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    MembershipType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MembershipId = table.Column<int>(type: "int", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertisementImages_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyLocationServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    CityService = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyLocationServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyLocationServices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyServices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    ContentPayment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPayment = table.Column<bool>(type: "bit", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementImages_CompanyId",
                table: "AdvertisementImages",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyLocationServices_CompanyId",
                table: "CompanyLocationServices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServices_CompanyId",
                table: "CompanyServices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CompanyId",
                table: "Payments",
                column: "CompanyId",
                unique: true,
                filter: "[CompanyId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementImages");

            migrationBuilder.DropTable(
                name: "CompanyLocationServices");

            migrationBuilder.DropTable(
                name: "CompanyServices");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
