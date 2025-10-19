using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EzyClassroomz.Library.Migrations
{
    /// <inheritdoc />
    public partial class _202510191944_addUserAuthorizationPolicies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email_Name",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserAuthorizationPolicies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthorizationPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAuthorizationPolicies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorizationPolicies_Name_UserId",
                table: "UserAuthorizationPolicies",
                columns: new[] { "Name", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorizationPolicies_UserId",
                table: "UserAuthorizationPolicies",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAuthorizationPolicies");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_Name",
                table: "Users",
                columns: new[] { "Email", "Name" },
                unique: true);
        }
    }
}
