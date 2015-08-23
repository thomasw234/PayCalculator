

CREATE PROCEDURE [dbo].[GetLatestSalaryDetail]
	@SessionId uniqueidentifier
AS
BEGIN
	SELECT TOP 1 SalaryValue, SalaryType, CreatedUtc, WorkingHoursId
	FROM dbo.SalaryDetail
	WHERE SessionId = @SessionId
	ORDER BY CreatedUtc DESC
END

