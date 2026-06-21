using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSystem.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorrectStreateProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address_Streat",
                table: "Trainers",
                newName: "Address_Street");

            migrationBuilder.RenameColumn(
                name: "Address_Streat",
                table: "Members",
                newName: "Address_Street");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address_Street",
                table: "Trainers",
                newName: "Address_Streat");

            migrationBuilder.RenameColumn(
                name: "Address_Street",
                table: "Members",
                newName: "Address_Streat");
        }
    }
}
