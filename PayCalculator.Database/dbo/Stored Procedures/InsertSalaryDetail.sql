


CREATE PROCEDURE [dbo].[InsertSalaryDetail]
	@SessionId uniqueidentifier,
	@SalaryType int,
	@SalaryValue money
AS
BEGIN

	DECLARE @WorkingHoursId int;
	SET @WorkingHoursId = 
		(select top 1 WorkingHoursId
		from dbo.SalaryDetail
		where SessionId = @SessionId
		order by CreatedUtc desc)

	INSERT INTO dbo.SalaryDetail (SessionId, SalaryType, SalaryValue, CreatedUtc, WorkingHoursId)
	VALUES (
		@SessionId,
		@SalaryType,
		@SalaryValue,
		GETUTCDATE(),
		@WorkingHoursId
	)
END



