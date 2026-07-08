using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantSuggestionApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "PriceIndex",
                table: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Restaurants",
                newName: "Category");

            migrationBuilder.AddColumn<decimal>(
                name: "AveragePrice",
                table: "Restaurants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "AveragePrice", "Category", "Latitude", "Longitude", "Name" },
                values: new object[,]
                {
                    { 1, 237m, "Kebap & Izgara", 40.99346212931823084m, 29.01841814596746962m, "Kadik�y Kebap & Izgara Restorani 1" },
                    { 2, 333m, "D�nya Mutfagi", 41.043255285520504826m, 29.00086868448339808m, "Besiktas D�nya Mutfagi Restorani 2" },
                    { 3, 271m, "Deniz �r�nleri", 41.06468816729452842m, 28.987958455830746542m, "Sisli Deniz �r�nleri Restorani 3" },
                    { 4, 330m, "Kafe & Tatli", 41.02172501117327484m, 28.94489176916624036m, "Fatih Kafe & Tatli Restorani 4" },
                    { 5, 416m, "Ev Yemekleri", 41.0268120708929896686m, 29.01170466374643364m, "�sk�dar Ev Yemekleri Restorani 5" },
                    { 6, 174m, "Kebap & Izgara", 40.98530489270109912m, 29.025949021996533974m, "Kadik�y Kebap & Izgara Restorani 6" },
                    { 7, 428m, "D�nya Mutfagi", 41.04908274751654022m, 29.009043937079396070m, "Besiktas D�nya Mutfagi Restorani 7" },
                    { 8, 643m, "Deniz �r�nleri", 41.05324386160478176m, 28.97950225220592798m, "Sisli Deniz �r�nleri Restorani 8" },
                    { 9, 180m, "Kafe & Tatli", 41.02282363899439742m, 28.950999411642737414m, "Fatih Kafe & Tatli Restorani 9" },
                    { 10, 784m, "Ev Yemekleri", 41.03092309179521310m, 29.00825522532563434m, "�sk�dar Ev Yemekleri Restorani 10" },
                    { 11, 280m, "Kebap & Izgara", 40.99395179445792538m, 29.025928274280917026m, "Kadik�y Kebap & Izgara Restorani 11" },
                    { 12, 558m, "D�nya Mutfagi", 41.03363019932770646m, 29.00366399325717426m, "Besiktas D�nya Mutfagi Restorani 12" },
                    { 13, 525m, "Deniz �r�nleri", 41.06662130303961286m, 28.97777909721698570m, "Sisli Deniz �r�nleri Restorani 13" },
                    { 14, 419m, "Kafe & Tatli", 41.02260368933347226m, 28.955167178541686m, "Fatih Kafe & Tatli Restorani 14" },
                    { 15, 185m, "Ev Yemekleri", 41.01968238143463730m, 29.00582679028386566m, "�sk�dar Ev Yemekleri Restorani 15" },
                    { 16, 230m, "Kebap & Izgara", 40.99573285833021294m, 29.0281291276874622m, "Kadik�y Kebap & Izgara Restorani 16" },
                    { 17, 352m, "D�nya Mutfagi", 41.03779921656446496m, 29.01294856648680222m, "Besiktas D�nya Mutfagi Restorani 17" },
                    { 18, 181m, "Deniz �r�nleri", 41.06316291814789312m, 28.98057292969546882m, "Sisli Deniz �r�nleri Restorani 18" },
                    { 19, 185m, "Kafe & Tatli", 41.01859118567970170m, 28.950518267713449090m, "Fatih Kafe & Tatli Restorani 19" },
                    { 20, 589m, "Ev Yemekleri", 41.025107323187406792m, 29.0194316465261074m, "�sk�dar Ev Yemekleri Restorani 20" },
                    { 21, 405m, "Kebap & Izgara", 40.98446891074495804m, 29.01586507549931532m, "Kadik�y Kebap & Izgara Restorani 21" },
                    { 22, 652m, "D�nya Mutfagi", 41.0381487682181172m, 28.99887321671534946m, "Besiktas D�nya Mutfagi Restorani 22" },
                    { 23, 158m, "Deniz �r�nleri", 41.05161885883240908m, 28.986981871565283216m, "Sisli Deniz �r�nleri Restorani 23" },
                    { 24, 509m, "Kafe & Tatli", 41.02245618653854178m, 28.948983589661486256m, "Fatih Kafe & Tatli Restorani 24" },
                    { 25, 730m, "Ev Yemekleri", 41.02669193593393635782m, 29.00547905818306796m, "�sk�dar Ev Yemekleri Restorani 25" },
                    { 26, 194m, "Kebap & Izgara", 40.98035793644611628m, 29.01566148017014446m, "Kadik�y Kebap & Izgara Restorani 26" },
                    { 27, 789m, "D�nya Mutfagi", 41.04588286241864918m, 28.99819078066418450m, "Besiktas D�nya Mutfagi Restorani 27" },
                    { 28, 412m, "Deniz �r�nleri", 41.06877941533372710m, 28.986923875230748148m, "Sisli Deniz �r�nleri Restorani 28" },
                    { 29, 410m, "Kafe & Tatli", 41.01883212161452142m, 28.94108887631003228m, "Fatih Kafe & Tatli Restorani 29" },
                    { 30, 704m, "Ev Yemekleri", 41.01953760198524110m, 29.014039013648004744m, "�sk�dar Ev Yemekleri Restorani 30" },
                    { 31, 358m, "Kebap & Izgara", 40.990747311969961650m, 29.023613979852206064m, "Kadik�y Kebap & Izgara Restorani 31" },
                    { 32, 664m, "D�nya Mutfagi", 41.05094516757528538m, 29.009234778588514206m, "Besiktas D�nya Mutfagi Restorani 32" },
                    { 33, 820m, "Deniz �r�nleri", 41.05058824332896072m, 28.99533361321652010m, "Sisli Deniz �r�nleri Restorani 33" },
                    { 34, 263m, "Kafe & Tatli", 41.0131566852976832m, 28.95441399949780386m, "Fatih Kafe & Tatli Restorani 34" },
                    { 35, 383m, "Ev Yemekleri", 41.01876285693778790m, 29.014343030025923174m, "�sk�dar Ev Yemekleri Restorani 35" },
                    { 36, 244m, "Kebap & Izgara", 40.99562220879845424m, 29.01857098221395676m, "Kadik�y Kebap & Izgara Restorani 36" },
                    { 37, 508m, "D�nya Mutfagi", 41.03711320860251468m, 29.009333249233585432m, "Besiktas D�nya Mutfagi Restorani 37" },
                    { 38, 620m, "Deniz �r�nleri", 41.06994043183947934m, 28.9930244607687576m, "Sisli Deniz �r�nleri Restorani 38" },
                    { 39, 536m, "Kafe & Tatli", 41.02079898888538544m, 28.94598949541123094m, "Fatih Kafe & Tatli Restorani 39" },
                    { 40, 166m, "Ev Yemekleri", 41.03649657166628008m, 29.01831664660359576m, "�sk�dar Ev Yemekleri Restorani 40" },
                    { 41, 169m, "Kebap & Izgara", 40.98700815029987514m, 29.03418125684716798m, "Kadik�y Kebap & Izgara Restorani 41" },
                    { 42, 176m, "D�nya Mutfagi", 41.0371949449455342m, 29.00280887456858012m, "Besiktas D�nya Mutfagi Restorani 42" },
                    { 43, 445m, "Deniz �r�nleri", 41.05452247408866998m, 28.989546768358650974m, "Sisli Deniz �r�nleri Restorani 43" },
                    { 44, 234m, "Kafe & Tatli", 41.00837465549533938m, 28.96006969311715556m, "Fatih Kafe & Tatli Restorani 44" },
                    { 45, 376m, "Ev Yemekleri", 41.01882188835354610m, 29.01172617712096598m, "�sk�dar Ev Yemekleri Restorani 45" },
                    { 46, 156m, "Kebap & Izgara", 40.99512302783309624m, 29.0321165583959392m, "Kadik�y Kebap & Izgara Restorani 46" },
                    { 47, 777m, "D�nya Mutfagi", 41.03364168201351616m, 29.01430991281606718m, "Besiktas D�nya Mutfagi Restorani 47" },
                    { 48, 741m, "Deniz �r�nleri", 41.05496147307304734m, 28.99622439338272642m, "Sisli Deniz �r�nleri Restorani 48" },
                    { 49, 594m, "Kafe & Tatli", 41.01208893800042054m, 28.951106880130482318m, "Fatih Kafe & Tatli Restorani 49" },
                    { 50, 358m, "Ev Yemekleri", 41.0292065848289554m, 29.00914617868989994m, "�sk�dar Ev Yemekleri Restorani 50" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "ozgur@email.com", "�zg�r", "hashed_password_1" },
                    { 2, "ahmet@email.com", "Ahmet", "hashed_password_2" },
                    { 3, "zeynep@email.com", "Zeynep", "hashed_password_3" }
                });

            migrationBuilder.InsertData(
                table: "UserExpenditures",
                columns: new[] { "Id", "Amount", "RestaurantId", "SpentAt", "UserId" },
                values: new object[,]
                {
                    { 1, 636.477411670832592m, 10, new DateTime(2026, 6, 30, 12, 11, 22, 710, DateTimeKind.Local).AddTicks(3029), 1 },
                    { 2, 928.49223386593632m, 47, new DateTime(2026, 6, 14, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9284), 1 },
                    { 3, 493.63732665368996m, 7, new DateTime(2026, 7, 1, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9297), 1 },
                    { 4, 189.43901842620265m, 15, new DateTime(2026, 6, 20, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9299), 1 },
                    { 5, 207.100139163015464m, 36, new DateTime(2026, 6, 19, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9300), 1 },
                    { 6, 678.74644226429074m, 20, new DateTime(2026, 6, 24, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9306), 2 },
                    { 7, 214.57897900928618m, 18, new DateTime(2026, 6, 23, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9307), 2 },
                    { 8, 485.78829073877536m, 28, new DateTime(2026, 6, 13, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9312), 2 },
                    { 9, 360.775557061645968m, 7, new DateTime(2026, 7, 6, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9313), 2 },
                    { 10, 922.07818889937968m, 10, new DateTime(2026, 6, 16, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9315), 2 },
                    { 11, 419.70870300368745m, 21, new DateTime(2026, 6, 25, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9317), 3 },
                    { 12, 498.30972329634816m, 7, new DateTime(2026, 6, 9, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9318), 3 },
                    { 13, 326.763967031409816m, 45, new DateTime(2026, 6, 27, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9320), 3 },
                    { 14, 431.10550813633092m, 28, new DateTime(2026, 6, 9, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9321), 3 },
                    { 15, 193.41102879113136m, 42, new DateTime(2026, 6, 28, 12, 11, 22, 711, DateTimeKind.Local).AddTicks(9322), 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "UserExpenditures",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "AveragePrice",
                table: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Restaurants",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriceIndex",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
