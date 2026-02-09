using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaSApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CalculatorOperatorAndSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "calculatorSessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calculatorSessions", x => x.SessionId);
                });

            migrationBuilder.CreateTable(
                name: "calculatorOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Operand1 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Operand2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Operator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Result = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calculatorOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_calculatorOperations_calculatorSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "calculatorSessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_calculatorOperations_SessionId",
                table: "calculatorOperations",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "calculatorOperations");

            migrationBuilder.DropTable(
                name: "calculatorSessions");
        }
    }
}
