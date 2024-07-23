using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsApp.Migrations
{
    /// <inheritdoc />
    public partial class AddIndenttiy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                    name:"DriverId",
                    table:"Drivers",
                    type:"uniqueidentifier",
                    nullable:false,
                    defaultValueSql:"NEWID()",
                    oldClrType:typeof(Guid),
                    oldType:"uniqueidentifier");
            migrationBuilder.AlterColumn<Guid>(
                name: "OrderTrackingId",
                table: "OrderTrackings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "OrderTrackingId",
                table: "OrderTrackings",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWID()");
        }
    }
}
