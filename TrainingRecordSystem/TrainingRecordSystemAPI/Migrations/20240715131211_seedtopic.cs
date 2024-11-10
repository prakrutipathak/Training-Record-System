using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingRecordSystemAPI.Migrations
{
    public partial class seedtopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
      table: "Topics",
      columns: new[] { "TopicName", "JobId" },
      values: new object[,]
      {
        {".Net MVC",1},
        {"Angular",1 },
        {"Java" ,1},
         {"UnitTest" ,1},
          {"Web Development" ,1},
           {"SQL Testing",2},
        {"Regression Testing",2 },
        {"Penetration Testing" ,2},
         {"Automation Testing" ,2},
          {"Manual Testing" ,2},
           {"Data Analysis",3},
        {"BI Tools",3 },
        {"Process Modeling and Analysis" ,3},
         {"Requirement Management" ,3},
          {"Agile Methodology" ,3},


      });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
     table: "Topics",
     keyColumn: "TopicId",
     keyValues: new object[]
     {
              1,2,3,4,5,6,7,8,9,10,11,12,13,14,15
     });

        }
    }
}
