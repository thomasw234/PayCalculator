CREATE TABLE [dbo].[SalaryDetail] (
    [SessionId]      UNIQUEIDENTIFIER NOT NULL,
    [SalaryType]     INT              NOT NULL,
    [SalaryValue]    MONEY            NOT NULL,
    [WorkingHoursId] INT              NULL,
    [CreatedUtc]     DATETIME2 (7)    NOT NULL,
    PRIMARY KEY CLUSTERED ([SessionId] ASC, [CreatedUtc] ASC)
);

