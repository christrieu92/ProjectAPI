using Microsoft.EntityFrameworkCore.Migrations;

namespace yortrip.Migrations
{
    public partial class Database_and_Store_Procedure_Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Viewed",
                table: "Notifications",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            string procedure = @"CREATE PROCEDURE [dbo].[pAvailabilities] @CalendarId uniqueidentifier, @Duration nvarchar(50), @UserList nvarchar(MAX)
                                    AS
                                    BEGIN

                                        DECLARE @StartDate DATETIME
                                        DECLARE @EndDate DATETIME

                                        DECLARE @UnavailabilityId uniqueidentifier
                                        DECLARE @UnavailabilityDate Date

                                        DECLARE @AvailableDateRangeId INT
                                        DECLARE @StartDateRangeTime DATETIME
                                        DECLARE @EndDateRangeTime DATETIME

                                        DECLARE @AllAvailableDates Table(AvailableDate date);

                                        SELECT @StartDate = CONVERT(varchar(10), StartMonth, 101),
                                                @EndDate = CONVERT(varchar(10), EndMonth, 101)
                                        FROM Calendars
                                        WHERE CalendarId = @CalendarId

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
                                                    WHEN @Duration <> 'ALL' THEN DATEADD(DAY, CONVERT(INT, SUBSTRING(@Duration, 1 , Len(@Duration) - 5)) - 1 , theDate)
                                                END
                                        FROM DayTable
    
                                        SELECT una.UnavailabilityId, una.UnavailabilityDate
                                        INTO #TempUnaTable
                                        FROM Unavailabilities una
                                        WHERE CalendarId = @CalendarId
                                        AND UserId IN (SELECT value FROM STRING_SPLIT(@UserList, ','))

                                        IF (@Duration <> 'ALL')
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

                                            SELECT CONVERT(datetime, calendarDate) AS AvailableDate
                                            FROM @DayTable
                                            WHERE calendarDate IN (SELECT AvailableDate FROM @AllAvailableDates)
                                        END

                                        IF (@Duration = 'ALL')
                                        BEGIN
                                            SELECT CONVERT(datetime, calendarDate) AS AvailableDate
                                            FROM @DayTable
                                            WHERE calendarDate NOT IN (SELECT CONVERT(DATE, UnavailabilityDate) FROM #TempUnaTable)
                                        END
                                    END";

            migrationBuilder.Sql(procedure);

            string procedure1 = @"CREATE PROCEDURE [dbo].[pUnavailabilities] @CalendarId uniqueidentifier, @Duration nvarchar(50), @UserList nvarchar(MAX)
                                    AS
                                    BEGIN

                                        DECLARE @StartDate DATETIME
                                        DECLARE @EndDate DATETIME

                                        DECLARE @UnavailabilityId uniqueidentifier
                                        DECLARE @UnavailabilityDate Date

                                        DECLARE @AvailableDateRangeId INT
                                        DECLARE @StartDateRangeTime DATETIME
                                        DECLARE @EndDateRangeTime DATETIME

                                        DECLARE @AllAvailableDates Table(AvailableDate date);

                                        SELECT @StartDate = CONVERT(varchar(10), StartMonth, 101),
                                                @EndDate = CONVERT(varchar(10), EndMonth, 101)
                                        FROM Calendars
                                        WHERE CalendarId = @CalendarId

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
                                                    WHEN @Duration <> 'ALL' THEN DATEADD(DAY, CONVERT(INT, SUBSTRING(@Duration, 1 , Len(@Duration) - 5)) - 1 , theDate)
                                                END
                                        FROM DayTable
    
                                        SELECT una.UnavailabilityId, una.UnavailabilityDate
                                        INTO #TempUnaTable
                                        FROM Unavailabilities una
                                        WHERE CalendarId = @CalendarId
                                        AND UserId IN (SELECT value FROM STRING_SPLIT(@UserList, ','))

                                        IF (@Duration <> 'ALL')
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
                                    END";

            migrationBuilder.Sql(procedure1);

            string procedure2 = @"CREATE PROCEDURE [dbo].[pUnavailabilitiesRanges] @CalendarId uniqueidentifier, @Duration nvarchar(50), @UserList nvarchar(MAX)
                                    AS
                                    BEGIN

                                        DECLARE @StartDate DATETIME
                                        DECLARE @EndDate DATETIME

                                        DECLARE @UnavailabilityId uniqueidentifier
                                        DECLARE @UnavailabilityDate Date

                                        DECLARE @AvailableDateRangeId INT
                                        DECLARE @StartDateRangeTime DATETIME
                                        DECLARE @EndDateRangeTime DATETIME

                                        SELECT @StartDate = CONVERT(varchar(10), StartMonth, 101),
                                                @EndDate = CONVERT(varchar(10), EndMonth, 101)
                                        FROM Calendars
                                        WHERE CalendarId = @CalendarId

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
                                                    WHEN @Duration <> 'ALL' THEN DATEADD(DAY, CONVERT(INT, SUBSTRING(@Duration, 1 , Len(@Duration) - 5)) - 1 , theDate)
                                                END
                                        FROM DayTable
    
                                        SELECT una.UnavailabilityId, una.UnavailabilityDate, una.UserId
                                        INTO #TempUnaTable
                                        FROM Unavailabilities una
                                        WHERE CalendarId = @CalendarId
                                        AND UserId IN (SELECT value FROM STRING_SPLIT(@UserList, ','))

                                        IF (@Duration <> 'ALL')
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

                                            SELECT CONVERT(datetime, calendarDate) StartRange, CONVERT(datetime, dateRange) EndRange
                                            FROM #Temp_availableDateRange_Table
                                        END
                                    END";

            migrationBuilder.Sql(procedure2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Viewed",
                table: "Notifications",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.Sql(@"DROP PROCEDURE [dbo].[pAvailabilities]");
            migrationBuilder.Sql(@"DROP PROCEDURE [dbo].[pUnavailabilities]");
            migrationBuilder.Sql(@"DROP PROCEDURE [dbo].[pUnavailabilitiesRanges]");
        }
    }
}
