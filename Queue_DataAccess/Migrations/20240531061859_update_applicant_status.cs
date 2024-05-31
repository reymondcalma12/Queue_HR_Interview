using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Queue.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class update_applicant_status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Stage1_Table",
                table: "Applicant_Status",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stage2_Table",
                table: "Applicant_Status",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stage3_Table",
                table: "Applicant_Status",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Table",
                keyColumn: "TableId",
                keyValue: 18,
                column: "Username",
                value: "HUMILITY");

            migrationBuilder.UpdateData(
                table: "Table",
                keyColumn: "TableId",
                keyValue: 19,
                column: "Username",
                value: "OPENNESS");

            migrationBuilder.UpdateData(
                table: "Table",
                keyColumn: "TableId",
                keyValue: 20,
                column: "Username",
                value: "OWNER'S MINDSET");

            migrationBuilder.UpdateData(
                table: "Table",
                keyColumn: "TableId",
                keyValue: 21,
                column: "Username",
                value: "TRANSPARENCY");

            migrationBuilder.UpdateData(
                table: "Table",
                keyColumn: "TableId",
                keyValue: 22,
                column: "Username",
                value: "UNITY AND LUCIA");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stage1_Table",
                table: "Applicant_Status");

            migrationBuilder.DropColumn(
                name: "Stage2_Table",
                table: "Applicant_Status");

            migrationBuilder.DropColumn(
                name: "Stage3_Table",
                table: "Applicant_Status");

            migrationBuilder.UpdateData(
                table: "Table",
                keyColumn: "TableId",
                keyValue: 18,
                column: "Username",
                value: "Room - HUMILITY");

            migrationBuilder.UpdateData(
                table: "Table",
                keyColumn: "TableId",
                keyValue: 19,
                column: "Username",
                value: "Room - OPENNESS");

            migrationBuilder.UpdateData(
                table: "Table",
                keyColumn: "TableId",
                keyValue: 20,
                column: "Username",
                value: "Room - OWNER'S MINDSET");

            migrationBuilder.UpdateData(
                table: "Table",
                keyColumn: "TableId",
                keyValue: 21,
                column: "Username",
                value: "Room - TRANSPARENCY");

            migrationBuilder.UpdateData(
                table: "Table",
                keyColumn: "TableId",
                keyValue: 22,
                column: "Username",
                value: "Room - UNITY AND LUCIA");
        }
    }
}
