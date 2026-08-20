using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSuggestionApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCommentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF OBJECT_ID(N'[Comments]', N'U') IS NOT NULL DROP TABLE [Comments];");

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 1,
                column: "Amount",
                value: 636.48m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 2,
                column: "Amount",
                value: 928.49m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 3,
                column: "Amount",
                value: 493.64m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 4,
                column: "Amount",
                value: 189.44m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 5,
                column: "Amount",
                value: 207.10m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 6,
                column: "Amount",
                value: 678.75m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 7,
                column: "Amount",
                value: 214.58m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 8,
                column: "Amount",
                value: 485.79m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 9,
                column: "Amount",
                value: 360.78m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 10,
                column: "Amount",
                value: 922.08m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 11,
                column: "Amount",
                value: 419.71m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 12,
                column: "Amount",
                value: 498.31m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 13,
                column: "Amount",
                value: 326.76m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 14,
                column: "Amount",
                value: 431.11m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 15,
                column: "Amount",
                value: 193.41m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavorites");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VerificationCodeExpiresAt",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 1,
                column: "Amount",
                value: 636.477411670832592m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 2,
                column: "Amount",
                value: 928.49223386593632m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 3,
                column: "Amount",
                value: 493.63732665368996m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 4,
                column: "Amount",
                value: 189.43901842620265m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 5,
                column: "Amount",
                value: 207.100139163015464m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 6,
                column: "Amount",
                value: 678.74644226429074m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 7,
                column: "Amount",
                value: 214.57897900928618m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 8,
                column: "Amount",
                value: 485.78829073877536m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 9,
                column: "Amount",
                value: 360.775557061645968m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 10,
                column: "Amount",
                value: 922.07818889937968m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 11,
                column: "Amount",
                value: 419.70870300368745m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 12,
                column: "Amount",
                value: 498.30972329634816m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 13,
                column: "Amount",
                value: 326.763967031409816m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 14,
                column: "Amount",
                value: 431.10550813633092m);

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 15,
                column: "Amount",
                value: 193.41102879113136m);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RestaurantId",
                table: "Comments",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }
    }
}
