using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTracker.Migrations
{
	/// <inheritdoc />
	public partial class CreationDate : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "Title",
				table: "Tickets",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Description",
				table: "Tickets",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AddColumn<DateTime>(
				name: "CreatedAt",
				table: "Tickets",
				type: "datetime2",
				nullable: true);

			migrationBuilder.AddColumn<DateTime>(
				name: "UpdatedAt",
				table: "Tickets",
				type: "datetime2",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_Tickets_ProjectId",
				table: "Tickets",
				column: "ProjectId");

			migrationBuilder.AddForeignKey(
				name: "FK_Tickets_Projects_ProjectId",
				table: "Tickets",
				column: "ProjectId",
				principalTable: "Projects",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Tickets_Projects_ProjectId",
				table: "Tickets");

			migrationBuilder.DropIndex(
				name: "IX_Tickets_ProjectId",
				table: "Tickets");

			migrationBuilder.DropColumn(
				name: "CreatedAt",
				table: "Tickets");

			migrationBuilder.DropColumn(
				name: "UpdatedAt",
				table: "Tickets");

			migrationBuilder.AlterColumn<string>(
				name: "Title",
				table: "Tickets",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<string>(
				name: "Description",
				table: "Tickets",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");
		}
	}
}
