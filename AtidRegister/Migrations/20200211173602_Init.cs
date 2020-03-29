using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AtidRegister.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FAQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Question = table.Column<string>(nullable: false),
                    Answer = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(maxLength: 40, nullable: false),
                    JobTitle = table.Column<string>(maxLength: 70, nullable: false),
                    ImageFile = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeStrips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeStrips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassNumber = table.Column<int>(nullable: false),
                    ClassId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FriendlyName = table.Column<string>(nullable: true),
                    GradeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 300, nullable: false),
                    ImageFile = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    TimeStripId = table.Column<int>(nullable: false),
                    IsDoubleTimeStrip = table.Column<bool>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contents_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contents_TimeStrips_TimeStripId",
                        column: x => x.TimeStripId,
                        principalTable: "TimeStrips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contents_ContentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ContentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentPeople",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PersonId = table.Column<int>(nullable: false),
                    ContentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentPeople", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentPeople_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentPeople_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContentUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    ContentId = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentUsers_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "ClassName" },
                values: new object[] { -1, "י'" });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "ClassName" },
                values: new object[] { -2, "י\"א" });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "ClassName" },
                values: new object[] { -3, "י\"ב" });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -11, -1, 1 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -35, -3, 5 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -34, -3, 4 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -33, -3, 3 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -32, -3, 2 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -31, -3, 1 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -27, -2, 7 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -26, -2, 6 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -25, -2, 5 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -36, -3, 6 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -24, -2, 4 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -22, -2, 2 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -21, -2, 1 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -17, -1, 7 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -16, -1, 6 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -15, -1, 5 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -14, -1, 4 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -13, -1, 3 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -12, -1, 2 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -23, -2, 3 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "ClassId", "ClassNumber" },
                values: new object[] { -37, -3, 7 });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GradeId",
                table: "AspNetUsers",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentPeople_ContentId",
                table: "ContentPeople",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentPeople_PersonId",
                table: "ContentPeople",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_AppUserId",
                table: "Contents",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_TimeStripId",
                table: "Contents",
                column: "TimeStripId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_TypeId",
                table: "Contents",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentUsers_ContentId",
                table: "ContentUsers",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentUsers_UserId",
                table: "ContentUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ClassId",
                table: "Grades",
                column: "ClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ContentPeople");

            migrationBuilder.DropTable(
                name: "ContentUsers");

            migrationBuilder.DropTable(
                name: "FAQuestions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TimeStrips");

            migrationBuilder.DropTable(
                name: "ContentTypes");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Classes");
        }
    }
}
