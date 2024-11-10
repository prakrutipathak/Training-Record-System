using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace TrainingRecordSystemAPI.Migrations
{
    public partial class seedusermanager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            byte[] passwordSalt;
            byte[] passwordHash;

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Password@123"));
            }

            migrationBuilder.InsertData(
              table: "Users",
              columns: new[] { "LoginId", "FirstName", "LastName", "Email", "PasswordHash", "PasswordSalt", "Role", "Loginbit", "JobId" },
              values: new object[,]
              {
         {"admin","AdminF","AdminL","admin@gmail.com",passwordHash,passwordSalt,0,true,4},
              {"kavit","Kavit","Trivedi","kavit@gmail.com",passwordHash,passwordSalt,1,true,5},
               {"chirag","Chirag","Dani","chirag@gmail.com",passwordHash,passwordSalt,1,true,5},
            
              });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
          table: "Users",
          keyColumn: "UserId",
          keyValues: new object[]
          {
               1,2,3
          });

        }
    }
}
