using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagementServer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RtpFuzzingPresets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Header_Padding = table.Column<bool>(type: "INTEGER", nullable: false),
                    Header_Extension = table.Column<bool>(type: "INTEGER", nullable: false),
                    Header_CSRCCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    Header_Marker = table.Column<bool>(type: "INTEGER", nullable: false),
                    Header_PayloadType = table.Column<uint>(type: "INTEGER", nullable: false),
                    Header_SequenceNumber = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Header_Timestamp = table.Column<int>(type: "INTEGER", nullable: false),
                    Header_SSRCIdentifier = table.Column<uint>(type: "INTEGER", nullable: false),
                    Header_CSRC = table.Column<string>(type: "TEXT", nullable: false),
                    Header_HeaderExtensionLength = table.Column<ushort>(type: "INTEGER", nullable: false),
                    ContentHeader = table.Column<string>(type: "TEXT", nullable: false),
                    ContentHeaderType = table.Column<int>(type: "INTEGER", nullable: false),
                    Payload = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RtpFuzzingPresets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RtpFuzzingPresets");
        }
    }
}
