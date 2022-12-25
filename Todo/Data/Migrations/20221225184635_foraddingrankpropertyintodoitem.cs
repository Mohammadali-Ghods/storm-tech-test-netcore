using Microsoft.EntityFrameworkCore.Migrations;

namespace Todo.Data.Migrations
{
    public partial class foraddingrankpropertyintodoitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "TodoItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "TodoItems");
        }
    }
}
