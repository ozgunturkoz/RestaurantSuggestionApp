using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSuggestionApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedAndDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 1,
                column: "SpentAt",
                value: new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 2,
                column: "SpentAt",
                value: new DateTime(2025, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 3,
                column: "SpentAt",
                value: new DateTime(2025, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 4,
                column: "SpentAt",
                value: new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 5,
                column: "SpentAt",
                value: new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 6,
                column: "SpentAt",
                value: new DateTime(2025, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 7,
                column: "SpentAt",
                value: new DateTime(2025, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 8,
                column: "SpentAt",
                value: new DateTime(2025, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 9,
                column: "SpentAt",
                value: new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 10,
                column: "SpentAt",
                value: new DateTime(2025, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 11,
                column: "SpentAt",
                value: new DateTime(2025, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 12,
                column: "SpentAt",
                value: new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 13,
                column: "SpentAt",
                value: new DateTime(2025, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 14,
                column: "SpentAt",
                value: new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 15,
                column: "SpentAt",
                value: new DateTime(2025, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 1,
                column: "SpentAt",
                value: new DateTime(2026, 6, 30, 12, 11, 22, 710, DateTimeKind.Local).AddTicks(3029));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 2,
                column: "SpentAt",
                value: new DateTime(2026, 6, 14, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9284));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 3,
                column: "SpentAt",
                value: new DateTime(2026, 7, 1, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9297));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 4,
                column: "SpentAt",
                value: new DateTime(2026, 6, 20, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9299));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 5,
                column: "SpentAt",
                value: new DateTime(2026, 6, 19, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9300));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 6,
                column: "SpentAt",
                value: new DateTime(2026, 6, 24, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9306));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 7,
                column: "SpentAt",
                value: new DateTime(2026, 6, 23, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9307));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 8,
                column: "SpentAt",
                value: new DateTime(2026, 6, 13, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9312));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 9,
                column: "SpentAt",
                value: new DateTime(2026, 7, 6, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9313));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 10,
                column: "SpentAt",
                value: new DateTime(2026, 6, 16, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9315));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 11,
                column: "SpentAt",
                value: new DateTime(2026, 6, 25, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9317));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 12,
                column: "SpentAt",
                value: new DateTime(2026, 6, 9, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9318));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 13,
                column: "SpentAt",
                value: new DateTime(2026, 6, 27, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9320));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 14,
                column: "SpentAt",
                value: new DateTime(2026, 6, 9, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9321));

            migrationBuilder.UpdateData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 15,
                column: "SpentAt",
                value: new DateTime(2026, 6, 28, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9322));
        }
    }
}
