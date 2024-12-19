using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagementServer.Migrations
{
    /// <inheritdoc />
    public partial class Add_AppendSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AppendSettings_UseCustomPayload",
                table: "RtpFuzzingPresets",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AppendSettings_UseCustomSequence",
                table: "RtpFuzzingPresets",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AppendSettings_UseCustomTimestamp",
                table: "RtpFuzzingPresets",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppendSettings_UseCustomPayload",
                table: "RtpFuzzingPresets");

            migrationBuilder.DropColumn(
                name: "AppendSettings_UseCustomSequence",
                table: "RtpFuzzingPresets");

            migrationBuilder.DropColumn(
                name: "AppendSettings_UseCustomTimestamp",
                table: "RtpFuzzingPresets");
        }
    }
}
