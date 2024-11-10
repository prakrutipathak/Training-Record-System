using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingRecordSystemAPI.Migrations
{
    public partial class seedjob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
             table: "Jobs",
             columns: new[] { "JobName" },
             values: new object[,]
             {
         {"Developer"},
         {"Tester" },
         {"Business Analyst" },
         {"Admin"},
         {"Manager" }

             });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
             table: "Jobs",
             keyColumn: "JobId",
             keyValues: new object[]
             {
               1,2,3,4,5
             });

        }
    }
}
