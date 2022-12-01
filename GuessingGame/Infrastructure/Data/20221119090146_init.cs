using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuessingGame.Infrastructure.Data
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserGuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserCanGuess = table.Column<bool>(type: "INTEGER", nullable: false),
                    GameFinished = table.Column<bool>(type: "INTEGER", nullable: false),
                    GameWon = table.Column<bool>(type: "INTEGER", nullable: false),
                    UsedGuesses = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectGuess = table.Column<string>(type: "TEXT", nullable: false),
                    ImageId = table.Column<int>(type: "INTEGER", nullable: false),
                    segmentsInImage = table.Column<int>(type: "INTEGER", nullable: false),
                    ShownSegmentsNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameStateId = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageId = table.Column<int>(type: "INTEGER", nullable: false),
                    UncoveredSegments = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ImageId = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageName = table.Column<string>(type: "TEXT", nullable: false),
                    SegmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Segment = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    PriorityScoreForBetterSegment = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BetterSegments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ImageId = table.Column<int>(type: "INTEGER", nullable: false),
                    SegmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameStateId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetterSegments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BetterSegments_GameStates_GameStateId",
                        column: x => x.GameStateId,
                        principalTable: "GameStates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChoosenSegment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ImageId = table.Column<int>(type: "INTEGER", nullable: false),
                    SegmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameStateId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoosenSegment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChoosenSegment_GameStates_GameStateId",
                        column: x => x.GameStateId,
                        principalTable: "GameStates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BetterSegments_GameStateId",
                table: "BetterSegments",
                column: "GameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_ChoosenSegment_GameStateId",
                table: "ChoosenSegment",
                column: "GameStateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "BetterSegments");

            migrationBuilder.DropTable(
                name: "ChoosenSegment");

            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "GameStates");
        }
    }
}
