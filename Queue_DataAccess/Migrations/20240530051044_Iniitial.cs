using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Queue.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Iniitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applicant_Form",
                columns: table => new
                {
                    ApplicantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicant_Form", x => x.ApplicantId);
                });

            migrationBuilder.CreateTable(
                name: "Queue_Status",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queue_Status", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "Stage",
                columns: table => new
                {
                    StageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stage", x => x.StageId);
                });

            migrationBuilder.CreateTable(
                name: "Applicant_Status",
                columns: table => new
                {
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    Stage_1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stage_2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stage_3 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicant_Status", x => x.ApplicantId);
                    table.ForeignKey(
                        name: "FK_Applicant_Status_Applicant_Form_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicant_Form",
                        principalColumn: "ApplicantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Queue_Stage_1",
                columns: table => new
                {
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    TemporaryRejected_At = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    Generated_At = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queue_Stage_1", x => new { x.ApplicantId, x.StatusId });
                    table.ForeignKey(
                        name: "FK_Queue_Stage_1_Applicant_Form_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicant_Form",
                        principalColumn: "ApplicantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Queue_Stage_1_Queue_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Queue_Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Queue_Stage_1_Stage_StageId",
                        column: x => x.StageId,
                        principalTable: "Stage",
                        principalColumn: "StageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Queue_Stage_2",
                columns: table => new
                {
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    TemporaryRejected_At = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    Generated_At = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queue_Stage_2", x => new { x.ApplicantId, x.StatusId });
                    table.ForeignKey(
                        name: "FK_Queue_Stage_2_Applicant_Form_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicant_Form",
                        principalColumn: "ApplicantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Queue_Stage_2_Queue_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Queue_Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Queue_Stage_2_Stage_StageId",
                        column: x => x.StageId,
                        principalTable: "Stage",
                        principalColumn: "StageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Queue_Stage_3",
                columns: table => new
                {
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    TemporaryRejected_At = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    Generated_At = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queue_Stage_3", x => new { x.ApplicantId, x.StatusId });
                    table.ForeignKey(
                        name: "FK_Queue_Stage_3_Applicant_Form_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicant_Form",
                        principalColumn: "ApplicantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Queue_Stage_3_Queue_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Queue_Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Queue_Stage_3_Stage_StageId",
                        column: x => x.StageId,
                        principalTable: "Stage",
                        principalColumn: "StageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Table",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table", x => x.TableId);
                    table.ForeignKey(
                        name: "FK_Table_Stage_StageId",
                        column: x => x.StageId,
                        principalTable: "Stage",
                        principalColumn: "StageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Serving",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    Served_At = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Serving", x => new { x.TableId, x.ApplicantId, x.StageId });
                    table.ForeignKey(
                        name: "FK_Serving_Applicant_Form_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicant_Form",
                        principalColumn: "ApplicantId");
                    table.ForeignKey(
                        name: "FK_Serving_Stage_StageId",
                        column: x => x.StageId,
                        principalTable: "Stage",
                        principalColumn: "StageId");
                    table.ForeignKey(
                        name: "FK_Serving_Table_TableId",
                        column: x => x.TableId,
                        principalTable: "Table",
                        principalColumn: "TableId");
                });

            migrationBuilder.CreateTable(
                name: "Table_Serve",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false),
                    TotalPassed = table.Column<int>(type: "int", nullable: true),
                    TotalPooled = table.Column<int>(type: "int", nullable: true),
                    TotalFailed = table.Column<int>(type: "int", nullable: true),
                    Served_At = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table_Serve", x => x.TableId);
                    table.ForeignKey(
                        name: "FK_Table_Serve_Table_TableId",
                        column: x => x.TableId,
                        principalTable: "Table",
                        principalColumn: "TableId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Queue_Status",
                columns: new[] { "StatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "waiting" },
                    { 2, "temp_reject" },
                    { 3, "rejected" }
                });

            migrationBuilder.InsertData(
                table: "Stage",
                columns: new[] { "StageId", "StageName" },
                values: new object[,]
                {
                    { 1, "Pre-Screening - (Ground Floor)" },
                    { 2, "Initial Interviewing - (Third Floor)" },
                    { 3, "Final Interviewing - (Second Floor)" }
                });

            migrationBuilder.InsertData(
                table: "Table",
                columns: new[] { "TableId", "StageId", "Username" },
                values: new object[,]
                {
                    { 1, 1, "Table 1" },
                    { 2, 1, "Table 2" },
                    { 3, 1, "Table 3" },
                    { 4, 1, "Table 4" },
                    { 5, 1, "Table 5" },
                    { 6, 2, "Table 1" },
                    { 7, 2, "Table 2" },
                    { 8, 2, "Table 3" },
                    { 9, 2, "Table 4" },
                    { 10, 2, "Table 5" },
                    { 11, 2, "Table 6" },
                    { 12, 2, "Table 7" },
                    { 13, 2, "Table 8" },
                    { 14, 2, "Table 9" },
                    { 15, 2, "Table 10" },
                    { 16, 2, "Table 11" },
                    { 17, 2, "Table 12" },
                    { 18, 3, "Room - HUMILITY" },
                    { 19, 3, "Room - OPENNESS" },
                    { 20, 3, "Room - OWNER'S MINDSET" },
                    { 21, 3, "Room - TRANSPARENCY" },
                    { 22, 3, "Room - UNITY AND LUCIA" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Queue_Stage_1_StageId",
                table: "Queue_Stage_1",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Queue_Stage_1_StatusId",
                table: "Queue_Stage_1",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Queue_Stage_2_StageId",
                table: "Queue_Stage_2",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Queue_Stage_2_StatusId",
                table: "Queue_Stage_2",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Queue_Stage_3_StageId",
                table: "Queue_Stage_3",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Queue_Stage_3_StatusId",
                table: "Queue_Stage_3",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Serving_ApplicantId",
                table: "Serving",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_Serving_StageId",
                table: "Serving",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Table_StageId",
                table: "Table",
                column: "StageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applicant_Status");

            migrationBuilder.DropTable(
                name: "Queue_Stage_1");

            migrationBuilder.DropTable(
                name: "Queue_Stage_2");

            migrationBuilder.DropTable(
                name: "Queue_Stage_3");

            migrationBuilder.DropTable(
                name: "Serving");

            migrationBuilder.DropTable(
                name: "Table_Serve");

            migrationBuilder.DropTable(
                name: "Queue_Status");

            migrationBuilder.DropTable(
                name: "Applicant_Form");

            migrationBuilder.DropTable(
                name: "Table");

            migrationBuilder.DropTable(
                name: "Stage");
        }
    }
}
