using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservaHotel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjusteNombreColumnasyEliminarVoter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Voter");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Voter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voter", x => x.Id);
                });
        }
    }
}
