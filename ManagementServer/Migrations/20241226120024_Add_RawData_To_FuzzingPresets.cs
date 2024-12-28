using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagementServer.Migrations
{
    /// <inheritdoc />
    public partial class Add_RawData_To_FuzzingPresets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RawFuzzingData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PresetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RawData = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawFuzzingData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RawFuzzingData_RtpFuzzingPresets_PresetId",
                        column: x => x.PresetId,
                        principalTable: "RtpFuzzingPresets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RawFuzzingData_PresetId",
                table: "RawFuzzingData",
                column: "PresetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RawFuzzingData");
        }
    }
}
