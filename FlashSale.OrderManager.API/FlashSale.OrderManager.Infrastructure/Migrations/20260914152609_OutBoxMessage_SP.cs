using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashSale.OrderManager.Infrastructure.Migrations
{
    public partial class AddClaimOutboxMessagesProcedure:Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE dbo.ClaimOutboxMessages
                    @BatchSize INT = 20,
                    @LeaseSeconds INT = 30
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @LeaseUntil DATETIMEOFFSET = DATEADD(SECOND, @LeaseSeconds, SYSDATETIMEOFFSET());
                    DECLARE @Claimed TABLE (Id UNIQUEIDENTIFIER);

                    ;WITH Candidates AS (
                        SELECT TOP (@BatchSize) *
                        FROM dbo.OutboxMessages WITH (READPAST, ROWLOCK)
                        WHERE PublishedOnUtc IS NULL
                          AND (LockedUntilUtc IS NULL OR LockedUntilUtc < SYSDATETIMEOFFSET())
                        ORDER BY OccurredOnUtc
                    )
                    UPDATE Candidates
                    SET LockedUntilUtc = @LeaseUntil
                    OUTPUT inserted.Id INTO @Claimed;

                    SELECT o.Id, o.EventType, o.Payload, o.OccurredOnUtc, o.PublishedOnUtc,
                           o.RetryCount, o.Error, o.LockedUntilUtc
                    FROM dbo.OutboxMessages o
                    JOIN @Claimed c ON c.Id = o.Id;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.ClaimOutboxMessages");
        }
    }
}