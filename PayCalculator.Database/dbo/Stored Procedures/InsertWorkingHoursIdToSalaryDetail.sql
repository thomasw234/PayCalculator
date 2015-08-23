-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertWorkingHoursIdToSalaryDetail]
	@SessionId uniqueidentifier,
	@WorkingHoursId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @LatestUpdate datetime2(7);
	set @LatestUpdate = (select max(CreatedUtc) from dbo.SalaryDetail where SessionId = @SessionId);

	update dbo.SalaryDetail
	set WorkingHoursId = @WorkingHoursId
	where SessionId = @SessionId and CreatedUtc = @LatestUpdate
END
