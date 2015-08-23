CREATE PROCEDURE [dbo].[GetWorkingHours]
	@WorkingHoursId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT WeekDayId, StartTime, EndTime
	FROM dbo.WorkingHours
	WHERE Id = @WorkingHoursId
END
