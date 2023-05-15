using FootballApi.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballApi.Data
{
    public static class ExtraMigration
    {
        public static void Steps(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetPlayerTimestampOnUpdate
                    AFTER UPDATE ON Players
                    BEGIN
                        UPDATE Players
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetPlayerTimestampOnInsert
                    AFTER INSERT ON Players
                    BEGIN
                        UPDATE Players
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
           
        }
    }
}
