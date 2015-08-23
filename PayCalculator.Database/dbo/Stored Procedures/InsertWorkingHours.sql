



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertWorkingHours]
	@Id int,
	@WeekdayId int,
	@StartTime datetime2(7),
	@EndTime datetime2(7)
AS
BEGIN
	SET NOCOUNT ON;

	SET IDENTITY_INSERT PayCalculator.dbo.WorkingHours ON;

	if (@Id is null)
	begin
		set @Id = (select max(Id) from dbo.WorkingHours) + 1;

		-- Base case when there is no Id
		if (@Id is null)
		begin
			set @Id = 1;
		end
	end

	INSERT INTO dbo.WorkingHours (Id, WeekdayId, StartTime, EndTime)
	VALUES (
		@Id,
		@WeekdayId,
		@StartTime,
		@EndTime
	)

	SELECT CAST(SCOPE_IDENTITY() as int) AS WorkingHoursId
END




