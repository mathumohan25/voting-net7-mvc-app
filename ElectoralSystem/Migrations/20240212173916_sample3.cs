using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectoralSystem.Migrations
{
    /// <inheritdoc />
    public partial class sample3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElectionResultId",
                table: "VotingSession",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VotingSessionId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Results_VotingSession_VotingSessionId",
                        column: x => x.VotingSessionId,
                        principalTable: "VotingSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VotesCountResult = table.Column<int>(type: "int", nullable: false),
                    PartyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartySymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElectionResultId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateResult_Results_ElectionResultId",
                        column: x => x.ElectionResultId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateResult_ElectionResultId",
                table: "CandidateResult",
                column: "ElectionResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_VotingSessionId",
                table: "Results",
                column: "VotingSessionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidateResult");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropColumn(
                name: "ElectionResultId",
                table: "VotingSession");
        }
    }
}
