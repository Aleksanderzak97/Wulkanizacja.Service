using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Wulkanizacja.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TireTypes",
                columns: table => new
                {
                    TireTypeId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TireTypes", x => x.TireTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Tires",
                columns: table => new
                {
                    TireId = table.Column<Guid>(type: "uuid", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Size = table.Column<string>(type: "text", nullable: false),
                    TireTypeId = table.Column<short>(type: "smallint", nullable: false),
                    SpeedIndex = table.Column<string>(type: "text", nullable: false),
                    LoadIndex = table.Column<string>(type: "text", nullable: false),
                    ManufactureDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EditDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Comments = table.Column<string>(type: "text", nullable: true),
                    QuantityInStock = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tires", x => x.TireId);
                    table.ForeignKey(
                        name: "FK_Tires_TireTypes_TireTypeId",
                        column: x => x.TireTypeId,
                        principalTable: "TireTypes",
                        principalColumn: "TireTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TireTypes",
                columns: new[] { "TireTypeId", "Name" },
                values: new object[,]
                {
                    { (short)1, "Letnia" },
                    { (short)2, "Zimowa" },
                    { (short)3, "Całoroczna" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tires_TireTypeId",
                table: "Tires",
                column: "TireTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tires");

            migrationBuilder.DropTable(
                name: "TireTypes");
        }
    }
}
