using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantSuggestionApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class LastDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    AveragePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "UserExpenditures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SpentAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExpenditures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExpenditures_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserExpenditures_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "AveragePrice", "Category", "Latitude", "Longitude", "Name" },
                values: new object[,]
                {
                    { 1, 237m, "Kebab & Grill", 40.99346212931823084m, 29.01841814596746962m, "Kadikoy Kebab & Grill Restaurant 1" },
                    { 2, 333m, "World Cuisine", 41.043255285520504826m, 29.00086868448339808m, "Besiktas World Cuisine Restaurant 2" },
                    { 3, 271m, "Seafood", 41.06468816729452842m, 28.987958455830746542m, "Sisli Seafood Restaurant 3" },
                    { 4, 330m, "Cafe & Dessert", 41.02172501117327484m, 28.94489176916624036m, "Fatih Cafe & Dessert Restaurant 4" },
                    { 5, 416m, "Home Cooking", 41.0268120708929896686m, 29.01170466374643364m, "Uskudar Home Cooking Restaurant 5" },
                    { 6, 174m, "Kebab & Grill", 40.98530489270109912m, 29.025949021996533974m, "Kadikoy Kebab & Grill Restaurant 6" },
                    { 7, 428m, "World Cuisine", 41.04908274751654022m, 29.009043937079396070m, "Besiktas World Cuisine Restaurant 7" },
                    { 8, 643m, "Seafood", 41.05324386160478176m, 28.97950225220592798m, "Sisli Seafood Restaurant 8" },
                    { 9, 180m, "Cafe & Dessert", 41.02282363899439742m, 28.950999411642737414m, "Fatih Cafe & Dessert Restaurant 9" },
                    { 10, 784m, "Home Cooking", 41.03092309179521310m, 29.00825522532563434m, "Uskudar Home Cooking Restaurant 10" },
                    { 11, 280m, "Kebab & Grill", 40.99395179445792538m, 29.025928274280917026m, "Kadikoy Kebab & Grill Restaurant 11" },
                    { 12, 558m, "World Cuisine", 41.03363019932770646m, 29.00366399325717426m, "Besiktas World Cuisine Restaurant 12" },
                    { 13, 525m, "Seafood", 41.06662130303961286m, 28.97777909721698570m, "Sisli Seafood Restaurant 13" },
                    { 14, 419m, "Cafe & Dessert", 41.02260368933347226m, 28.955167178541686m, "Fatih Cafe & Dessert Restaurant 14" },
                    { 15, 185m, "Home Cooking", 41.01968238143463730m, 29.00582679028386566m, "Uskudar Home Cooking Restaurant 15" },
                    { 16, 230m, "Kebab & Grill", 40.99573285833021294m, 29.0281291276874622m, "Kadikoy Kebab & Grill Restaurant 16" },
                    { 17, 352m, "World Cuisine", 41.03779921656446496m, 29.01294856648680222m, "Besiktas World Cuisine Restaurant 17" },
                    { 18, 181m, "Seafood", 41.06316291814789312m, 28.98057292969546882m, "Sisli Seafood Restaurant 18" },
                    { 19, 185m, "Cafe & Dessert", 41.01859118567970170m, 28.950518267713449090m, "Fatih Cafe & Dessert Restaurant 19" },
                    { 20, 589m, "Home Cooking", 41.025107323187406792m, 29.0194316465261074m, "Uskudar Home Cooking Restaurant 20" },
                    { 21, 405m, "Kebab & Grill", 40.98446891074495804m, 29.01586507549931532m, "Kadikoy Kebab & Grill Restaurant 21" },
                    { 22, 652m, "World Cuisine", 41.0381487682181172m, 28.99887321671534946m, "Besiktas World Cuisine Restaurant 22" },
                    { 23, 158m, "Seafood", 41.05161885883240908m, 28.986981871565283216m, "Sisli Seafood Restaurant 23" },
                    { 24, 509m, "Cafe & Dessert", 41.02245618653854178m, 28.948983589661486256m, "Fatih Cafe & Dessert Restaurant 24" },
                    { 25, 730m, "Home Cooking", 41.02669193593393635782m, 29.00547905818306796m, "Uskudar Home Cooking Restaurant 25" },
                    { 26, 194m, "Kebab & Grill", 40.98035793644611628m, 29.01566148017014446m, "Kadikoy Kebab & Grill Restaurant 26" },
                    { 27, 789m, "World Cuisine", 41.04588286241864918m, 28.99819078066418450m, "Besiktas World Cuisine Restaurant 27" },
                    { 28, 412m, "Seafood", 41.06877941533372710m, 28.986923875230748148m, "Sisli Seafood Restaurant 28" },
                    { 29, 410m, "Cafe & Dessert", 41.01883212161452142m, 28.94108887631003228m, "Fatih Cafe & Dessert Restaurant 29" },
                    { 30, 704m, "Home Cooking", 41.01953760198524110m, 29.014039013648004744m, "Uskudar Home Cooking Restaurant 30" },
                    { 31, 358m, "Kebab & Grill", 40.990747311969961650m, 29.023613979852206064m, "Kadikoy Kebab & Grill Restaurant 31" },
                    { 32, 664m, "World Cuisine", 41.05094516757528538m, 29.009234778588514206m, "Besiktas World Cuisine Restaurant 32" },
                    { 33, 820m, "Seafood", 41.05058824332896072m, 28.99533361321652010m, "Sisli Seafood Restaurant 33" },
                    { 34, 263m, "Cafe & Dessert", 41.0131566852976832m, 28.95441399949780386m, "Fatih Cafe & Dessert Restaurant 34" },
                    { 35, 383m, "Home Cooking", 41.01876285693778790m, 29.014343030025923174m, "Uskudar Home Cooking Restaurant 35" },
                    { 36, 244m, "Kebab & Grill", 40.99562220879845424m, 29.01857098221395676m, "Kadikoy Kebab & Grill Restaurant 36" },
                    { 37, 508m, "World Cuisine", 41.03711320860251468m, 29.009333249233585432m, "Besiktas World Cuisine Restaurant 37" },
                    { 38, 620m, "Seafood", 41.06994043183947934m, 28.9930244607687576m, "Sisli Seafood Restaurant 38" },
                    { 39, 536m, "Cafe & Dessert", 41.02079898888538544m, 28.94598949541123094m, "Fatih Cafe & Dessert Restaurant 39" },
                    { 40, 166m, "Home Cooking", 41.03649657166628008m, 29.01831664660359576m, "Uskudar Home Cooking Restaurant 40" },
                    { 41, 169m, "Kebab & Grill", 40.98700815029987514m, 29.03418125684716798m, "Kadikoy Kebab & Grill Restaurant 41" },
                    { 42, 176m, "World Cuisine", 41.0371949449455342m, 29.00280887456858012m, "Besiktas World Cuisine Restaurant 42" },
                    { 43, 445m, "Seafood", 41.05452247408866998m, 28.989546768358650974m, "Sisli Seafood Restaurant 43" },
                    { 44, 234m, "Cafe & Dessert", 41.00837465549533938m, 28.96006969311715556m, "Fatih Cafe & Dessert Restaurant 44" },
                    { 45, 376m, "Home Cooking", 41.01882188835354610m, 29.01172617712096598m, "Uskudar Home Cooking Restaurant 45" },
                    { 46, 156m, "Kebab & Grill", 40.99512302783309624m, 29.0321165583959392m, "Kadikoy Kebab & Grill Restaurant 46" },
                    { 47, 777m, "World Cuisine", 41.03364168201351616m, 29.01430991281606718m, "Besiktas World Cuisine Restaurant 47" },
                    { 48, 741m, "Seafood", 41.05496147307304734m, 28.99622439338272642m, "Sisli Seafood Restaurant 48" },
                    { 49, 594m, "Cafe & Dessert", 41.01208893800042054m, 28.951106880130482318m, "Fatih Cafe & Dessert Restaurant 49" },
                    { 50, 358m, "Home Cooking", 41.0292065848289554m, 29.00914617868989994m, "Uskudar Home Cooking Restaurant 50" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "ozgur@email.com", "Ozgur", "hashed_password_1" },
                    { 2, "ahmet@email.com", "Ahmet", "hashed_password_2" },
                    { 3, "zeynep@email.com", "Zeynep", "hashed_password_3" }
                });

            migrationBuilder.InsertData(
                table: "UserExpenditures",
                columns: new[] { "Id", "Amount", "RestaurantId", "SpentAt", "UserId" },
                values: new object[,]
                {
                    { 1, 636.477411670832592m, 10, new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 928.49223386593632m, 47, new DateTime(2025, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, 493.63732665368996m, 7, new DateTime(2025, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 4, 189.43901842620265m, 15, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 5, 207.100139163015464m, 36, new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 6, 678.74644226429074m, 20, new DateTime(2025, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 7, 214.57897900928618m, 18, new DateTime(2025, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 8, 485.78829073877536m, 28, new DateTime(2025, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 9, 360.775557061645968m, 7, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 10, 922.07818889937968m, 10, new DateTime(2025, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 11, 419.70870300368745m, 21, new DateTime(2025, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 12, 498.30972329634816m, 7, new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 13, 326.763967031409816m, 45, new DateTime(2025, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 14, 431.10550813633092m, 28, new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 15, 193.41102879113136m, 42, new DateTime(2025, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RestaurantId",
                table: "Comments",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExpenditures_RestaurantId",
                table: "UserExpenditures",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExpenditures_UserId",
                table: "UserExpenditures",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "UserExpenditures");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
