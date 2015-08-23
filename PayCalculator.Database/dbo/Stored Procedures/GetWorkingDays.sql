-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetWorkingDays]
	@SessionId uniqueidentifier
AS
BEGIN
	
	declare @WorkingHoursId int;
	set @WorkingHoursId = 
		(select top 1 WorkingHoursId
		 from dbo.SalaryDetail
		 where SessionId = @SessionId
		 order by CreatedUtc desc)


	select distinct(WeekdayId)
	from dbo.WorkingHours
	where Id = @WorkingHoursId

END
