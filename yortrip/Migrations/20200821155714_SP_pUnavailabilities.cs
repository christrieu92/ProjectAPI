using Microsoft.EntityFrameworkCore.Migrations;

namespace yortrip.Migrations
{
    public partial class SP_pUnavailabilities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE [dbo].[pUnavailabilities] @CalendarId uniqueidentifier, @Duration nvarchar(50)
                                    AS
                                    BEGIN

                                        DECLARE @StartDate DATETIME
                                        DECLARE @EndDate DATETIME

                                        DECLARE @UnavailabilityId INT
                                        DECLARE @UnavailabilityDate Date

                                        DECLARE @AvailableDateRangeId INT
                                        DECLARE @StartDateRangeTime DATETIME
                                        DECLARE @EndDateRangeTime DATETIME

                                        DECLARE @AllAvailableDates Table(AvailableDate date);

                                        SELECT @StartDate = CONVERT(varchar(10), StartMonth, 101),
                                                @EndDate = CONVERT(varchar(10), EndMonth, 101)
                                        FROM Calendars

                                        DECLARE @DayTable Table(calendarDate date, theDayOfWeek nvarchar(50), dateRange date);
                                        WITH DayTable AS 
                                        (
                                            SELECT CAST(@StartDate AS DATETIME) theDate, DATENAME(dw, @StartDate) theDayOfWeek 
                                            UNION ALL 
                                            SELECT DATEADD(dd, 1, theDate), DATENAME(dw,DATEADD(dd, 1, theDate)) 
                                            FROM DayTable s  
                                            WHERE DATEADD(dd, 1, theDate) <= CAST(@EndDate AS DATETIME)
                                        ) 
                                        INSERT INTO @DayTable(calendarDate, theDayOfWeek, dateRange)
                                        SELECT  theDate, 
                                                theDayOfWeek, 
                                                CASE
                                                    WHEN @Duration = '7 Days' THEN DATEADD(DAY, 6, theDate) 
                                                END
                                        FROM DayTable
    
                                        SELECT una.UnavailabilityId, una.UnavailabilityDate
                                        INTO #TempUnaTable
                                        FROM Unavailabilities una
                                        WHERE CalendarId = @CalendarId

                                        IF (@Duration = '7 Days')
                                        BEGIN
                                            CREATE TABLE #Temp_UnavailableDate_Table
                                            (
                                                UnavailableDates Date,
                                                DateRange Date
                                            )

                                            WHILE (SELECT COUNT(*) from #TempUnaTable) > 0
                                            BEGIN
    
                                                SELECT TOP 1 @UnavailabilityId = UnavailabilityId,
                                                                @UnavailabilityDate = UnavailabilityDate FROM #TempUnaTable
                 
                                                ;WITH CTE_Dates AS (
                                                    SELECT a.CalendarDate, a.DateRange, @UnavailabilityDate UnDate
                                                    FROM (
                                                        SELECT calendarDate, dateRange
                                                        FROM @DayTable
                                                    ) a
                                                    WHERE @UnavailabilityDate BETWEEN a.CalendarDate AND a.DateRange
                                                )
    
                                                INSERT INTO #Temp_UnavailableDate_Table
                                                SELECT cte.CalendarDate, cte.DateRange
                                                FROM CTE_Dates cte
                                                WHERE cte.CalendarDate NOT IN (
                                                    SELECT UnavailableDates
                                                    FROM #Temp_UnavailableDate_Table
                                                )

                                                DELETE #TempUnaTable WHERE UnavailabilityId = @UnavailabilityId
                                            END
    
                                            SELECT ROW_NUMBER() OVER(ORDER BY calendarDate) rn, calendarDate, dateRange
                                            INTO #Temp_availableDateRange_Table
                                            FROM @DayTable
                                            WHERE calendarDate NOT IN (SELECT UnavailableDates FROM #Temp_UnavailableDate_Table)

                                            WHILE (SELECT COUNT(*) from #Temp_availableDateRange_Table) > 0
                                            BEGIN
            
                                                SELECT TOP 1 @AvailableDateRangeId = rn,
                                                             @StartDateRangeTime = calendarDate,
                                                             @EndDateRangeTime = dateRange FROM #Temp_availableDateRange_Table

                                                ;WITH DateRange(DateData) AS 
                                                (
                                                    SELECT @StartDateRangeTime as Date
                                                    UNION ALL
                                                    SELECT DATEADD(d,1,DateData)
                                                    FROM DateRange 
                                                    WHERE DateData < @EndDateRangeTime
                                                )

                                                INSERT INTO @AllAvailableDates
                                                SELECT DateData
                                                FROM DateRange

                                                DELETE #Temp_availableDateRange_Table WHERE rn = @AvailableDateRangeId

                                            END

                                            SELECT CONVERT(datetime, calendarDate) AS UnavailableDate
                                            FROM @DayTable
                                            WHERE calendarDate NOT IN (SELECT AvailableDate FROM @AllAvailableDates)
                                        END

                                        IF (@Duration = 'ALL')
                                        BEGIN
                                            SELECT CONVERT(datetime, calendarDate) AS UnavailableDate
                                            FROM @DayTable
                                            WHERE calendarDate IN (SELECT CONVERT(DATE, UnavailabilityDate) FROM #TempUnaTable)
                                        END
                                    END
";

            migrationBuilder.Sql(procedure);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"DROP PROCEDURE pUnavailabilities @CalendarId int, @Duration nvarchar(50)";

            migrationBuilder.Sql(procedure);
        }
    }
}
