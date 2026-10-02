using Microsoft.EntityFrameworkCore.Migrations;

namespace _5417_Sagenergy.Migrations
{
    public partial class AddServiceRequestServiceRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequestDetails_ServiceRequests_ServiceRequestId",
                table: "ServiceRequestDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceRequestId",
                table: "ServiceRequestDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequestDetails_ServiceRequests_ServiceRequestId",
                table: "ServiceRequestDetails",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequestDetails_ServiceRequests_ServiceRequestId",
                table: "ServiceRequestDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceRequestId",
                table: "ServiceRequestDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequestDetails_ServiceRequests_ServiceRequestId",
                table: "ServiceRequestDetails",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
