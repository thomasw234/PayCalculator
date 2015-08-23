CREATE TABLE [dbo].[UserDetail] (
    [SessionId]      UNIQUEIDENTIFIER NOT NULL,
    [IpAddress]      VARCHAR (45)     NULL,
    [Latitude]       DECIMAL (9, 6)   NULL,
    [Longitude]      DECIMAL (9, 6)   NULL,
    [CreatedUtc]     DATETIME2 (7)    NOT NULL,
    [LastUpdatedUtc] DATETIME2 (7)    NOT NULL,
    PRIMARY KEY CLUSTERED ([SessionId] ASC)
);

