CREATE TABLE [dbo].[WorkingHours] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [WeekdayId] INT           NOT NULL,
    [StartTime] DATETIME2 (7) NOT NULL,
    [EndTime]   DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_WorkingHours] PRIMARY KEY CLUSTERED ([Id] ASC, [WeekdayId] ASC, [StartTime] ASC)
);

