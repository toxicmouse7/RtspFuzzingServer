using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagementServer.Migrations
{
    /// <inheritdoc />
    public partial class Rename_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppendSettings_UseCustomTimestamp",
                table: "RtpFuzzingPresets",
                newName: "AppendSettings_UseOriginalTimestamp");

            migrationBuilder.RenameColumn(
                name: "AppendSettings_UseCustomSequence",
                table: "RtpFuzzingPresets",
                newName: "AppendSettings_UseOriginalSequence");

            migrationBuilder.RenameColumn(
                name: "AppendSettings_UseCustomPayload",
                table: "RtpFuzzingPresets",
                newName: "AppendSettings_UseOriginalPayload");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppendSettings_UseOriginalTimestamp",
                table: "RtpFuzzingPresets",
                newName: "AppendSettings_UseCustomTimestamp");

            migrationBuilder.RenameColumn(
                name: "AppendSettings_UseOriginalSequence",
                table: "RtpFuzzingPresets",
                newName: "AppendSettings_UseCustomSequence");

            migrationBuilder.RenameColumn(
                name: "AppendSettings_UseOriginalPayload",
                table: "RtpFuzzingPresets",
                newName: "AppendSettings_UseCustomPayload");
        }
    }
}
