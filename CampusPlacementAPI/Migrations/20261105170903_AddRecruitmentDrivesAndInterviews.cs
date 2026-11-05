using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusPlacementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRecruitmentDrivesAndInterviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RecruitmentDrives",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InterviewRoundId1",
                table: "InterviewSchedules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSchedules_InterviewRoundId1",
                table: "InterviewSchedules",
                column: "InterviewRoundId1");

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewSchedules_InterviewRound_InterviewRoundId1",
                table: "InterviewSchedules",
                column: "InterviewRoundId1",
                principalTable: "InterviewRound",
                principalColumn: "InterviewRoundId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewSchedules_InterviewRound_InterviewRoundId1",
                table: "InterviewSchedules");

            migrationBuilder.DropIndex(
                name: "IX_InterviewSchedules_InterviewRoundId1",
                table: "InterviewSchedules");

            migrationBuilder.DropColumn(
                name: "InterviewRoundId1",
                table: "InterviewSchedules");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RecruitmentDrives",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
