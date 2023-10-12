using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservaHotel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DBHotelInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Identificacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadoHabitacion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoHabitacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadoReservacion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoReservacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoHabitacion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoHabitacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Voter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Habitacion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroHabitacion = table.Column<int>(name: "NumeroHabitacion", type: "int", nullable: false),
                    CapacidadPersonas = table.Column<int>(name: "CapacidadPersonas", type: "int", nullable: false),
                    NumeroCamas = table.Column<int>(name: "NumeroCamas", type: "int", nullable: false),
                    PrecioNoche = table.Column<double>(name: "PrecioNoche", type: "float", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoHabitacionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstadoHabitacionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habitacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Habitacion_EstadoHabitacion_EstadoHabitacionId",
                        column: x => x.EstadoHabitacionId,
                        principalTable: "EstadoHabitacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Habitacion_TipoHabitacion_TipoHabitacionId",
                        column: x => x.TipoHabitacionId,
                        principalTable: "TipoHabitacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservacion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaEntrada = table.Column<DateTime>(name: "FechaEntrada", type: "datetime2", nullable: false),
                    FechaSalida = table.Column<DateTime>(name: "FechaSalida", type: "datetime2", nullable: false),
                    PrecioTotal = table.Column<double>(name: "PrecioTotal", type: "float", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CantidadDias = table.Column<int>(name: "CantidadDias", type: "int", nullable: false),
                    CodigoReservacion = table.Column<string>(name: "CodigoReservacion", type: "nvarchar(max)", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HabitacionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstadoReservacionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservacion_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservacion_EstadoReservacion_EstadoReservacionId",
                        column: x => x.EstadoReservacionId,
                        principalTable: "EstadoReservacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservacion_Habitacion_HabitacionId",
                        column: x => x.HabitacionId,
                        principalTable: "Habitacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Habitacion_EstadoHabitacionId",
                table: "Habitacion",
                column: "EstadoHabitacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Habitacion_TipoHabitacionId",
                table: "Habitacion",
                column: "TipoHabitacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservacion_ClienteId",
                table: "Reservacion",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservacion_EstadoReservacionId",
                table: "Reservacion",
                column: "EstadoReservacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservacion_HabitacionId",
                table: "Reservacion",
                column: "HabitacionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservacion");

            migrationBuilder.DropTable(
                name: "Voter");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "EstadoReservacion");

            migrationBuilder.DropTable(
                name: "Habitacion");

            migrationBuilder.DropTable(
                name: "EstadoHabitacion");

            migrationBuilder.DropTable(
                name: "TipoHabitacion");
        }
    }
}
