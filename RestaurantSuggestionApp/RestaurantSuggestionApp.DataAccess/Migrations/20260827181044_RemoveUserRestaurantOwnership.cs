using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantSuggestionApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserRestaurantOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Restaurants_RestaurantId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RestaurantId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Restaurants");

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(5918));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6457));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6459));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 4,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6460));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 5,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6461));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 6,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6468));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 7,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6469));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 8,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6470));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 9,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6471));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 10,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6472));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 11,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6474));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 12,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6475));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 13,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6476));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 14,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6477));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 15,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6478));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 16,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6479));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 17,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6480));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 18,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6499));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 19,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6500));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 20,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6501));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 21,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6502));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 22,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6503));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 23,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6503));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 24,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6504));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 25,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6505));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 26,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6535));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 27,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6536));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 28,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6537));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 29,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6538));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 30,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6539));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 31,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6539));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 32,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6540));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 33,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6542));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 34,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6544));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 35,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6545));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 36,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6546));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 37,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6547));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 38,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6548));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 39,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6548));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 40,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6549));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 41,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6550));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 42,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6551));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 43,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6552));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 44,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6553));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 45,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6553));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 46,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6554));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 47,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6555));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 48,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6556));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 49,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6557));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 50,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6558));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 51,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6560));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 52,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6560));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 53,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6561));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 54,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6562));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 55,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6563));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 56,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6564));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 57,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6565));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 58,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6566));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 59,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6566));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 60,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6567));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 61,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6568));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 62,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6569));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 63,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6570));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 64,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6570));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 65,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6571));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 66,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6573));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 67,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6574));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 68,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6575));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 69,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6594));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 70,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6595));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 71,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6596));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 72,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6597));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 73,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6597));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 74,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6598));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 75,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6599));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 76,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6600));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 77,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6601));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 78,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6602));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 79,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6603));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 80,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6603));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 81,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6604));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 82,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6605));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 83,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6606));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 84,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6607));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 85,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6608));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 86,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6609));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 87,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6609));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 88,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6610));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 89,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6611));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 90,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6612));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 91,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6613));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 92,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6613));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 93,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6614));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 94,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6615));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 95,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6616));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 96,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6617));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 97,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6618));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 98,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6618));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 99,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6619));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 100,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6620));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 101,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6621));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 102,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6622));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 103,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6623));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 104,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6623));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 105,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6624));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 106,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6625));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 107,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6626));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 108,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6627));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 109,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6628));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 110,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6629));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 111,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6629));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 112,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6649));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 113,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6650));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 114,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6651));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 115,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6652));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 116,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6652));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 117,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6653));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 118,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6654));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 119,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6655));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 120,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6656));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 121,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6657));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 122,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6658));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 123,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6659));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 124,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6660));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 125,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6662));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 126,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6662));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 127,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6663));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 128,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6664));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 129,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6665));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 130,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6673));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 131,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6674));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 132,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6675));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 133,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6676));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 134,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6677));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 135,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6678));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 136,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6679));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 137,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6679));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 138,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6680));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 139,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6681));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 140,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6682));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 141,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6683));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 142,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6684));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 143,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6684));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 144,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6686));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 145,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6687));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 146,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6688));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 147,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6689));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 148,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6690));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 149,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6691));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 150,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6691));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 151,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6692));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 152,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6693));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 153,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6694));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 154,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6695));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 155,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6714));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 156,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6715));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 157,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6716));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 158,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6717));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 159,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6718));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 160,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6719));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 161,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6719));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 162,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6720));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 163,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6721));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 164,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6722));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 165,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6723));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 166,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6724));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 167,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6725));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 168,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6725));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 169,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6726));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 170,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6727));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 171,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6728));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 172,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6729));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 173,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6730));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 174,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6730));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 175,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6731));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 176,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6732));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 177,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6733));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 178,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6734));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 179,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6735));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 180,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6736));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 181,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6737));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 182,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6738));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 183,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6738));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 184,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6740));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 185,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6741));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 186,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6741));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 187,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6742));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 188,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6743));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 189,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6744));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 190,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6745));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 191,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6745));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 192,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6746));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 193,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6747));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 194,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6748));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 195,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6749));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 196,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6750));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 197,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6750));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 198,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6770));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 199,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6770));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 200,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6771));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 201,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6772));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 202,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6773));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 203,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6774));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 204,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6775));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 205,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6775));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 206,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6776));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 207,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6777));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 208,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6778));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 209,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6779));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 210,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6779));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 211,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6780));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 212,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6781));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 213,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6782));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 214,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6783));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 215,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6783));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 216,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6784));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 217,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6785));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 218,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6786));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 219,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6787));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 220,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6787));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 221,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6788));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 222,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6789));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 223,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6790));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 224,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6791));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 225,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6792));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 226,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6792));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 227,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6793));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 228,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6794));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 229,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6795));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 230,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6796));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 231,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6796));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 232,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6797));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 233,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6798));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 234,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6799));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 235,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6800));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 236,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6801));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 237,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6801));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 238,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6802));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 239,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6803));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 240,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6813));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 241,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6834));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 242,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6834));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 243,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6835));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 244,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6836));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 245,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6837));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 246,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6838));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 247,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6838));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 248,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6839));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 249,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6840));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 250,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6841));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 251,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6842));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 252,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6842));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 253,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6843));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 254,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6844));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 255,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6845));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 256,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6846));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 257,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6847));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 258,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6849));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 259,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6850));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 260,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6851));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 261,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6851));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 262,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6852));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 263,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6853));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 264,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6854));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 265,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6855));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 266,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6856));

            migrationBuilder.InsertData(
                table: "LocalizationResources",
                columns: new[] { "Id", "Culture", "ResourceKey", "UpdatedAt", "Value" },
                values: new object[,]
                {
                    { 267, "tr", "InvalidEmailFormat", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6856), "Lütfen geçerli bir e-posta adresi giriniz." },
                    { 268, "en", "InvalidEmailFormat", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6857), "Please enter a valid email address." },
                    { 269, "tr", "PasswordsDoNotMatch", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6858), "Girdiğiniz şifreler birbiriyle eşleşmiyor." },
                    { 270, "en", "PasswordsDoNotMatch", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6859), "Passwords do not match." },
                    { 271, "tr", "ForgotPasswordSuccess", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6860), "Şifre sıfırlama kodu e-posta adresinize gönderildi." },
                    { 272, "tr", "PasswordResetSuccess", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6861), "Şifreniz başarıyla güncellendi. Yeni şifrenizle giriş yapabilirsiniz." },
                    { 273, "tr", "ForgotPasswordEmailSubject", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6861), "Şifre Sıfırlama Kodunuz - Sofra Restoran" },
                    { 274, "tr", "ForgotPasswordEmailBody", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6862), "<h3>Merhaba {0},</h3><p>Şifrenizi sıfırlamak için kullanabileceğiniz doğrulama kodunuz: <b style='font-size: 24px; color: #E0A458;'>{1}</b></p><p>Bu kod 10 dakika süreyle geçerlidir.</p>" },
                    { 275, "tr", "PasswordChangeEmailSubject", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6863), "Şifre Değiştirme Kodunuz - Sofra Restoran" },
                    { 276, "tr", "PasswordChangeEmailBody", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6864), "<h3>Merhaba {0},</h3><p>Şifrenizi değiştirmek için kullanabileceğiniz doğrulama kodunuz: <b style='font-size: 24px; color: #E0A458;'>{1}</b></p><p>Bu kod 5 dakika süreyle geçerlidir.</p>" },
                    { 277, "tr", "EmailChangeEmailSubject", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6865), "Sofra - E-Posta Değişikliği Doğrulama Kodu" },
                    { 278, "tr", "EmailChangeEmailBody", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6865), "<h3>Merhaba {0},</h3><p>Hesabınızın e-posta adresini <strong>{1}</strong> olarak değiştirmek üzeresiniz.</p><p>Doğrulama Kodunuz: <b style='font-size: 24px; color: #E0A458;'>{2}</b></p><p>Bu kod 5 dakika boyunca geçerlidir.</p>" },
                    { 279, "en", "ForgotPasswordSuccess", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6866), "Password reset code has been sent to your email address." },
                    { 280, "en", "PasswordResetSuccess", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6867), "Your password has been reset successfully. You can now log in." },
                    { 281, "en", "ForgotPasswordEmailSubject", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6868), "Your Password Reset Code - Sofra Restaurant" },
                    { 282, "en", "ForgotPasswordEmailBody", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6869), "<h3>Hello {0},</h3><p>Your verification code to reset your password is: <b style='font-size: 24px; color: #E0A458;'>{1}</b></p><p>This code is valid for 10 minutes.</p>" },
                    { 283, "en", "PasswordChangeEmailSubject", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6869), "Your Password Change Code - Sofra Restaurant" },
                    { 284, "en", "PasswordChangeEmailBody", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6870), "<h3>Hello {0},</h3><p>Your verification code to change your password is: <b style='font-size: 24px; color: #E0A458;'>{1}</b></p><p>This code is valid for 5 minutes.</p>" },
                    { 285, "en", "EmailChangeEmailSubject", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6871), "Sofra - Email Change Verification Code" },
                    { 286, "en", "EmailChangeEmailBody", new DateTime(2026, 8, 27, 18, 10, 43, 95, DateTimeKind.Utc).AddTicks(6872), "<h3>Hello {0},</h3><p>You are about to change your account email to <strong>{1}</strong>.</p><p>Verification Code: <b style='font-size: 24px; color: #E0A458;'>{2}</b></p><p>This code is valid for 5 minutes.</p>" }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "AveragePrice", "Category", "Latitude", "Longitude", "Name" },
                values: new object[,]
                {
                    { 51, 750m, "Kebab & Grill", 40.9752m, 28.7994m, "Beyti Restoran Florya" },
                    { 52, 700m, "Kebab & Grill", 40.9701m, 28.8021m, "Kaşıbeyaz Florya" },
                    { 53, 450m, "Kebab & Grill", 40.9765m, 28.8720m, "Gelik Restoran Ataköy" },
                    { 54, 680m, "Kebab & Grill", 40.9734m, 28.7981m, "Develi Florya" },
                    { 55, 380m, "Kebab & Grill", 40.9772m, 28.8741m, "Sahan Kebap Bakırköy" },
                    { 56, 220m, "Home Cooking", 40.9789m, 28.8715m, "Tarihi Dikilitaş Köftecisi Bakırköy" },
                    { 57, 280m, "Cafe & Dessert", 40.9723m, 28.8012m, "Sütiş Florya" },
                    { 58, 260m, "Kebab & Grill", 41.0135m, 28.8920m, "Öz Kilis Kebap & Lahmacun Merter" },
                    { 59, 220m, "Cafe & Dessert", 41.0021m, 28.8614m, "Hacı Sayid Bahçelievler" },
                    { 60, 240m, "Home Cooking", 41.0122m, 28.8945m, "Nizam Pide Merter" },
                    { 61, 250m, "Kebab & Grill", 41.0185m, 28.8872m, "Köfteci Yusuf Güngören" },
                    { 62, 200m, "Kebab & Grill", 41.0012m, 28.9034m, "Kardeşler Döner Zeytinburnu" },
                    { 63, 210m, "Home Cooking", 41.0150m, 28.8830m, "Sarayçiftliği Restoran Güngören" },
                    { 64, 210m, "Kebab & Grill", 41.0034m, 28.8472m, "Şirinevler Köftecisi" },
                    { 65, 290m, "Kebab & Grill", 40.9995m, 28.8628m, "Urfa Kebapçısı Bahçelievler" },
                    { 66, 240m, "Cafe & Dessert", 40.9982m, 28.8645m, "Mado Bahçelievler Yayla" },
                    { 67, 320m, "Kebab & Grill", 40.9942m, 28.7756m, "Gülhane İskender Küçükçekmece" },
                    { 68, 400m, "Kebab & Grill", 41.0882m, 28.7994m, "Hacıbozanoğulları Başakşehir" },
                    { 69, 230m, "Cafe & Dessert", 41.0628m, 28.8078m, "Saray Muhallebicisi Mall of Istanbul" },
                    { 70, 480m, "World Cuisine", 41.0631m, 28.8082m, "Big Chefs Mall of Istanbul" },
                    { 71, 520m, "World Cuisine", 41.0635m, 28.8075m, "Midpoint Mall of Istanbul" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 267);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 268);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 269);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 270);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 271);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 272);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 273);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 274);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 275);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 276);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 277);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 278);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 279);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 280);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 281);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 282);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 283);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 284);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 285);

            migrationBuilder.DeleteData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 286);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Restaurants",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(1776));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2292));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2294));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 4,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2295));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 5,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2296));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 6,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2299));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 7,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2300));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 8,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2301));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 9,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2302));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 10,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2304));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 11,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2306));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 12,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2307));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 13,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2308));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 14,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2309));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 15,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2311));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 16,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2312));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 17,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2313));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 18,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2316));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 19,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2317));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 20,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2318));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 21,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2322));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 22,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2323));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 23,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2325));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 24,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2325));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 25,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2326));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 26,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2327));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 27,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2328));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 28,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2329));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 29,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2330));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 30,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2331));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 31,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2332));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 32,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2333));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 33,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2334));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 34,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2335));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 35,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2336));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 36,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2337));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 37,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2367));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 38,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2368));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 39,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2369));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 40,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2370));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 41,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2371));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 42,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2372));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 43,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2373));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 44,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2374));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 45,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2375));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 46,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2376));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 47,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2377));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 48,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2378));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 49,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2379));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 50,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2379));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 51,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2380));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 52,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2381));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 53,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2382));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 54,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2383));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 55,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2384));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 56,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2385));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 57,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2386));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 58,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2387));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 59,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2388));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 60,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2389));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 61,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2390));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 62,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2391));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 63,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2392));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 64,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2394));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 65,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2395));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 66,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2397));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 67,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2398));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 68,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2400));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 69,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2401));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 70,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2402));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 71,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2403));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 72,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2404));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 73,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2414));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 74,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2415));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 75,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2417));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 76,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2417));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 77,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2418));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 78,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2419));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 79,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2441));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 80,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2442));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 81,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2443));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 82,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2444));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 83,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2445));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 84,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2445));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 85,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2446));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 86,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2447));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 87,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2448));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 88,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2449));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 89,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2450));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 90,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2451));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 91,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2452));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 92,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2453));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 93,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2453));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 94,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2454));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 95,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2455));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 96,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2456));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 97,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2457));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 98,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2458));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 99,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2459));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 100,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2460));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 101,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2461));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 102,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2462));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 103,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2463));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 104,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2463));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 105,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2464));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 106,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2465));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 107,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2466));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 108,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2467));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 109,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2468));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 110,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2468));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 111,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2469));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 112,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2471));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 113,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2472));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 114,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2473));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 115,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2474));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 116,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2475));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 117,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 118,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 119,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2477));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 120,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2478));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 121,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2499));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 122,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2500));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 123,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2501));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 124,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2502));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 125,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2502));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 126,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2503));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 127,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2504));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 128,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2505));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 129,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2506));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 130,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2508));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 131,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2508));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 132,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2509));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 133,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2510));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 134,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2511));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 135,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2512));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 136,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2514));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 137,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2514));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 138,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2516));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 139,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2517));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 140,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2518));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 141,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2518));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 142,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2519));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 143,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2520));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 144,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2521));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 145,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2522));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 146,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2523));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 147,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2525));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 148,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2526));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 149,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2527));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 150,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2528));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 151,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2529));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 152,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2530));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 153,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2531));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 154,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2532));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 155,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2533));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 156,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2534));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 157,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2535));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 158,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2535));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 159,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2536));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 160,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2537));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 161,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2538));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 162,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2539));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 163,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2561));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 164,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2562));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 165,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2563));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 166,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2564));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 167,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2565));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 168,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2566));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 169,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2567));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 170,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2567));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 171,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2568));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 172,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2569));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 173,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2570));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 174,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2571));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 175,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2572));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 176,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2573));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 177,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2573));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 178,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2574));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 179,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2575));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 180,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2576));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 181,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2577));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 182,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2578));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 183,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2585));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 184,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2586));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 185,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2587));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 186,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2587));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 187,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2588));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 188,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2589));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 189,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2590));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 190,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2592));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 191,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2593));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 192,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2594));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 193,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2595));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 194,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2596));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 195,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2596));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 196,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2597));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 197,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2598));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 198,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2599));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 199,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2600));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 200,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2601));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 201,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2602));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 202,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2602));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 203,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2604));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 204,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2605));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 205,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2629));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 206,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2630));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 207,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2631));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 208,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2632));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 209,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2633));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 210,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2634));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 211,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2635));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 212,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2636));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 213,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2636));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 214,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2637));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 215,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2638));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 216,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2639));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 217,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2640));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 218,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2641));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 219,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2642));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 220,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2642));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 221,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2643));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 222,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2644));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 223,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2646));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 224,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 225,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2648));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 226,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2649));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 227,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2650));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 228,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2650));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 229,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2651));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 230,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2653));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 231,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2654));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 232,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2655));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 233,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2656));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 234,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2657));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 235,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2658));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 236,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2659));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 237,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2660));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 238,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2661));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 239,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2662));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 240,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2662));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 241,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2663));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 242,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2664));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 243,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2665));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 244,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2666));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 245,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2666));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 246,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2668));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 247,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2669));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 248,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2670));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 249,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2670));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 250,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2671));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 251,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2672));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 252,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2673));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 253,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2674));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 254,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2675));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 255,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2676));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 256,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2676));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 257,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2677));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 258,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2685));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 259,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2686));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 260,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2687));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 261,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2688));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 262,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2688));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 263,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2689));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 264,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2690));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 265,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2691));

            migrationBuilder.UpdateData(
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 266,
                column: "UpdatedAt",
                value: new DateTime(2026, 8, 20, 11, 11, 11, 37, DateTimeKind.Utc).AddTicks(2692));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 4,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 5,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 6,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 7,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 8,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 9,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 10,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 11,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 12,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 13,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 14,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 15,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 16,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 17,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 18,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 19,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 20,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 21,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 22,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 23,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 24,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 25,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 26,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 27,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 28,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 29,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 30,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 31,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 32,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 33,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 34,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 35,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 36,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 37,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 38,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 39,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 40,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 41,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 42,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 43,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 44,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 45,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 46,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 47,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 48,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 49,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 50,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "RestaurantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "RestaurantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "RestaurantId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RestaurantId",
                table: "Users",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_OwnerId",
                table: "Restaurants",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Restaurants_RestaurantId",
                table: "Users",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
