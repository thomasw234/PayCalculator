

CREATE PROCEDURE [dbo].[CreateUser]
	@SessionId uniqueidentifier,
	@IpAddress varchar(45) NULL,
	@Latitude decimal(9,6) NULL,
	@Longitude decimal(9,6) NULL
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.UserDetail (SessionId, IpAddress, Latitude, Longitude, CreatedUtc, LastUpdatedUtc)
	VALUES (
		@SessionId,
		@IpAddress,
		@Latitude,
		@Longitude,
		GETUTCDATE(),
		GETUTCDATE()
	)		
END

