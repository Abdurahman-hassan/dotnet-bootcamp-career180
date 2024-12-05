using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiCoreEcommerce.Migrations
{
    public partial class adduserid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create a new temporary table with the desired schema
            migrationBuilder.CreateTable(
                name: "AspNetUserRoles_Temp",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false),
                    UserId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles_Temp", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Temp_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Temp_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Temp_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Copy data from the old table to the new table
            migrationBuilder.Sql(
                "INSERT INTO AspNetUserRoles_Temp (UserId, RoleId, UserId1) SELECT UserId, RoleId, UserId2 FROM AspNetUserRoles");

            // Drop the old table
            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            // Rename the temporary table to the original table name
            migrationBuilder.RenameTable(
                name: "AspNetUserRoles_Temp",
                newName: "AspNetUserRoles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert by recreating the old structure
            migrationBuilder.CreateTable(
                name: "AspNetUserRoles_Temp",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false),
                    UserId2 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles_Temp", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Temp_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Temp_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Temp_AspNetUsers_UserId2",
                        column: x => x.UserId2,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Copy data back from the current table
            migrationBuilder.Sql(
                "INSERT INTO AspNetUserRoles_Temp (UserId, RoleId, UserId2) SELECT UserId, RoleId, UserId1 FROM AspNetUserRoles");

            // Drop the new table
            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            // Rename back to the original table
            migrationBuilder.RenameTable(
                name: "AspNetUserRoles_Temp",
                newName: "AspNetUserRoles");
        }
    }
}
